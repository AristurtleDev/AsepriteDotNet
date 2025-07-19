// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

public static class AsepriteCelExtensions
{
    public static RenderedImage ToRenderedImage(this AsepriteCel cel) => cel switch
    {
        AsepriteImageCel imageCel => ToRenderedImage(imageCel),
        AsepriteLinkedCel linkedCel => ToRenderedImage(linkedCel),
        AsepriteTilemapCel tilemapCel => ToRenderedImage(tilemapCel),
        _ => throw new InvalidOperationException("Invalid cel type")
    };

    private static RenderedImage ToRenderedImage(AsepriteImageCel imageCel)
    {
        ArgumentNullException.ThrowIfNull(imageCel);
        return new RenderedImage(imageCel.Size, imageCel.Pixels.ToArray());
    }

    private static RenderedImage ToRenderedImage(AsepriteLinkedCel linkedCel)
    {
        ArgumentNullException.ThrowIfNull(linkedCel);
        AsepriteCel originCel = linkedCel.Cel;
        return ToRenderedImage(originCel);
    }

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
