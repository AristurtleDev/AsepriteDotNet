// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;

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
