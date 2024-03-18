// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

public static class TextureAtlasProcessor
{
    public static TextureAtlas Process(AsepriteFile file, ProcessorOptions options)
    {
        int frameWidth = file.CanvasWidth;
        int frameHeight = file.CanvasHeight;
        int frameCount = file.Frames.Length;

        Rgba32[][] flattenedFrames = new Rgba32[frameCount][];

        for (int i = 0; i < frameCount; i++)
        {
            flattenedFrames[i] = file.Frames[i].FlattenFrame(options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
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

        Rgba32[] imagePixels = new Rgba32[imageWidth * imageHeight];
        TextureRegion[] regions = new TextureRegion[file.Frames.Length];

        int offset = 0;

        for (int i = 0; i < flattenedFrames.GetLength(0); i++)
        {
            if (options.MergeDuplicateFrames && duplicateMap.ContainsKey(i))
            {
                TextureRegion original = originalToDuplicateLookup[duplicateMap[i]];
                TextureRegion duplicate = original with { Name = $"{file.Name} {i}", Slices = ProcessorUtilities.GetSlicesForFrame(i, file.Slices) };
                regions[i] = duplicate;
                offset++;
                continue;
            }

            int column = (i - offset) % columns;
            int row = (i - offset) / columns;
            Rgba32[] frame = flattenedFrames[i];

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
            TextureRegion textureRegion = new TextureRegion($"{file.Name} {i}", bounds, ProcessorUtilities.GetSlicesForFrame(i, file.Slices));
            regions[i] = textureRegion;
            originalToDuplicateLookup.Add(i, textureRegion);
        }

        Texture texture = new Texture(file.Name, new Size(imageWidth, imageHeight), imagePixels);
        return new TextureAtlas(file.Name, texture, regions);
    }
}
