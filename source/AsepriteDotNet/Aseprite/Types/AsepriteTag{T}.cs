//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of an Aseprite animation tag.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteTag<TColor> where TColor : struct, IColor<TColor>
{
    private readonly TColor _color;

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
    public TColor Color => _color;

    /// <summary>
    /// Gets the number of times the animation defined by this tag repeats.
    /// </summary>
    public int Repeat { get; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this tag in Aseprite.
    /// </summary>
    public AsepriteUserData<TColor> UserData { get; } = new AsepriteUserData<TColor>();

    internal unsafe AsepriteTag(AsepriteTagProperties properties, string name)
    {
        From = properties.From;
        To = properties.To;
        LoopDirection = (AsepriteLoopDirection)properties.Direction;
        Name = name;
        _color = default(TColor);
        _color.R = properties.R;
        _color.G = properties.G;
        _color.B = properties.B;
        _color.A = 255;
    }
}
