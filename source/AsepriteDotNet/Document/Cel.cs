//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the core properties of a cel in an Aseprite file.
/// </summary>
public abstract class Cel
{
    /// <summary>
    /// Gets the layer that this cel exists on.
    /// </summary>
    public Layer Layer { get; }

    /// <summary>
    /// Gets the top-left x-coordinate position of the this cel relative to the bounds of the frame it is in.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the top-left y-coordinate position of this cel relative to the bounds of the frame it is in.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the opacity level of this cel.
    /// </summary>
    public int Opacity { get; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this cel in Aseprite.
    /// </summary>
    public UserData UserData { get; }

    internal Cel(CelProperties celProperties, Layer layer)
    {
        Layer = layer;
        X = celProperties.X;
        Y = celProperties.Y;
        Opacity = celProperties.Opacity;
        UserData = new UserData();
    }
}
