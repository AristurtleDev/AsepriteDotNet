// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Defines a frame of a tile in an animated tile map.
/// This class cannot be inherited.
/// </summary>
public sealed class TilemapFrame : IEquatable<TilemapFrame>
{
    private readonly TilemapLayer[] _layers;

    /// <summary>
    /// Gets the duration of this frame.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Gets a read-only collection of the tilemap layers used by this tilemap during this frame.
    /// </summary>
    public ReadOnlySpan<TilemapLayer> Layers => _layers;

    internal TilemapFrame(TimeSpan duration, TilemapLayer[] layers) =>
        (Duration, _layers) = (duration, layers);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TilemapFrame other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] TilemapFrame? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Duration.Equals(other?.Duration)
            && _layers.SequenceEqual(other._layers);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Duration, _layers);

}
