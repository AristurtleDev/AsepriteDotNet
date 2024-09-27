// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// A tile map that contains animation frames.
/// This class cannot be inherited.
/// </summary>
public sealed class AnimatedTilemap : IEquatable<AnimatedTilemap>
{
    /// <summary>
    /// An empty animated tilemap with no name, an empty collection of tilesets, and an empty collection of tilemap
    /// frames.
    /// </summary>
    public static readonly AnimatedTilemap Empty = new AnimatedTilemap(string.Empty, Array.Empty<Tileset>(), Array.Empty<TilemapFrame>());

    private readonly Tileset[] _tilests;
    private readonly TilemapFrame[] _frames;

    /// <summary>
    /// Gets the name of this animated tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only collection of the tilesets used by the layers of this animated tilemap.
    /// </summary>
    public ReadOnlySpan<Tileset> Tilesets => _tilests;

    /// <summary>
    /// Gets a read-only collection of the frames of animation for this animated tilemap.
    /// </summary>
    public ReadOnlySpan<TilemapFrame> Frames => _frames;

    internal AnimatedTilemap(string name, Tileset[] tilesets, TilemapFrame[] frames) =>
        (Name, _tilests, _frames) = (name, tilesets, frames);


    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AnimatedTilemap other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] AnimatedTilemap? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.Ordinal)
            && _tilests.SequenceEqual(other._tilests)
            && _frames.SequenceEqual(other._frames);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, _tilests, _frames);
}
