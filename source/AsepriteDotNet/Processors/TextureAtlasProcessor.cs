// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="TextureAtlas{T}"/> from an <see cref="AsepriteFile{T}"/>.
/// </summary>
public static class TextureAtlasProcessor
{
    /// <summary>
    /// Processes a <see cref="TextureAtlas{T}"/> from an <see cref="AsepriteFile{T}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process.</param>
    /// <returns>The <see cref="TextureAtlas{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static TextureAtlas<T> Process<T>(AsepriteFile<T> file)
        where T: IColor, new() => Process(file, ProcessorOptions.Default);

    /// <summary>
    /// Processes a <see cref="TextureAtlas{T}"/> from an <see cref="AsepriteFile{T}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>
    /// <returns>The <see cref="TextureAtlas{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static TextureAtlas<T> Process<T>(AsepriteFile<T> file, ProcessorOptions options)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(file);

        int frameWidth = file.CanvasWidth;
        int frameHeight = file.CanvasHeight;
        int frameCount = file.Frames.Length;

        T[][] flattenedFrames = new T[frameCount][];

        for (int i = 0; i < frameCount; i++)
        {
            flattenedFrames[i] = file.Frames[i].FlattenFrame(options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
        }

        Dictionary<int, int> duplicateMap = new Dictionary<int, int>();
        Dictionary<int, TextureRegion<T>> originalToDuplicateLookup = new Dictionary<int, TextureRegion<T>>();

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

        T[] imagePixels = new T[imageWidth * imageHeight];
        TextureRegion<T>[] regions = new TextureRegion<T>[file.Frames.Length];

        int offset = 0;

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (options.MergeDuplicateFrames && duplicateMap.TryGetValue(i, out int value))
            {
                TextureRegion<T> original = originalToDuplicateLookup[value];
                TextureRegion<T> duplicate = new TextureRegion<T>($"{file.Name} {i}", original.Bounds, ProcessorUtilities.GetSlicesForFrame(i, file.Slices));
                regions[i] = duplicate;
                offset++;
                continue;
            }

            int column = (i - offset) % columns;
            int row = (i - offset) / columns;
            T[] frame = flattenedFrames[i];

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
            TextureRegion<T> textureRegion = new TextureRegion<T>($"{file.Name} {i}", bounds, ProcessorUtilities.GetSlicesForFrame<T>(i, file.Slices));
            regions[i] = textureRegion;
            originalToDuplicateLookup.Add(i, textureRegion);
        }

        Texture<T> texture = new Texture<T>(file.Name, new Size(imageWidth, imageHeight), imagePixels);
        return new TextureAtlas<T>(file.Name, texture, regions);
    }
}
