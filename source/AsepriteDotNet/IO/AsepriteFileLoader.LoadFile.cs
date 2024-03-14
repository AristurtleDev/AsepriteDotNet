//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Document;

namespace AsepriteDotNet.IO;

public static unsafe partial class AsepriteFileLoader
{
    public static AsepriteFile FromFile(string fileName)
    {
        using (AsepriteBinaryReader reader = new AsepriteBinaryReader(File.OpenRead(fileName), false))
        {
            return LoadFile(Path.GetFileNameWithoutExtension(fileName), reader);
        }
    }

    public static AsepriteFile FromStream(string fileName, Stream stream, bool leaveOpen = false)
    {
        using (AsepriteBinaryReader reader = new AsepriteBinaryReader(stream, leaveOpen))
        {
            return LoadFile(fileName, reader);
        }
    }

    internal static AsepriteFile LoadFile(string fileName, AsepriteBinaryReader reader)
    {
        //  Collection of non-fatal warnings accumulated while loading the Aseprite file. Provided to the consumer as a
        //  means of seeing why some data may not be what they expect it to be.
        List<string> warnings = new List<string>();

        //  Reference to the last group layer that was read so that subsequent child layers can be added to it.
        GroupLayer? lastGroupLayer = null;

        //  Flag to determine if the palette has been read.  This is used to flag that a user data chunk is for the
        //  sprite due to changes in Aseprite 1.3
        bool paletteRead = false;

        //  Read the file header
        FileHeader fileHeader = reader.ReadUnsafe<FileHeader>(FileHeader.StructSize);

        //  Validate the file header magic number
        if (fileHeader.MagicNumber != ASE_HEADER_MAGIC)
        {
            reader.Dispose();
            throw new InvalidOperationException($"invalid file header magic number: 0x{fileHeader.MagicNumber:X4}.  This does not appear to be a valid Aseprite file.");
        }

        //  Validate canvas size
        if (fileHeader.CanvasWidth < 1 || fileHeader.CanvasHeight < 1)
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid canvas size: {fileHeader.CanvasWidth}x{fileHeader.CanvasHeight}");
        }

        AsepriteColorDepth depth = (AsepriteColorDepth)fileHeader.Depth;

