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
using System.Collections;
using System.Collections.ObjectModel;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO.Image;

namespace AsepriteDotNet.Image;

/// <summary>
///     Represents a tilesheet generated from a tileset in an Aseprite file.
/// </summary>
public sealed class Tilesheet : IEnumerable<TilesheetTile>
{
    private Color[] _pixels;
    private List<TilesheetTile> _tiles = new();

    /// <summary>
    ///     Gets the name of tis <see cref="Tilesheet"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the width and height of this <see cref="Tilesheet"/>.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="TilesheetTile"/>
    ///     elements in this <see cref="Tilesheet"/>.
    /// </summary>
    public ReadOnlyCollection<TilesheetTile> Tiles { get; }

    /// <summary>
    ///     Gets a read-only collection of all pixels that make up the image for
    ///     this <see cref="Tilesheet"/>.  Order of pixels is from 
    ///     top-to-bottom, read left-to-right.
    /// </summary>
    public ReadOnlyCollection<Color> Pixels { get; }

    internal Tilesheet(string name, Size size,  List<TilesheetTile> tiles, Color[] pixels)
    {
        Name = name;
        Size = size;
        _tiles = tiles;
        Tiles = _tiles.AsReadOnly();
        _pixels = pixels;
        Pixels = _pixels.AsReadOnly();
    }

    /// <summary>
    ///     Writes the pixel data for this <see cref="Tilesheet"/> to disk as a
    ///     .png file.
    /// </summary>
    /// <param name="path">
    ///     The absolute file path to save the generated .png file to.
    /// </param>
    public void ToPng(string path)
    {
        PngWriter.SaveTo(path, Size, _pixels);
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the 
    ///     <see cref="TilesheetTile"/> elements in this 
    ///     <see cref="Tilesheet"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through the <see cref="TilesheetTile"/>
    ///     elements in this <see cref="Tilesheet"/>.
    /// </returns>
    public IEnumerator<TilesheetTile> GetEnumerator() => _tiles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}