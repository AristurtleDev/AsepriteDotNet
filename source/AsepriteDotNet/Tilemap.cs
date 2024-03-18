// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines a tilemap composed of tile map layers.
/// </summary>
/// <param name="Name">The name of the tilemap.</param>
/// <param name="Tilesets">The tilesets used by the layers in the tilemap.</param>
/// <param name="Layers">The layers that compose the tilemap.</param>
public record Tilemap(string Name, Tileset[] Tilesets, TilemapLayer[] Layers);
