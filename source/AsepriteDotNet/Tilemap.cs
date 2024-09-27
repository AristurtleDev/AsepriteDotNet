// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Defines a tilemap composed of tile map layers.
/// This class cannot be inherited.
/// </summary>
public sealed class Tilemap : IEquatable<Tilemap>
{
    /// <summary>
    /// An empty tilemap with no name, an empty collection of tilesets, and an empty collection of tilemap layers.
    /// </summary>
    public static readonly Tilemap Empty = new Tilemap(string.Empty, Array.Empty<Tileset>(), Array.Empty<TilemapLayer>());

    private readonly Tileset[] _tilesets;
    private readonly TilemapLayer[] _layers;

    /// <summary>
    /// Gets the name of this tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only collection of the tilesets used by the layers of this tilemap.
    /// </summary>
    public ReadOnlySpan<Tileset> Tilesets => _tilesets;

    /// <summary>
    /// Gets a read-only collection of the layers that compose this tilemap.
    /// </summary>
    public ReadOnlySpan<TilemapLayer> Layers => _layers;

    internal Tilemap(string name, Tileset[] tilesets, TilemapLayer[] layers) =>
        (Name, _tilesets, _layers) = (name, tilesets, layers);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Tilemap other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Tilemap? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && _tilesets.SequenceEqual(other._tilesets)
            && _layers.SequenceEqual(other._layers);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, _tilesets, _layers);
}
