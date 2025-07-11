// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="SpriteSheet"/> from an <see cref="AsepriteFile"/>.
/// </summary>
public static class SpriteSheetProcessor
{
    /// <summary>
    /// Processes a <see cref="SpriteSheet"/> from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="options">
    /// Optional options to use when processing.  If <see langword="null"/>, then
    /// <see cref="ProcessorOptions.Default"/> will be used.
    /// </param>
    /// <returns>The <see cref="SpriteSheet"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate tag names are found.</exception>
    [Obsolete("This method will be removed in a future release.  Users should switch to one of the other appropriate Process methods", false)]
    public static SpriteSheet Process(AsepriteFile file, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        options ??= ProcessorOptions.Default;

        TextureAtlas textureAtlas = TextureAtlasProcessor.Process(file, options);
        AnimationTag[] tags = new AnimationTag[file.Tags.Length];
        HashSet<string> tagNameCheck = new HashSet<string>();

        for (int i = 0; i < file.Tags.Length; i++)
        {
            AsepriteTag aseTag = file.Tags[i];
            if (!tagNameCheck.Add(aseTag.Name))
            {
                throw new InvalidOperationException($"Duplicate tag name '{aseTag.Name}' found.  Tags must have unique names for a sprite sheet");
            }

            tags[i] = ProcessTag(aseTag, file.Frames);
        }

        return new SpriteSheet(file.Name, textureAtlas, tags);
    }

    /// <summary>
    /// Processes a <see cref="SpriteSheet"/> from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be processed.</param>
    /// <param name="mergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
    /// <param name="borderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
    /// <param name="spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
    /// <param name="innerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
    /// <returns>The <see cref="SpriteSheet"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static SpriteSheet Process(AsepriteFile file,
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
    /// Processes a <see cref="SpriteSheet"/> from an <see cref="AsepriteFile"/>.
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
    /// The <see cref="SpriteSheet"/> created by this method.  If <paramref name="layers"/> is empty or contains zero
    /// elements, then <see cref="SpriteSheet.Empty"/> is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    public static SpriteSheet Process(AsepriteFile file, ICollection<string> layers, bool mergeDuplicateFrames = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (layers is null || layers.Count == 0)
        {
            return SpriteSheet.Empty;
        }

        TextureAtlas textureAtlas = TextureAtlasProcessor.Process(file, layers, mergeDuplicateFrames, borderPadding, spacing, innerPadding);
        AnimationTag[] tags = new AnimationTag[file.Tags.Length];
        HashSet<string> tagNameCheck = new HashSet<string>();

        for (int i = 0; i < file.Tags.Length; i++)
        {
            AsepriteTag aseTag = file.Tags[i];
            if (!tagNameCheck.Add(aseTag.Name))
            {
                throw new InvalidOperationException($"Duplicate tag name '{aseTag.Name}' found.  Tags must have unique names for a sprite sheet");
            }

            tags[i] = ProcessTag(aseTag, file.Frames);
        }

        return new SpriteSheet(file.Name, textureAtlas, tags);
    }

    private static AnimationTag ProcessTag(AsepriteTag aseTag, ReadOnlySpan<AsepriteFrame> aseFrames)
    {
        int frameCount = aseTag.To - aseTag.From + 1;
        AnimationFrame[] animationFrames = new AnimationFrame[frameCount];
        int[] frames = new int[frameCount];
        int[] durations = new int[frameCount];

        for (int i = 0; i < frameCount; i++)
        {
            int index = aseTag.From + i;
            animationFrames[i] = new AnimationFrame(index, aseFrames[index].Duration);
        }

        //  In Aseprite, all tags are looping
        int loopCount = aseTag.Repeat;
        bool isReversed = aseTag.LoopDirection == AsepriteLoopDirection.Reverse || aseTag.LoopDirection == AsepriteLoopDirection.PingPongReverse;
        bool isPingPong = aseTag.LoopDirection == AsepriteLoopDirection.PingPong || aseTag.LoopDirection == AsepriteLoopDirection.PingPongReverse;

        return new AnimationTag(aseTag.Name, animationFrames, loopCount, isReversed, isPingPong);
    }
}
