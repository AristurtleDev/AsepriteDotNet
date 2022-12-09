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
///     Represents a tile reference in a tilemap cel in an Asperite image.
/// </summary>
public sealed class Tile
{
    /// <summary>
    ///     Gets the ID of the tile in the tileset
    /// </summary>
    public uint TileID { get; }

    /// <summary>
    ///     Gets the X Flip Bitmask for this <see cref="Tile"/>.
    /// </summary>
    [Obsolete("Tile X-Flip is not implemented in Aseprite yet and this will always be 0", false)]
    public uint XFlip { get; }

    /// <summary>
    ///     Gets the Y Flip Bitmask for this <see cref="Tile"/>.
    /// </summary>
    [Obsolete("Tile Y-Flip is not implemented in Aseprite yet and this will always be 0", false)]
    public uint YFlip { get; }

    /// <summary>
    ///     Gets the 90CW Rotation Bitmask for this <see cref="Tile"/>.
    /// </summary>
    [Obsolete("Tile Rotation is not implemented in Aseprite yet and this will always be 0", false)]
    public uint Rotate90 { get; }

    internal Tile(uint id, uint xflip, uint yflip, uint rotate) =>
#pragma warning disable 0618
        (TileID, XFlip, YFlip, Rotate90) = (id, xflip, yflip, rotate);
#pragma warning restore 0618
}