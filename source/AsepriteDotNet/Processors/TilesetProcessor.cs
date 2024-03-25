// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="Tileset{T}"/> from an <see cref="AsepriteFile{T}"/>.
/// </summary>
public static class TilesetProcessor
{
    /// <summary>
    /// Processes an <see cref="Tileset{T}"/> from an <see cref="AsepriteFile{T}"/> by index.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process.</param>
    /// <param name="tilesetIndex">The index of the tileset to process.</param>
    /// <returns>The <see cref="Tileset{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="tilesetIndex"/> is less than zero or is greater than or equal to the total number
    /// of tilesets in the file.
    /// </exception>
    public static Tileset<T> Process<T>(AsepriteFile<T> file, int tilesetIndex)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(file);
        AsepriteTileset<T> aseTileset = file.Tilesets[tilesetIndex];
        return Process(aseTileset);
    }

    /// <summary>
    /// Processes an <see cref="Tileset{T}"/> from an <see cref="AsepriteFile{T}"/> by name.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process.</param>
    /// <param name="tilesetName">The name of the tileset to process.</param>
    /// <returns>The <see cref="Tileset{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the file does not contain a tileset with the specified name.
    /// </exception>
    public static Tileset<T> Process<T>(AsepriteFile<T> file, string tilesetName)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(file);
        AsepriteTileset<T> aseTileset = file.GetTileset(tilesetName);
        return Process(aseTileset);
    }

    /// <summary>
    /// Processes a <see cref="Tileset{T}"/> from an <see cref="AsepriteTileset{T}"/>.
    /// </summary>
    /// <param name="aseTileset">The <see cref="AsepriteTileset{T}"/> to process.</param>
    /// <returns>The <see cref="Tileset{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="aseTileset"/> is <see langword="null"/>.
    /// </exception>
    public static Tileset<T> Process<T>(AsepriteTileset<T> aseTileset)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(aseTileset);
        Texture<T> texture = new Texture<T>(aseTileset.Name, aseTileset.Size, aseTileset.Pixels.ToArray());
        return new Tileset<T>(aseTileset.ID, aseTileset.Name, texture, aseTileset.Size);
    }
}
