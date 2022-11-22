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

        RawHeader rawHeader;
        rawHeader.FileSize = reader.ReadDword();
        rawHeader.MagicNumber = reader.ReadWord();
        rawHeader.Frames = reader.ReadWord();
        rawHeader.Width = reader.ReadWord();
        rawHeader.Height = reader.ReadWord();
        rawHeader.ColorDepth = reader.ReadWord();
        rawHeader.Flags = reader.ReadDword();
        rawHeader.Speed = reader.ReadWord();
        rawHeader.Ignore1 = reader.ReadDword();
        rawHeader.Ignore2 = reader.ReadDword();
        rawHeader.TransparentIndex = reader.ReadByte();
        rawHeader.Ignore3 = reader.ReadBytes(3);
        rawHeader.NumberOfColors = reader.ReadWord();
        rawHeader.PixelWidth = reader.ReadByte();
        rawHeader.PixelHeight = reader.ReadByte();
        rawHeader.GridX = reader.ReadShort();
        rawHeader.GridY = reader.ReadShort();
        rawHeader.GridWidth = reader.ReadWord();
        rawHeader.GridHeight = reader.ReadWord();
        rawHeader.Ignore4 = reader.ReadBytes(84);

        AseHeader header = new(rawHeader);

        IList<AseFrame> frames = new List<AseFrame>();

        for (int frameNum = 0; frameNum < rawHeader.Frames; frameNum++)
        {
            RawFrameHeader rawFrameHeader;
            rawFrameHeader.Length = reader.ReadDword();
            rawFrameHeader.Magic = reader.ReadWord();
            rawFrameHeader.OldCount = reader.ReadWord();
            rawFrameHeader.Duration = reader.ReadWord();
            rawFrameHeader.Ignore1 = reader.ReadBytes(2);
            rawFrameHeader.NewCount = reader.ReadDword();

            int nchunks = rawFrameHeader.OldCount == 0xFFFF && rawFrameHeader.OldCount < rawFrameHeader.NewCount ?
                          (int)rawFrameHeader.NewCount :
                          rawFrameHeader.OldCount;

            for (int chunkNum = 0; chunkNum < nchunks; chunkNum++)
            {
                long chunkStart = reader.Position;

                RawChunkHeader rawChunkHeader;
                rawChunkHeader.Length = reader.ReadDword();
                rawChunkHeader.Type = reader.ReadWord();

                long chunkEnd = chunkStart + rawChunkHeader.Length;

                AseChunk chunk = (AseChunkType)rawChunkHeader.Type switch
                {
                    AseChunkType.OldPaletteChunkA => throw new NotSupportedException("Old Palette Chunk detected. The versio of Aseprite used to create this file is not supported"),
                    AseChunkType.OldPaletteChunkB => throw new NotSupportedException("Old Palette Chunk detected. The versio of Aseprite used to create this file is not supported"),
                    AseChunkType.LayerChunk => ReadLayerChunk(reader, rawChunkHeader),
                    AseChunkType.CelChunk => ReadCelChunk(reader, rawChunkHeader, chunkEnd),
                    AseChunkType.CelExtraChunk => throw new NotImplementedException(),
                    _ => throw new InvalidOperationException($"Unknown chunk type '0x{rawChunkHeader.Type}'")
                };
            }

        }
    }

    private static AseLayer ReadLayerChunk(AsepriteBinaryReader reader, RawChunkHeader rawHeader)
    {
        RawLayerChunk rawChunk;
        rawChunk.Flags = reader.ReadWord();
        rawChunk.Type = reader.ReadWord();
        rawChunk.ChildLevel = reader.ReadWord();
        rawChunk.DefaultWidth = reader.ReadWord();
        rawChunk.DefaultHeight = reader.ReadWord();
        rawChunk.BlendMode = reader.ReadWord();
        rawChunk.Opacity = reader.ReadByte();
        rawChunk.Ignore = reader.ReadBytes(3);
        rawChunk.Name = reader.ReadString();

        if (rawChunk.Type == 2)
        {
            rawChunk.TilsetIndex = reader.ReadDword();
            return new AseTilesetLayer(rawHeader, rawChunk);
        }
        else
        {
            rawChunk.TilsetIndex = default;
            return new AseLayer(rawHeader, rawChunk);
        }
    }

    private static AseCel ReadCelChunk(AsepriteBinaryReader reader, RawChunkHeader header, long chunkend)
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
            raw.Pixels = reader.ReadBytes((int)(chunkend - reader.Position));
            return new AseRawImageCel(header, raw);
        }
        else if(raw.Type == 1)
        {
            raw.FramePosition = reader.ReadWord();
            return new AseLinkedCel(header, raw);
        }
        else if(raw.Type == 2)
        {
            raw.Width = reader.ReadWord();
            raw.Height = reader.ReadWord();
            raw.CompressedPixels = reader.ReadBytes((int)(chunkend - reader.Position));
            return new AseCompressedImageCel(header, raw);
        }
        else if(raw.Type == 3)
        {
            raw.Width = reader.ReadWord();
            raw.Height = reader.ReadWord();
            raw.BitsPerTile = reader.ReadWord();
            raw.TileIdBitmask = reader.ReadDword();
            raw.XFlipBitmask = reader.ReadDword();
            raw.YFlipBitmask = reader.ReadDword();
            raw.RotationBitmask = reader.ReadDword();
            raw.Ignore2 = reader.ReadBytes(10);
            raw.CompressedTiles = reader.ReadBytes((int)(chunkend - reader.Position));
            return new AseTilemapCel(header, raw);
        }
        else
        {
            throw new InvalidOperationException($"Unknown cel type '{raw.Type}'");
        }
    }
}