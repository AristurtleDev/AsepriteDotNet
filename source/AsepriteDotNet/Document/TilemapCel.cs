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

namespace AsepriteDotNet.Document;

public class TilemapCel : Cel
{
    private Size _size = new Size(1, 1);

    /// <summary>
    ///     Gets or Sets a <see cref="Size"/> value that defines the width and
    ///     height of this <see cref="TilemapCel"/>.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <see cref="Size.Width"/> or <see cref="Size.Height"/>
    ///     value is not greater than 0 when setting value.
    /// </exception>
    public Size Size
    {
        get => _size;
        set
        {
            if (value.Width < 1 || value.Height < 1)
            {
                throw new ArgumentException(nameof(Size), $"{nameof(Size)} must have a width and height greater than 0 each");
            }
        }
    }

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value taht defines the number of
    ///     bits per tile.
    /// </summary>
    /// <remarks>
    ///     Per Aseprite file spec, at the moment this is always 32-bits per
    ///     tile.
    /// </remarks>
    public int BitsPerTile { get; } = 32;

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the bitmask
    ///     for tile ID.
    /// </summary>
    public int TileIdBitmask { get; set; }

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the bitmask
    ///     for X flip.
    /// </summary>
    public int XFlipBitmask { get; set; }

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the bitmask
    ///     for Y flip.
    /// </summary>
    public int YFlipBitmask { get; set; }

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the bitmask
    ///     for 90CW rotation.
    /// </summary>
    public int RotationBitmask { get; set; }

    /// <summary>
    ///     Gets or Sets an <see cref="Array"/> of <see cref="byte"/> elements
    ///     that represents the raw tile data for this <see cref="TilemapCel"/>.
    /// </summary>
    /// <remarks>
    ///     Order of tiles is row by row, from top to bottom, for each scanline
    ///     read tiles from left to right.
    /// </remarks>
    public byte[] Tiles { get; set; } = Array.Empty<byte>();

    /// <summary>
    ///     Initializes a new instance of the <see cref="TilemapCel"/> class.
    /// </summary>
    public TilemapCel() { }
}