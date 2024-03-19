//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a tile in an Aseprite file.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteTile
{
    /// <summary>
    /// Gets the ID of the tile in the tileset that is represented by this tile.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets a value that indicates whether this tile is flipped horizontally.
    /// </summary>
    public bool FlipHorizontally { get; }

    /// <summary>
    /// Gets a value that indicates whether this tile is flipped vertically.
    /// </summary>
    public bool FlipVertically { get; }

    /// <summary>
    /// Gets a value that indicates whether this tile is flipped diagonally.
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
