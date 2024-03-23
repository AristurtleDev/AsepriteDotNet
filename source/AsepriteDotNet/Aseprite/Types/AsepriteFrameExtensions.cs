// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.


using System.Diagnostics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines extension methods for the <see cref="AsepriteFrame{TColor}"/> type.
/// </summary>
public static class AsepriteFrameExtensions
{
    /// <summary>
    /// Flattens the a <see cref="AsepriteFrame{TColor}"/> into an array of <typeparamref name="TColor"/> values.
    /// </summary>
    /// <param name="frame">The <see cref="AsepriteFrame{TColor}"/> to flatten.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only cels on visible layers should be included.</param>
    /// <param name="includeBackgroundLayer">Indicates if the layer marked background should be included.</param>
    /// <param name="includeTilemapCels">Indicates if tile map cels should be included.</param>
    /// <typeparam name="TColor">The color type.</typeparam>
    /// <returns>A array of <typeparamref name="TColor"/> value representing the flattened frame.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="frame"/> is <see langword="null"/>.</exception>
    public static TColor[] FlattenFrame<TColor>(this AsepriteFrame<TColor> frame, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapCels = true)
        where TColor : struct, IColor<TColor>
    {
        ArgumentNullException.ThrowIfNull(frame);

        TColor[] result = new TColor[frame.Size.Width * frame.Size.Height];
        ReadOnlySpan<AsepriteCel<TColor>> cels = frame.Cels;

        for (int celNum = 0; celNum < cels.Length; celNum++)
        {
            AsepriteCel<TColor> cel = cels[celNum];
            if (cel is AsepriteLinkedCel<TColor> linkedCel)
            {
                cel = linkedCel.Cel;
            }

            if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }
            if (cel.Layer.IsBackgroundLayer && !includeBackgroundLayer) { continue; }

            if (cel is AsepriteImageCel<TColor> imageCel)
            {
                BlendCel<TColor>(result, imageCel.Pixels, imageCel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (includeTilemapCels && cel is AsepriteTilemapCel<TColor> tilemapCel)
            {
                BlendTilemapCel<TColor>(result, tilemapCel);
            }
        }

        return result;
    }

    private static void BlendCel<TColor>(Span<TColor> backdrop, ReadOnlySpan<TColor> source, AsepriteBlendMode blendMode, Rectangle bounds, int celOpacity, int layerOpacity)
            where TColor : struct, IColor<TColor>
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

            TColor b = backdrop[index];
            TColor s = source[i];
            TColor blended = AsepriteColorUtilities.Blend(b, s, opacity, blendMode);
            backdrop[index] = blended;
        }
    }

    private static void BlendTilemapCel<TColor>(Span<TColor> backdrop, AsepriteTilemapCel<TColor> cel)
            where TColor : struct, IColor<TColor>
    {
        byte opacity = Calc.MultiplyUnsigned8Bit(cel.Opacity, cel.Layer.Opacity);

        Debug.Assert(cel.Layer is AsepriteTilemapLayer<TColor>);
        AsepriteTilemapLayer<TColor> aseTilemapLayer = (AsepriteTilemapLayer<TColor>)cel.Layer;

        AsepriteTileset<TColor> tileset = aseTilemapLayer.Tileset;
        Rectangle bounds;
        bounds.Width = cel.Size.Width * tileset.Size.Width;
        bounds.Height = cel.Size.Height * tileset.Size.Height;
        bounds.X = cel.Location.X;
        bounds.Y = cel.Location.Y;

        Span<TColor> pixels = new TColor[bounds.Width * bounds.Height];
        ReadOnlySpan<AsepriteTile> tiles = cel.Tiles;
        for (int i = 0; i < tiles.Length; i++)
        {
            AsepriteTile tile = tiles[i];
            int column = i % cel.Size.Width;
            int row = i / cel.Size.Width;
            ReadOnlySpan<TColor> tilePixels = tileset.Pixels;

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

