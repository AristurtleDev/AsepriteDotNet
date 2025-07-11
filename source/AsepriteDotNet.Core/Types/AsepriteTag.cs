//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using AsepriteDotNet.Core.Document;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of an Aseprite animation tag.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteTag
{
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
    public Rgba32 Color { get; }

    /// <summary>
    /// Gets the number of times the animation defined by this tag repeats.
    /// </summary>
    public int Repeat { get; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this tag in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new AsepriteUserData();

    internal unsafe AsepriteTag(AsepriteTagProperties properties, string name)
    {
        From = properties.From;
        To = properties.To;
        LoopDirection = (AsepriteLoopDirection)properties.Direction;
        Name = name;
        Repeat = properties.Repeat;
        Color = new Rgba32(properties.R, properties.G, properties.B);
    }
}
