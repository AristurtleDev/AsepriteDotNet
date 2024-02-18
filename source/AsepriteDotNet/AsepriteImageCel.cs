//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Collections.Frozen;
using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteCel"/> that contains image data.
/// </summary>
public sealed class AsepriteImageCel : AsepriteCel
{
    /// <summary>
    /// The size of this <see cref="AsepriteImageCel"/>, in pixels.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// A collection of the <see cref="Color"/> elements that represents the pixel data that makes up the image
    /// for this <see cref="AsepriteCel"/>.  Order of the color elements starts with the top-left most pixel and is read
    /// left-to-right from top-to-bottom.
    /// </summary>
    public FrozenSet<Color> Pixels { get; }

    internal AsepriteImageCel(AsepriteLayer layer, int x, int y, int opacity, AsepriteUserData? userData, Size size, Color[] pixels)
        : base(layer, x, y, opacity, userData)
    {
        Size = size;
        Pixels = pixels.ToFrozenSet();
    }
}
