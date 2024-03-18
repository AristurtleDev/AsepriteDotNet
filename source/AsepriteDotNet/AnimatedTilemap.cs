// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// A tile map that contains animation frames.  This record cannot be inherited.
/// </summary>
/// <param name="Name">The name of this animated tilemap.</param>
/// <param name="Tilesets">The tilesets used by the layers of this animated tilemap.</param>
/// <param name="TilemapFrames">The frames of animation for this animated tilemap.</param>
public sealed record AnimatedTilemap(string Name, Tileset[] Tilesets, TilemapFrame[] TilemapFrames) { }
