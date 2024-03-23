// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="TextureAtlas{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
/// </summary>
public static class TextureAtlasProcessor
{
    /// <summary>
    /// Processes a <see cref="TextureAtlas{TColor}"/> from an <see cref="AsepriteFile{TColor}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process.</param>
    /// <returns>The <see cref="TextureAtlas{TColor}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static TextureAtlas<TColor> Process<TColor>(AsepriteFile<TColor> file)
        where TColor : struct, IColor<TColor> => Process(file, ProcessorOptions.Default);

    /// <summary>
    /// Processes a <see cref="TextureAtlas{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>
    /// <returns>The <see cref="TextureAtlas{TColor}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static TextureAtlas<TColor> Process<TColor>(AsepriteFile<TColor> file, ProcessorOptions options)
        where TColor : struct, IColor<TColor>
    {
        ArgumentNullException.ThrowIfNull(file);

        int frameWidth = file.CanvasWidth;
        int frameHeight = file.CanvasHeight;
        int frameCount = file.Frames.Length;

        TColor[][] flattenedFrames = new TColor[frameCount][];

        for (int i = 0; i < frameCount; i++)
        {
            flattenedFrames[i] = file.Frames[i].FlattenFrame(options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
        }

        Dictionary<int, int> duplicateMap = new Dictionary<int, int>();
        Dictionary<int, TextureRegion<TColor>> originalToDuplicateLookup = new Dictionary<int, TextureRegion<TColor>>();

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

        if (options.MergeDuplicateFrames)
        {
            frameCount -= duplicateMap.Count;
        }

        double sqrt = Math.Sqrt(frameCount);
        int columns = (int)Math.Ceiling(sqrt);
        int rows = (frameCount + columns - 1) / columns;

        int imageWidth = (columns * frameWidth)
                       + (options.BorderPadding * 2)
                       + (options.Spacing * (columns - 1))
                       + (options.InnerPadding * 2 * columns);

        int imageHeight = (columns * frameHeight)
                        + (options.BorderPadding * 2)
                        + (options.Spacing * (rows - 1))
                        + (options.InnerPadding * 2 * rows);

        TColor[] imagePixels = new TColor[imageWidth * imageHeight];
        TextureRegion<TColor>[] regions = new TextureRegion<TColor>[file.Frames.Length];

        int offset = 0;

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (options.MergeDuplicateFrames && duplicateMap.TryGetValue(i, out int value))
            {
                TextureRegion<TColor> original = originalToDuplicateLookup[value];
                TextureRegion<TColor> duplicate = new TextureRegion<TColor>($"{file.Name} {i}", original.Bounds, ProcessorUtilities.GetSlicesForFrame(i, file.Slices));
                regions[i] = duplicate;
                offset++;
                continue;
            }

            int column = (i - offset) % columns;
            int row = (i - offset) / columns;
            TColor[] frame = flattenedFrames[i];

            int x = (column * frameWidth)
                  + options.BorderPadding
                  + (options.Spacing * column)
                  + (options.InnerPadding * (column + column + 1));

            int y = (row * frameHeight)
                  + options.BorderPadding
                  + (options.Spacing * row)
                  + (options.InnerPadding * (row + row + 1));

            for (int p = 0; p < frame.Length; p++)
            {
                int px = (p % frameWidth) + x;
                int py = (p / frameWidth) + y;
                int index = py * imageWidth + px;
                imagePixels[index] = frame[p];
            }

            Rectangle bounds = new Rectangle(x, y, frameWidth, frameHeight);
            TextureRegion<TColor> textureRegion = new TextureRegion<TColor>($"{file.Name} {i}", bounds, ProcessorUtilities.GetSlicesForFrame<TColor>(i, file.Slices));
            regions[i] = textureRegion;
            originalToDuplicateLookup.Add(i, textureRegion);
        }

        Texture<TColor> texture = new Texture<TColor>(file.Name, new Size(imageWidth, imageHeight), imagePixels);
        return new TextureAtlas<TColor>(file.Name, texture, regions);
    }
}
