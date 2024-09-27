// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="TextureAtlas"/> from an <see cref="AsepriteFile"/>.
/// </summary>
public static class TextureAtlasProcessor
{
    /// <summary>
    /// Processes a <see cref="TextureAtlas"/> from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="options">
    /// Optional options to use when processing.  If <see langword="null"/>, then
    /// <see cref="ProcessorOptions.Default"/> will be used.
    /// </param>
    /// <returns>The <see cref="TextureAtlas"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    [Obsolete("This method will be removed in a future release.  Users should switch to one of the other appropriate Process methods", false)]
    public static TextureAtlas Process(AsepriteFile file, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        options ??= ProcessorOptions.Default;

        return Process(file, options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers, options.MergeDuplicateFrames, options.BorderPadding, options.Spacing, options.InnerPadding);
    }

    /// <summary>
    /// Processes a <see cref="TextureAtlas"/> from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be processed.</param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>The <see cref="TextureAtlas"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static TextureAtlas Process(AsepriteFile file,
                                       bool onlyVisibleLayers = true,
                                       bool includeBackgroundLayer = false,
                                       bool includeTilemapLayers = false,
                                       bool mergeDuplicateFrames = true,
                                       int borderPadding = 0,
                                       int spacing = 0,
                                       int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(file);

        List<string> layers = new List<string>();
        for (int i = 0; i < file.Layers.Length; i++)
        {
            AsepriteLayer layer = file.Layers[i];
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            if (!includeBackgroundLayer && layer.IsBackgroundLayer) { continue; }
            if (!includeTilemapLayers && layer is AsepriteTilemapLayer) { continue; }
            layers.Add(layer.Name);
        }

        return Process(file, layers, mergeDuplicateFrames, borderPadding, spacing, innerPadding);
    }

    /// <summary>
    /// Processes a <see cref="TextureAtlas"/> from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>
    /// The <see cref="TextureAtlas"/> created by this method.  If <paramref name="layers"/> is empty or contains zero
    /// elements, then <see cref="TextureAtlas.Empty"/> is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static TextureAtlas Process(AsepriteFile file, ICollection<string> layers, bool mergeDuplicateFrames = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(file);

        if(layers is null || layers.Count == 0)
        {
            return TextureAtlas.Empty;
        }

        int frameWidth = file.CanvasWidth;
        int frameHeight = file.CanvasHeight;
        int frameCount = file.Frames.Length;

        Rgba32[][] flattenedFrames = new Rgba32[frameCount][];

        for (int i = 0; i < frameCount; i++)
        {
            flattenedFrames[i] = file.Frames[i].FlattenFrame(layers);
        }

        Dictionary<int, int> duplicateMap = new Dictionary<int, int>();
        Dictionary<int, TextureRegion> originalToDuplicateLookup = new Dictionary<int, TextureRegion>();

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            for (int d = 0; d < i; d++)
            {
                if (flattenedFrames[i].SequenceEqual(flattenedFrames[d]))
                {
                    duplicateMap.Add(i, d);
                    break;
                }
            }
        }

        if (mergeDuplicateFrames)
        {
            frameCount -= duplicateMap.Count;
        }

        double sqrt = Math.Sqrt(frameCount);
        int columns = (int)Math.Ceiling(sqrt);
        int rows = (frameCount + columns - 1) / columns;

        int imageWidth = (columns * frameWidth)
                       + (borderPadding * 2)
                       + (spacing * (columns - 1))
                       + (innerPadding * 2 * columns);

        int imageHeight = (columns * frameHeight)
                        + (borderPadding * 2)
                        + (spacing * (rows - 1))
                        + (innerPadding * 2 * rows);

        Rgba32[] imagePixels = new Rgba32[imageWidth * imageHeight];
        TextureRegion[] regions = new TextureRegion[file.Frames.Length];

        int offset = 0;

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (mergeDuplicateFrames && duplicateMap.TryGetValue(i, out int value))
            {
                TextureRegion original = originalToDuplicateLookup[value];
                TextureRegion duplicate = new TextureRegion($"{file.Name} {i}", original.Bounds, ProcessorUtilities.GetSlicesForFrame(i, file.Slices));
                regions[i] = duplicate;
                offset++;
                continue;
            }

            int column = (i - offset) % columns;
            int row = (i - offset) / columns;
            Rgba32[] frame = flattenedFrames[i];

            int x = (column * frameWidth)
                  + borderPadding
                  + (spacing * column)
                  + (innerPadding * (column + column + 1));

            int y = (row * frameHeight)
                  + borderPadding
                  + (spacing * row)
                  + (innerPadding * (row + row + 1));

            for (int p = 0; p < frame.Length; p++)
            {
                int px = (p % frameWidth) + x;
                int py = (p / frameWidth) + y;
                int index = py * imageWidth + px;
                imagePixels[index] = frame[p];
            }

            Rectangle bounds = new Rectangle(x, y, frameWidth, frameHeight);
            TextureRegion textureRegion = new TextureRegion($"{file.Name} {i}", bounds, ProcessorUtilities.GetSlicesForFrame(i, file.Slices));
            regions[i] = textureRegion;
            originalToDuplicateLookup.Add(i, textureRegion);
        }

        Texture texture = new Texture(file.Name, new Size(imageWidth, imageHeight), imagePixels);
        return new TextureAtlas(file.Name, texture, regions);
    }
}
