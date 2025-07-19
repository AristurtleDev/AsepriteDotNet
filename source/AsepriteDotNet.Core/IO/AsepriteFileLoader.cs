//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Drawing;
using AsepriteDotNet.Core.Compression;
using AsepriteDotNet.Core.FileFormat;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core.IO;

/// <summary>
/// Defines a utility class used for loading an Aseprite file.
/// </summary>
public static partial class AsepriteFileLoader
{

    public static AsepriteFile FromFile(string path)
    {
        string fileName = Path.GetFileNameWithoutExtension(path);
        using FileStream stream = File.OpenRead(path);
        return FromStream(fileName, stream, true);
    }

    public static AsepriteFile FromStream(string fileName, Stream stream, bool leaveOpen = false)
    {
        using BinaryReader reader = new(stream, Encoding.UTF8, leaveOpen);
        return LoadFile(fileName, reader);
    }

    private static AsepriteFile LoadFile(string fileName, AsepriteBinaryReader reader)
    {
        AsepriteReaderContext context = new();

        ReadOnlySpan<byte> fileHeaderData = reader.ReadBytes(AsperiteFileFormat.HEADER_SIZE);
        ReadFileHeader(fileHeaderData, context);

        for (int i = 0; i < context.FrameCount; i++)
        {
            context.LastReadChunkType = ChunkType.None;
            context.CurrentUserData = null;
            context.TagIterator = 0;

            uint frameLen = reader.ReadUInt32();

            // Since the total frame size includes the DWORD we've already just read
            // we can subtract the size fo the DWORD from the length;
            frameLen -= sizeof(uint);

            ReadOnlySpan<byte> frameData = reader.ReadBytes((int)frameLen);
            ReadFrame(i, frameData, context);
        }

        AsepriteFile file = new();
        file.Name = Path.GetFileNameWithoutExtension(fileName);
        file.CanvasSize = context.CanvasSize;
        file.ColorDepth = context.ColorDepth;
        file.InternalFrames = context.Frames.ToArray();
        file.InternalLayers = context.Layers.ToArray();
        file.InternalTags = context.Tags.ToArray();
        file.InternalSlices = context.Slices.ToArray();
        file.InternalTilesets = context.Tilesets.ToArray();
        file.Palette = context.Palette;
        file.UserData = context.SpriteUserData;

        return file;
    }

    private static void ReadFileHeader(ReadOnlySpan<byte> fileHeaderData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(fileHeaderData);

        // We don't care about file size, read and discard it
        _ = reader.ReadDword();

        ushort magicNumber = reader.ReadWord();
        if (magicNumber != AsperiteFileFormat.HEADER_MAGIC)
        {
            throw new InvalidDataException($"Invalid file header magic number.  Expected '0x{AsperiteFileFormat.HEADER_MAGIC:X4}' got {magicNumber:X4}'.  This does not appear to be a valid Aseprite file or it may be corrupted.");
        }

        context.FrameCount = reader.ReadWord();

        Size canvasSize = new Size();
        canvasSize.Width = reader.ReadWord();
        canvasSize.Height = reader.ReadWord();
        context.CanvasSize = canvasSize;

        context.ColorDepth = (AsepriteColorDepth)reader.ReadWord();
        if (!Enum.IsDefined(context.ColorDepth))
        {
            throw new InvalidDataException($"Invalid color depth '{context.ColorDepth}'.  This Asperite file may be corrupted.");
        }

        HeaderFlags flags = (HeaderFlags)reader.ReadDword();
        context.LayerOpacityValid = flags.HasFlag(HeaderFlags.LayerOpacityValid);

        // Per Aseprite File spec
        // The Speed value in the header is deprecated and instead users
        // should use the per frame duration values.  Read value and discard
        _ = reader.ReadWord();

        // Per Aseprite File spec
        // Next two DWORD values are reserved, so ignore them by reading and
        // discarding
        _ = reader.ReadDword();
        _ = reader.ReadDword();

        context.Palette.TransparentIndex = reader.ReadByte();
        if (context.Palette.TransparentIndex > 0 && context.ColorDepth != AsepriteColorDepth.Indexed)
        {
            // A transparent index that is not zero is only valid when the color depth mode is indexed.
            context.Palette.TransparentIndex = 0;
        }

        // Per Aseprite File spec
        // Next 3 bytes are reserved
        reader.Ignore(3);

        ushort numColors = reader.ReadWord();
        context.Palette.Resize(numColors);

        // The reamining bytes in the file header are for the Aseprite editor
        // itself and are not needed as part of this library. As such, not
        // reading the rest and just returning here.
    }

