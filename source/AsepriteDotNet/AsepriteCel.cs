//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Defines a single cel within a <see cref="AsepriteFrame"/>,
/// </summary>
public abstract class AsepriteCel
{
    /// <summary>
    /// The <see cref="AsepriteLayer"/> that this <see cref="AsepriteCel"/> exists on.
    /// </summary>
    public AsepriteLayer Layer { get; }

    /// <summary>
    /// The top-left xy-coordinate position of this <see cref="AsepriteCel"/> relative to the bounds of the
    /// <see cref="AsepriteFrame"/> it is in.
    /// </summary>
    public Point Position { get; }

    /// <summary>
    /// The opacity level of this <see cref="AsepriteCel"/>.
    /// </summary>
    public int Opacity { get; }

    /// <summary>
    /// The <see cref="AsepriteUserData"/> that was set in the properties for this <see cref="AsepriteCel"/> in
    /// Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; }

    internal AsepriteCel(CelProperties celProperties, AsepriteLayer layer)
    {
        Layer = layer;
        Position = new Point(celProperties.X, celProperties.Y);
        Opacity = celProperties.Opacity;
        UserData = new AsepriteUserData();
    }
}