        if (!Enum.IsDefined(typeof(AsepriteColorDepth), depth))
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid color depth mode: {fileHeader.Depth}");
        }

        bool isLayerOpacityValid = fileHeader.Flags.HasFlag(ASE_HEADER_FLAG_LAYER_OPACITY_VALID);
        if (!isLayerOpacityValid)
        {
            warnings.Add("Layer opacity valid flag is not set.  All layer opacity will default to 255");
        }

        if (fileHeader.TransparentIndex > 0 && depth != AsepriteColorDepth.Indexed)
        {
            fileHeader.TransparentIndex = 0;
            warnings.Add("Transparent index only valid for Indexed Color Depth mode.  Defaulting to 0");
        }

        Palette palette = new Palette(fileHeader.TransparentIndex);
        List<Frame> frames = new List<Frame>();
        List<Layer> layers = new List<Layer>();
        List<Tag> tags = new List<Tag>();
        List<Slice> slices = new List<Slice>();
        List<Tileset> tilesets = new List<Tileset>();
        UserData spriteUserData = new UserData();

        //  Read frame-by-frame until all frames are read.
        for (int frameNum = 0; frameNum < fileHeader.FrameCount; frameNum++)
        {
            List<Cel> cels = new List<Cel>();

            uint? lastReadChunkType = null;

            //  Reference to the user data object to apply user data to from the last chunk that was read that
            //  could have had user data
            UserData? currentUserData = null;

            //  Tracks the iteration of the tags when reading user data for tags chunk.
            int tagIterator = 0;

            FrameHeader frameHeader = reader.ReadUnsafe<FrameHeader>(FrameHeader.StructSize);

            //  Validate the magic number in frame header
            if (frameHeader.MagicNumber != ASE_FRAME_MAGIC)
            {
                throw new InvalidOperationException($"Frame {frameNum} contains an invalid magic number: 0x{frameHeader.MagicNumber:X4}");
            }

            //  Determine the number of chunks to read
            int chunkCount = frameHeader.OldChunkCount;
            if (chunkCount == 0xFFFF && chunkCount < frameHeader.NewChunkCount)
            {
                chunkCount = (int)frameHeader.NewChunkCount;
            }

            //  Read chunk-by-chunk until all chunks are read.
            for (int chunkNum = 0; chunkNum < chunkCount; chunkNum++)
            {
                long chunkStart = reader.Position;
                ChunkHeader chunkHeader = reader.ReadUnsafe<ChunkHeader>(ChunkHeader.StructSize);
                long chunkEnd = chunkStart + chunkHeader.ChunkSize;

                switch (chunkHeader.ChunkType)
                {
                    case ASE_CHUNK_LAYER:
                        {
                            LayerProperties properties = reader.ReadUnsafe<LayerProperties>(LayerProperties.StructSize);
                            string layerName = reader.ReadString(properties.NameLen);
                            Layer layer;
                            switch (properties.Type)
                            {
                                case ASE_LAYER_TYPE_NORMAL:
                                    layer = new ImageLayer(properties, fileName);
                                    break;
                                case ASE_LAYER_TYPE_GROUP:
                                    layer = new GroupLayer(properties, fileName);
                                    break;
                                case ASE_LAYER_TYPE_TILEMAP:
                                    uint tilesetIndex = reader.ReadDword();
                                    Tileset tileset = tilesets[(int)tilesetIndex];
                                    layer = new TilemapLayer(properties, fileName, tileset);
                                    break;
                                default:
                                    reader.Dispose();
                                    throw new InvalidOperationException($"Unknown layer type: {properties.Type}");
                            }

                            if (properties.Level != 0 && lastGroupLayer is not null)
                            {
                                lastGroupLayer.AddChild(layer);
                            }

                            if (layer is GroupLayer groupLayer)
                            {
                                lastGroupLayer = groupLayer;
                            }

                            currentUserData = layer.UserData;
                            layers.Add(layer);
                        }
                        break;

                    case ASE_CHUNK_CEL:
                        {
                            CelProperties properties = reader.ReadUnsafe<CelProperties>(CelProperties.StructSize);
                            Cel cel;
                            Layer celLayer = layers[properties.LayerIndex];

                            switch (properties.Type)
                            {
                                case ASE_CEL_TYPE_RAW_IMAGE:
                                case ASE_CEL_TYPE_COMPRESSED_IMAGE:
                                    {
                                        ImageCelProperties imageCelProperties = reader.ReadUnsafe<ImageCelProperties>(ImageCelProperties.StructSize);
                                        int len = (int)(chunkEnd - reader.Position);
                                        byte[] data = properties.Type == ASE_CEL_TYPE_COMPRESSED_IMAGE ? reader.ReadCompressed(len) : reader.ReadBytes(len);
                                        AseColor[] pixels = PixelsToColor(data, depth, palette);
                                        cel = new ImageCel(properties, celLayer, imageCelProperties, pixels);
                                    }
                                    break;

                                case ASE_CEL_TYPE_LINKED:
                                    {
                                        ushort frameIndex = reader.ReadWord();
                                        Cel otherCel = frames[frameIndex].Cels[cels.Count];
                                        cel = new LinkedCel(properties, otherCel);
                                    }
                                    break;

                                case ASE_CEL_TYPE_COMPRESSED_TILEMAP:
                                    {
                                        TilemapCelProperties tilemapCelProperties = reader.ReadUnsafe<TilemapCelProperties>(TilemapCelProperties.StructSize);
                                        int len = (int)(chunkEnd - reader.Position);
                                        byte[] data = reader.ReadCompressed(len);

                                        //  Per Aseprite file spec, the "bits" per tile is, at the moment, always
                                        //  32-bits.  This means it's 4-bytes per tile (32 / 8 = 4).  Meaning that each
                                        //  tile value is a uint (DWORD)
                                        int bytesPerTile = sizeof(uint);
                                        Tile[] tiles = new Tile[data.Length / bytesPerTile];

                                        unsafe
                                        {
                                            fixed (byte* tileDataPtr = data)
                                            {
                                                for (int i = 0; i < tiles.Length; i++)
                                                {
                                                    uint value = *(uint*)(tileDataPtr + i * bytesPerTile);
                                                    uint id = (value & tilemapCelProperties.TileIdBitmask) >> TILE_ID_SHIFT;
                                                    bool flipHorizontally = HasFlag(value, tilemapCelProperties.HorizontalFlipBitmask);
                                                    bool flipVertically = HasFlag(value, tilemapCelProperties.VerticalFlipBitmask);
                                                    bool flipDiagonally = HasFlag(value, tilemapCelProperties.DiagonalFlipBitmask);

                                                    Tile tile = new Tile((int)id, flipHorizontally, flipVertically, flipDiagonally);
                                                    tiles[i] = tile;
                                                }
                                            }
                                        }
                                        cel = new TilemapCel(properties, celLayer, tilemapCelProperties, tiles);
                                    }
                                    break;

                                default:
                                    reader.Dispose();
                                    throw new InvalidOperationException($"Unknown cel type {properties.Type}");
                            }

                            currentUserData = cel.UserData;
                            cels.Add(cel);
                        }
                        break;

                    case ASE_CHUNK_TAGS:
                        {
                            ushort tagCount = reader.ReadWord();
                            reader.Ignore(8);

                            for (int i = 0; i < tagCount; i++)
                            {
                                TagProperties properties = reader.ReadUnsafe<TagProperties>(TagProperties.StructSize);

                                //  Validate loop direction
                                if (!Enum.IsDefined<AsepriteLoopDirection>((AsepriteLoopDirection)properties.Direction))
                                {
                                    reader.Dispose();
                                    throw new InvalidOperationException($"Unknown loop direction: {properties.Direction}");
                                }

                                string tagName = reader.ReadString(properties.NameLen);
                                AseColor tagColor = new AseColor(properties.RGB[0], properties.RGB[1], properties.RGB[2]);
                                AsepriteLoopDirection loopDirection = (AsepriteLoopDirection)properties.Direction;

                                Tag tag = new Tag(properties, tagName);
                            }
                        }
                        break;

                    case ASE_CHUNK_PALETTE:
                        {
                            PaletteProperties properties = reader.ReadUnsafe<PaletteProperties>(PaletteProperties.StructSize);

                            if (properties.NewSize > 0)
                            {
                                palette.Resize((int)properties.NewSize);
                            }

                            for (int i = (int)properties.FirstIndex; i <= (int)properties.LastIndex; i++)
                            {
                                PaletteEntry entry = reader.ReadUnsafe<PaletteEntry>(PaletteEntry.StructSize);
                                if (HasFlag(entry.Flags, ASE_PALETTE_FLAG_HAS_NAME))
                                {
                                    //  Ignore color name
                                    reader.Ignore(reader.ReadWord());
                                }
                                palette[i] = entry.Color;
                            }

                            paletteRead = true;
                        }
                        break;

                    case ASE_CHUNK_USER_DATA:
                        {
                            uint flags = reader.ReadDword();
                            string? text = null;
                            AseColor? color = null;

                            if (HasFlag(flags, ASE_USER_DATA_FLAG_HAS_TEXT))
                            {
                                text = reader.ReadString();
                            }

                            if (HasFlag(flags, ASE_USER_DATA_FLAG_HAS_COLOR))
                            {
                                color = reader.ReadUnsafe<AseColor>(AseColor.StructSize);
                            }

                            if (currentUserData is null && paletteRead)
                            {
                                spriteUserData.Text = text;
                                spriteUserData.Color = color;
                            }
                            else if (currentUserData is not null)
                            {
                                currentUserData.Text = text;
                                currentUserData.Color = color;

                                if (lastReadChunkType == ASE_CHUNK_TAGS)
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
                                    tagIterator++;

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
                            SliceProperties properties = reader.ReadUnsafe<SliceProperties>(SliceProperties.StructSize);
                            string sliceName = reader.ReadString(properties.NameLen);
                            bool isNinePatch = HasFlag(properties.Flags, ASE_SLICE_FLAGS_IS_NINE_PATCH);
                            bool hasPivot = HasFlag(properties.Flags, ASE_SLICE_FLAGS_HAS_PIVOT);
                            SliceKey[] keys = new SliceKey[properties.KeyCount];
                            for (int i = 0; i < properties.KeyCount; i++)
                            {
                                SliceKeyProperties sliceKeyProperties = reader.ReadUnsafe<SliceKeyProperties>(SliceKeyProperties.StructSize);
                                NinePatchProperties? ninePatchProperties = isNinePatch ? reader.ReadUnsafe<NinePatchProperties>(NinePatchProperties.StructSize) : null;
                                PivotProperties? pivotProperties = hasPivot ? reader.ReadUnsafe<PivotProperties>(PivotProperties.StructSize) : null;
                                keys[i] = new SliceKey(sliceKeyProperties, ninePatchProperties, pivotProperties);
                            }

                            Slice slice = new Slice(sliceName, isNinePatch, hasPivot, keys);
                            slices.Add(slice);
                            currentUserData = slice.UserData;
                        }
                        break;

                    case ASE_CHUNK_TILESET:
                        {
                            TilesetProperties properties = reader.ReadUnsafe<TilesetProperties>(TilesetProperties.StructSize);
                            string tilesetName = reader.ReadString(properties.NameLen);

                            if (HasFlag(properties.Flags, ASE_TILESET_FLAG_EXTERNAL_FILE))
                            {
                                //  No support for external files at this time. To my knowledge, Aseprite doesn't
                                //  support this directly in the UI and is only something that can be added through the
                                //  LUA scripting extensions.  So unless someone opens an issue and needs this, not
                                //  implementing it.
                                throw new InvalidOperationException($"Tileset '{tilesetName}' includes the tileset in an external file. This is not supported at this time.");
                            }

                            if (DoesNotHaveFlag(properties.Flags, ASE_TILESET_FLAG_EMBEDDED))
                            {
                                //  Only support at this time for tileset data that is embedded in the file.
                                throw new InvalidOperationException($"Tileset '{tilesetName}' does not include tileset image embedded in file.");
                            }

                            uint len = reader.ReadDword();
                            byte[] pixelData = reader.ReadCompressed((int)len);
                            AseColor[] pixels = PixelsToColor(pixelData, depth, palette);
                            Tileset tileset = new Tileset(properties, tilesetName, pixels);
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
                            Span<byte> rgb = stackalloc byte[4];

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
                                    rgb.Clear();
                                    rgb[0] = reader.ReadByte();
                                    rgb[1] = reader.ReadByte();
                                    rgb[2] = reader.ReadByte();
                                    rgb[3] = byte.MaxValue;

                                    if(chunkHeader.ChunkType ==  ASE_CHUNK_OLD_PALETTE2)
                                    {
                                        //  Old palette type 2 uses six bit values (0-63) that must be expanded to
                                        //  eight bit values.
                                        rgb[0] = (byte)((rgb[0] << 2) | (rgb[0] >> 4));
                                        rgb[1] = (byte)((rgb[1] << 2) | (rgb[1] >> 4));
                                        rgb[2] = (byte)((rgb[2] << 2) | (rgb[2] >> 4));
                                    }
                                    palette[c] = new AseColor(rgb);
                                }
                            }

                            paletteRead = true;
                        }
                        break;

                    case ASE_CHUNK_CEL_EXTRA:
                        warnings.Add($"Cel Extra Chunk 0x{chunkHeader.ChunkType:X4} ignored.");
                        break;

                    case ASE_CHUNK_COLOR_PROFILE:
                        warnings.Add($"Color Profile Chunk 0x{chunkHeader.ChunkType:X4} ignored.");
                        break;

                    case ASE_CHUNK_EXTERNAL_FILES:
                        warnings.Add($"External Files Chunk 0x{chunkHeader.ChunkType:X4} ignored.");
                        break;

                    case ASE_CHUNK_MASK:
                        warnings.Add($"Mask Chunk 0x{chunkHeader.ChunkType:X4} ignored.");
                        break;

                    case ASE_CHUNK_PATH:
                        warnings.Add($"Path Chunk 0x{chunkHeader.ChunkType:X4} ignored.");
                        break;

                    default:
                        warnings.Add($"Unknown chunk type 0x{chunkHeader.ChunkType:X4} encountered.  Ignored");
                        break;
                }
                reader.Seek(chunkEnd, SeekOrigin.Begin);
            }

            Frame frame = new Frame($"{fileName}{frameNum}", fileHeader.CanvasWidth, fileHeader.CanvasHeight, frameHeader.Duration, cels);
            frames.Add(frame);
        }

        if (palette.Colors.Length != fileHeader.NumberOfColors)
        {
            warnings.Add($"Number of colors in file header ({fileHeader.NumberOfColors}) does not match the final palette count ({palette.Colors.Length})");
        }

        return new AsepriteFile(fileName, palette, fileHeader.CanvasWidth, fileHeader.CanvasHeight, depth, frames, layers, tags, slices, spriteUserData, warnings);
    }
}