    private static void ReadFrame(int frameIndex, ReadOnlySpan<byte> frameData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(frameData);

        context.CurrentFrame = new();
        context.CurrentFrame.OriginalIndex = frameIndex;

        ushort magicNumber = reader.ReadWord();
        if (magicNumber != AsperiteFileFormat.FRAME_MAGIC)
        {
            throw new InvalidDataException($"Invalid frame  magic number in frame {context.CurrentFrame.OriginalIndex}.  Expected '0x{AsperiteFileFormat.FRAME_MAGIC:X4}' got {magicNumber:X4}'.  The Asperite file may be corrupted.");
        }

        ushort oldChunkCount = reader.ReadWord();

        ushort duration = reader.ReadWord();
        context.CurrentFrame.Duration = TimeSpan.FromMilliseconds(duration);

        // Per Aseprite File spec
        // Next two bytes are reserved
        reader.Ignore(2);

        uint newChunkCount = reader.ReadDword();

        // Determine the number of chunks to read
        int chunkCount = oldChunkCount;
        if (chunkCount == 0xFFFF && newChunkCount > 0)
        {
            chunkCount = (int)newChunkCount;
        }

        for (int i = 0; i < chunkCount; i++)
        {
            uint chunkSize = reader.ReadDword();
            ChunkType chunkType = (ChunkType)reader.ReadWord();

            int chunkDataSize = (int)chunkSize - sizeof(uint) - sizeof(ushort);
            ReadOnlySpan<byte> chunkData = reader.ReadBytes(chunkDataSize);
            ProcessChunk(chunkType, chunkData, context);
        }

        context.Frames.Add(context.CurrentFrame);
    }

    private static void ProcessChunk(ChunkType chunkType, ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        switch (chunkType)
        {
            case ChunkType.OldPalette1:
                ReadAsepriteOldPalette1Chunk(chunkData, context);
                break;

            case ChunkType.OldPalette2:
                ReadAsepriteOldPalette2Chunk(chunkData, context);
                break;

            case ChunkType.Layer:
                ReadAsepriteLayerChunk(chunkData, context);
                break;

            case ChunkType.Cel:
                ReadAsepriteCelChunk(chunkData, context);
                break;

            case ChunkType.Tags:
                ReadAsepriteTagChunk(chunkData, context);
                break;

            case ChunkType.Palette:
                ReadAsepritePaletteChunk(chunkData, context);
                break;

            case ChunkType.UserData:
                ReadAsepriteUserDataChunk(chunkData, context);
                break;

            case ChunkType.Slice:
                ReadAsepriteSliceChunk(chunkData, context);
                break;

            case ChunkType.Tileset:
                ReadAsepriteTilesetChunk(chunkData, context);
                break;
        }
    }

    private static void ReadAsepriteOldPalette1Chunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        if (context.PaletteRead)
        {
            // Do not read this chunk if the new 0x2019 Palette Chunk has already been read.
            return;
        }

        SpanBinaryReader reader = new(chunkData);

        ushort packets = reader.ReadWord();
        int skip = 0;
        int size = 0;

        for (int i = 0; i < packets; i++)
        {
            skip += reader.ReadByte();
            size = reader.ReadByte();

            if (size == 0)
            {
                size = 256;
            }

            context.Palette.Resize(size);

            for (int c = skip; c < skip + size; c++)
            {
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();
                byte a = byte.MaxValue;

                context.Palette[c] = new Rgba32(r, g, b, a);
            }
        }

