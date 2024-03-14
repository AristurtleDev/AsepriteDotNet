//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a cel in an Aseprite file that contains tilemap data.  This class cannot be inherited.
/// </summary>
public sealed class TilemapCel : Cel
{
    private readonly Tile[] _tiles;

    /// <summary>
    /// Gets the width, in tiles, of this tilemap cel.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height, in tiles, of this tilemap cel.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the collection of tile data for this tilemap cel. Tile elements are in the order of left-to-right, read
    /// top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Tile> Tiles => _tiles;

    internal TilemapCel(CelProperties celProperties, Layer layer, TilemapCelProperties tilemapCelProperties, Tile[] tiles)
        : base(celProperties, layer)
    {
        Width = tilemapCelProperties.Width;
        Height = tilemapCelProperties.Height;
        _tiles = tiles;
    }

}
