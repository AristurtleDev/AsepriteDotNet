//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents a tileset used by tilemap cels.
/// </summary>
public sealed class AsepriteTileset
{
    private readonly AseColor[] _pixels;

    /// <summary>
    /// Gets the ID of this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets the total number of tiles in this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    /// Gets the width of each tile in this <see cref="AsepriteTileset"/>, in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of each tile in this <see cref="AsepriteTileset"/>, in pixels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the name of this <see cref="AsepriteTileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// An array of color elements that represent the pixel data for image of this tileset.  Order of elements is
    /// from top-left pixel read left-to-right top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AseColor> Pixels => _pixels;

    internal AsepriteTileset(TilesetProperties tilesetProperties, string name, AseColor[] pixels)
    {
        ID = (int)tilesetProperties.Id;
        TileCount = (int)tilesetProperties.NumberOfTiles;
        Width = (int)tilesetProperties.TileWidth;
        Height = (int)tilesetProperties.TileHeight;
        Name = name;
        _pixels = pixels;
    }
}
