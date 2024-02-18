//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines a single cel within a <see cref="AsepriteFrame"/>,
/// </summary>
public abstract class AsepriteCel
{
    /// <summary>
    /// The <see cref="AsepriteLayer"/> that this <see cref="AsepriteCel"/> exists on.
    /// </summary>
    public AsepriteLayer Layer;

    /// <summary>
    /// The x-coordinate position of this <see cref="AsepriteCel"/>, relative to the bounds of the
    /// <see cref="AsperiteFrame"/> it is in.
    /// </summary>
    public int X;

    /// <summary>
    /// The y-coordinate position of this <see cref="AsepriteCel"/>, relative to the bounds of the
    /// <see cref="AsepriteFrame"/> it is in.
    /// </summary>
    public int Y;

    /// <summary>
    /// The opacity level of this <see cref="AsepriteCel"/>.
    /// </summary>
    public int Opacity;

    /// <summary>
    /// The <see cref="AsepriteUserData"/> that was set in the properties for this <see cref="AsepriteCel"/> in
    /// Aseprite.
    /// </summary>
    public AsepriteUserData UserData;
}