        context.PaletteRead = true;
        context.LastReadChunkType = ChunkType.OldPalette1;
    }

    private static void ReadAsepriteOldPalette2Chunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        if (context.PaletteRead)
        {
            // Do not read this chunk if the new 0x2019 Palette Chunk has already been read.
            return;
        }

        SpanBinaryReader reader = new(chunkData);

        ushort packets = reader.ReadWord();
        int skip = 0;
        int size = 0;

        for (int i = 0; i < packets; i++)
        {
            skip += reader.ReadByte();
            size = reader.ReadByte();

            if (size == 0)
            {
                size = 256;
            }

            context.Palette.Resize(size);

            for (int c = skip; c < skip + size; c++)
            {
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();
                byte a = byte.MaxValue;

                // Old palette type 2 uses six bit values (0-63) that must be expanded to
                // eight bit values
                r = (byte)((r << 2) | (r >> 4));
                g = (byte)((g << 2) | (g >> 4));
                b = (byte)((b << 2) | (b >> 4));

                context.Palette[c] = new Rgba32(r, g, b, a);
            }
        }

        context.PaletteRead = true;
        context.LastReadChunkType = ChunkType.OldPalette2;
    }

    private static void ReadAsepriteLayerChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);
        LayerFlags flags = (LayerFlags)reader.ReadWord();
        LayerType layerType = (LayerType)reader.ReadWord();
        ushort layerChildLevel = reader.ReadWord();
        ushort defaultLayerWidth = reader.ReadWord();
        ushort defaultLayerHeight = reader.ReadWord();
        AsepriteBlendMode blendMode = (AsepriteBlendMode)reader.ReadWord();
        byte opacity = reader.ReadByte();

        // Per Aseprite File Spec
        // Next 3 bytes are reserved
        reader.Ignore(3);

        string layerName = reader.ReadString();

        AsepriteLayer layer;
        if (layerType == LayerType.Normal)
        {
            AsepriteImageLayer imageLayer = new();
            imageLayer.IsVisible = flags.HasFlag(LayerFlags.Visible);
            imageLayer.IsBackgroundLayer = flags.HasFlag(LayerFlags.Background);
            imageLayer.IsReferenceLayer = flags.HasFlag(LayerFlags.Reference);
            imageLayer.ChildLevel = layerChildLevel;
            imageLayer.BlendMode = blendMode;
            imageLayer.Opacity = opacity;
            imageLayer.Name = layerName;

            context.Layers.Add(imageLayer);
            context.CurrentUserData = imageLayer.UserData;
            context.LastReadChunkType = ChunkType.Layer;
            layer = imageLayer;
        }
        else if (layerType == LayerType.Group)
        {
            AsepriteGroupLayer groupLayer = new();
            groupLayer.IsVisible = flags.HasFlag(LayerFlags.Visible);
            groupLayer.IsBackgroundLayer = flags.HasFlag(LayerFlags.Background);
            groupLayer.IsReferenceLayer = flags.HasFlag(LayerFlags.Reference);
            groupLayer.ChildLevel = layerChildLevel;
            groupLayer.BlendMode = blendMode;
            groupLayer.Opacity = opacity;
            groupLayer.Name = layerName;

            context.Layers.Add(groupLayer);
            context.LastGroupsByChildLevel[groupLayer.ChildLevel] = groupLayer;
            context.CurrentUserData = groupLayer.UserData;
            context.LastReadChunkType = ChunkType.Layer;
            layer = groupLayer;
        }
        else if (layerType == LayerType.Tilemap)
        {
            uint tilesetIndex = reader.ReadDword();

            AsepriteTilemapLayer tilemapLayer = new();
            tilemapLayer.IsVisible = flags.HasFlag(LayerFlags.Visible);
            tilemapLayer.IsBackgroundLayer = flags.HasFlag(LayerFlags.Background);
            tilemapLayer.IsReferenceLayer = flags.HasFlag(LayerFlags.Reference);
            tilemapLayer.ChildLevel = layerChildLevel;
            tilemapLayer.BlendMode = blendMode;
            tilemapLayer.Opacity = opacity;
            tilemapLayer.Name = layerName;
            tilemapLayer.Tileset = context.Tilesets[(int)tilesetIndex];

            context.Layers.Add(tilemapLayer);
            context.CurrentUserData = tilemapLayer.UserData;
            context.LastReadChunkType = ChunkType.Layer;
            layer = tilemapLayer;
        }
        else
        {
            throw new InvalidDataException($"Invalid layer type '{layerType} in layer chunk.  The Aseprite file may be corrupted.");
        }

        if (layer.ChildLevel != 0 && context.LastGroupsByChildLevel.TryGetValue(layer.ChildLevel - 1, out AsepriteGroupLayer group))
        {
            group.InternalChildren.Add(layer);
        }
    }

    private static void ReadAsepriteCelChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);
        ushort layerIndex = reader.ReadWord();
        short xPosition = reader.ReadShort();
        short yPosition = reader.ReadShort();
        byte opacityLevel = reader.ReadByte();
        CelType celType = (CelType)reader.ReadWord();
        short zIndex = reader.ReadShort();


        // Per Aseprite File Spec
        // Next 5 byte are for future use
        reader.Ignore(5);

        AsepriteLayer celLayer = context.Layers[layerIndex];

        if (celType == CelType.RawImage || celType == CelType.CompressedImage)
        {
            ushort widthInPixels = reader.ReadWord();
            ushort heightInPixels = reader.ReadWord();
            ReadOnlySpan<byte> pixeldata = reader.ReadBytes(reader.Remaining);
            Rgba32[] pixels;

            if (celType == CelType.RawImage)
            {
                pixels = PixelsToColor(pixeldata, context.ColorDepth, context.Palette);
            }
            else
            {
                // Deflate pixel data using zlib
                byte[] decompressedPixels = Zlib.Deflate(pixeldata.ToArray());
                pixels = PixelsToColor(decompressedPixels, context.ColorDepth, context.Palette);
            }

            AsepriteImageCel imageCel = new();
            imageCel.Layer = celLayer;
            imageCel.Location = new Point(xPosition, yPosition);
            imageCel.Opacity = opacityLevel;
            imageCel.ZIndex = zIndex;
            imageCel.InternalPixels = pixels;
            imageCel.Size = new Size(widthInPixels, heightInPixels);

            context.CurrentFrame?.InternalCels.Add(imageCel);
            context.CurrentUserData = imageCel.UserData;
            context.LastReadChunkType = ChunkType.Cel;
        }
        else if (celType == CelType.Linked)
        {
            ushort frameIndex = reader.ReadWord();
            AsepriteFrame originFrame = context.Frames[frameIndex];
            AsepriteCel originCel = null;
            for (int i = 0; i < originFrame.Cels.Length; i++)
            {
                if (originFrame.Cels[i].Layer == celLayer)
                {
                    originCel = originFrame.Cels[i];
                    break;
                }
            }

            if (originCel is null)
            {
                throw new InvalidDataException($"Invalid layer index '{layerIndex}' for linked cel");
            }

            AsepriteLinkedCel linkedCel = new();
            linkedCel.Layer = celLayer;
            linkedCel.Location = new Point(xPosition, yPosition);
            linkedCel.Opacity = opacityLevel;
            linkedCel.ZIndex = zIndex;
            linkedCel.Cel = originCel;

            context.CurrentFrame?.InternalCels.Add(linkedCel);
            context.CurrentUserData = linkedCel.UserData;
            context.LastReadChunkType = ChunkType.Cel;
        }
        else if (celType == CelType.CompressedTilemap)
        {
            ushort widthInTiles = reader.ReadWord();
            ushort heightInTiles = reader.ReadWord();
            ushort bitsPerTile = reader.ReadWord();
            uint tileIdBitmask = reader.ReadDword();
            uint xFlipBitmask = reader.ReadDword();
            uint yFlipBitmask = reader.ReadDword();
            uint diagFlipBitmask = reader.ReadDword();

            // Per Aseprite File spec
            // next 10 bytes are reserved
            reader.Ignore(10);

            ReadOnlySpan<byte> compressedTileData = reader.ReadBytes(reader.Remaining);
            byte[] tileData = Zlib.Deflate(compressedTileData.ToArray());

            int bytesPerTile = bitsPerTile / 8;
            int numTiles = tileData.Length / bytesPerTile;
            AsepriteTile[] tiles = new AsepriteTile[numTiles];

            unsafe
            {
                fixed (byte* tileDataPtr = tileData)
                {
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        uint value = *(uint*)(tileDataPtr + i * bytesPerTile);
                        uint id = (value & tileIdBitmask) >> AsperiteFileFormat.TILE_ID_SHIFT;
                        bool flipVertically = Calc.HasFlag(value, xFlipBitmask);
                        bool flipHorizontally = Calc.HasFlag(value, yFlipBitmask);
                        bool flipDiagonally = Calc.HasFlag(value, diagFlipBitmask);

                        AsepriteTile tile = new();
                        tile.ID = (int)id;
                        tile.FlipVertically = flipVertically;
                        tile.FlipHorizontally = flipHorizontally;
                        tile.FlipDiagonally = flipDiagonally;
                        tiles[i] = tile;
                    }
                }
            }

            AsepriteTilemapCel tilemapCel = new();
            tilemapCel.Layer = celLayer;
            tilemapCel.Location = new Point(xPosition, yPosition);
            tilemapCel.Opacity = opacityLevel;
            tilemapCel.ZIndex = zIndex;
            tilemapCel.Size = new Size(widthInTiles, heightInTiles);
            tilemapCel.InternalTiles = tiles;

            context.CurrentFrame?.InternalCels.Add(tilemapCel);
            context.CurrentUserData = tilemapCel.UserData;
            context.LastReadChunkType = ChunkType.Cel;
        }
        else
        {
            throw new InvalidDataException($"Invalid cel type '{celType} in cel chunk.  The Aseprite file may be corrupted.");
        }
    }

    private static void ReadAsepriteTagChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);
        ushort numTags = reader.ReadWord();

        // Per Aseprite File Spec
        // Next 8 bytes are reserved
        reader.Ignore(8);

        for (int i = 0; i < numTags; i++)
        {
            AsepriteTag tag = new();
            tag.FromFrame = reader.ReadWord();
            tag.ToFrame = reader.ReadWord();
            tag.LoopDirection = (AsepriteLoopDirection)reader.ReadByte();
            tag.RepeatCount = reader.ReadWord();

            // Per Aseprite File Spec
            // Next 6 bytes are reserved
            reader.Ignore(6);

            byte tagColorRed = reader.ReadByte();
            byte tagColorGreen = reader.ReadByte();
            byte tagColorBlue = reader.ReadByte();
            tag.Color = new(tagColorRed, tagColorGreen, tagColorBlue);

            // Per Aseprite File Spec
            // Next byte is just an extra byte
            reader.Ignore(1);

            tag.Name = reader.ReadString();

            context.Tags.Add(tag);
            context.CurrentUserData = tag.UserData;
        }

        context.LastReadChunkType = ChunkType.Tags;
    }

    private static void ReadAsepritePaletteChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);
        uint newPaletteSize = reader.ReadDword();
        uint firstColorIndex = reader.ReadDword();
        uint lastColorIndex = reader.ReadDword();

        // Per Aseprite File Spec
        // Next 8 bytes are reserved
        reader.Ignore(8);

        if (newPaletteSize > 0)
        {
            context.Palette.Resize((int)newPaletteSize);
        }

        for (int i = (int)firstColorIndex; i <= (int)lastColorIndex; i++)
        {
            PaletteFlags flags = (PaletteFlags)reader.ReadWord();
            byte red = reader.ReadByte();
            byte green = reader.ReadByte();
            byte blue = reader.ReadByte();
            byte alpha = reader.ReadByte();

            context.Palette[i] = new Rgba32(red, green, blue, alpha);

            // if the color has a name, we'll just read and discard the value
            // since we don't care about the name
            if (flags.HasFlag(PaletteFlags.HasName))
            {
                _ = reader.ReadString();
            }
        }

        context.PaletteRead = true;
        context.LastReadChunkType = ChunkType.Palette;
    }

    private static void ReadAsepriteUserDataChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);
        UserDataFlags flags = (UserDataFlags)reader.ReadDword();

        string text = null;
        Rgba32? color = null;

        if (flags.HasFlag(UserDataFlags.HasText))
        {
            text = reader.ReadString();
        }

        if (flags.HasFlag(UserDataFlags.HasColor))
        {
            byte red = reader.ReadByte();
            byte green = reader.ReadByte();
            byte blue = reader.ReadByte();
            byte alpha = reader.ReadByte();
            color = new Rgba32(red, green, blue, alpha);
        }

        if (context.CurrentUserData is null && context.PaletteRead)
        {
            context.SpriteUserData.Text = text;
            context.SpriteUserData.Color = color;
        }
        else if (context.CurrentUserData is not null)
        {
            if (context.LastReadChunkType != ChunkType.Tags)
            {
                context.CurrentUserData.Text = text;
                context.CurrentUserData.Color = color;
            }
            else
            {
                //  Tags are a special case.  User data for tags comes all together
                //  (one next to the other) after the tags chunk, in the same order:
                //
                //  TAGS CHUNK (TAG1, TAG2, ..., TAGn)
                //  USER DATA CHUNK FOR TAG1
                //  USER DATA CHUNK FOR TAG2
                //  ...
                //  USER DATA CHUNK FOR TAGn
                //
                //  So here we expect that the next user data chunk will correspond to the next tag
                //  int he tags collection
                //
                // However, `currentUserData` is actually the one of last tags after read last chunk of ASE_CHUNK_TAGS
                // So here we fix the `currentUserData` to the right one
                context.CurrentUserData = context.Tags[context.TagIterator++].UserData;
                context.CurrentUserData.Text = text;
                context.CurrentUserData.Color = color;

                if (context.TagIterator < context.Tags.Count)
                {
                    context.CurrentUserData = context.Tags[context.TagIterator].UserData;
                }
                else
                {
                    context.CurrentUserData = null;
                    context.LastReadChunkType = ChunkType.None;
                }
            }
        }
    }

    private static void ReadAsepriteSliceChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);
        uint numKeys = reader.ReadDword();
        SliceFlags flags = (SliceFlags)reader.ReadDword();

        // Per Aseprite File spec
        // Next DWORD is reserved so we just discard it
        _ = reader.ReadDword();

        AsepriteSlice slice = new();
        slice.Name = reader.ReadString();
        slice.InternalKeys = new AsepriteSliceKey[numKeys];
        slice.IsNinePatch = flags.HasFlag(SliceFlags.IsNinePatch);
        slice.HasPivot = flags.HasFlag(SliceFlags.HasPivot);

        for (int i = 0; i < slice.InternalKeys.Length; i++)
        {
            AsepriteSliceKey key = new();
            key.FrameIndex = (int)reader.ReadDword();

            Rectangle bounds = new();
            bounds.X = reader.ReadLong();
            bounds.Y = reader.ReadLong();
            bounds.Width = (int)reader.ReadDword();
            bounds.Height = (int)reader.ReadDword();
            key.Bounds = bounds;

            if (flags.HasFlag(SliceFlags.IsNinePatch))
            {
                Rectangle centerBounds = new();
                centerBounds.X = reader.ReadLong();
                centerBounds.Y = reader.ReadLong();
                centerBounds.Width = (int)reader.ReadDword();
                centerBounds.Height = (int)reader.ReadDword();
                key.CenterBounds = centerBounds;
            }

            if (flags.HasFlag(SliceFlags.HasPivot))
            {
                Point pivot = new();
                pivot.X = reader.ReadLong();
                pivot.Y = reader.ReadLong();
                key.Pivot = pivot;
            }

            slice.InternalKeys[i] = key;
        }

        context.Slices.Add(slice);
        context.CurrentUserData = slice.UserData;
        context.LastReadChunkType = ChunkType.Slice;
    }

    private static void ReadAsepriteTilesetChunk(ReadOnlySpan<byte> chunkData, AsepriteReaderContext context)
    {
        SpanBinaryReader reader = new(chunkData);

        AsepriteTileset tileset = new();
        tileset.ID = (int)reader.ReadDword();

        TilesetFlags flags = (TilesetFlags)reader.ReadDword();

        tileset.TileCount = (int)reader.ReadDword();

        Size tileSize = new();
        tileSize.Width = reader.ReadWord();
        tileSize.Height = reader.ReadWord();
        tileset.TileSize = tileSize;

        // We don't need the Base Index number, this is just a value used
        // within the aseprite editor, so we'll read it and discord it
        _ = reader.ReadShort();

        // Per Aseprite File spec
        // Net 14 bytes are reserved
        reader.Ignore(14);

        tileset.Name = reader.ReadString();

        if (flags.HasFlag(TilesetFlags.ExternalFile))
        {
            //  No support for external files at this time. To my knowledge, Aseprite doesn't
            //  support this directly in the UI and is only something that can be added through the
            //  LUA scripting extensions.  So until Aseprite officially adds this as something every
            //  user can do within the UI or someone opens an issue and needs this, not
            //  implementing it.
            throw new NotSupportedException($"Tileset '{tileset.Name}' includes the tileset in an external file. This is not supported at this time.");
        }

        if (!flags.HasFlag(TilesetFlags.Embedded))
        {
            //  Only support at this time for tileset data that is embedded in the file.
            throw new NotSupportedException($"Tileset '{tileset.Name}' does not include tileset image embedded in file.");
        }

        uint dataLen = reader.ReadDword();
        ReadOnlySpan<byte> compressedPixelData = reader.ReadBytes((int)dataLen);
        byte[] decompressedPixelData = Zlib.Deflate(compressedPixelData.ToArray());
        tileset.InternalPixels = PixelsToColor(decompressedPixelData, context.ColorDepth, context.Palette);

        context.Tilesets.Add(tileset);
        context.LastReadChunkType = ChunkType.Tileset;
    }

    private static Rgba32[] PixelsToColor(ReadOnlySpan<byte> pixels, AsepriteColorDepth depth, AsepritePalette palette)
    {
        const int BITS_PER_PIXEL = 8;
        int bytesPerPixel = (int)depth / BITS_PER_PIXEL;
        Rgba32[] result = new Rgba32[pixels.Length / bytesPerPixel];

        for (int i = 0, b = 0; i < result.Length; i++, b += bytesPerPixel)
        {
            byte red, green, blue, alpha;
            red = green = blue = alpha = 0;

            switch (depth)
            {
                case AsepriteColorDepth.RGBA:
                    red = pixels[b];
                    green = pixels[b + 1];
                    blue = pixels[b + 2];
                    alpha = pixels[b + 3];
                    break;

                case AsepriteColorDepth.Grayscale:
                    red = green = blue = pixels[b];
                    alpha = pixels[b + 1];
                    break;

                case AsepriteColorDepth.Indexed:
                    int index = pixels[b];
                    if (index != palette.TransparentIndex && index < palette.Colors.Length)
                    {
                        palette.Colors[index].Deconstruct(out red, out green, out blue, out alpha);
                    }
                    break;

                default:
                    throw new InvalidOperationException($"Unkown color depth: {depth}");
            }

            result[i] = new Rgba32(red, green, blue, alpha);
        }

        return result;
    }
}
