// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Represents a single frame in an animation.
/// This class cannot be inherited.
/// </summary>
public sealed class AnimationFrame : IEquatable<AnimationFrame>
{
    /// <summary>
    /// Gets the index of the source frame for this animation.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// Gets the duration of this frame.
    /// </summary>
    public TimeSpan Duration { get; }

    internal AnimationFrame(int frameIndex, TimeSpan duration) => (FrameIndex, Duration) = (frameIndex, duration);

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is AnimationFrame other && Equals(other);

    /// <inheritdoc/>
    public bool Equals([NotNullWhen(true)] AnimationFrame? other)
    {
        if (ReferenceEquals(this, other)) { return true; }
        return FrameIndex.Equals(other?.FrameIndex) && Duration.Equals(other.Duration);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(FrameIndex, Duration);
}
