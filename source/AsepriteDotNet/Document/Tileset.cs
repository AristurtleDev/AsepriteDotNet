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
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a tileset in an Aseprite image.
/// </summary>
public class Tileset
{
    /// <summary>
    ///     Gets the ID of this <see cref="Tileset"/>.
    /// </summary>
    public int ID { get; }

    /// <summary>
    ///     Gets the total number of tiles in this ,<see cref="Tileset"/>.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    ///     Gets the width and height, in pixels of each tile in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    public Size TileSize { get; }

    /// <summary>
    ///     Gets the name of this <see cref="Tileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets or Sets an <see cref="Array"/> of <see cref="Color"/> elements 
    ///     that represents the raw pixel data for this <see cref="Tileset"/>.
    /// </summary>
    public Color[] Pixels { get; }

    internal Tileset(int id, int count, Size tileSize, string name, Color[] pixels) =>
        (ID, TileCount, TileSize, Name, Pixels) = (id, count, tileSize, name, pixels);
}