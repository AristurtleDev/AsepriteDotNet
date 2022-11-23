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
using System.Diagnostics.CodeAnalysis;

using AsepriteDotNet.Document.Native;

namespace AsepriteDotNet.Document;

public class TilemapCel : Cel
{
    required public int Width { get; init; }
    required public int Height { get; init; }
    required public int BitsPerTile { get; init; }
    required public int TileIdBitmask { get; init; }
    required public int XFlipBitmask { get; init; }
    required public int YFlipBitmask { get; init; }
    required public int RotationBitmask { get; init; }
    required public byte[] Tiles { get; init; }

    [SetsRequiredMembers]
    internal TilemapCel(RawCelChunk chunk) : base(chunk)
    {
        if (chunk.Width is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.Height is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.BitsPerTile is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.TileIdBitmask is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.XFlipBitmask is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.YFlipBitmask is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.RotationBitmask is null)
        {
            throw new InvalidOperationException();
        }

        if (chunk.Tiles is null)
        {
            throw new InvalidOperationException();
        }

        Width = (int)chunk.Width;
        Height = (int)chunk.Height;
        BitsPerTile = (int)chunk.BitsPerTile;
        TileIdBitmask = (int)chunk.TileIdBitmask;
        XFlipBitmask = (int)chunk.XFlipBitmask;
        YFlipBitmask = (int)chunk.YFlipBitmask;
        RotationBitmask = (int)chunk.RotationBitmask;
        Tiles = chunk.Tiles;
    }
}

// public class TilemapCel : Cel
// {
//     public int Width { get; }
//     public int Height { get; }
//     public int BitsPerTile { get; }
//     public int TileIdBitmask { get; }
//     public int XFlipBitmask { get; }
//     public int YFlipBitmask { get; }
//     public int RotationBitmask { get; }
//     public byte[] Tiles { get; }


//     public TilemapCel(int layerIndex, int x, int y, int opacity, int width, int height, int bitsPerTile, int tileIdBitmask, int xFlipBitmask, int yFlipBitmask, int rotationBitmask, byte[] tiles)
//         : base(layerIndex, x, y, opacity)
//     {
//         Width = width;
//         Height = height;
//         BitsPerTile = bitsPerTile;
//         TileIdBitmask = tileIdBitmask;
//         XFlipBitmask = xFlipBitmask;
//         YFlipBitmask = yFlipBitmask;
//         RotationBitmask = rotationBitmask;
//         Tiles = tiles;
//     }
// }