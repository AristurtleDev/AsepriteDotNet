// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a single frame in an animation.  This record cannot be inherited.
/// </summary>
/// <param name="FrameIndex">The index of the source frame for this animation.</param>
/// <param name="Duration">The duration of the frame.</param>
public sealed record AnimationFrame(int FrameIndex, TimeSpan Duration) { }
