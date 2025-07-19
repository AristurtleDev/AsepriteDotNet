//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a single tile entry in a tilemap with transformation flags and tileset reference.
/// </summary>
public sealed class AsepriteTile
{
    /// <summary>
    /// Gets the identifier that references a specific tile within the associated tileset.
    /// </summary>
    public int ID { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the tile should be rendered with horizontal mirroring.
    /// </summary>
    public bool FlipHorizontally { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the tile should be rendered with vertical mirroring.
    /// </summary>
    public bool FlipVertically { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the tile should be rendered with diagonal mirroring.
    /// </summary>
    public bool FlipDiagonally { get; internal set; }

    internal AsepriteTile() { }
}
