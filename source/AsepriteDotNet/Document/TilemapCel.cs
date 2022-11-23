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
using AsepriteDotNet.IO.Compression;

namespace AsepriteDotNet.Document;

public class TilemapCel : Cel
{
    public int Width { get; }
    public int Height { get; }
    public int BitsPerTile { get; }
    public int TileIdBitmask { get; }
    public int XFlipBitmask { get; }
    public int YFlipBitmask { get; }
    public int RotationBitmask { get; }
    public byte[] Tiles { get; }


    public TilemapCel(int layerIndex, int x, int y, int opacity, int width, int height, int bitsPerTile, int tileIdBitmask, int xFlipBitmask, int yFlipBitmask, int rotationBitmask, byte[] tiles)
        : base(layerIndex, x, y, opacity)
    {
        Width = width;
        Height = height;
        BitsPerTile = bitsPerTile;
        TileIdBitmask = tileIdBitmask;
        XFlipBitmask = xFlipBitmask;
        YFlipBitmask = yFlipBitmask;
        RotationBitmask = rotationBitmask;
        Tiles = tiles;
    }
}