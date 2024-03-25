// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="SpriteSheet{T}"/> from an <see cref="AsepriteFile{T}"/>.
/// </summary>
public static class SpriteSheetProcessor
{
    /// <summary>
    /// Processes a <see cref="SpriteSheet{T}"/> from an <see cref="AsepriteFile{T}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process.</param>
    /// <returns>The <see cref="SpriteSheet{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate tag names are found.</exception>
    public static SpriteSheet<T> Process<T>(AsepriteFile<T> file)
        where T: IColor, new() => Process(file, ProcessorOptions.Default);

    /// <summary>
    /// Processes a <see cref="SpriteSheet{T}"/> from an <see cref="AsepriteFile{T}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>>
    /// <returns>The <see cref="SpriteSheet{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate tag names are found.</exception>
    public static SpriteSheet<T> Process<T>(AsepriteFile<T> file, ProcessorOptions options)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(file);

        TextureAtlas<T> textureAtlas = TextureAtlasProcessor.Process<T>(file, options);
        AnimationTag[] tags = new AnimationTag[file.Tags.Length];
        HashSet<string> tagNameCheck = new HashSet<string>();

        for (int i = 0; i < file.Tags.Length; i++)
        {
            AsepriteTag<T> aseTag = file.Tags[i];
            if (!tagNameCheck.Add(aseTag.Name))
            {
                throw new InvalidOperationException($"Duplicate tag name '{aseTag.Name}' found.  Tags must have unique names for a sprite sheet");
            }

            tags[i] = ProcessTag<T>(aseTag, file.Frames);
        }

        return new SpriteSheet<T>(file.Name, textureAtlas, tags);
    }

    private static AnimationTag ProcessTag<T>(AsepriteTag<T> aseTag, ReadOnlySpan<AsepriteFrame<T>> aseFrames)
        where T: IColor, new()
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
