// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a tileset composed of source texture.
/// </summary>
/// <param name="ID">The unique ID assigned to the tileset.</param>
/// <param name="Name">The name of the tileset.</param>
/// <param name="Texture">The source texture of the tileset.</param>
/// <param name="TileSize">The size of each tile in the tileset.</param>
public record Tileset(int ID, string Name, Texture Texture, Size TileSize);
