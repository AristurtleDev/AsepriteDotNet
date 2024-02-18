//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents a tile in an <see cref="AsepriteTilemapCel"/>
/// Represents a single tile in a tilemap cel.
/// </summary>
public sealed class AsepriteTile
{
    /// <summary>
    /// Gets the id of hte tile in the <see cref="AsepriteTileset"/> that represents this <see cref="AsepriteTile"/>.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Get a value that indicates if this <see cref="AsepriteTile"/> is flipped along the horizontal axis.
    /// </summary>
    public bool FlipHorizontally { get; }

    /// <summary>
    /// Gets a value that indicates if this <see cref="AsepriteTile"/> is flipped along the vertical axis.
    /// </summary>
    public bool FlipVertically { get; }

    /// <summary>
    /// Gets a value that indicates if this <see cref="AsepriteTile"/> is flipped along a diagonal axis from the
    /// top-left corner to the bottom right corner.
    /// </summary>
    public bool FlipDiagonally { get; }

    internal AsepriteTile(int id, bool flipHorizontally, bool flipVertically, bool flipDiagonally)
    {
        ID = id;
        FlipHorizontally = flipHorizontally;
        FlipVertically = flipVertically;
        FlipDiagonally = flipDiagonally;
    }
}
