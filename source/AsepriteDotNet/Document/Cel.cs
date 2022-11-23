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

public class Cel : IUserData
{
    required public int LayerIndex { get; init; }
    required public int X { get; init; }
    required public int Y { get; init; }
    required public int Opacity { get; init; }

    [MemberNotNullWhen(true, nameof(PreciseX))]
    [MemberNotNullWhen(true, nameof(PreciseY))]
    [MemberNotNullWhen(true, nameof(PreciseWidth))]
    [MemberNotNullWhen(true, nameof(PReciseHeight))]
    public bool HasPreciseValues { get; private set; } = false;

    public float? PreciseX { get; private set; } = default;
    public float? PreciseY { get; private set; } = default;
    public float? PreciseWidth { get; private set; } = default;
    public float? PReciseHeight { get; private set; } = default;

    [MemberNotNullWhen(true, nameof(UserData))]
    public bool HasUserData => UserData is not null;
    
    public UserData? UserData { get; set; }

    [SetsRequiredMembers]
    internal Cel(RawCelChunk chunk)
    {
        LayerIndex = chunk.LayerIndex;
        X = chunk.X;
        Y = chunk.Y;
        Opacity = chunk.Opacity;
    }

    internal void SetPreciseBounds(RawCelExtraChunk chunk)
    {
        PreciseX = chunk.PreciseX;
        PreciseY = chunk.PreciseY;
        PreciseWidth = chunk.PreciseWidth;
        PReciseHeight = chunk.PreciseHeight;
        HasPreciseValues = true;
    }
}

// public class Cel : Chunk, IUserData
// {
//     public int LayerIndex { get; }
//     public int X { get; }
//     public int Y { get; }
//     public int Opacity { get; }

//     [MemberNotNullWhen(true, nameof(CelExtra))]
//     public bool HasCelExtraData => CelExtra is not null;

//     public CelExtra? CelExtra { get; internal set; }

//     public Cel(int layerIndex, int x, int y, int opacity)
//         : base(ChunkType.CelChunk)
//     {
//         LayerIndex = layerIndex;
//         X = x;
//         Y = y;
//         Opacity = opacity;
//     }
// }