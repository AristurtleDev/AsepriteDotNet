//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a tilemap cel containing tile references and transformation data for tile-based layers.
/// </summary>
public sealed class AsepriteTilemapCel : AsepriteCel
{
    internal AsepriteTile[] InternalTiles { get; set; }

    /// <summary>
    /// Gets the dimensions of the tilemap grid in number of tiles.
    /// </summary>
    /// <remarks>
    /// Unlike image cels that specify pixel dimensions, tilemap cels define their size in terms of
    /// tile count. The actual pixel dimensions depend on the associated tileset's tile size.
    /// Total tile count equals Size.Width × Size.Height.
    /// </remarks>
    public Size Size { get; internal set; }

    /// <summary>
    /// Gets the tile data organized in a grid from top-left to bottom-right.
    /// </summary>
    /// <remarks>
    /// Tiles are stored row by row from top to bottom, with each row containing tiles from left to right.
    /// Each tile contains an ID referencing the associated tileset and transformation flags for
    /// horizontal flip, vertical flip, and diagonal flip operations during rendering.
    /// </remarks>
    public ReadOnlySpan<AsepriteTile> Tiles => InternalTiles;

    internal AsepriteTilemapCel() { }
}
