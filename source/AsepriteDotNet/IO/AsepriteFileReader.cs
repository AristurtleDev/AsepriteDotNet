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

namespace AsepriteDotNet.IO;

public static class AsepriteFileReader
{
    public static void ReadFile(string path)
    {
        using AsepriteBinaryReader reader = new(File.OpenRead(path));

        RawHeader rawHeader = ReadRawHeader(reader);
        AseHeader header = new(rawHeader);

        IList<AseFrame> frames = new List<AseFrame>(header.Frames);

        for (int frameNum = 0; frameNum < header.Frames; frameNum++)
        {
            RawFrameHeader rawFrameHeader = ReadRawFrameHeader(reader);
            AseFrame frame = new(rawFrameHeader);

            //  If the old chunk count is 0xFFFF there may be more chunks to
            //  read, so we would use the new chunk count.
            int nchunks = rawFrameHeader.OldCount;
            if (rawFrameHeader.OldCount == 0xFFFF && rawFrameHeader.OldCount < rawFrameHeader.NewCount)
            {
                nchunks = (int)rawFrameHeader.NewCount;
            }

            for (int chunkNum = 0; chunkNum < nchunks; chunkNum++)
            {
                AseChunk chunk = ReadChunk(reader);
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

    internal static AseChunk ReadChunk(AsepriteBinaryReader reader)
    {
        long start = reader.Position;

        RawChunkHeader rawHeader = ReadRawChunkHeader(reader);

        //  Chunk end position needs to be calcualted for eading some chunks
        //  like the cel chunk.
        long end = start + rawHeader.Length;

        AseChunkType chunkType = (AseChunkType)rawHeader.Type;

        return chunkType switch
        {
            AseChunkType.OldPaletteChunkA => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
            AseChunkType.OldPaletteChunkB => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
            AseChunkType.LayerChunk => ReadLayerChunk(reader, rawHeader),
            AseChunkType.CelChunk => ReadCelChunk(reader, rawHeader, end),
            AseChunkType.CelExtraChunk => ReadCelExtraChunk(reader, rawHeader),
            AseChunkType.ColorProfileChunk => ReadColorProfileChunk(reader, rawHeader),
            AseChunkType.ExternalFilesChunk => ReadExternalFilesChunk(reader, rawHeader),
            AseChunkType.MaskChunk => throw new NotSupportedException($"Mask Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
            AseChunkType.PathChunk => throw new NotSupportedException($"Path Chunk (0x{rawHeader.Type:X2}) detected.  The version of Aseprite used to create this file is not supported"),
            AseChunkType.TagsChunk => ReadTagsChunk(reader, rawHeader),
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

    private static AseLayerChunk ReadLayerChunk(AsepriteBinaryReader reader, RawChunkHeader header)
    {
        RawLayerChunk raw = ReadRawLayerChunk(reader);

        return raw.Type switch
        {
            0 or 1 => new AseLayerChunk(header, raw),
            2 => new AseTilesetLayerChunk(header, raw),
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

    private static AseCelChunk ReadCelChunk(AsepriteBinaryReader reader, RawChunkHeader header, long end)
    {
        RawCelChunk raw = ReadRawCelChunk(reader, end);

        return raw.Type switch
        {
            0 or 2 => new AseImageCelChunk(header, raw),
            1 => new AseLinkedCelChunk(header, raw),
            3 => new AseTilemapCelChunk(header, raw),
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

    private static AseCelExtraChunk ReadCelExtraChunk(AsepriteBinaryReader reader, RawChunkHeader header)
    {
        RawCelExtraChunk raw = ReadRawCelExtraChunk(reader);
        return new AseCelExtraChunk(header, raw);
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

    private static AseColorProfileChunk ReadColorProfileChunk(AsepriteBinaryReader reader, RawChunkHeader header)
    {
        RawColorProfileChunk raw = ReadRawColorProfileChunk(reader);
        return new AseColorProfileChunk(header, raw);
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

    private static AseExternalFilesChunk ReadExternalFilesChunk(AsepriteBinaryReader reader, RawChunkHeader header)
    {
        ExternalFilesChunk raw = ReadRawExternalFilesChunk(reader);
        return new AseExternalFilesChunk(header, raw);
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
        return new AseTagsChunk(header, raw);
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
}