// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Defines the tag of an animation.
/// This class cannot be inherited.
/// </summary>
public sealed class AnimationTag : IEquatable<AnimationTag>
{
    private readonly AnimationFrame[] _frames;

    /// <summary>
    /// Gets the name of the animation defined by this animation tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only collection of the frames of animation for the animation defined by this animation tag.
    /// </summary>
    public ReadOnlySpan<AnimationFrame> Frames => _frames;

    /// <summary>
    /// Gets the total number of loops/cycles that should play for the animation defined by this animation tag.
    /// 0 = Infinite.
    /// </summary>
    public int LoopCount { get; }

    /// <summary>
    /// Gets a value that indicates whether the animation defined by this animation tag should play in reverse.
    /// </summary>
    public bool IsReversed { get; }

    /// <summary>
    /// Gets a value that indicates whether the animation defined by this animation tag should ping-pong once reaching
    /// the last frame of animation.
    /// </summary>
    public bool IsPingPong { get; }

    internal AnimationTag(string name, AnimationFrame[] frames, int loopCount, bool isReversed, bool isPingPong) =>
            (Name, _frames, LoopCount, IsReversed, IsPingPong) = (name, frames, loopCount, isReversed, isPingPong);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AnimationTag other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] AnimationTag? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return Name.Equals(other?.Name, StringComparison.Ordinal)
            && _frames.SequenceEqual(other._frames)
            && LoopCount.Equals(other.LoopCount)
            && IsReversed.Equals(other.IsReversed)
            && IsPingPong.Equals(other.IsPingPong);
    }
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Name, _frames, LoopCount, IsReversed, IsPingPong);
}
