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
    /// <param name="layers">The layers to include (case sensitive)</param>
    /// <returns>
    /// A array of <see cref="Rgba32"/> value representing the flattened frame.  If <paramref name="layers"/> is
    /// <see langword="null"/> or contains zero elements, then an empty array is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="frame"/> is <see langword="null"/>.</exception>
    public static Rgba32[] FlattenFrame(this AsepriteFrame frame, ICollection<string> layers)
    {
        ArgumentNullException.ThrowIfNull(frame);
        if (layers is null || layers.Count == 0)
        {
            return Array.Empty<Rgba32>();
        }

        Rgba32[] result = new Rgba32[frame.Size.Width * frame.Size.Height];
        HashSet<string> layerNames = new HashSet<string>(layers);
        ReadOnlySpan<AsepriteCel> cels = frame.Cels;

        for (int celNum = 0; celNum < cels.Length; celNum++)
        {
            AsepriteCel cel = cels[celNum];

            if (!layerNames.Contains(cel.Layer.Name)) { continue; }

            if (cel is AsepriteLinkedCel linkedCel)
            {
                cel = linkedCel.Cel;
            }

            if (cel is AsepriteImageCel imageCel)
            {
                BlendCel(result, imageCel.Pixels, imageCel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), frame.Size.Width, imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (cel is AsepriteTilemapCel tilemapCel)
            {
                BlendTilemapCel(result, tilemapCel, frame.Size.Width);
            }
        }

        return result;
    }

    /// <summary>
    /// Flattens the a <see cref="AsepriteFrame"/> into an array of <see cref="Rgba32"/> values.
    /// </summary>
    /// <param name="frame">The <see cref="AsepriteFrame"/> to flatten.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only cels on visible layers should be included.</param>
    /// <param name="includeBackgroundLayer">Indicates whether cels on the background layer should be included.</param>
    /// <param name="includeTilemapCels">Indicates whether tilemap cels should be included.</param>
    /// <returns>A array of <see cref="Rgba32"/> value representing the flattened frame.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="frame"/> is <see langword="null"/>.</exception>
    public static Rgba32[] FlattenFrame(this AsepriteFrame frame, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapCels = true)
    {
        ArgumentNullException.ThrowIfNull(frame);

        List<AsepriteLayer> layers = new List<AsepriteLayer>();

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
                BlendCel(result, imageCel.Pixels, imageCel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), frame.Size.Width, imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (includeTilemapCels && cel is AsepriteTilemapCel tilemapCel)
            {
                BlendTilemapCel(result, tilemapCel, frame.Size.Width);
            }
        }

        return result;
    }

    private static void BlendCel(Span<Rgba32> backdrop, ReadOnlySpan<Rgba32> source, AsepriteBlendMode blendMode, Rectangle bounds, int frameWidth, int celOpacity, int layerOpacity)
    {
        byte opacity = Calc.MultiplyUnsigned8Bit(celOpacity, layerOpacity);

        //  Sometimes a cel can have a negative x- and/or y-coordinate location, or an x- and/or y-coordinate location
        //  that extends outside of the bounds of the frame.  This is caused by selecting an area within Aseprite and
        //  then moving a portion of the selected pixels outside the canvas.  We don't care about these pixels, we only
        //  want the ones inside the frame bounds.
        //
        //  So we need to determine the starting and ending xy-coordinate locations within the pixels of the
        //  cel (backdrop) that are within the frame bounds so we only process those.
        int startX = Math.Max(0, -bounds.X);
        int startY = Math.Max(0, -bounds.Y);
        int endX = Math.Min(bounds.Width, frameWidth - bounds.X);
        int endY = Math.Min(bounds.Height, backdrop.Length / frameWidth - bounds.Y);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                int index = (y + bounds.Y) * frameWidth + (x + bounds.X);
                Rgba32 b = backdrop[index];
                Rgba32 s = source[y * bounds.Width + x]; // Index within the sliced source
                backdrop[index] = AsepriteColorUtilities.Blend(b, s, opacity, blendMode);
            }
        }
    }

    private static void BlendTilemapCel(Span<Rgba32> backdrop, AsepriteTilemapCel cel, int frameWidth)
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

        BlendCel(backdrop, pixels, cel.Layer.BlendMode, bounds, frameWidth, cel.Opacity, cel.Layer.Opacity);
    }
}

