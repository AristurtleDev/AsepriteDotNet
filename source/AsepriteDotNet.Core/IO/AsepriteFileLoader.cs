//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core.FileFormat.Data;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core.IO;

/// <summary>
/// Defines a utility class used for loading an Aseprite file.
/// </summary>
public static partial class AsepriteFileLoader
{
    /// <summary>
    /// Loads the Aseprite file at the specified path.
    /// </summary>
    /// <param name="path">The absolute file path to the Aseprite file to load.</param>
    /// <param name="preMultiplyAlpha">
    /// Indicates whether color values should be translated to a premultiplied alpha value.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="AsepriteFile"/> class containing the contents of the Aseprite file that was
    /// loaded.
    /// </returns>
    public static AsepriteFile FromFile(string path, bool preMultiplyAlpha = true)
    {
        string fileName = Path.GetFileNameWithoutExtension(path);
        using FileStream stream = File.OpenRead(path);
        return FromStream(fileName, stream, true, preMultiplyAlpha);
    }

    /// <summary>
    /// Loads an Aseprite file from a given stream.
    /// </summary>
    /// <param name="fileName">The name of the Aseprite file, minus the extension.</param>
    /// <param name="stream">The stream to load the Aseprite file from.</param>
    /// <param name="leaveOpen">
    /// <see langword="true"/> to leave the given <paramref name="stream"/> open after loading the Aseprite file;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="preMultiplyAlpha">
    /// Indicates whether color values should be translated to a premultiplied alpha value.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="AsepriteFile"/> class containing the contents for the Aseprite file that was
    /// loaded.
    /// </returns>
    public static AsepriteFile FromStream(string fileName, Stream stream, bool leaveOpen = false, bool preMultiplyAlpha = true)
    {
        using AsepriteBinaryReader reader = new AsepriteBinaryReader(stream, leaveOpen);
        return LoadFile(fileName, reader, preMultiplyAlpha);
    }

    /// <summary>
    /// Reads only the tag data from the Aseprite file.
    /// </summary>
    /// <param name="path">The absolute file path to the Aseprite file to load.</param>
    /// <returns>An array of all tag data from the Aseprite file.</returns>
    public static AsepriteTag[] ReadTags(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return ReadTags(stream);
    }

    /// <summary>
    /// Reads only the tag data from the Aseprite file.
    /// </summary>
    /// <param name="stream">The stream to load the Aseprite file from.</param>
    /// <param name="leaveOpen">
    /// <see langword="true"/> to leave the given <paramref name="stream"/> open after loading the Aseprite file;
    /// otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>An array of all tag data from the Aseprite file.</returns>
    public static AsepriteTag[] ReadTags(Stream stream, bool leaveOpen = false)
    {
        using AsepriteBinaryReader reader = new AsepriteBinaryReader(stream, leaveOpen);
        return ReadTags(reader);
    }

