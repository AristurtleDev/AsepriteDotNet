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
using System.Drawing;

namespace AsepriteDotNet.Document;

public class Tag : IUserData
{
    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the inclusive
    ///     index of the frame the animation this <see cref="Tag"/> is for
    ///     starts on.
    /// </summary>
    public int From { get; set; }

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the inclusive
    ///     index of the frame the animation this <see cref="Tag"/> is for goes
    ///     to.
    /// </summary>
    public int To { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="LoopDirection"/> value that defines the 
    ///     animation loop direction for the animation this <see cref="Tag"/>
    ///     is for.
    /// </summary>
    public LoopDirection LoopDirection { get; set; } = LoopDirection.Forward;

    /// <summary>
    ///     Gets or Sets a <see cref="Color"/> value that defines the color of
    ///     this <see cref="Tag"/>.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="string"/> that contains the name of this
    ///     <see cref="Tag"/>.
    /// </summary>
    public string Name { get; set; } = "Tag";

    public UserData UserData { get; set; } = new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tag"/> class.
    /// </summary>
    public Tag() { }
}