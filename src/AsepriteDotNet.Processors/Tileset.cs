// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Defines a tileset composed of source texture.
/// This class cannot be inherited.
/// </summary>
public sealed class Tileset : IEquatable<Tileset>
{
    /// <summary>
    /// Gets the unique ID assigned to this tileset.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets the name of this tileset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture of this tileset.
    /// </summary>
    public Texture Texture { get; }

    /// <summary>
    /// Gets the size of each tile in the tileset.
    /// </summary>
    public Size TileSize { get; }

    internal Tileset(int id, string name, Texture texture, Size tileSize) =>
        (ID, Name, Texture, TileSize) = (id, name, texture, tileSize);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Tileset other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] Tileset? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return ID.Equals(other?.ID)
            && Name.Equals(other.Name, StringComparison.Ordinal)
            && Texture.Equals(other.Texture)
            && TileSize.Equals(other.TileSize);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(ID, Name, Texture, TileSize);
}