    private static AsepriteTag[] ReadTags(AsepriteBinaryReader reader)
    {
        List<AsepriteTag> tags = new List<AsepriteTag>();

        //  Read the file header
        FileHeaderData fileHeaderData = reader.ReadUnsafe<FileHeaderData>(FileHeaderData.SizeInBytes);

        //  Validate the file header magic number
        if (fileHeaderData.MagicNumber != ASE_HEADER_MAGIC)
        {
            reader.Dispose();
            throw new InvalidOperationException($"invalid file header magic number: 0x{fileHeaderData.MagicNumber:X4}.  This does not appear to be a valid Aseprite file.");
        }

        //  All tags exist within the first frame data so we only need to read one frame
        FrameHeaderData frameHeaderData = reader.ReadUnsafe<FrameHeaderData>(FrameHeaderData.SizeInBytes);

        //  Validate the magic number in frame header
        if (frameHeaderData.MagicNumber != ASE_FRAME_MAGIC)
        {
            throw new InvalidOperationException($"Frame 0 contains an invalid magic number: 0x{frameHeaderData.MagicNumber:X4}");
        }

        //  Determine the number of chunks to read
        int chunkCount = frameHeaderData.OldChunkCount;
        if (chunkCount == 0xFFFF && chunkCount < frameHeaderData.NewChunkCount)
        {
            chunkCount = (int)frameHeaderData.NewChunkCount;
        }

        //  Reference to the user data object to apply user data to from the last chunk that was read that
        //  could have had user data
        AsepriteUserData? currentUserData = null;

        //  Tracks the iteration of the tags when reading user data for tags chunk.
        int tagIterator = 0;

        uint? lastReadChunkType = null;

        //  Read until we get the tags then exit early
        for (int chunkNum = 0; chunkNum < chunkCount; chunkNum++)
        {
            long chunkStart = reader.Position;
            ChunkHeaderData chunkHeaderData = reader.ReadUnsafe<ChunkHeaderData>(ChunkHeaderData.SizeInBytes);
            long chunkEnd = chunkStart + chunkHeaderData.ChunkSize;

            switch (chunkHeaderData.ChunkType)
            {
                case ASE_CHUNK_TAGS:
                    {
                        ushort tagCount = reader.ReadWord();
                        reader.Ignore(8);

                        for (int i = 0; i < tagCount; i++)
                        {
                            TagData tagData = reader.ReadUnsafe<TagData>(TagData.SizeInBytes);

                            //  Validate loop direction
                            if (!Enum.IsDefined<AsepriteLoopDirection>((AsepriteLoopDirection)tagData.Direction))
                            {
                                reader.Dispose();
                                throw new InvalidOperationException($"Unknown loop direction: {tagData.Direction}");
                            }

                            string tagName = reader.ReadString(tagData.NameLen);

                            AsepriteTag tag = new AsepriteTag(tagData, tagName);
                            currentUserData = tag.UserData;
                            lastReadChunkType = chunkHeaderData.ChunkType;
                            tags.Add(tag);
                        }
                    }
                    break;

                case ASE_CHUNK_USER_DATA:
                    {
                        uint flags = reader.ReadDword();
                        string? text = null;
                        Rgba32? color = null;

                        if (Calc.HasFlag(flags, ASE_USER_DATA_FLAG_HAS_TEXT))
                        {
                            text = reader.ReadString();
                        }

                        if (Calc.HasFlag(flags, ASE_USER_DATA_FLAG_HAS_COLOR))
                        {
                            color = reader.ReadUnsafe<Rgba32>(Rgba32.StructSize);
                        }

                        if (currentUserData is not null)
                        {
                            if (lastReadChunkType != ASE_CHUNK_TAGS)
                            {
                                currentUserData.Text = text;
                                currentUserData.Color = color;
                            }
                            else {
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
                                currentUserData = tags[tagIterator++].UserData;
                                currentUserData.Text = text;
                                currentUserData.Color = color;

                                if (tagIterator < tags.Count)
                                {
                                    currentUserData = tags[tagIterator].UserData;
                                }
                                else
                                {
                                    currentUserData = null;
                                    lastReadChunkType = null;
                                }
                            }
                        }
                    }
                    break;

            }
            reader.Seek(chunkEnd, SeekOrigin.Begin);
        }

        return tags.ToArray();
    }


    private static AsepriteFile LoadFile(string fileName, AsepriteBinaryReader reader, bool preMultiplyAlpha)
    {
        //  Collection of non-fatal warnings accumulated while loading the Aseprite file. Provided to the consumer as a
        //  means of seeing why some data may not be what they expect it to be.
        List<string> warnings = new List<string>();

        // References to the most recent previous group layers read by child level.
        // Key: Child level of the group layer
        // Value: Group layer
        Dictionary<int, AsepriteGroupLayer> lastGroupsByChildLevel = new();

        //  Flag to determine if the palette has been read.  This is used to flag that a user data chunk is for the
        //  sprite due to changes in Aseprite 1.3
        bool paletteRead = false;

        //  Read the file header
        FileHeaderData fileHeaderData = reader.ReadUnsafe<FileHeaderData>(FileHeaderData.SizeInBytes);

        //  Validate the file header magic number
        if (fileHeaderData.MagicNumber != ASE_HEADER_MAGIC)
        {
            reader.Dispose();
            throw new InvalidOperationException($"invalid file header magic number: 0x{fileHeaderData.MagicNumber:X4}.  This does not appear to be a valid Aseprite file.");
        }

        //  Validate canvas size
        if (fileHeaderData.CanvasWidth < 1 || fileHeaderData.CanvasHeight < 1)
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid canvas size: {fileHeaderData.CanvasWidth}x{fileHeaderData.CanvasHeight}");
        }

        AsepriteColorDepth depth = (AsepriteColorDepth)fileHeaderData.Depth;

