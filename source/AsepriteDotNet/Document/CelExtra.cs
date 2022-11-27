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
namespace AsepriteDotNet.Document;

/// <summary>
///     Represents extra precision data for a cel in an Aserpite image.
/// </summary>
public sealed class CelExtra
{
    /// <summary>
    ///     Gets whether precise bounds are set for the <see cref="Cel"/> this
    ///     <see cref="CelExtra"/> is for.
    /// </summary>
    public bool PreciseBoundsSet { get; internal set; }

    /// <summary>
    ///     Gets the precise x-coordinate position, relative to the bounds of
    ///     the sprite, of the <see cref="Cel"/> this <see cref="CelExtra"/> is
    ///     for.
    /// </summary>
    public float PreciseX { get; internal set; }

    /// <summary>
    ///     Gets the precise y-coordinate position, relative to the bounds of
    ///     the sprite, of the <see cref="Cel"/> this <see cref="CelExtra"/> is
    ///     for.
    /// </summary>
    public float PreciseY { get; internal set; }

    /// <summary>
    ///     Gets the width, scaled in real-time, of the <see cref="Cel"/> this
    ///     <see cref="CelExtra"/> is for.
    /// </summary>
    public float WidthInSprite { get; internal set; }

    /// <summary>
    ///     Gets the height, scaled in real-time, of the <see cref="Cel"/> this
    ///     <see cref="CelExtra"/> is for.
    /// </summary>
    public float HeightInSprite { get; internal set; }

    internal CelExtra() { }
}