// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a texture that is composed of color values that represent an image.
/// This class cannot be inherited.
/// </summary>
public sealed class Texture : IEquatable<Texture>
{
    /// <summary>
    /// An empty texture, with the no name, 0 width, 0, height, and an empty array of pixels
    /// </summary>
    public static readonly Texture Empty = new Texture(string.Empty, Size.Empty, Array.Empty<Rgba32>());

    private readonly Rgba32[] _pixels;

    /// <summary>
    /// Gets the name of this texture.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the size of the texture.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets a read-only collection of the pixels that represent the image of this texture.  Pixels are read from
    /// left-to-right top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Rgba32> Pixels => _pixels;

    internal Texture(string name, Size size, Rgba32[] pixels) => (Name, Size, _pixels) = (name, size, pixels);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Texture other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Texture? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.Ordinal)
            && Size.Equals(other.Size)
            && _pixels.SequenceEqual(other._pixels);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, Size, _pixels);
}
