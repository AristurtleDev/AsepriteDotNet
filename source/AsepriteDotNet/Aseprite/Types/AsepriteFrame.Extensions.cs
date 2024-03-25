// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.


using System.Diagnostics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines extension methods for the <see cref="AsepriteFrame"/> type.
/// </summary>
public static class AsepriteFrameExtensions
{
    /// <summary>
    /// Flattens the a <see cref="AsepriteFrame"/> into an array of <see cref="Rgba32"/> values.
    /// </summary>
    /// <param name="frame">The <see cref="AsepriteFrame"/> to flatten.</param>
    /// <param name="onlyVisibleLayers">
    /// Indicates whether only cels on visible layers should be included.
    /// </param>
    /// <returns>A array of <see cref="Rgba32"/> value representing the flattened frame.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="frame"/> is <see langword="null"/>.</exception>
    public static Rgba32[] FlattenFrame(this AsepriteFrame frame, bool onlyVisibleLayers, bool includeBackgroundLayer, bool includeTilemapCels = true)
    {
        ArgumentNullException.ThrowIfNull(frame);

        Rgba32[] result = new Rgba32[frame.Size.Width * frame.Size.Height];
        ReadOnlySpan<AsepriteCel> cels = frame.Cels;

        for (int celNum = 0; celNum < cels.Length; celNum++)
        {
            AsepriteCel cel = cels[celNum];
            if (cel is AsepriteLinkedCel linkedCel)
            {
                cel = linkedCel.Cel;
            }

            if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }
            if (cel.Layer.IsBackgroundLayer && !includeBackgroundLayer) { continue; }

            if (cel is AsepriteImageCel imageCel)
            {
                BlendCel(result, imageCel.Pixels, imageCel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (includeTilemapCels && cel is AsepriteTilemapCel tilemapCel)
            {
                BlendTilemapCel(result, tilemapCel);
            }
        }

        return result;
    }

    private static void BlendCel(Span<Rgba32> backdrop, ReadOnlySpan<Rgba32> source, AsepriteBlendMode blendMode, Rectangle bounds, int celOpacity, int layerOpacity)
    {
        byte opacity = Calc.MultiplyUnsigned8Bit(celOpacity, layerOpacity);

        for (int i = 0; i < source.Length; i++)
        {
            int x = (i % bounds.Width) + bounds.X;
            int y = (i / bounds.Width) + bounds.Y;
            int index = y * bounds.Width + x;

            //  Sometimes a cel can have a negative x and/or y value.  This is caused by selecting an area within
            //  aseprite and then moving a portion of the selected pixels outside the canvas.  We don't care about
            //  these pixels, so if the index is outside the range of the array to store them in, we'll just
            //  discard them
            if (index < 0 || index > backdrop.Length) { continue; }

            Rgba32 b = backdrop[index];
            Rgba32 s = source[i];
            backdrop[index] = AsepriteColorUtilities.Blend(b, s, opacity, blendMode);
        }
    }

    private static void BlendTilemapCel(Span<Rgba32> backdrop, AsepriteTilemapCel cel)
    {
        byte opacity = Calc.MultiplyUnsigned8Bit(cel.Opacity, cel.Layer.Opacity);

        Debug.Assert(cel.Layer is AsepriteTilemapLayer);
        AsepriteTilemapLayer aseTilemapLayer = (AsepriteTilemapLayer)cel.Layer;

        AsepriteTileset tileset = aseTilemapLayer.Tileset;
        Rectangle bounds;
        bounds.Width = cel.Size.Width * tileset.Size.Width;
        bounds.Height = cel.Size.Height * tileset.Size.Height;
        bounds.X = cel.Location.X;
        bounds.Y = cel.Location.Y;

        Span<Rgba32> pixels = new Rgba32[bounds.Width * bounds.Height];
        ReadOnlySpan<AsepriteTile> tiles = cel.Tiles;
        for (int i = 0; i < tiles.Length; i++)
        {
            AsepriteTile tile = tiles[i];
            int column = i % cel.Size.Width;
            int row = i / cel.Size.Width;
            ReadOnlySpan<Rgba32> tilePixels = tileset.Pixels;

            for (int j = 0; j < tilePixels.Length; j++)
            {
                int px = (j % tileset.Size.Width) + (column * tileset.Size.Height);
                int py = (j / tileset.Size.Width) + (row * tileset.Size.Height);
                int index = py * bounds.Width + px;
                pixels[index] = tilePixels[j];
            }
        }

        BlendCel(backdrop, pixels, cel.Layer.BlendMode, bounds, cel.Opacity, cel.Layer.Opacity);
    }
}

