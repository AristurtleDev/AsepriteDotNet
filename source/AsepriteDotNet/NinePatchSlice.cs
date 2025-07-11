// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AsepriteDotNet.Core;

namespace AsepriteDotNet;

/// <summary>
/// Represents a slice with nine-patch data.
/// This class cannot be inherited.
/// </summary>
public sealed class NinePatchSlice : Slice, IEquatable<NinePatchSlice>
{
    /// <summary>
    /// Gets the bounds of the center of this slice.
    /// </summary>
    public Rectangle CenterBounds { get; }

    internal NinePatchSlice(string name, Rectangle bounds, Point origin, Rgba32 color, Rectangle centerBounds) :
        base(name, bounds, origin, color) => CenterBounds = centerBounds;


    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is NinePatchSlice other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] NinePatchSlice? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return base.Equals(other) && CenterBounds.Equals(other.CenterBounds);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), CenterBounds);
}
