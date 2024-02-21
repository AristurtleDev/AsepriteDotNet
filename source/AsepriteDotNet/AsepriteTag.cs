//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents an animation tag define in the Aseprite file.
/// </summary>
public sealed class AsepriteTag
{
    private AseColor _color;

    /// <summary>
    /// Gets the index of the <see cref="AsepriteFrame"/> that the animation defined by this <see cref="AsepriteTag"/>
    /// starts on.
    /// </summary>
    public int From { get; }

    /// <summary>
    /// Gets the index of the <see cref="AsepriteFrame"/> that the animation defined by this <see cref="AsepriteTag"/>
    /// ends on.
    /// </summary>
    public int To { get; }

    /// <summary>
    /// Gets the <see cref="AsepriteLoopDirection"/> used by the animation defined by this <see cref="AsepriteTag"/>.
    /// </summary>
    public AsepriteLoopDirection LoopDirection { get; }

    /// <summary>
    /// Gets the name fo this <see cref="AsepriteTag"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the <see cref="AseColor"/> that defines the color of this <see cref="AsepriteTag"/>.
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
    /// Gets the <see cref="AsepriteUserData"/> that was set for this <see cref="AsepriteTag"/> in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new AsepriteUserData();

    internal AsepriteTag(TagProperties properties, string name)
    {
        From = properties.From;
        To = properties.To;
        LoopDirection = (AsepriteLoopDirection)properties.Direction;
        Name = name;
        _color = new AseColor(properties.RGB[0], properties.RGB[1], properties.RGB[2]);
    }
}
