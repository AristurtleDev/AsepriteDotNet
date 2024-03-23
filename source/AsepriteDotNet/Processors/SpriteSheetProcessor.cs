// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="SpriteSheet{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
/// </summary>
public static class SpriteSheetProcessor
{
    /// <summary>
    /// Processes a <see cref="SpriteSheet{TColor}"/> from an <see cref="AsepriteFile{TColor}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process.</param>
    /// <returns>The <see cref="SpriteSheet{TColor}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate tag names are found.</exception>
    public static SpriteSheet<TColor> Process<TColor>(AsepriteFile<TColor> file)
        where TColor : struct, IColor<TColor> => Process(file, ProcessorOptions.Default);

    /// <summary>
    /// Processes a <see cref="SpriteSheet{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>>
    /// <returns>The <see cref="SpriteSheet{TColor}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate tag names are found.</exception>
    public static SpriteSheet<TColor> Process<TColor>(AsepriteFile<TColor> file, ProcessorOptions options)
        where TColor : struct, IColor<TColor>
    {
        ArgumentNullException.ThrowIfNull(file);

        TextureAtlas<TColor> textureAtlas = TextureAtlasProcessor.Process<TColor>(file, options);
        AnimationTag[] tags = new AnimationTag[file.Tags.Length];
        HashSet<string> tagNameCheck = new HashSet<string>();

        for (int i = 0; i < file.Tags.Length; i++)
        {
            AsepriteTag<TColor> aseTag = file.Tags[i];
            if (!tagNameCheck.Add(aseTag.Name))
            {
                throw new InvalidOperationException($"Duplicate tag name '{aseTag.Name}' found.  Tags must have unique names for a sprite sheet");
            }

            tags[i] = ProcessTag<TColor>(aseTag, file.Frames);
        }

        return new SpriteSheet<TColor>(file.Name, textureAtlas, tags);
    }

    private static AnimationTag ProcessTag<TColor>(AsepriteTag<TColor> aseTag, ReadOnlySpan<AsepriteFrame<TColor>> aseFrames)
        where TColor : struct, IColor<TColor>
    {
        int frameCount = aseTag.To - aseTag.From + 1;
        AnimationFrame[] animationFrames = new AnimationFrame[frameCount];

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
