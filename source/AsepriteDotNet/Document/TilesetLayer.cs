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

public class TilesetLayer : Layer
{
    required public int TilesetIndex { get; init; }

    [SetsRequiredMembers]
    internal TilesetLayer(RawLayerChunk chunk) : base(chunk)
    {
        if (chunk.TilsetIndex is null)
        {
            throw new InvalidOperationException();
        }

        TilesetIndex = (int)chunk.TilsetIndex;
    }
}

// public class TilesetLayer : Layer
// {
//     public int TilesetIndex { get; }

//     internal TilesetLayer(bool visible, int childLevel, BlendMode blendMode, int opacity, string name, int tilesetIndex)
//         : base(visible, childLevel, blendMode, opacity, name) => TilesetIndex = tilesetIndex;
// }