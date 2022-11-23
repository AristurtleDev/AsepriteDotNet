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

public class UserData
{
    required public UserDataFlags Flags { get; init; }

    [MemberNotNullWhen(true, nameof(Text))]
    public bool HasText => (Flags & UserDataFlags.HasText) != 0;

    [MemberNotNullWhen(true, nameof(Color))]
    public bool HasColor => (Flags & UserDataFlags.HasColor) != 0;

    required public string? Text { get; init; } = default;
    required public Color? Color { get; init; } = default;

    [SetsRequiredMembers]
    internal UserData(RawUserDataChunk chunk)
    {
        Flags = (UserDataFlags)chunk.Flags;
        Text = chunk.Text;

        if (chunk.Red is not null && chunk.Blue is not null && chunk.Green is not null && chunk.Alpha is not null)
        {
            int r = chunk.Red.Value;
            int g = chunk.Green.Value;
            int b = chunk.Blue.Value;
            int a = chunk.Alpha.Value;
            Color = System.Drawing.Color.FromArgb(a, r, g, b);
        }
    }
}