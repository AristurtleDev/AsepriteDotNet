//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a tilemap layer that contains tile-based content using references to a specific tileset.
/// </summary>
public sealed class AsepriteTilemapLayer : AsepriteLayer
{
    /// <summary>
    /// Gets the tileset that provides the tile definitions for this tilemap layer.
    /// </summary>
    public AsepriteTileset Tileset { get; internal set; }

    internal AsepriteTilemapLayer() { }
}
