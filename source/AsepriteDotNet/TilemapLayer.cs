// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines the layer of a tilemap which is composed of tiles.
/// </summary>
/// <param name="Name">The name of the tilemap layer.</param>
/// <param name="TilesetID">The id of the source tileset used by the tiles in the tilemap layer.</param>
/// <param name="Columns">The total number of columns in the tilemap layer.</param>
/// <param name="Rows">The total number of rows in the tilemap layer.</param>
/// <param name="Offset">The offset of the tilemap layer relative to the bounds of the frame.</param>
/// <param name="Tiles">The tiles contained on this tilemap layer.</param>
public record TilemapLayer(string Name, int TilesetID, int Columns, int Rows, Point Offset, TilemapTile[] Tiles);
