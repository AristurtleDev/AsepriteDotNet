//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core;

/// <summary>
/// Specifies the direction and pattern for looping animation playback in Aseprite tags.
/// </summary>
/// <remarks>
/// These values correspond directly to the loop animation direction field in the Aseprite file format
/// Tags Chunk (0x2018) specification, determining how frame sequences are played during animation.
/// </remarks>
public enum AsepriteLoopDirection
{
    /// <summary>
    /// Plays frames in sequential order from first to last frame repeatedly.
    /// </summary>
    /// <remarks>
    /// Animation plays frames 1, 2, 3, ..., N, then repeats from frame 1.
    /// </remarks>
    Forward = 0,

    /// <summary>
    /// Plays frames in reverse order from last to first frame repeatedly.
    /// </summary>
    /// <remarks>
    /// Animation plays frames N, N-1, N-2, ..., 1, then repeats from frame N.
    /// </remarks>
    Reverse = 1,

    /// <summary>
    /// Plays frames forward then backward in a ping-pong pattern.
    /// </summary>
    /// <remarks>
    /// Animation plays frames 1, 2, 3, ..., N, N-1, N-2, ..., 1, then repeats.
    /// The first and last frames are not duplicated at the transition points.
    /// </remarks>
    PingPong = 2,

    /// <summary>
    /// Plays frames backward then forward in a reverse ping-pong pattern.
    /// </summary>
    /// <remarks>
    /// Animation plays frames N, N-1, N-2, ..., 1, 2, 3, ..., N, then repeats.
    /// The first and last frames are not duplicated at the transition points.
    /// </remarks>
    PingPongReverse = 3
}
