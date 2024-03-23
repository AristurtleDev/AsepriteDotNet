// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a texture atlas that is composed of a texture with defined texture regions.
/// This class cannot be inherited.
/// </summary>
public sealed class TextureAtlas<TColor> : IEquatable<TextureAtlas<TColor>> where TColor : struct, IColor<TColor>
{
    private readonly TextureRegion<TColor>[] _regions;

    /// <summary>
    /// Gets the name of this texture atlas.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture of this texture atlas.
    /// </summary>
    public Texture<TColor> Texture { get; }

    /// <summary>
    /// Gets a read-only collection of the texture regions within this atlas.
    /// </summary>
    public ReadOnlySpan<TextureRegion<TColor>> Regions => _regions;

    internal TextureAtlas(string name, Texture<TColor> texture, TextureRegion<TColor>[] regions) =>
        (Name, Texture, _regions) = (name, texture, regions);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TextureAtlas<TColor> other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] TextureAtlas<TColor>? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && Texture.Equals(other.Texture)
            && _regions.SequenceEqual(other._regions);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, Texture, _regions);
}
