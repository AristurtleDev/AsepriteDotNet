//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines the animation direction values set for an <see cref="AsepriteTag"/>.
/// </summary>
public enum AsepriteLoopDirection
{
    /// <summary>
    /// Defines that the animation for the <see cref="AsepriteTag"/> is played in a forward direction from the first
    /// frame of animation to the last.
    /// </summary>
    Forward = 0,

    /// <summary>
    /// Defines that the animation for the <see cref="AsepriteTag"/> is played in reverse from the last frame of
    /// animation to the first.
    /// </summary>
    Reverse = 1,

    /// <summary>
    /// Defines that the animation for the <see cref="AsepriteTag"/> ping-pongs by first going from the first frame of
    /// animation to the last frame, then playing in reverse from the last frame of animation to the first.
    /// </summary>
    PingPong = 2,

    /// <summary>
    /// Defines that the animation for the <see cref="AsepriteTag"/> ping-pongs by first going in reverse from the last
    /// frame of animation to the first frame, then playing forward from the first frame of animation to the last.
    /// </summary>
    PingPongReverse = 3
}
