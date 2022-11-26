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

public class Tileset
{
    private Size _tileSize = new Size(1, 1);

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the ID of
    ///     this <see cref="Tileset"/>.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    ///     Gets the total number of tiles in this <see cref="Tileset"/>.
    ///     (Number of Pixels / (Tile Width * Tile Height)).
    /// </summary>
    public int TileCount => Pixels.Length / (TileSize.Width * TileSize.Height);

    /// <summary>
    ///     Gets or Sets a <see cref="Size"/> value that defines the size of
    ///     each tile in this <see cref="Tileset"/>.
    /// </summary>
    public Size TileSize
    {
        get => _tileSize;
        set
        {
            if(value.Width <= 0 || value.Height <= 0)
            {
                throw new ArgumentException($"Invalid tile size {value.Width}x{value.Height}.  Width and height must be greater than zero.");
            }

            _tileSize = value;
        }
    }

    /// <summary>
    ///     Gets a <see cref="string"/> that contains the name of this
    ///     <see cref="Tileset"/>.
    /// </summary>
    public string Name { get; set; } = "Tileset";

    /// <summary>
    ///     Gets or Sets an <see cref="Array"/> of <see cref="byte"/> elements
    ///     that represents the raw pixel data for this <see cref="Tileset"/>,
    /// </summary>
    /// <remarks>
    ///     Order of pixels is row by row, from top to bottom, for each scanline
    ///     read pixels from left to right.
    /// </remarks>
    public byte[] Pixels { get; set; } = Array.Empty<byte>();

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tileset"/> class.
    /// </summary>
    public Tileset() { }
}