        if (!Enum.IsDefined(typeof(AsepriteColorDepth), depth))
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid color depth mode: {fileHeaderData.Depth}");
        }

        bool isLayerOpacityValid = fileHeaderData.Flags.HasFlag(ASE_HEADER_FLAG_LAYER_OPACITY_VALID);
        if (!isLayerOpacityValid)
        {
            warnings.Add("Layer opacity valid flag is not set.  All layer opacity will default to 255");
        }

        if (fileHeaderData.TransparentIndex > 0 && depth != AsepriteColorDepth.Indexed)
        {
            fileHeaderData.TransparentIndex = 0;
            warnings.Add("Transparent index only valid for Indexed Color Depth mode.  Defaulting to 0");
        }

        AsepritePalette palette = new AsepritePalette(fileHeaderData.TransparentIndex);
        List<AsepriteFrame> frames = new List<AsepriteFrame>();
        List<AsepriteLayer> layers = new List<AsepriteLayer>();
        List<AsepriteTag> tags = new List<AsepriteTag>();
        List<AsepriteSlice> slices = new List<AsepriteSlice>();
        List<AsepriteTileset> tilesets = new List<AsepriteTileset>();
        AsepriteUserData spriteUserData = new AsepriteUserData();

        //  Read frame-by-frame until all frames are read.
        for (int frameNum = 0; frameNum < fileHeaderData.FrameCount; frameNum++)
        {
            List<AsepriteCel> cels = new List<AsepriteCel>();

            uint? lastReadChunkType = null;

            //  Reference to the user data object to apply user data to from the last chunk that was read that
            //  could have had user data
            AsepriteUserData? currentUserData = null;

            //  Tracks the iteration of the tags when reading user data for tags chunk.
            int tagIterator = 0;

            FrameHeaderData frameHeaderData = reader.ReadUnsafe<FrameHeaderData>(FrameHeaderData.SizeInBytes);

            //  Validate the magic number in frame header
            if (frameHeaderData.MagicNumber != ASE_FRAME_MAGIC)
            {
                throw new InvalidOperationException($"Frame {frameNum} contains an invalid magic number: 0x{frameHeaderData.MagicNumber:X4}");
            }

            //  Determine the number of chunks to read
            int chunkCount = frameHeaderData.OldChunkCount;
            if (chunkCount == 0xFFFF && chunkCount < frameHeaderData.NewChunkCount)
            {
                chunkCount = (int)frameHeaderData.NewChunkCount;
            }

            //  Read chunk-by-chunk until all chunks are read.
            for (int chunkNum = 0; chunkNum < chunkCount; chunkNum++)
            {
                long chunkStart = reader.Position;
                ChunkHeaderData chunkHeaderData = reader.ReadUnsafe<ChunkHeaderData>(ChunkHeaderData.SizeInBytes);
                long chunkEnd = chunkStart + chunkHeaderData.ChunkSize;

                switch (chunkHeaderData.ChunkType)
                {
                    case ASE_CHUNK_LAYER:
                        {
                            LayerData layerData = reader.ReadUnsafe<LayerData>(LayerData.SizeInBytes);
                            string layerName = reader.ReadString(layerData.NameLen);
                            AsepriteLayer layer;
                            switch (layerData.Type)
                            {
                                case ASE_LAYER_TYPE_NORMAL:
                                    layer = new AsepriteImageLayer(layerData, layerName);
                                    break;
                                case ASE_LAYER_TYPE_GROUP:
                                    layer = new AsepriteGroupLayer(layerData, layerName);
                                    break;
                                case ASE_LAYER_TYPE_TILEMAP:
                                    uint tilesetIndex = reader.ReadDword();
                                    AsepriteTileset tileset = tilesets[(int)tilesetIndex];
                                    layer = new AsepriteTilemapLayer(layerData, layerName, tileset);
                                    break;
                                default:
                                    reader.Dispose();
                                    throw new InvalidOperationException($"Unknown layer type: {layerData.Type}");
                            }

                            if (layerData.Level != 0 && lastGroupsByChildLevel.TryGetValue(layerData.Level - 1, out var group))
                            {
                                group.AddChild(layer);
                            }

                            if (layer is AsepriteGroupLayer groupLayer)
                            {
                                lastGroupsByChildLevel[groupLayer.ChildLevel] = groupLayer;
                            }

                            currentUserData = layer.UserData;
                            lastReadChunkType = chunkHeaderData.ChunkType;
                            layers.Add(layer);
                        }
                        break;

                    case ASE_CHUNK_CEL:
                        {
                            CelHeaderData celHeaderData = reader.ReadUnsafe<CelHeaderData>(CelHeaderData.SizeInBytes);
                            AsepriteCel cel;
                            AsepriteLayer celLayer = layers[celHeaderData.LayerIndex];

                            switch (celHeaderData.Type)
                            {
                                case ASE_CEL_TYPE_RAW_IMAGE:
                                case ASE_CEL_TYPE_COMPRESSED_IMAGE:
                                    {
                                        ImageCelData imageCelData = reader.ReadUnsafe<ImageCelData>(ImageCelData.SizeInBytes);
                                        int len = (int)(chunkEnd - reader.Position);
                                        byte[] data = celHeaderData.Type == ASE_CEL_TYPE_COMPRESSED_IMAGE ? reader.ReadCompressed(len) : reader.ReadBytes(len);
                                        Rgba32[] pixels = AsepriteColorUtilities.PixelsToColor(data, depth, preMultiplyAlpha, palette);
                                        cel = new AsepriteImageCel(celHeaderData, celLayer, imageCelData, pixels);
                                    }
                                    break;

                                case ASE_CEL_TYPE_LINKED:
                                    {
                                        ushort frameIndex = reader.ReadWord();
                                        AsepriteFrame originFrame = frames[frameIndex];
                                        AsepriteCel? originCel = null;
                                        for(int i = 0; i < originFrame.Cels.Length; i++)
                                        {
                                            if(originFrame.Cels[i].Layer == celLayer)
                                            {
                                                originCel = originFrame.Cels[i];
                                                break;
                                            }
                                        }

                                        if(originCel is null)
                                        {
                                            throw new InvalidOperationException("Unable to find origin cel for linked cel");
                                        }

                                        cel = new AsepriteLinkedCel(celHeaderData, originCel);
                                    }
                                    break;

                                case ASE_CEL_TYPE_COMPRESSED_TILEMAP:
                                    {
                                        TilemapCelData tilemapCelData = reader.ReadUnsafe<TilemapCelData>(TilemapCelData.SizeInBytes);
                                        int len = (int)(chunkEnd - reader.Position);
                                        byte[] data = reader.ReadCompressed(len);

                                        //  Per Aseprite file spec, the "bits" per tile is, at the moment, always
                                        //  32-bits.  This means it's 4-bytes per tile (32 / 8 = 4).  Meaning that each
                                        //  tile value is a uint (DWORD)
                                        int bytesPerTile = sizeof(uint);
                                        AsepriteTile[] tiles = new AsepriteTile[data.Length / bytesPerTile];

                                        unsafe
                                        {
                                            fixed (byte* tileDataPtr = data)
                                            {
                                                for (int i = 0; i < tiles.Length; i++)
                                                {
                                                    uint value = *(uint*)(tileDataPtr + i * bytesPerTile);
                                                    uint id = (value & tilemapCelData.TileIdBitmask) >> TILE_ID_SHIFT;
                                                    bool flipHorizontally = Calc.HasFlag(value, tilemapCelData.HorizontalFlipBitmask);
                                                    bool flipVertically = Calc.HasFlag(value, tilemapCelData.VerticalFlipBitmask);
                                                    bool flipDiagonally = Calc.HasFlag(value, tilemapCelData.DiagonalFlipBitmask);

                                                    AsepriteTile tile = new AsepriteTile((int)id, flipHorizontally, flipVertically, flipDiagonally);
                                                    tiles[i] = tile;
                                                }
                                            }
                                        }
                                        cel = new AsepriteTilemapCel(celHeaderData, celLayer, tilemapCelData, tiles);
                                    }
                                    break;

                                default:
                                    reader.Dispose();
                                    throw new InvalidOperationException($"Unknown cel type {celHeaderData.Type}");
                            }

                            currentUserData = cel.UserData;
                            lastReadChunkType = chunkHeaderData.ChunkType;
                            cels.Add(cel);
                        }
                        break;

                    case ASE_CHUNK_TAGS:
                        {
                            ushort tagCount = reader.ReadWord();
                            reader.Ignore(8);

                            for (int i = 0; i < tagCount; i++)
                            {
                                TagData tagData = reader.ReadUnsafe<TagData>(TagData.SizeInBytes);

                                //  Validate loop direction
                                if (!Enum.IsDefined<AsepriteLoopDirection>((AsepriteLoopDirection)tagData.Direction))
                                {
                                    reader.Dispose();
                                    throw new InvalidOperationException($"Unknown loop direction: {tagData.Direction}");
                                }

                                string tagName = reader.ReadString(tagData.NameLen);

                                AsepriteTag tag = new AsepriteTag(tagData, tagName);
                                currentUserData = tag.UserData;
                                lastReadChunkType = chunkHeaderData.ChunkType;
                                tags.Add(tag);
                            }
                        }
                        break;

                    case ASE_CHUNK_PALETTE:
                        {
                            PaletteData paletteData = reader.ReadUnsafe<PaletteData>(PaletteData.SizeInBytes);

                            if (paletteData.NewSize > 0)
                            {
                                palette.Resize((int)paletteData.NewSize);
                            }

                            for (int i = (int)paletteData.FirstIndex; i <= (int)paletteData.LastIndex; i++)
                            {
                                PaletteEntryData entry = reader.ReadUnsafe<PaletteEntryData>(PaletteEntryData.SizeInBytes);
                                if (Calc.HasFlag(entry.Flags, ASE_PALETTE_FLAG_HAS_NAME))
                                {
                                    //  Ignore color name
                                    reader.Ignore(reader.ReadWord());
                                }
                                palette[i] = entry.Color;
                            }

                            paletteRead = true;
                            lastReadChunkType = chunkHeaderData.ChunkType;
                        }
                        break;

                    case ASE_CHUNK_USER_DATA:
                        {
                            uint flags = reader.ReadDword();
                            string? text = null;
                            Rgba32? color = null;

                            if (Calc.HasFlag(flags, ASE_USER_DATA_FLAG_HAS_TEXT))
                            {
                                text = reader.ReadString();
                            }

                            if (Calc.HasFlag(flags, ASE_USER_DATA_FLAG_HAS_COLOR))
                            {
                                color = reader.ReadUnsafe<Rgba32>(Rgba32.StructSize);
                            }

                            if (currentUserData is null && paletteRead)
                            {
                                spriteUserData.Text = text;
                                spriteUserData.Color = color;
                            }
                            else if (currentUserData is not null)
                            {
                                if (lastReadChunkType != ASE_CHUNK_TAGS)
                                {
                                    currentUserData.Text = text;
                                    currentUserData.Color = color;
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
                                    currentUserData = tags[tagIterator++].UserData;
                                    currentUserData.Text = text;
                                    currentUserData.Color = color;

                                    if (tagIterator < tags.Count)
                                    {
                                        currentUserData = tags[tagIterator].UserData;
                                    }
                                    else
                                    {
                                        currentUserData = null;
                                        lastReadChunkType = null;
                                    }
                                }
                            }
                        }
                        break;

                    case ASE_CHUNK_SLICE:
                        {
                            SliceData sliceData = reader.ReadUnsafe<SliceData>(SliceData.SizeInBytes);
                            string sliceName = reader.ReadString(sliceData.NameLen);
                            bool isNinePatch = Calc.HasFlag(sliceData.Flags, ASE_SLICE_FLAGS_IS_NINE_PATCH);
                            bool hasPivot = Calc.HasFlag(sliceData.Flags, ASE_SLICE_FLAGS_HAS_PIVOT);
                            AsepriteSliceKey[] keys = new AsepriteSliceKey[sliceData.KeyCount];
                            for (int i = 0; i < sliceData.KeyCount; i++)
                            {
                                SliceKeyData sliceKeyData = reader.ReadUnsafe<SliceKeyData>(SliceKeyData.SizeInBytes);
                                NinePatchData? ninePatchData = isNinePatch ? reader.ReadUnsafe<NinePatchData>(NinePatchData.SizeInBytes) : null;
                                PivotData? pivotData = hasPivot ? reader.ReadUnsafe<PivotData>(PivotData.SizeInBytes) : null;
                                keys[i] = new AsepriteSliceKey(sliceKeyData, ninePatchData, pivotData);
                            }

                            AsepriteSlice slice = new AsepriteSlice(sliceName, isNinePatch, hasPivot, keys);
                            currentUserData = slice.UserData;
                            lastReadChunkType = chunkHeaderData.ChunkType;
                            slices.Add(slice);
                        }
                        break;

                    case ASE_CHUNK_TILESET:
                        {
                            TilesetData tilesetData = reader.ReadUnsafe<TilesetData>(TilesetData.SizeInBytes);
                            string tilesetName = reader.ReadString(tilesetData.NameLen);

                            if (Calc.HasFlag(tilesetData.Flags, ASE_TILESET_FLAG_EXTERNAL_FILE))
                            {
                                //  No support for external files at this time. To my knowledge, Aseprite doesn't
                                //  support this directly in the UI and is only something that can be added through the
                                //  LUA scripting extensions.  So unless someone opens an issue and needs this, not
                                //  implementing it.
                                throw new InvalidOperationException($"Tileset '{tilesetName}' includes the tileset in an external file. This is not supported at this time.");
                            }

                            if (Calc.DoesNotHaveFlag(tilesetData.Flags, ASE_TILESET_FLAG_EMBEDDED))
                            {
                                //  Only support at this time for tileset data that is embedded in the file.
                                throw new InvalidOperationException($"Tileset '{tilesetName}' does not include tileset image embedded in file.");
                            }

                            uint len = reader.ReadDword();
                            byte[] pixelData = reader.ReadCompressed((int)len);
                            Rgba32[] pixels = AsepriteColorUtilities.PixelsToColor(pixelData, depth, preMultiplyAlpha, palette);
                            AsepriteTileset tileset = new AsepriteTileset(tilesetData, tilesetName, pixels);
                            tilesets.Add(tileset);
                            lastReadChunkType = chunkHeaderData.ChunkType;
                        }
                        break;

                    case ASE_CHUNK_OLD_PALETTE1:
                    case ASE_CHUNK_OLD_PALETTE2:
                        {
                            if (paletteRead)
                            {
                                break;
                            }

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

                                palette.Resize(size);

                                for (int c = skip; c < skip + size; c++)
                                {
                                    byte r = reader.ReadByte();
                                    byte g = reader.ReadByte();
                                    byte b = reader.ReadByte();
                                    byte a = byte.MaxValue;

                                    if(chunkHeaderData.ChunkType ==  ASE_CHUNK_OLD_PALETTE2)
                                    {
                                        //  Old palette type 2 uses six bit values (0-63) that must be expanded to
                                        //  eight bit values.
                                        r = (byte)((r << 2) | (r >> 4));
                                        g = (byte)((g << 2) | (g >> 4));
                                        b = (byte)((b << 2) | (b >> 4));
                                    }
                                    palette[c] = new Rgba32(r, g, b, a);
                                }
                            }

                            paletteRead = true;
                            lastReadChunkType = chunkHeaderData.ChunkType;
                        }
                        break;

                    case ASE_CHUNK_CEL_EXTRA:
                        warnings.Add($"Cel Extra Chunk 0x{chunkHeaderData.ChunkType:X4} ignored.");
                        lastReadChunkType = chunkHeaderData.ChunkType;
                        break;

                    case ASE_CHUNK_COLOR_PROFILE:
                        warnings.Add($"Color Profile Chunk 0x{chunkHeaderData.ChunkType:X4} ignored.");
                        lastReadChunkType = chunkHeaderData.ChunkType;
                        break;

                    case ASE_CHUNK_EXTERNAL_FILES:
                        warnings.Add($"External Files Chunk 0x{chunkHeaderData.ChunkType:X4} ignored.");
                        lastReadChunkType = chunkHeaderData.ChunkType;
                        break;

                    case ASE_CHUNK_MASK:
                        warnings.Add($"Mask Chunk 0x{chunkHeaderData.ChunkType:X4} ignored.");
                        lastReadChunkType = chunkHeaderData.ChunkType;
                        break;

                    case ASE_CHUNK_PATH:
                        warnings.Add($"Path Chunk 0x{chunkHeaderData.ChunkType:X4} ignored.");
                        lastReadChunkType = chunkHeaderData.ChunkType;
                        break;

                    default:
                        warnings.Add($"Unknown chunk type 0x{chunkHeaderData.ChunkType:X4} encountered.  Ignored");
                        lastReadChunkType = chunkHeaderData.ChunkType;
                        break;
                }
                reader.Seek(chunkEnd, SeekOrigin.Begin);
            }

            AsepriteFrame frame = new AsepriteFrame($"{fileName}{frameNum}", fileHeaderData.CanvasWidth, fileHeaderData.CanvasHeight, frameHeaderData.Duration, cels);
            frames.Add(frame);
        }

        if (palette.Colors.Length != fileHeaderData.NumberOfColors)
        {
            warnings.Add($"Number of colors in file header ({fileHeaderData.NumberOfColors}) does not match the final palette count ({palette.Colors.Length})");
        }

        return new AsepriteFile(fileName, palette, fileHeaderData.CanvasWidth, fileHeaderData.CanvasHeight, depth, frames, layers, tags, slices, tilesets, spriteUserData, warnings);
    }
}
