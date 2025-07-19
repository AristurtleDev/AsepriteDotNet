// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core;

/// <summary>
/// Represents a method that determines whether a frame should be selected based on frame data, index, and associated tags.
/// </summary>
/// <param name="frame">The frame to evaluate.</param>
/// <param name="index">The zero-based index of the frame in the collection.</param>
/// <param name="tags">The collection of tags associated with the frame sequence.</param>
/// <returns><c>true</c> if the frame should be selected; otherwise, <c>false</c>.</returns>
public delegate bool FramePredicate(AsepriteFrame frame, int index, ReadOnlySpan<AsepriteTag> tags);

/// <summary>
/// Provides frame filtering strategies for selecting subsets of frames from Aseprite animations.
/// </summary>
/// <remarks>
/// All selector implementations return new arrays containing references to the original frames,
/// preserving the original frame order while filtering based on specific criteria such as indices, tags, or custom predicates.
/// </remarks>
public abstract class FrameSelector
{
    /// <summary>
    /// Creates a selector that returns all frames without filtering.
    /// </summary>
    /// <returns>A <see cref="FrameSelector"/> that selects every frame in the sequence.</returns>
    public static FrameSelector AllFrames() => new AllFramesSelector();

    /// <summary>
    /// Creates a selector that returns frames at the specified zero-based indices.
    /// </summary>
    /// <param name="frameIndices">The indices of frames to select.</param>
    /// <returns>A <see cref="FrameSelector"/> that filters frames by their position in the sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="frameIndices"/> is null.</exception>
    public static FrameSelector ByIndices(params int[] frameIndices) => new IndexFrameSelector(frameIndices);

    /// <summary>
    /// Creates a selector that returns frames within the specified tag's range.
    /// </summary>
    /// <param name="tagName">The name of the tag whose frame range should be selected.</param>
    /// <returns>A <see cref="FrameSelector"/> that selects frames covered by the named tag.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="tagName"/> is null or empty.</exception>
    public static FrameSelector ByTag(string tagName) => new TagFrameSelector(tagName);

    /// <summary>
    /// Creates a selector that returns frames within any of the specified tags' ranges.
    /// </summary>
    /// <param name="tagNames">The names of tags whose frame ranges should be selected.</param>
    /// <returns>A <see cref="FrameSelector"/> that selects frames covered by any of the named tags.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tagNames"/> is null.</exception>
    public static FrameSelector ByTags(params string[] tagNames) => new MultipleTagsFrameSelector(tagNames);

    /// <summary>
    /// Creates a selector that returns frames matching the specified predicate function.
    /// </summary>
    /// <param name="predicate">The function that determines whether a frame should be selected.</param>
    /// <returns>A <see cref="FrameSelector"/> that filters frames using the provided predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is null.</exception>
    public static FrameSelector Predicate(FramePredicate predicate) => new PredicateFrameSelector(predicate);

    /// <summary>
    /// Selects frames from the provided collection based on the selector's filtering criteria.
    /// </summary>
    /// <param name="frames">The collection of frames to filter.</param>
    /// <param name="tags">The collection of tags associated with the frame sequence.</param>
    /// <returns>A span containing the selected frames in their original order.</returns>
    public abstract ReadOnlySpan<AsepriteFrame> SelectFrames(ReadOnlySpan<AsepriteFrame> frames, ReadOnlySpan<AsepriteTag> tags);

    private sealed class AllFramesSelector : FrameSelector
    {
        public override ReadOnlySpan<AsepriteFrame> SelectFrames(ReadOnlySpan<AsepriteFrame> frames, ReadOnlySpan<AsepriteTag> tags)
        {
            return frames.ToArray();
        }
    }

    private sealed class IndexFrameSelector : FrameSelector
    {
        private readonly HashSet<int> _frameIndices;

        public IndexFrameSelector(int[] frameIndices)
        {
            ArgumentNullException.ThrowIfNull(frameIndices);
            _frameIndices = new HashSet<int>(frameIndices);
        }

        public override ReadOnlySpan<AsepriteFrame> SelectFrames(ReadOnlySpan<AsepriteFrame> frames, ReadOnlySpan<AsepriteTag> tags)
        {
            List<AsepriteFrame> matchedFrames = [];

            for (int i = 0; i < frames.Length; i++)
            {
                if (_frameIndices.Contains(i))
                {
                    matchedFrames.Add(frames[i]);
                }
            }

            return matchedFrames.ToArray();
        }
    }

    private sealed class TagFrameSelector : FrameSelector
    {
        private readonly string _tagName;

        public TagFrameSelector(string tagName)
        {
            ArgumentException.ThrowIfNullOrEmpty(tagName);
            _tagName = tagName;
        }

        public override ReadOnlySpan<AsepriteFrame> SelectFrames(ReadOnlySpan<AsepriteFrame> frames, ReadOnlySpan<AsepriteTag> tags)
        {
            List<AsepriteFrame> matchedFrames = [];

            for (int tagIndex = 0; tagIndex < tags.Length; tagIndex++)
            {
                AsepriteTag tag = tags[tagIndex];
                if (tag.Name.Equals(_tagName, StringComparison.Ordinal))
                {
                    for (int frameIndex = tag.FromFrame; frameIndex <= tag.ToFrame && frameIndex < frames.Length; frameIndex++)
                    {
                        matchedFrames.Add(frames[frameIndex]);
                    }
                    break;
                }
            }

            return matchedFrames.ToArray();
        }
    }

    private sealed class MultipleTagsFrameSelector : FrameSelector
    {
        private readonly HashSet<string> _tagNames;

        public MultipleTagsFrameSelector(string[] tagNames)
        {
            ArgumentNullException.ThrowIfNull(tagNames);
            _tagNames = new HashSet<string>(tagNames, StringComparer.Ordinal);
        }

        public override ReadOnlySpan<AsepriteFrame> SelectFrames(ReadOnlySpan<AsepriteFrame> frames, ReadOnlySpan<AsepriteTag> tags)
        {
            SortedSet<int> frameIndices = [];

            for (int tagIndex = 0; tagIndex < tags.Length; tagIndex++)
            {
                AsepriteTag tag = tags[tagIndex];
                if (_tagNames.Contains(tag.Name))
                {
                    for (int frameIndex = tag.FromFrame; frameIndex <= tag.ToFrame && frameIndex < frames.Length; frameIndex++)
                    {
                        frameIndices.Add(frameIndex);
                    }
                }
            }

            List<AsepriteFrame> matchedFrames = [];
            foreach (int frameIndex in frameIndices)
            {
                matchedFrames.Add(frames[frameIndex]);
            }

            return matchedFrames.ToArray();
        }
    }

    private sealed class PredicateFrameSelector : FrameSelector
    {
        private readonly FramePredicate _predicate;

        public PredicateFrameSelector(FramePredicate predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            _predicate = predicate;
        }

        public override ReadOnlySpan<AsepriteFrame> SelectFrames(ReadOnlySpan<AsepriteFrame> frames, ReadOnlySpan<AsepriteTag> tags)
        {
            List<AsepriteFrame> matchedFrames = [];

            for (int i = 0; i < frames.Length; i++)
            {
                if (_predicate(frames[i], i, tags))
                {
                    matchedFrames.Add(frames[i]);
                }
            }

            return matchedFrames.ToArray();
        }
    }
}
