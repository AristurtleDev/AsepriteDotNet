using System.Drawing;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core;

/// <summary>
/// Provides extension methods for rendering frames from <see cref="AsepriteFile"/>.
/// </summary>
public static class AsepriteFileExtensions
{
    /// <summary>
    /// Renders a frame as an array of color values using all layers.
    /// </summary>
    /// <param name="file">The Aseprite file to render from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to render.</param>
    /// <returns>A span of RGBA color values representing the rendered frame, ordered left-to-right, top-to-bottom.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is negative or greater than or equal to the number of frames.
    /// </exception>
    public static ReadOnlySpan<Rgba32> RenderFrame(this AsepriteFile file, int frameIndex)
    {
        LayerSelector layerSelector = LayerSelector.AllLayers();
        return RenderFrame(file, frameIndex, layerSelector);
    }

    /// <summary>
    /// Renders a frame as an array of color values using the specified layer selector.
    /// </summary>
    /// <param name="file">The Aseprite file to render from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to render.</param>
    /// <param name="layerSelector">The strategy for selecting which layers to include in the rendering.</param>
    /// <returns>A span of RGBA color values representing the rendered frame, ordered left-to-right, top-to-bottom.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> or <paramref name="layerSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is negative or greater than or equal to the number of frames.
    /// </exception>
    public static ReadOnlySpan<Rgba32> RenderFrame(this AsepriteFile file, int frameIndex, LayerSelector layerSelector)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(layerSelector);
        ArgumentOutOfRangeException.ThrowIfNegative(frameIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(frameIndex, file.Frames.Length);

        AsepriteFrame frame = file.Frames[frameIndex];
        return RenderFrame(file, frame, layerSelector);
    }

    /// <summary>
    /// Renders a frame as an array of color values using all layers.
    /// </summary>
    /// <param name="file">The Aseprite file to render from.</param>
    /// <param name="frame">The frame to render.</param>
    /// <returns>A span of RGBA color values representing the rendered frame, ordered left-to-right, top-to-bottom.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> or <paramref name="frame"/> is <see langword="null"/>.</exception>
    public static ReadOnlySpan<Rgba32> RenderFrame(this AsepriteFile file, AsepriteFrame frame)
    {
        LayerSelector layerSelector = LayerSelector.AllLayers();
        return RenderFrame(file, frame, layerSelector);
    }

    /// <summary>
    /// Renders a frame as an array of color values using the specified layer selector.
    /// </summary>
    /// <param name="file">The Aseprite file to render from.</param>
    /// <param name="frame">The frame to render.</param>
    /// <param name="layerSelector">The strategy for selecting which layers to include in the rendering.</param>
    /// <returns>A span of RGBA color values representing the rendered frame, ordered left-to-right, top-to-bottom.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/>, <paramref name="frame"/>, or <paramref name="layerSelector"/> is <see langword="null"/>.</exception>
    public static ReadOnlySpan<Rgba32> RenderFrame(this AsepriteFile file, AsepriteFrame frame, LayerSelector layerSelector)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(frame);
        ArgumentNullException.ThrowIfNull(layerSelector);

        ReadOnlySpan<AsepriteLayer> selectedLayers = layerSelector.SelectLayers(file.Layers);

        // Using HashSet for O(1) layer lookup up when checking for contains later.
        HashSet<AsepriteLayer> layers = new(selectedLayers.ToArray());

        if (layers.Count == 0)
        {
            // If no layers, return empty Rgba32 array
            return [];
        }


        Rgba32[] result = new Rgba32[frame.Size.Width * frame.Size.Height];
        ReadOnlySpan<AsepriteCel> cels = frame.Cels;

        for (int celNum = 0; celNum < cels.Length; celNum++)
        {
            AsepriteCel cel = cels[celNum];

            if (!layers.Contains(cel.Layer))
            {
                // Only process cells that are on layers specified.
                continue;
            }

            // If the cel is a linked cel, we need to get the original cel
            if (cel is AsepriteLinkedCel linkedCel)
            {
                cel = linkedCel.Cel;
            }

            if (cel is AsepriteImageCel imageCel)
            {
                BlendCel(result, imageCel.Pixels, cel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), frame.Size.Width, imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (cel is AsepriteTilemapCel tilemapCel)
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

        AsepriteTilemapLayer aseTilemapLayer = (AsepriteTilemapLayer)cel.Layer;

        AsepriteTileset tileset = aseTilemapLayer.Tileset;
        Rectangle bounds = new()
        {
            Width = cel.Size.Width * tileset.TileSize.Width,
            Height = cel.Size.Height * tileset.TileSize.Height,
            X = cel.Location.X,
            Y = cel.Location.Y
        };

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
                int px = (j % tileset.TileSize.Width) + (column * tileset.TileSize.Width);
                int py = (j / tileset.TileSize.Width) + (row * tileset.TileSize.Height);
                int index = py * bounds.Width + px;
                pixels[index] = tilePixels[j];
            }
        }

        BlendCel(backdrop, pixels, cel.Layer.BlendMode, bounds, frameWidth, cel.Opacity, cel.Layer.Opacity);
    }
}
