// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines a tile of a tilemap.
/// </summary>
/// <param name="TilesetTileID">The ID of the source tile in the tileset represented by this tile.</param>
/// <param name="FlipHorizontally">Indicates whether this tile should be horizontally flipped when rendered.</param>
/// <param name="FlipVertically">Indicates whether this tile should be vertically flipped when rendered.</param>
/// <param name="FlipDiagonally">Indicates whether this tile should be diagonally flipped when rendered.</param>
public record TilemapTile(int TilesetTileID, bool FlipHorizontally, bool FlipVertically, bool FlipDiagonally);
