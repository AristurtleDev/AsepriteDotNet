// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a sprite sheet that contains a texture atlas and animation tag definitions.
/// This class cannot be inherited.
/// </summary>
public sealed class SpriteSheet<TColor> : IEquatable<SpriteSheet<TColor>> where TColor : struct, IColor<TColor>
{
    private readonly AnimationTag[] _tags;
    /// <summary>
    /// Gets the name of this sprite sheet.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture atlas of this sprite sheet.
    /// </summary>
    public TextureAtlas<TColor> TextureAtlas { get; }

    /// <summary>
    /// Gets a read-only collection of the animation tags for this sprite sheet.
    /// </summary>
    public ReadOnlySpan<AnimationTag> Tags => _tags;


    internal SpriteSheet(string name, TextureAtlas<TColor> textureAtlas, AnimationTag[] tags) =>
        (Name, TextureAtlas, _tags) = (name, textureAtlas, tags);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is SpriteSheet<TColor> other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] SpriteSheet<TColor>? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && TextureAtlas.Equals(other.TextureAtlas)
            && _tags.SequenceEqual(other._tags);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, TextureAtlas, _tags);
}

