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
using AsepriteDotNet.Document.Native;

namespace AsepriteDotNet.Document;

public class AseTilemapCel : AseCel
{
    public int Width => (int)RawCelChunk.Width!;
    public int Height => (int)RawCelChunk.Height!;
    public int BitsPerTile => (int)RawCelChunk.BitsPerTile!;
    public int TileIdBitmask => (int)RawCelChunk.TileIdBitmask!;
    public int XFlipBitmask => (int)RawCelChunk.XFlipBitmask!;
    public int YFlipBitmask => (int)RawCelChunk.YFlipBitmask!;
    public int RotationBitmask => (int)RawCelChunk.RotationBitmask!;
    public byte[] CompressedTiles => RawCelChunk.CompressedTiles!;


    public AseTilemapCel(RawChunkHeader chunkHeader, RawCelChunk celChunk)
        : base(chunkHeader, celChunk)
    {
        if (celChunk.Width is null)
        {
            throw new ArgumentException();
        }

        if (celChunk.Height is null)
        {
            throw new ArgumentException();
        }

        if(celChunk.BitsPerTile is null)
        {
            throw new ArgumentException();
        }

        if(celChunk.TileIdBitmask is null)
        {
            throw new ArgumentException();
        }

        if(celChunk.XFlipBitmask is null)
        {
            throw new ArgumentException();
        }

        if(celChunk.YFlipBitmask is null)
        {
            throw new ArgumentException();
        }

        if(celChunk.RotationBitmask is null)
        {
            throw new ArgumentException();
        }

        if(celChunk.CompressedTiles is null)
        {
            throw new ArgumentException();
        }
    }
}