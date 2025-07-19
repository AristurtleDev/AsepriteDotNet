using System.Drawing;
using AsepriteDotNet.Types;

namespace AsepriteDotNet;

/// <summary>
/// Provides extension methods for rendering Aseprite sprites into raster images with layer composition and cel blending.
/// </summary>
/// <remarks>
/// Implements the complete Aseprite rendering pipeline including layer hierarchy processing, cel composition,
/// blend mode calculations, and opacity handling. Supports both image and tilemap cel types with proper
/// coordinate clipping for cels that extend beyond canvas bounds.
/// </remarks>
public static class AsepriteFileExtensions
{
    /// <summary>
    /// Renders a single frame using only visible layers and default composition settings.
    /// </summary>
    /// <param name="file">The Aseprite file containing the frame data.</param>
    /// <param name="frameIndex">The zero-based index of the frame to render.</param>
    /// <returns>A <see cref="RenderedImage"/> containing the composited frame data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="frameIndex"/> is negative or exceeds the frame count.</exception>
    /// <remarks>
    /// Convenience method that automatically selects visible layers and performs standard frame composition.
    /// Equivalent to calling RenderFrame with LayerSelector.VisibleLayers().
    /// </remarks>
    public static RenderedImage RenderFrame(this AsepriteFile file, int frameIndex)
    {
        LayerSelector layerSelector = LayerSelector.VisibleLayers();
        return RenderFrame(file, frameIndex, layerSelector);
    }

    /// <summary>
    /// Renders a single frame using the specified layer selection criteria.
    /// </summary>
    /// <param name="file">The Aseprite file containing the frame data.</param>
    /// <param name="frameIndex">The zero-based index of the frame to render.</param>
    /// <param name="layerSelector">The selector that determines which layers to include in rendering.</param>
    /// <returns>A <see cref="RenderedImage"/> containing the composited frame data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> or <paramref name="layerSelector"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="frameIndex"/> is negative or exceeds the frame count.</exception>
    /// <remarks>
    /// Provides precise control over which layers are included in the final rendering through
    /// custom layer selection criteria such as name-based filtering or visibility states.
    /// </remarks>
    public static RenderedImage RenderFrame(this AsepriteFile file, int frameIndex, LayerSelector layerSelector)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(layerSelector);
        ArgumentOutOfRangeException.ThrowIfNegative(frameIndex);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(frameIndex, file.Frames.Length);

