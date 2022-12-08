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
using System.Drawing;

namespace AsepriteDotNet.Image;

/// <summary>
///     Represents a rectangular boundry within a
///     <see cref="SpritesheetFrame"/>.
/// </summary>
public sealed class SpritesheetSlice
{
    /// <summary>
    ///     Gets the bounds of this <see cref="SpritesheetSlice"/>, relative to 
    ///     the bounds of the <see cref="SpritesheetFrame"/> it is in.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Gets the bounds for the center rectangle of this 
    ///     <see cref="SpritesheetSlice"/> if it is a 9-patches slice; 
    ///     otherwise, <see langword="null"/>.
    /// </summary>
    public Rectangle? CenterBounds { get; }

    /// <summary>
    ///     Gets the pivot point for this slice if it has a pivot point;
    ///     otherwise, <see langword="null"/>.
    /// </summary>
    public Point? Pivot { get; }

    /// <summary>
    ///     Gets the name of this <see cref="SpritesheetSlice"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the color of this <see cref="SpritesheetSlice"/>.
    /// </summary>
    public Color Color { get; }

    internal SpritesheetSlice(Rectangle bounds, Rectangle? centerBounds, Point? pivot, string name, Color color) =>
        (Bounds, CenterBounds, Pivot, Name, Color) = (bounds, centerBounds, pivot, name, color);
}