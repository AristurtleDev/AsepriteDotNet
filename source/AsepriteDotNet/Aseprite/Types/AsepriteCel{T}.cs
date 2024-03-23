//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the core properties of a cel in an Aseprite file.
/// </summary>
public abstract class AsepriteCel<TColor> where TColor : struct, IColor<TColor>
{
    /// <summary>
    /// Gets the layer that this cel exists on.
    /// </summary>
    public AsepriteLayer<TColor> Layer { get; }

    /// <summary>
    /// Gets the top-left xy-coordinate position of this el relative to the bounds of the frame it is in.
    /// </summary>
    public Point Location { get; }

    /// <summary>
    /// Gets the opacity level of this cel.
    /// </summary>
    public int Opacity { get; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this cel in Aseprite.
    /// </summary>
    public AsepriteUserData<TColor> UserData { get; }

    internal AsepriteCel(AsepriteCelProperties celProperties, AsepriteLayer<TColor> layer)
    {
        Layer = layer;
        Location = new Point(celProperties.X, celProperties.Y);
        Opacity = celProperties.Opacity;
        UserData = new AsepriteUserData<TColor>();
    }
}
