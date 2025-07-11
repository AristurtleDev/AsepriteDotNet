// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Defines a tile of a tilemap.
/// This class cannot be inherited.
/// </summary>
public sealed class TilemapTile : IEquatable<TilemapTile>
{
    /// <summary>
    /// Gets the ID of the source tile in the tileset represented by this tile.
    /// </summary>
    public int TilesetTileID { get; }

    /// <summary>
    /// Gets a value that indicates whether this tile should be horizontally flipped when rendered.
    /// </summary>
    public bool FlipHorizontally { get; }

    /// <summary>
    /// Gets a value that indicates whether this tile should be vertically flipped when rendered.
    /// </summary>
    public bool FlipVertically { get; }

    /// <summary>
    /// Gets a value that indicates whether this tile should be diagonally flipped when rendered.
    /// </summary>
    public bool FlipDiagonally { get; }

    internal TilemapTile(int tilesetTileID, bool flipHorizontally, bool flipVertically, bool flipDiagonally) =>
        (TilesetTileID, FlipHorizontally, FlipVertically, FlipDiagonally) = (tilesetTileID, flipHorizontally, flipVertically, flipDiagonally);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TilemapTile other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] TilemapTile? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return TilesetTileID.Equals(other?.TilesetTileID)
            && FlipHorizontally.Equals(other.FlipHorizontally)
            && FlipVertically.Equals(other.FlipVertically)
            && FlipDiagonally.Equals(other.FlipDiagonally);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(TilesetTileID, FlipHorizontally, FlipVertically, FlipDiagonally);
}
