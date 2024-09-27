// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Defines a texture atlas that is composed of a texture with defined texture regions.
/// This class cannot be inherited.
/// </summary>
public sealed class TextureAtlas : IEquatable<TextureAtlas>
{
    /// <summary>
    /// An empty texture atlas with no name, an empty texture, and an empty collection of regions.
    /// </summary>
    public static readonly TextureAtlas Empty = new TextureAtlas(string.Empty, Texture.Empty, Array.Empty<TextureRegion>());

    private readonly TextureRegion[] _regions;

    /// <summary>
    /// Gets the name of this texture atlas.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture of this texture atlas.
    /// </summary>
    public Texture Texture { get; }

    /// <summary>
    /// Gets a read-only collection of the texture regions within this atlas.
    /// </summary>
    public ReadOnlySpan<TextureRegion> Regions => _regions;

    internal TextureAtlas(string name, Texture texture, TextureRegion[] regions) =>
        (Name, Texture, _regions) = (name, texture, regions);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is TextureAtlas other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] TextureAtlas? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && Texture.Equals(other.Texture)
            && _regions.SequenceEqual(other._regions);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, Texture, _regions);
}
