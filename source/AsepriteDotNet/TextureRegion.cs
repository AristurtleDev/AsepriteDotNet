// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a rectangular region within a larger texture that represents a sub texture.
/// This class cannot be inherited.
/// </summary>
public sealed class TextureRegion : IEquatable<TextureRegion>
{
    private readonly Slice[] _slices;

    /// <summary>
    /// Gets the name of this texture region.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the bounds of this texture region.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Gets a read-only collection of the slices within this texture region.
    /// </summary>
    public ReadOnlySpan<Slice> Slices => _slices;
    internal TextureRegion(string name, Rectangle bounds, Slice[] slices) =>
        (Name, Bounds, _slices) = (name, bounds, slices);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TextureRegion other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] TextureRegion? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && Bounds.Equals(other.Bounds)
            && _slices.SequenceEqual(other._slices);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, Bounds, _slices);
}
