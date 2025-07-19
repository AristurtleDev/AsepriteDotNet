using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core;

/// <summary>
/// Defines a predicate function for evaluating frames during selection operations.
/// </summary>
/// <param name="frame">The frame being evaluated.</param>
/// <param name="index">The zero-based index of the frame within the sprite.</param>
/// <param name="tags">All tags associated with the sprite for contextual evaluation.</param>
/// <returns><see langword="true"/> if the frame matches the selection criteria; otherwise, <see langword="false"/>.</returns>
public delegate bool FramePredicate(AsepriteFrame frame, int index, ReadOnlySpan<AsepriteTag> tags);

/// <summary>
/// Provides frame selection strategies for filtering frames from Aseprite sprites based on various criteria.
/// </summary>
/// <remarks>
/// Frame selectors use different strategies to filter frames: by index, by tag association, or by custom predicates.
/// All selection operations maintain the original frame order from the sprite.
/// </remarks>
public abstract class FrameSelector
{
    /// <summary>
    /// Creates a selector that returns all frames without filtering.
    /// </summary>
    /// <returns>A selector that includes every frame in the sprite.</returns>
    public static FrameSelector AllFrames() => new AllFramesSelector();

    /// <summary>
    /// Creates a selector that returns frames at specific zero-based indices.
    /// </summary>
    /// <param name="frameIndices">The zero-based indices of frames to select. Invalid indices are ignored.</param>
    /// <returns>A selector that includes only frames at the specified indices.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="frameIndices"/> is <see langword="null"/>.</exception>
    public static FrameSelector ByIndices(params int[] frameIndices) => new IndexFrameSelector(frameIndices);

    /// <summary>
    /// Creates a selector that returns frames within a specific animation tag.
    /// </summary>
    /// <param name="tagName">The exact name of the tag to match. Comparison is case-sensitive.</param>
    /// <returns>A selector that includes frames from the first matching tag's range.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="tagName"/> is <see langword="null"/> or empty.</exception>
    /// <remarks>
    /// Only the first tag with the specified name is processed. If multiple tags share the same name, subsequent tags are ignored.
    /// </remarks>
    public static FrameSelector ByTag(string tagName) => new TagFrameSelector(tagName);

    /// <summary>
    /// Creates a selector that returns frames within any of the specified animation tags.
    /// </summary>
    /// <param name="tagNames">The exact names of tags to match. Comparison is case-sensitive.</param>
    /// <returns>A selector that includes frames from all matching tags, maintaining original frame order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="tagNames"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// Frames that belong to multiple matching tags are included only once in the result.
    /// </remarks>
    public static FrameSelector ByTags(params string[] tagNames) => new MultipleTagsFrameSelector(tagNames);

    /// <summary>
    /// Creates a selector that returns frames matching a custom predicate function.
    /// </summary>
    /// <param name="predicate">The function that evaluates each frame for inclusion.</param>
    /// <returns>A selector that includes frames where the predicate returns <see langword="true"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static FrameSelector Predicate(FramePredicate predicate) => new PredicateFrameSelector(predicate);

    /// <summary>
    /// Selects frames from the provided collection based on the implemented selection strategy.
    /// </summary>
    /// <param name="frames">The complete collection of frames to select from.</param>
    /// <param name="tags">The sprite's animation tags for context-aware selection.</param>
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
