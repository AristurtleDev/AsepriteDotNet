//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of an Aseprite animation tag.  This class cannot be inherited.
/// </summary>
public sealed class Tag
{
    private AseColor _color;

    /// <summary>
    /// Gets the index of the frame that the animation defined by this tag starts on.
    /// </summary>
    public int From { get; }

    /// <summary>
    /// Gets the index of the frame that the animation defined by this tag ends on.
    /// </summary>
    public int To { get; }

    /// <summary>
    /// Gets the loop direction used by the animation defined by this tag.
    /// </summary>
    public AsepriteLoopDirection LoopDirection { get; }

    /// <summary>
    /// Gets the name of this tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the color defined for this tag.
    /// </summary>
    public AseColor Color
    {
        get
        {
            if (UserData.HasColor)
            {
                return UserData.Color.Value;
            }

            return _color;
        }
    }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this tag in Aseprite.
    /// </summary>
    public UserData UserData { get; } = new UserData();

    internal Tag(TagProperties properties, string name)
    {
        From = properties.From;
        To = properties.To;
        LoopDirection = (AsepriteLoopDirection)properties.Direction;
        Name = name;
        _color = new AseColor(properties.RGB[0], properties.RGB[1], properties.RGB[2]);
    }
}
