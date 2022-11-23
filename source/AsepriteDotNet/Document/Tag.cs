/* -----------------------------------------------------------------------------
Copyright 2022 Aristurtle

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

using AsepriteDotNet.Document.Native;

namespace AsepriteDotNet.Document;

public class Tag : IUserData
{
    required public int From { get; init; }
    required public int To { get; init; }
    required public LoopDirection LoopDirection { get; init; }
    required public Color Color { get; init; }
    required public string Name { get; init; }

    [MemberNotNullWhen(true, nameof(UserData))]
    public bool HasUserData => UserData is not null;

    public UserData? UserData { get; set; }

    [SetsRequiredMembers]
    internal Tag(RawTagsChunkTag chunk)
    {
        From = chunk.From;
        To = chunk.To;
        LoopDirection = (LoopDirection)chunk.LoopDirection;
        Color = Color.FromArgb(255, chunk.Color[0], chunk.Color[1], chunk.Color[2]);
        Name = chunk.Name;
    }
}