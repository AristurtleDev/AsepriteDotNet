// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Provides extension methods for converting Aseprite cels into rendered images without layer composition.
/// </summary>
/// <remarks>
/// Unlike full frame rendering, these methods do not apply blend modes, opacity, or layer hierarchy - they
/// produce the raw visual content of each cel type in isolation.
/// </remarks>
public static class AsepriteCelExtensions
{
    /// <summary>
    /// Converts any cel type to a rendered image using the appropriate conversion algorithm.
    /// </summary>
    /// <param name="cel">The cel to convert to a rendered image.</param>
    /// <returns>A <see cref="RenderedImage"/> containing the cel's visual content without composition effects.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the cel type is not recognized or supported.</exception>
    /// <remarks>
    /// The resulting image contains raw pixel data without blend modes, opacity, or positioning applied.
    /// </remarks>
    public static RenderedImage ToRenderedImage(this AsepriteCel cel) => cel switch
    {
        AsepriteImageCel imageCel => ToRenderedImage(imageCel),
        AsepriteLinkedCel linkedCel => ToRenderedImage(linkedCel),
        AsepriteTilemapCel tilemapCel => ToRenderedImage(tilemapCel),
        _ => throw new InvalidOperationException("Invalid cel type")
    };

    /// <summary>
    /// Converts an image cel to a rendered image by copying its pixel data.
    /// </summary>
    /// <param name="imageCel">The image cel containing RGBA pixel data.</param>
    /// <returns>A <see cref="RenderedImage"/> with the cel's dimensions and pixel content.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="imageCel"/> is null.</exception>
    /// <remarks>
    /// Creates a new pixel array copy to ensure the rendered image is independent of the original cel data.
    /// </remarks>
    private static RenderedImage ToRenderedImage(AsepriteImageCel imageCel)
    {
        ArgumentNullException.ThrowIfNull(imageCel);
        return new RenderedImage(imageCel.Size, imageCel.Pixels.ToArray());
    }

    /// <summary>
    /// Converts a linked cel to a rendered image by resolving the link and converting the target cel.
    /// </summary>
    /// <param name="linkedCel">The linked cel that references another cel's content.</param>
    /// <returns>A <see cref="RenderedImage"/> containing the referenced cel's visual content.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="linkedCel"/> is null.</exception>
    /// <remarks>
    /// Follows the cel reference chain to obtain the actual content cel and recursively applies
    /// the appropriate conversion method. This ensures linked cels render identically to their targets.
    /// </remarks>
    private static RenderedImage ToRenderedImage(AsepriteLinkedCel linkedCel)
    {
        ArgumentNullException.ThrowIfNull(linkedCel);
        AsepriteCel originCel = linkedCel.Cel;
        return ToRenderedImage(originCel);
    }

    /// <summary>
    /// Converts a tilemap cel to a rendered image by reconstructing the tile arrangement from the associated tileset.
    /// </summary>
    /// <param name="tilemapCel">The tilemap cel containing tile indices and grid dimensions.</param>
    /// <returns>A <see cref="RenderedImage"/> with the reconstructed tile arrangement as pixel data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tilemapCel"/> is null.</exception>
    /// <remarks>
    /// Note: This method does not apply tile transformations (flips/rotations) as specified
    /// in the tile data - it renders tiles in their base orientation from the tileset.
    /// </remarks>
    private static RenderedImage ToRenderedImage(AsepriteTilemapCel tilemapCel)
    {
        ArgumentNullException.ThrowIfNull(tilemapCel);

        AsepriteTilemapLayer tilemapLayer = (AsepriteTilemapLayer)tilemapCel.Layer;
        AsepriteTileset tileset = tilemapLayer.Tileset;

        int width = tilemapCel.Size.Width * tileset.TileSize.Width;
        int height = tilemapCel.Size.Height * tileset.TileSize.Height;
        Size size = new(width, height);

        Rgba32[] pixels = new Rgba32[size.Width * size.Height];



        for (int i = 0; i < tilemapCel.Tiles.Length; i++)
        {
            int column = i % tilemapCel.Size.Width;
            int row = i / tilemapCel.Size.Width;

            for (int j = 0; j < tileset.Pixels.Length; j++)
            {
                int x = (j % tileset.TileSize.Width) + (column * tileset.TileSize.Width);
                int y = (j / tileset.TileSize.Width) + (row * tileset.TileSize.Height);
                int index = y * size.Width + x;
                pixels[index] = tileset.Pixels[j];
            }
        }

        return new RenderedImage(size, pixels);
    }
}
