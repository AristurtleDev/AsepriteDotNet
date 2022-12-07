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

/// <summary>
///     Represents an animation Tag in an Asperite image.
/// </summary>
public sealed class Tag : IUserData
{
    private Color _oldVersionColor;

    /// <summary>
    ///     Gets the inclusive index of the frame the animation for this
    ///     <see cref="Tag"/> starts on.
    /// </summary>
    public int From { get; }

    /// <summary>
    ///     Gets the inclusive index of the frame the animation for this
    ///     <see cref="Tag"/> ends on.
    /// </summary>
    public int To { get; }

    /// <summary>
    ///     Gets the <see cref="LoopDirection"/> value for the animation for
    ///     this <see cref="Tag"/>.
    /// </summary>
    public LoopDirection LoopDirection { get; }

    /// <summary>
    ///     Gest the color of this <see cref="Tag"/>.
    /// </summary>
    public Color Color
    {
        get
        {
            if (UserData.HasColor)
            {
                return UserData.Color.Value;
            }

            return _oldVersionColor;
        }
    }

    /// <summary>
    ///     Gets the name of this <see cref="Tag"/>,
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="UserData"/> for this <see cref="Tag"/>.
    /// </summary>
    public UserData UserData { get; internal set; } = new();

    internal Tag(int from, int to, LoopDirection direction, Color color, string name) =>
        (From, To, LoopDirection, _oldVersionColor, Name) = (from, to, direction, color, name);
}