// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Defines a sprite sheet that contains a texture atlas and animation tag definitions.
/// This class cannot be inherited.
/// </summary>
public sealed class SpriteSheet : IEquatable<SpriteSheet>
{
    /// <summary>
    /// An empty sprite sheet with no name, an empty texture atlas, and an empty collection of animation tags.
    /// </summary>
    public static readonly SpriteSheet Empty = new SpriteSheet(string.Empty, TextureAtlas.Empty, Array.Empty<AnimationTag>());

    private readonly AnimationTag[] _tags;
    /// <summary>
    /// Gets the name of this sprite sheet.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture atlas of this sprite sheet.
    /// </summary>
    public TextureAtlas TextureAtlas { get; }

    /// <summary>
    /// Gets a read-only collection of the animation tags for this sprite sheet.
    /// </summary>
    public ReadOnlySpan<AnimationTag> Tags => _tags;


    internal SpriteSheet(string name, TextureAtlas textureAtlas, AnimationTag[] tags) =>
        (Name, TextureAtlas, _tags) = (name, textureAtlas, tags);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is SpriteSheet other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] SpriteSheet? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && TextureAtlas.Equals(other.TextureAtlas)
            && _tags.SequenceEqual(other._tags);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, TextureAtlas, _tags);
}

