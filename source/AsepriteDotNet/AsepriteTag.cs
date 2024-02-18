//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents an animation tag define in the Aseprite file.
/// </summary>
public sealed class AsepriteTag
{
    /// <summary>
    /// The frame that the animation defined by this tag starts on.
    /// </summary>
    public int From { get; }

    /// <summary>
    /// The frame that the animation defined by this tag ends on.
    /// </summary>
    public int To { get; }

    /// <summary>
    /// The loop direction defined for the animation represented by this tag.
    /// </summary>
    public AsepriteLoopDirection LoopDirection { get; }

    /// <summary>
    /// The name given this tag in Aseprite.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The color assigned to this tag in Aseprite.
    /// </summary>
    public Rgba32 Color { get; }

    /// <summary>
    /// The custom user data that was set in the properties for this tag in Aseprite.
    /// </summary>
    public AsepriteUserData? UserData { get; }

    internal AsepriteTag(int from, int to, AsepriteLoopDirection loopDirection, Rgba32 color, string name, AsepriteUserData? userData)
    {
        From = from;
        To = to;
        LoopDirection = loopDirection;
        Name = name;
        UserData = userData;
        Color = userData?.Color ?? color;
    }
}
