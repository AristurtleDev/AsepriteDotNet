// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.


using System.Diagnostics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines extension methods for the <see cref="AsepriteFrame{T}"/> type.
/// </summary>
public static class AsepriteFrameExtensions
{
    /// <summary>
    /// Flattens the a <see cref="AsepriteFrame{T}"/> into an array of <typeparamref name="T"/> values.
    /// </summary>
    /// <param name="frame">The <see cref="AsepriteFrame{T}"/> to flatten.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only cels on visible layers should be included.</param>
    /// <param name="includeBackgroundLayer">Indicates if the layer marked background should be included.</param>
    /// <param name="includeTilemapCels">Indicates if tile map cels should be included.</param>
    /// <typeparam name="T">The color type.</typeparam>
    /// <returns>A array of <typeparamref name="T"/> value representing the flattened frame.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="frame"/> is <see langword="null"/>.</exception>
    public static T[] FlattenFrame<T>(this AsepriteFrame<T> frame, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapCels = true)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(frame);

        T[] result = new T[frame.Size.Width * frame.Size.Height];
        ReadOnlySpan<AsepriteCel<T>> cels = frame.Cels;

        for (int celNum = 0; celNum < cels.Length; celNum++)
        {
            AsepriteCel<T> cel = cels[celNum];
            if (cel is AsepriteLinkedCel<T> linkedCel)
            {
                cel = linkedCel.Cel;
            }

            if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }
            if (cel.Layer.IsBackgroundLayer && !includeBackgroundLayer) { continue; }

            if (cel is AsepriteImageCel<T> imageCel)
            {
                BlendCel<T>(result, imageCel.Pixels, imageCel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (includeTilemapCels && cel is AsepriteTilemapCel<T> tilemapCel)
            {
                BlendTilemapCel<T>(result, tilemapCel);
            }
        }

        return result;
    }

    private static void BlendCel<T>(Span<T> backdrop, ReadOnlySpan<T> source, AsepriteBlendMode blendMode, Rectangle bounds, int celOpacity, int layerOpacity)
            where T: IColor, new()
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

            T b = backdrop[index];
            T s = source[i];
            T blended = AsepriteBlendFunctions.Blend<T>(b, s, opacity, blendMode);
            backdrop[index] = blended;
        }
    }

    private static void BlendTilemapCel<T>(Span<T> backdrop, AsepriteTilemapCel<T> cel)
            where T: IColor, new()
    {
        byte opacity = Calc.MultiplyUnsigned8Bit(cel.Opacity, cel.Layer.Opacity);

        Debug.Assert(cel.Layer is AsepriteTilemapLayer<T>);
        AsepriteTilemapLayer<T> aseTilemapLayer = (AsepriteTilemapLayer<T>)cel.Layer;

        AsepriteTileset<T> tileset = aseTilemapLayer.Tileset;
        Rectangle bounds;
        bounds.Width = cel.Size.Width * tileset.Size.Width;
        bounds.Height = cel.Size.Height * tileset.Size.Height;
        bounds.X = cel.Location.X;
        bounds.Y = cel.Location.Y;

        Span<T> pixels = new T[bounds.Width * bounds.Height];
        ReadOnlySpan<AsepriteTile> tiles = cel.Tiles;
        for (int i = 0; i < tiles.Length; i++)
        {
            AsepriteTile tile = tiles[i];
            int column = i % cel.Size.Width;
            int row = i / cel.Size.Width;
            ReadOnlySpan<T> tilePixels = tileset.Pixels;

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

