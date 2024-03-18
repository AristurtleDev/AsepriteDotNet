// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines the tag of an animation.  This record cannot be inherited.
/// </summary>
/// <param name="Name">The name of this animation tag.</param>
/// <param name="AnimationFrames">THe frames of animation for the animation defined by this animation tag.</param>
/// <param name="LoopCount">
/// The total number of loops/cycles that should play for the animation defined by this animation tag.
/// </param>
/// <param name="IsReversed">
/// Indicates whether the animation defined by this animation tag should play in reverse.
/// </param>
/// <param name="IsPingPong">
/// Indicates whether the animation defined by this animation tag should pin-pong once reaching the last frame of the animation.
/// </param>
public sealed record AnimationTag(string Name, AnimationFrame[] AnimationFrames, int LoopCount, bool IsReversed, bool IsPingPong) { }
