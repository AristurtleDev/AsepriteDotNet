// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="Tileset"/> from an <see cref="AsepriteFile"/>.
/// </summary>
public static class TilesetProcessor
{
    /// <summary>
    /// Processes an <see cref="Tileset"/> from an <see cref="AsepriteFile"/> by index.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="tilesetIndex">The index of the tileset to process.</param>
    /// <returns>The <see cref="Tileset"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="tilesetIndex"/> is less than zero or is greater than or equal to the total number
    /// of tilesets in the file.
    /// </exception>
    public static Tileset Process(AsepriteFile file, int tilesetIndex)
    {
        ArgumentNullException.ThrowIfNull(file);
        AsepriteTileset aseTileset = file.Tilesets[tilesetIndex];
        return Process(aseTileset);
    }

    /// <summary>
    /// Processes an <see cref="Tileset"/> from an <see cref="AsepriteFile"/> by name.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="tilesetName">The name of the tileset to process.</param>
    /// <returns>The <see cref="Tileset"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the file does not contain a tileset with the specified name.
    /// </exception>
    public static Tileset Process(AsepriteFile file, string tilesetName)
    {
        ArgumentNullException.ThrowIfNull(file);
        AsepriteTileset aseTileset = file.GetTileset(tilesetName);
        return Process(aseTileset);
    }

    /// <summary>
    /// Processes a <see cref="Tileset"/> from an <see cref="AsepriteTileset"/>.
    /// </summary>
    /// <param name="aseTileset">The <see cref="AsepriteTileset"/> to process.</param>
    /// <returns>The <see cref="Tileset"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="aseTileset"/> is <see langword="null"/>.
    /// </exception>
    public static Tileset Process(AsepriteTileset aseTileset)
    {
        ArgumentNullException.ThrowIfNull(aseTileset);
        //  Aseprite tile maps are stored in horizontal strips
        Size textureSize = new Size(aseTileset.TileSize.Width, aseTileset.TileSize.Height * aseTileset.TileCount);
        Texture texture = new Texture(aseTileset.Name, textureSize, aseTileset.Pixels.ToArray());
        return new Tileset(aseTileset.ID, aseTileset.Name, texture, aseTileset.TileSize);
    }
}
