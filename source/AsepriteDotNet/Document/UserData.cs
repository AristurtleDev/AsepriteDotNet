/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

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

namespace AsepriteDotNet.Document;

public class UserData
{
    /// <summary>
    ///     Gets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="UserData"/> has a <see cref="UserData.Text"/> value.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Text))]
    public bool HasText => Text is not null;

    /// <summary>
    ///     Gets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="UserData"/> has a <see cref="UserData.Color"/> value.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Color))]
    public bool HasColor => Color is not null;

    /// <summary>
    ///     Gets or Sets a <see cref="string"/> that contains the text for this
    ///     <see cref="UserData"/>.
    /// </summary>
    public string? Text { get; internal set; } = default;

    /// <summary>
    ///     Gets or Sets a <see cref="Color"/> value that defines the color of
    ///     this <see cref="UserData"/>.
    /// </summary>
    public Color? Color { get; internal set; } = default;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserData"/> class.
    /// </summary>
    public UserData() { }
}