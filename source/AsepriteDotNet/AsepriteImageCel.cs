//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteCel"/> that contains image data.
/// </summary>
public sealed class AsepriteImageCel : AsepriteCel
{
    private readonly AseColor[] _pixels;

    /// <summary>
    /// Gets the width of this <see cref="AsepriteImageCel"/>, in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of this <see cref="AsepriteImageCel"/>, in pixels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// A collection of the <see cref="AseColor"/> elements that represents the pixel data that makes up the image
    /// for this <see cref="AsepriteCel"/>.  Order of the color elements starts with the top-left most pixel and is read
    /// left-to-right from top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AseColor> Pixels => _pixels;

    internal AsepriteImageCel(CelProperties celProperties, AsepriteLayer layer, ImageCelProperties imageCelProperties, AseColor[] pixels)
        : base(celProperties, layer)
    {
        Width = imageCelProperties.Width;
        Height = imageCelProperties.Height;
        _pixels = pixels;
    }
}
