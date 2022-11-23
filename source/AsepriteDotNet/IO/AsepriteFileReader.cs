/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
using AsepriteDotNet.Document;
using AsepriteDotNet.Document.Native;
using AsepriteDotNet.IO.Compression;

namespace AsepriteDotNet.IO;

public static class AsepriteFileReader
{
    public static void ReadFile(string path)
    {
        using AsepriteBinaryReader reader = new(File.OpenRead(path));

        _ = reader.ReadDword(); //  Filesize, don't need
        ushort hmagic = reader.ReadWord();

        //  Validate header magic number
        if (hmagic != 0xA5E0)
        {
            throw new InvalidOperationException();
        }

        int nframes = reader.ReadWord();
        int width = reader.ReadWord();
        int height = reader.ReadWord();

        //  Validate width and height
        if (width == 0 || height == 0)
        {
            throw new InvalidOperationException();
        }

        int bpp = reader.ReadWord();

        //  Validate color depth
        if (bpp != 8 || bpp != 16 || bpp != 32)
        {
            throw new InvalidOperationException();
        }

        ColorDepth depth = (ColorDepth)bpp;

        uint hflags = reader.ReadDword();
        bool layerOpacityValid = (hflags & 1) != 0;

        _ = reader.ReadWord();  //  Speed, deprected, don't need it
        _ = reader.ReadDword(); //  Set to 0, can ignore
        _ = reader.ReadDword(); //  Set to 0, can ignore
        int transparentIndex = reader.ReadByte();

        //  Transparent index only valid if color depth is indexed
        if (depth != ColorDepth.Indexed)
        {
            transparentIndex = 0;
        }

        _ = reader.ReadBytes(3);    //  Ignore 3-byte array

        int ncolors = reader.ReadWord();

        //  Remainder of header is not needed, skip to end of header
        reader.Seek(128);

        IList<Layer> layers = new List<Layer>();

        for (int fnum = 0; fnum < nframes; fnum++)
        {
            IList<Cel> cels = new List<Cel>();
            Cel? lastCel = default;

            _ = reader.ReadDword(); //  frame size, don't need
            ushort fmagic = reader.ReadWord();

            //  Validate frame magic
            if (fmagic != 0xF1FA)
            {
                throw new InvalidOperationException();
            }

            int nchunks = reader.ReadWord();
            int duration = reader.ReadWord();
            _ = reader.ReadBytes(2);    //  Ignore 2-byte array

            int morechunks = (int)reader.ReadDword();

            //  Determine the actual number of chunks to read
            if(nchunks == 0xFFFF && nchunks < morechunks)
            {
                nchunks = morechunks;
            }

            for (int chunkNum = 0; chunkNum < nchunks; chunkNum++)
            {
                long chunkStart = reader.Position;
                uint chunkLen = reader.ReadDword();
                long chunkEnd = chunkStart + chunkLen;
                ChunkType chunkType = (ChunkType)reader.ReadWord();

                if(chunkType == ChunkType.OldPaletteChunkA)
                {
                    throw new NotSupportedException();
                }
                else if(chunkType == ChunkType.OldPaletteChunkB)
                {
                    throw new NotSupportedException();
                }
                else if (chunkType == ChunkType.LayerChunk)
                {
                    ushort lflags = reader.ReadWord();
                    
                    //  only care about the IsVisible flag
                    bool visible = (lflags & 1) != 0;

                    ushort ltype = reader.ReadWord();
                    ushort level = reader.ReadWord();

                    _ = reader.ReadWord();  //  default width, ignored
                    _ = reader.ReadWord();  //  default height, ignored
                    BlendMode blend = (BlendMode)reader.ReadWord();
                    int lopacity = reader.ReadByte();

                    //  Is layer opacity valid?
                    if(!layerOpacityValid)
                    {
                        lopacity = 255;
                    }

                    _ = reader.ReadBytes(3);    //  3-byte array, ignored
                    string lname = reader.ReadString();

                    Layer layer;
                    if(ltype == 0 || ltype == 1)
                    {
                        layer = new Layer(visible, level, blend, lopacity, lname);
                    }
                    else if(ltype == 2)
                    {
                        int tilesetIndex = (int)reader.ReadDword();
                        layer = new TilesetLayer(visible, level, blend, lopacity, lname, tilesetIndex);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    layers.Add(layer);
                }
                else if(chunkType == ChunkType.CelChunk)
                {
                    int lindex = reader.ReadWord();
                    int celx = reader.ReadShort();
                    int cely = reader.ReadShort();
                    int celOpacity = reader.ReadByte();
                    int ctype = reader.ReadWord();
                    _ = reader.ReadBytes(7);    //  7-byte array, ignore

                    Cel cel;

                    if(ctype == 1)
                    {
                        //  linked cel
                        int frameLink = reader.ReadWord();
                        cel = new LinkedCel(lindex, celx, cely, celOpacity, frameLink);
                    }
                    else if(ctype == 0 || ctype == 2 || ctype == 3)
                    {
                        int cWidth = reader.ReadWord();
                        int cHeight = reader.ReadWord();

                        if(ctype == 0)
                        {
                            //  Raw image cel
                            byte[] pixels = reader.ReadBytes((int)(chunkEnd - reader.Position));
                            cel = new ImageCel(lindex, celx, cely, celOpacity, width, height, pixels);
                        }
                        else if(ctype == 2)
                        {
                            //  Compressed image cel
                            byte[] compressed = reader.ReadBytes((int)(chunkEnd - reader.Position));
                            byte[] pixels = Zlib.Deflate(compressed);
                            cel = new ImageCel(lindex, celx, cely, celOpacity, width, height, pixels);
                        }
                        else
                        {
                            //  Tilemap cell
                            int bpt = reader.ReadWord();
                            int tileIdBitmask = (int)reader.ReadDword();
                            int xFlipBitmask = (int)reader.ReadDword();
                            int yFlipBitmask = (int)reader.ReadDword();
                            int rotationBitmask = (int)reader.ReadDword();
                            _ = reader.ReadBytes(10);   //  10-byte array, ignore
                            byte[] compressed = reader.ReadBytes((int)(chunkEnd - reader.Position));
                            byte[] tiles = Zlib.Deflate(compressed);
                            cel = new TilemapCel(lindex, celx, cely, celOpacity, width, height, bpt, tileIdBitmask, xFlipBitmask, yFlipBitmask, rotationBitmask, tiles);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }

                    cels.Add(cel);
                    lastCel = cel;
                }
            }

        }


        RawHeader rawHeader = ReadRawHeader(reader);
        Header header = new(rawHeader);

        IList<Frame> frames = new List<Frame>(header.Frames);
        IList<Layer> layers = new List<Layer>();
        ColorProfile? colorProfile = default;
        AseExternalFilesChunk? externalFiles = default;

        for (int frameNum = 0; frameNum < header.Frames; frameNum++)
        {
            RawFrameHeader rawFrameHeader = ReadRawFrameHeader(reader);
            Frame frame = new(rawFrameHeader);

            //  If the old chunk count is 0xFFFF there may be more chunks to
            //  read, so we would use the new chunk count.
            int nchunks = rawFrameHeader.OldCount;
            if (rawFrameHeader.OldCount == 0xFFFF && rawFrameHeader.OldCount < rawFrameHeader.NewCount)
            {
                nchunks = (int)rawFrameHeader.NewCount;
            }

            IList<Cel> cels = new List<Cel>();
            Cel? lastCel = default;

            for (int chunkNum = 0; chunkNum < nchunks; chunkNum++)
            {
                long chunkStart = reader.Position;

                RawChunkHeader chunkHeader = ReadRawChunkHeader(reader);

                long chunkEnd = chunkStart + chunkHeader.Length;

                ChunkType chunkType = (ChunkType)chunkHeader.Type;

                if (chunkType == ChunkType.LayerChunk)
                {
                    Layer chunk = ReadLayerChunk(reader);
                    layers.Add(chunk);
                }
                else if (chunkType == ChunkType.CelChunk)
                {
                    Cel chunk = ReadCelChunk(reader, chunkEnd);
                    cels.Add(chunk);
                    lastCel = chunk;
                }
                else if (chunkType == ChunkType.CelExtraChunk)
                {
                    CelExtra chunk = ReadCelExtraChunk(reader);
                    lastCel?.SetCelExtra(chunk);
                }
                else if (chunkType == ChunkType.ColorProfileChunk)
                {
                    colorProfile = ReadColorProfileChunk(reader);
                }
                else if (chunkType == ChunkType.ExternalFilesChunk)
                {
                    externalFiles = ReadExternalFilesChunk(reader);
                }



                // AseChunk chunk = chunkType switch
                // {
                //     AseChunkType.OldPaletteChunkA => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
                //     AseChunkType.OldPaletteChunkB => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
                //     AseChunkType.LayerChunk => ReadLayerChunk(reader),
                //     AseChunkType.CelChunk => ReadCelChunk(reader, end),
                //     AseChunkType.CelExtraChunk => ReadCelExtraChunk(reader),
                //     AseChunkType.ColorProfileChunk => ReadColorProfileChunk(reader),
                //     AseChunkType.ExternalFilesChunk => ReadExternalFilesChunk(reader),
                //     AseChunkType.MaskChunk => throw new NotSupportedException($"Mask Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
                //     AseChunkType.PathChunk => throw new NotSupportedException($"Path Chunk (0x{rawHeader.Type:X2}) detected.  The version of Aseprite used to create this file is not supported"),
                //     AseChunkType.TagsChunk => ReadTagsChunk(reader),
                //     AseChunkType.PaletteChunk => ReadPaletteChunk(reader),
                //     _ => throw new InvalidOperationException($"Unknown chunk type (0x{rawHeader.Type:X4})")
                // };
            }

        }
    }


    internal static RawHeader ReadRawHeader(AsepriteBinaryReader reader)
    {
        RawHeader raw;

        raw.FileSize = reader.ReadDword();
        raw.MagicNumber = reader.ReadWord();
        raw.Frames = reader.ReadWord();
        raw.Width = reader.ReadWord();
        raw.Height = reader.ReadWord();
        raw.ColorDepth = reader.ReadWord();
        raw.Flags = reader.ReadDword();
        raw.Speed = reader.ReadWord();
        raw.Ignore1 = reader.ReadDword();
        raw.Ignore2 = reader.ReadDword();
        raw.TransparentIndex = reader.ReadByte();
        raw.Ignore3 = reader.ReadBytes(3);
        raw.NumberOfColors = reader.ReadWord();
        raw.PixelWidth = reader.ReadByte();
        raw.PixelHeight = reader.ReadByte();
        raw.GridX = reader.ReadShort();
        raw.GridY = reader.ReadShort();
        raw.GridWidth = reader.ReadWord();
        raw.GridHeight = reader.ReadWord();
        raw.Ignore4 = reader.ReadBytes(84);

        return raw;
    }

    internal static RawFrameHeader ReadRawFrameHeader(AsepriteBinaryReader reader)
    {
        RawFrameHeader raw;
        raw.Length = reader.ReadDword();
        raw.Magic = reader.ReadWord();
        raw.OldCount = reader.ReadWord();
        raw.Duration = reader.ReadWord();
        raw.Ignore1 = reader.ReadBytes(2);
        raw.NewCount = reader.ReadDword();

        return raw;
    }

    internal static Chunk ReadChunk(AsepriteBinaryReader reader)
    {
        long start = reader.Position;

        RawChunkHeader rawHeader = ReadRawChunkHeader(reader);

        //  Chunk end position needs to be calcualted for eading some chunks
        //  like the cel chunk.
        long end = start + rawHeader.Length;

        ChunkType chunkType = (ChunkType)rawHeader.Type;

        return chunkType switch
        {
            ChunkType.OldPaletteChunkA => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
            ChunkType.OldPaletteChunkB => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
            ChunkType.LayerChunk => ReadLayerChunk(reader, rawHeader),
            ChunkType.CelChunk => ReadCelChunk(reader, rawHeader, end),
            ChunkType.CelExtraChunk => ReadCelExtraChunk(reader, rawHeader),
            ChunkType.ColorProfileChunk => ReadColorProfileChunk(reader, rawHeader),
            ChunkType.ExternalFilesChunk => ReadExternalFilesChunk(reader, rawHeader),
            ChunkType.MaskChunk => throw new NotSupportedException($"Mask Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
            ChunkType.PathChunk => throw new NotSupportedException($"Path Chunk (0x{rawHeader.Type:X2}) detected.  The version of Aseprite used to create this file is not supported"),
            ChunkType.TagsChunk => ReadTagsChunk(reader, rawHeader),
            ChunkType.PaletteChunk => ReadPaletteChunk(reader, rawHeader),
            _ => throw new InvalidOperationException($"Unknown chunk type (0x{rawHeader.Type:X4})")
        };

    }

    internal static RawChunkHeader ReadRawChunkHeader(AsepriteBinaryReader reader)
    {
        RawChunkHeader raw;
        raw.Length = reader.ReadDword();
        raw.Type = reader.ReadWord();

        return raw;
    }

    private static Layer ReadLayerChunk(AsepriteBinaryReader reader)
    {
        RawLayerChunk raw = ReadRawLayerChunk(reader);

        return raw.Type switch
        {
            0 or 1 => new Layer(raw),
            2 => new TilesetLayer(raw),
            _ => throw new InvalidOperationException($"Unknown Layer Type '{raw.Type}'")
        };
    }

    private static RawLayerChunk ReadRawLayerChunk(AsepriteBinaryReader reader)
    {
        RawLayerChunk raw;
        raw.Flags = reader.ReadWord();
        raw.Type = reader.ReadWord();
        raw.ChildLevel = reader.ReadWord();
        raw.DefaultWidth = reader.ReadWord();
        raw.DefaultHeight = reader.ReadWord();
        raw.BlendMode = reader.ReadWord();
        raw.Opacity = reader.ReadByte();
        raw.Ignore = reader.ReadBytes(3);
        raw.Name = reader.ReadString();
        raw.TilsetIndex = default;

        if (raw.Type == 2)
        {
            raw.TilsetIndex = reader.ReadDword();
        }

        return raw;
    }

    private static Cel ReadCelChunk(AsepriteBinaryReader reader, long end)
    {
        RawCelChunk raw = ReadRawCelChunk(reader, end);

        return raw.Type switch
        {
            0 or 2 => new ImageCel(raw),
            1 => new LinkedCel(raw),
            3 => new TilemapCel(raw),
            _ => throw new InvalidOperationException($"Unknown Cel Type '{raw.Type}'")
        };
    }

    private static RawCelChunk ReadRawCelChunk(AsepriteBinaryReader reader, long end)
    {
        RawCelChunk raw;
        raw.LayerIndex = reader.ReadWord();
        raw.X = reader.ReadShort();
        raw.Y = reader.ReadShort();
        raw.Opacity = reader.ReadByte();
        raw.Type = reader.ReadWord();
        raw.Ignroe = reader.ReadBytes(7);

        raw.Width = default;
        raw.Height = default;
        raw.Pixels = default;
        raw.FramePosition = default;
        raw.CompressedPixels = default;
        raw.BitsPerTile = default;
        raw.TileIdBitmask = default;
        raw.XFlipBitmask = default;
        raw.YFlipBitmask = default;
        raw.RotationBitmask = default;
        raw.Ignore2 = default;
        raw.CompressedTiles = default;

        if (raw.Type == 0)
        {
            raw.Width = reader.ReadWord();
            raw.Height = reader.ReadWord();
            raw.Pixels = reader.ReadBytes((int)(end - reader.Position));
        }
        else if (raw.Type == 1)
        {
            raw.FramePosition = reader.ReadWord();
        }
        else if (raw.Type == 2)
        {
            raw.Width = reader.ReadWord();
            raw.Height = reader.ReadWord();
            raw.CompressedPixels = reader.ReadBytes((int)(end - reader.Position));
        }
        else if (raw.Type == 3)
        {
            raw.Width = reader.ReadWord();
            raw.Height = reader.ReadWord();
            raw.BitsPerTile = reader.ReadWord();
            raw.TileIdBitmask = reader.ReadDword();
            raw.XFlipBitmask = reader.ReadDword();
            raw.YFlipBitmask = reader.ReadDword();
            raw.RotationBitmask = reader.ReadDword();
            raw.Ignore2 = reader.ReadBytes(10);
            raw.CompressedTiles = reader.ReadBytes((int)(end - reader.Position));
        }

        return raw;
    }

    private static CelExtra ReadCelExtraChunk(AsepriteBinaryReader reader)
    {
        RawCelExtraChunk raw = ReadRawCelExtraChunk(reader);
        return new CelExtra(raw);
    }

    private static RawCelExtraChunk ReadRawCelExtraChunk(AsepriteBinaryReader reader)
    {
        RawCelExtraChunk raw;
        raw.Flags = reader.ReadDword();
        raw.PreciseX = reader.ReadFixed();
        raw.PreciseY = reader.ReadFixed();
        raw.PreciseWidth = reader.ReadFixed();
        raw.PreciseHeight = reader.ReadFixed();
        raw.Ignore = reader.ReadBytes(16);

        return raw;
    }

    private static ColorProfile ReadColorProfileChunk(AsepriteBinaryReader reader)
    {
        RawColorProfileChunk raw = ReadRawColorProfileChunk(reader);
        return new ColorProfile(raw);
    }

    private static RawColorProfileChunk ReadRawColorProfileChunk(AsepriteBinaryReader reader)
    {
        RawColorProfileChunk raw;
        raw.Type = reader.ReadWord();
        raw.Flags = reader.ReadWord();
        raw.FixedGamma = reader.ReadFixed();
        raw.Ignore = reader.ReadBytes(8);
        raw.ICCProfileDataLength = default;
        raw.ICCProfileData = default;

        if (raw.Type == 2)
        {
            raw.ICCProfileDataLength = reader.ReadDword();
            raw.ICCProfileData = reader.ReadBytes((int)raw.ICCProfileDataLength);
        }

        return raw;
    }

    private static AseExternalFilesChunk ReadExternalFilesChunk(AsepriteBinaryReader reader)
    {
        ExternalFilesChunk raw = ReadRawExternalFilesChunk(reader);
        return new AseExternalFilesChunk(raw);
    }

    private static ExternalFilesChunk ReadRawExternalFilesChunk(AsepriteBinaryReader reader)
    {
        ExternalFilesChunk raw;
        raw.NumberOfEntries = reader.ReadDword();
        raw.Ignore = reader.ReadBytes(8);

        raw.Entries = new RawExternalFileChunkEntry[raw.NumberOfEntries];

        for (int i = 0; i < raw.NumberOfEntries; i++)
        {
            RawExternalFileChunkEntry entry = ReadRawExternalFilesChunkEntry(reader);
            raw.Entries[i] = entry;
        }

        return raw;
    }

    private static RawExternalFileChunkEntry ReadRawExternalFilesChunkEntry(AsepriteBinaryReader reader)
    {
        RawExternalFileChunkEntry raw;
        raw.EntryId = reader.ReadDword();
        raw.Ignore = reader.ReadBytes(8);
        raw.ExternalFileName = reader.ReadString();

        return raw;
    }

    private static AseTagsChunk ReadTagsChunk(AsepriteBinaryReader reader, RawChunkHeader header)
    {
        RawTagsChunk raw = ReadRawTagsChunk(reader);
        return new AseTagsChunk(raw);
    }

    private static RawTagsChunk ReadRawTagsChunk(AsepriteBinaryReader reader)
    {
        RawTagsChunk raw;
        raw.NumberOfTags = reader.ReadWord();
        raw.Ignore = reader.ReadBytes(8);

        raw.Tags = new RawTagsChunkTag[raw.NumberOfTags];

        for (int i = 0; i < raw.Tags.Length; i++)
        {
            RawTagsChunkTag rawTag = ReadRawTagsChunkTag(reader);
            raw.Tags[i] = rawTag;
        }

        return raw;
    }

    private static RawTagsChunkTag ReadRawTagsChunkTag(AsepriteBinaryReader reader)
    {
        RawTagsChunkTag raw;
        raw.From = reader.ReadWord();
        raw.To = reader.ReadWord();
        raw.LoopDirection = reader.ReadByte();
        raw.Ignore1 = reader.ReadBytes(8);
        raw.Color = reader.ReadBytes(3);
        raw.Ignore2 = reader.ReadByte();
        raw.Name = reader.ReadString();

        return raw;
    }

    private static RawPaletteChunk ReadRawPaletteChunk(AsepriteBinaryReader reader)
    {
        RawPaletteChunk raw;
        raw.NewPaletteSize = reader.ReadDword();
        raw.From = reader.ReadDword();
        raw.To = reader.ReadDword();
        raw.Ignore = reader.ReadBytes(8);

        raw.Entries = new RawPaletteChunkEntry

        for (uint i = raw.From; i <= raw.To; i++)
        {

        }
    }
}