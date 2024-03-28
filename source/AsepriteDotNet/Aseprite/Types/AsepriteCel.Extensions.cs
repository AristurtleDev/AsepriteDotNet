// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Provides extension methods for <see cref="AsepriteCel"/> instances.
/// </summary>
public static class AsepriteCelExtensions
{
    /// <summary>
    /// Extracts pixel data from an <see cref="AsepriteCel"/> and returns it as a <see cref="Texture"/>.
    /// </summary>
    /// <param name="cel">The <see cref="AsepriteCel"/> containing the pixel data to extract.</param>
    /// <param name="name">
    /// Optional name for the extracted <see cref="Texture"/>. If not provided, an empty string is used.
    /// </param>
    /// <returns>A <see cref="Texture"/> object containing the extracted pixel data.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input <see cref="AsepriteCel"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the <see cref="AsepriteCel"/> does not contain pixel data.
    /// </exception>
    public static Texture ExtractCel(this AsepriteCel cel, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(cel);


        if (cel is AsepriteImageCel imageCel)
        {
            return imageCel.ExtractCel(name);
        }

        if (cel is AsepriteTilemapCel tilemapCel)
        {
            return tilemapCel.ExtractCel(name);
        }

        throw new InvalidOperationException("This cel does not contain pixel data");
    }

    {
    /// <summary>
    /// Extracts pixel data from an <see cref="AsepriteImageCel"/> and returns it as a <see cref="Texture"/>.
    /// </summary>
    /// <param name="cel">The <see cref="AsepriteImageCel"/> containing the pixel data to extract.</param>
    /// <param name="name">
    /// Optional name for the extracted <see cref="Texture"/>. If not provided, an empty string is used.
    /// </param>
    /// <returns>A <see cref="Texture"/> object containing the extracted pixel data.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input <see cref="AsepriteImageCel"/> is <see langword="null"/>.
    /// </exception>
    public static Texture ExtractCel(this AsepriteImageCel cel, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(cel);

        return new Texture(name ?? string.Empty, cel.Size, cel.Pixels.ToArray());
    }

    /// <summary>
    /// Extracts pixel data from an <see cref="AsepriteTilemapCel"/> and returns it as a <see cref="Texture"/>.
    /// </summary>
    /// <param name="cel">The <see cref="AsepriteTilemapCel"/> containing the pixel data to extract.</param>
    /// <param name="name">
    /// Optional name for the extracted <see cref="Texture"/>. If not provided, an empty string is used.
    /// </param>
    /// <returns>A <see cref="Texture"/> object containing the extracted pixel data.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input <see cref="AsepriteTilemapCel"/> is <see langword="null"/>.
    /// </exception>
    public static Texture ExtractCel(this AsepriteTilemapCel cel, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(cel);

        AsepriteTilemapLayer layer = (AsepriteTilemapLayer)cel.Layer;
        AsepriteTileset tileset = layer.Tileset;
        Size size = new Size(cel.Size.Width * tileset.TileSize.Width, cel.Size.Height * tileset.TileSize.Height);
        Rgba32[] pixels = new Rgba32[size.Width * size.Height];

        Parallel.For(0, cel.Tiles.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
        {
            AsepriteTile tile = cel.Tiles[i];
            int column = i % cel.Size.Width;
            int row = i / cel.Size.Width;
            Parallel.For(0, tileset.Pixels.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, j =>
            {
                int px = (j % tileset.TileSize.Width) + (column + tileset.TileSize.Height);
                int py = (j / tileset.TileSize.Width) + (row + tileset.TileSize.Height);
                int index = py * size.Width + px;
                pixels[index] = tileset.Pixels[j];
            });
        });

        return new Texture(name ?? string.Empty, size, pixels);
    }
}
