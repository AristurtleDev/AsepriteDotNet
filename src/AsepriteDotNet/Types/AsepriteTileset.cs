//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;

namespace AsepriteDotNet.Types;

/// <summary>
/// Represents a tileset containing tile definitions and image data for tilemap layers.
/// </summary>
/// <remarks>
/// Tilesets provide the visual content referenced by tilemap layers, storing tile image data
/// in a vertical strip format where each tile occupies a rectangular region of specific dimensions.
/// </remarks>
public sealed class AsepriteTileset
{
    internal Rgba32[] InternalPixels { get; set; }

    /// <summary>
    /// Gets the unique identifier for this tileset within the sprite.
    /// </summary>
    /// <remarks>
    /// Tileset IDs are used by tilemap layers to associate with the correct tile definitions.
    /// </remarks>
    public int ID { get; internal set; }

    /// <summary>
    /// Gets the total number of tiles defined in this tileset.
    /// </summary>
    public int TileCount { get; internal set; }

    /// <summary>
    /// Gets the dimensions of each individual tile in pixels.
    /// </summary>
    /// <remarks>
    /// All tiles within a tileset must have identical dimensions. The tileset image data
    /// is organized as a vertical strip with total dimensions of TileSize.Width × (TileSize.Height × TileCount).
    /// </remarks>
    public Size TileSize { get; internal set; }

    /// <summary>
    /// Gets the descriptive name assigned to this tileset.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the pixel data for all tiles arranged in a vertical strip format.
    /// </summary>
    /// <remarks>
    /// Pixels are organized as a vertical strip with dimensions TileSize.Width × (TileSize.Height × TileCount).
    /// Each tile occupies a rectangular region starting at Y position (tile index × TileSize.Height)
    /// and extending for TileSize.Height rows. Pixels within each tile are stored in row-major order.
    /// </remarks>
    public ReadOnlySpan<Rgba32> Pixels => InternalPixels;

    internal AsepriteTileset() { }
}
