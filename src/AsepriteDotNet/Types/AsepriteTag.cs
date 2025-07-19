//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Types;

/// <summary>
/// Represents an animation tag that defines a named frame range with specific playback properties and visual organization.
/// </summary>
/// <remarks>
/// Tags organize frames into named animation sequences with custom loop directions and repeat counts.
/// Corresponds to the Tags Chunk (0x2018) specification and enables complex animation workflows
/// through frame range organization and playback control parameters.
/// </remarks>
public sealed class AsepriteTag
{
    /// <summary>
    /// Gets the starting frame index (inclusive) for this animation tag.
    /// </summary>
    /// <remarks>
    /// Frame indices correspond to the animation timeline and must be within the sprite's frame count.
    /// The tag includes all frames from FromFrame through ToFrame inclusive.
    /// </remarks>
    public int FromFrame { get; internal set; }

    /// <summary>
    /// Gets the ending frame index (inclusive) for this animation tag.
    /// </summary>
    /// <remarks>
    /// Must be greater than or equal to FromFrame to define a valid range.
    /// Single-frame tags have FromFrame equal to ToFrame.
    /// </remarks>
    public int ToFrame { get; internal set; }

    /// <summary>
    /// Gets the animation playback direction for this tag's frame sequence.
    /// </summary>
    /// <remarks>
    /// Determines the order in which frames are displayed: forward (1,2,3), reverse (3,2,1),
    /// ping-pong (1,2,3,2,1), or ping-pong reverse (3,2,1,2,3). Affects the total animation
    /// duration when combined with individual frame durations.
    /// </remarks>
    public AsepriteLoopDirection LoopDirection { get; internal set; }

    /// <summary>
    /// Gets the descriptive name assigned to this animation tag.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the color used for visual organization and identification of this tag in the editor interface.
    /// </summary>
    /// <remarks>
    /// The color is used in the Aseprite editor for visual organization of tags in the timeline.
    /// This value may come from the deprecated RGB fields in the tag chunk or from associated user data.
    /// </remarks>
    public Rgba32 Color { get; internal set; }

    /// <summary>
    /// Gets the number of times this animation sequence should repeat during playback.
    /// </summary>
    /// <remarks>
    /// <para>Repeat behavior varies by loop direction:</para>
    /// <list type="bullet">
    /// <item><description>Forward/Reverse: Each repeat plays the complete sequence once</description></item>
    /// <item><description>Ping-pong: Each repeat includes both forward and reverse directions</description></item>
    /// </list>
    /// <para>A value of 1 plays the sequence once, 2 plays it twice, etc.</para>
    /// </remarks>
    public int RepeatCount { get; internal set; }

    /// <summary>
    /// Gets the user-defined metadata associated with this animation tag.
    /// </summary>
    public AsepriteUserData UserData { get; } = new AsepriteUserData();

    internal unsafe AsepriteTag() { }
}
