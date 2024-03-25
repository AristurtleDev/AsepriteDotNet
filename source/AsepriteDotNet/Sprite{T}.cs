// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Represents a sprite texture.
/// This class cannot be inherited.
/// </summary>
public sealed class Sprite<T> : IEquatable<Sprite<T>> where T: IColor, new()
{
    private readonly Slice<T>[] _slices;

    /// <summary>
    /// Gets the name of this sprite.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture of this sprite.
    /// </summary>
    public Texture<T> Texture { get; }

    /// <summary>
    /// Gets a read-only collection of the slices contained within this sprite.
    /// </summary>
    public IReadOnlyCollection<Slice<T>> Slices => _slices;

    internal Sprite(string name, Texture<T> texture, Slice<T>[] slices) =>
        (Name, Texture, _slices) = (name, texture, slices);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Sprite<T> other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Sprite<T>? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.OrdinalIgnoreCase)
            && Texture.Equals(other.Texture);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, Texture);
}


