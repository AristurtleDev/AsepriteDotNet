//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteCel"/> that contains image data.
/// </summary>
public sealed class AsepriteImageCel : AsepriteCel
{
    private readonly AseColor[] _pixels;

    /// <summary>
    /// The size of this <see cref="AsepriteImageCel"/>, in pixels.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// A collection of the <see cref="Color"/> elements that represents the pixel data that makes up the image
    /// for this <see cref="AsepriteCel"/>.  Order of the color elements starts with the top-left most pixel and is read
    /// left-to-right from top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AseColor> Pixels => _pixels;

    internal AsepriteImageCel(CelProperties celProperties, AsepriteLayer layer, ImageCelProperties imageCelProperties, AseColor[] pixels)
        : base(celProperties, layer)
    {
        Size = new Size(imageCelProperties.Width, imageCelProperties.Height);
        _pixels = pixels;
    }
}