        AsepriteFrame frame = file.Frames[frameIndex];
        return RenderFrame(file, frame, layerSelector);
    }

    /// <summary>
    /// Renders the specified frame using only visible layers and default composition settings.
    /// </summary>
    /// <param name="file">The Aseprite file containing the frame data.</param>
    /// <param name="frame">The specific frame instance to render.</param>
    /// <returns>A <see cref="RenderedImage"/> containing the composited frame data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> or <paramref name="frame"/> is null.</exception>
    /// <remarks>
    /// Convenience method for rendering when you already have a frame reference.
    /// Automatically selects visible layers for standard composition workflow.
    /// </remarks>
    public static RenderedImage RenderFrame(this AsepriteFile file, AsepriteFrame frame)
    {
        LayerSelector layerSelector = LayerSelector.VisibleLayers();
        return RenderFrame(file, frame, layerSelector);
    }

    /// <summary>
    /// Renders the specified frame using the provided layer selection criteria.
    /// </summary>
    /// <param name="file">The Aseprite file containing the frame data.</param>
    /// <param name="frame">The specific frame instance to render.</param>
    /// <param name="layerSelector">The selector that determines which layers to include in rendering.</param>
    /// <returns>A <see cref="RenderedImage"/> containing the composited frame data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/>, <paramref name="frame"/>, or <paramref name="layerSelector"/> is null.</exception>
    /// <remarks>
    /// <para>Performs complete frame composition with the following algorithm:</para>
    /// <list type="number">
    /// <item><description>Filter layers based on the provided selector criteria</description></item>
    /// <item><description>Process cels in their original order from the file</description></item>
    /// <item><description>Resolve linked cel references to their target content</description></item>
    /// <item><description>Composite each cel using appropriate blend modes and opacity</description></item>
    /// <item><description>Handle coordinate clipping for cels extending beyond canvas bounds</description></item>
    /// </list>
    /// <para>Returns an empty image if no layers are selected for rendering.</para>
    /// </remarks>
    public static RenderedImage RenderFrame(this AsepriteFile file, AsepriteFrame frame, LayerSelector layerSelector)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(frame);
        ArgumentNullException.ThrowIfNull(layerSelector);

        ReadOnlySpan<AsepriteLayer> selectedLayers = layerSelector.SelectLayers(file.Layers);

        // Using HashSet for O(1) layer lookup up when checking for contains later.
        HashSet<AsepriteLayer> layers = new(selectedLayers.ToArray());

        if (layers.Count == 0)
        {
            // If no layers, return empty.
            return RenderedImage.Empty;
        }


        Rgba32[] pixels = new Rgba32[frame.Size.Width * frame.Size.Height];
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
                BlendCel(pixels, imageCel.Pixels, cel.Layer.BlendMode, new Rectangle(imageCel.Location, imageCel.Size), frame.Size.Width, imageCel.Opacity, imageCel.Layer.Opacity);
            }
            else if (cel is AsepriteTilemapCel tilemapCel)
            {
                BlendTilemapCel(pixels, tilemapCel, frame.Size.Width);
            }
        }

        return new RenderedImage(frame.Size, pixels);
    }

    /// <summary>
    /// Renders multiple frames selected by the frame selector using visible layers.
    /// </summary>
    /// <param name="file">The Aseprite file containing the frame data.</param>
    /// <param name="frameSelector">The selector that determines which frames to render.</param>
    /// <returns>A list of <see cref="RenderedImage"/> instances in the same order as the selected frames.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> or <paramref name="frameSelector"/> is null.</exception>
    /// <remarks>
    /// Convenience method for batch rendering operations such as animation export or sprite sheet generation.
    /// Each frame is rendered independently using the same visible layer criteria.
    /// </remarks>
    public static List<RenderedImage> RenderFrames(this AsepriteFile file, FrameSelector frameSelector)
    {
        LayerSelector layerSelector = LayerSelector.VisibleLayers();
        return RenderFrames(file, frameSelector, layerSelector);
    }

    /// <summary>
    /// Renders multiple frames using the specified frame and layer selection criteria.
    /// </summary>
    /// <param name="file">The Aseprite file containing the frame data.</param>
    /// <param name="frameSelector">The selector that determines which frames to render.</param>
    /// <param name="layerSelector">The selector that determines which layers to include in each frame.</param>
    /// <returns>A list of <see cref="RenderedImage"/> instances in the same order as the selected frames.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/>, <paramref name="frameSelector"/>, or <paramref name="layerSelector"/> is null.</exception>
    /// <remarks>
    /// Provides comprehensive control over batch rendering operations with consistent layer filtering
    /// applied across all selected frames. Useful for exporting animation sequences with specific
    /// layer combinations or generating sprite sheets with customized content.
    /// </remarks>
    public static List<RenderedImage> RenderFrames(this AsepriteFile file, FrameSelector frameSelector, LayerSelector layerSelector)
    {
        ArgumentNullException.ThrowIfNull(file);
        ArgumentNullException.ThrowIfNull(frameSelector);
        ArgumentNullException.ThrowIfNull(layerSelector);

        ReadOnlySpan<AsepriteFrame> selectedFrames = frameSelector.SelectFrames(file.Frames, file.Tags);
        List<RenderedImage> results = [];

        for (int i = 0; i < selectedFrames.Length; i++)
        {
            RenderedImage renderedImage = RenderFrame(file, selectedFrames[i], layerSelector);
            results.Add(renderedImage);
        }

        return results;
    }

    /// <summary>
    /// Composites an image cel onto the backdrop using the specified blend mode and opacity calculations.
    /// </summary>
    /// <param name="backdrop">The destination pixel buffer to composite onto.</param>
    /// <param name="source">The source pixel data from the cel.</param>
    /// <param name="blendMode">The blend mode to use for pixel combination.</param>
    /// <param name="bounds">The rectangular area defining cel position and dimensions.</param>
    /// <param name="frameWidth">The width of the frame canvas for coordinate calculations.</param>
    /// <param name="celOpacity">The opacity level of the cel (0-255).</param>
    /// <param name="layerOpacity">The opacity level of the layer (0-255).</param>
    /// <remarks>
    /// <para>Implements proper clipping for cels that extend beyond canvas bounds, which occurs when
    /// users move selected pixels outside the visible area in Aseprite. Only pixels within the
    /// frame bounds are processed to maintain performance and correctness.</para>
    /// <para>Combined opacity is calculated using fixed-point arithmetic: celOpacity × layerOpacity ÷ 255.</para>
    /// <para>Supports all Aseprite blend modes including mathematical operations like multiply, screen,
    /// overlay, and color space operations like hue, saturation, and luminosity blending.</para>
    /// </remarks>
    private static void BlendCel(Span<Rgba32> backdrop, ReadOnlySpan<Rgba32> source, AsepriteBlendMode blendMode, Rectangle bounds, int frameWidth, int celOpacity, int layerOpacity)
    {
        byte opacity = Calc.MultiplyUnsigned8Bit(celOpacity, layerOpacity);

        //  Sometimes a cel can have a negative x- and/or y-coordinate location, or an x- and/or y-coordinate
        //  location that extends outside of the bounds of the frame.  This is caused by selecting an area within Aseprite and
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

    /// <summary>
    /// Renders a tilemap cel by reconstructing the tile arrangement and compositing it onto the backdrop.
    /// </summary>
    /// <param name="backdrop">The destination pixel buffer to composite onto.</param>
    /// <param name="cel">The tilemap cel containing tile indices and transformation data.</param>
    /// <param name="frameWidth">The width of the frame canvas for coordinate calculations.</param>
    /// <remarks>
    /// <para>Reconstructs the full pixel representation of the tilemap by:</para>
    /// <list type="number">
    /// <item><description>Calculating the pixel dimensions from tile count and tile size</description></item>
    /// <item><description>Extracting tile pixel data from the associated tileset</description></item>
    /// <item><description>Arranging tiles in row-major order according to the tilemap grid</description></item>
    /// <item><description>Applying tile transformations (flips and rotations) as specified</description></item>
    /// <item><description>Compositing the result using the layer's blend mode and opacity</description></item>
    /// </list>
    /// <para>Combined opacity calculation follows the same algorithm as image cels for consistent rendering.</para>
    /// </remarks>
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
