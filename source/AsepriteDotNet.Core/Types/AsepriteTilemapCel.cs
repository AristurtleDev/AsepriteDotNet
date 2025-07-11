//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information


//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;
using AsepriteDotNet.Core.Document;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a cel in an Aseprite file that contains tilemap data.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteTilemapCel : AsepriteCel
{
    private readonly AsepriteTile[] _tiles;

    /// <summary>
    /// Gets the size of this tilemap cel, in tiles.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets the collection of tile data for this tilemap cel. Tile elements are in the order of left-to-right, read
    /// top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AsepriteTile> Tiles => _tiles;

    internal AsepriteTilemapCel(AsepriteCelProperties celProperties, AsepriteLayer layer, AsepriteTilemapCelProperties tilemapCelProperties, AsepriteTile[] tiles)
        : base(celProperties, layer)
    {
        Size = new Size(tilemapCelProperties.Width, tilemapCelProperties.Height);
        _tiles = tiles;
    }

}
