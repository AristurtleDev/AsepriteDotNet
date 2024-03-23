// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// A tile map that contains animation frames.
/// This class cannot be inherited.
/// </summary>
public sealed class AnimatedTilemap<TColor> : IEquatable<AnimatedTilemap<TColor>> where TColor : struct, IColor<TColor>
{
    private readonly Tileset<TColor>[] _tilests;
    private readonly TilemapFrame[] _frames;

    /// <summary>
    /// Gets the name of this animated tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only collection of the tilesets used by the layers of this animated tilemap.
    /// </summary>
    public ReadOnlySpan<Tileset<TColor>> Tilesets => _tilests;

    /// <summary>
    /// Gets a read-only collection of the frames of animation for this animated tilemap.
    /// </summary>
    public ReadOnlySpan<TilemapFrame> Frames => _frames;

    internal AnimatedTilemap(string name, Tileset<TColor>[] tilesets, TilemapFrame[] frames) =>
        (Name, _tilests, _frames) = (name, tilesets, frames);


    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AnimatedTilemap<TColor> other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] AnimatedTilemap<TColor>? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.Ordinal)
            && _tilests.SequenceEqual(other._tilests)
            && _frames.SequenceEqual(other._frames);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, _tilests, _frames);
}
