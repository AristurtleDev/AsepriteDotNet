// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines a frame of a tile in an animated tile map.
/// </summary>
/// <param name="Duration">The duration of this frame.</param>
/// <param name="Layers">THe layers tilemap layers used by the tilemap during this frame.</param>
public record TilemapFrame(TimeSpan Duration, TilemapLayer[] Layers);
