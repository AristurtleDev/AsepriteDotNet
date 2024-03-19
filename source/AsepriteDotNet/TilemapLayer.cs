// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines the layer of a tilemap which is composed of tiles.
/// This class cannot be inherited.
/// </summary>
public sealed class TilemapLayer : IEquatable<TilemapLayer>
{
    private readonly TilemapTile[] _tiles;

    /// <summary>
    /// Gets the name of this tilemap layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the ID of the source tileset used by the tiles in this tilemap layer.
    /// </summary>
    public int TilesetID { get; }

    /// <summary>
    /// Gets the total number of columns in this tilemap layer.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the total number of rows in this tilemap layer.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the offset of this tilemap layer relative to the bounds of the frame.
    /// </summary>
    public Point Offset { get; }

    /// <summary>
    /// Gets a read-only collection of the tiles that compose this tilemap layer.
    /// </summary>
    public ReadOnlySpan<TilemapTile> Tiles => _tiles;

    internal TilemapLayer(string name, int tilesetID, int columns, int rows, Point offset, TilemapTile[] tiles) =>
        (Name, TilesetID, Columns, Rows, Offset, _tiles) = (name, tilesetID, columns, rows, offset, tiles);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TilemapLayer other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] TilemapLayer? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.Ordinal)
            && TilesetID.Equals(other.TilesetID)
            && Columns.Equals(other.Columns)
            && Rows.Equals(other.Rows)
            && Offset.Equals(other.Offset)
            && _tiles.SequenceEqual(other._tiles);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, TilesetID, Columns, Rows, Offset, _tiles);
}
