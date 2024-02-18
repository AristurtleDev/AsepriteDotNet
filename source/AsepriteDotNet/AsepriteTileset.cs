//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents a tileset used by tilemap cels.
/// </summary>
public sealed class AsepriteTileset
{
    private readonly Color[] _pixels;

    /// <summary>
    /// Gets the ID of this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets the total number of tiles in this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    /// Gets the size of each tile in this <see cref="AsepriteTileset"/>.
    /// </summary>
    public Size TileSize { get; }

    /// <summary>
    /// Gets the name of this <see cref="AsepriteTileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// An array of color elements that represent the pixel data for image of this tileset.  Order of elements is
    /// from top-left pixel read left-to-right top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Color> Pixels => _pixels;

    internal AsepriteTileset(int id, int tileCount, Size tileSize, string name, Color[] pixels)
    {
        ID = id;
        TileCount = tileCount;
        TileSize = tileSize;
        Name = name;
        _pixels = pixels;
    }
}
