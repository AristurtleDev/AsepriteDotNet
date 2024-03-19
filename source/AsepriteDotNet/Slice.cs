// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Represents a slice defined by it's xy-coordinate position, width, height, and origin.rgskds
/// </summary>
public class Slice : IEquatable<Slice>
{
    /// <summary>
    /// Gets the name of this slice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the bounds of this slice.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Gets the xy-coordinate origin point of this slice.
    /// </summary>
    public Point Origin { get; }

    /// <summary>
    /// Gets the color of this slice.
    /// </summary>
    public Rgba32 Color { get; }

    internal Slice(string name, Rectangle bounds, Point origin, Rgba32 color) =>
        (Name, Bounds, Origin, Color) = (name, bounds, origin, color);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Slice other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Slice? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && Bounds.Equals(other.Bounds)
            && Origin.Equals(other.Origin)
            && Color.Equals(other.Color);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, Bounds, Origin, Color);
}
