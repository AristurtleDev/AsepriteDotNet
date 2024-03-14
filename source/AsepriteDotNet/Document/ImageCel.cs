//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a cel in an Aseprite file that contains image data.  This class cannot be inherited.
/// </summary>
public sealed class ImageCel : Cel
{
    private readonly AseColor[] _pixels;

    /// <summary>
    /// Gets the width, in pixels, of this image cel.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height, in pixels, of this image cel.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the collection of color data that represents the pixels that make up the image for this image cel.
    /// The order of color elements starts with the top-left most pixel in the image and is read left-to-right from
    /// top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AseColor> Pixels => _pixels;

    internal ImageCel(CelProperties celProperties, Layer layer, ImageCelProperties imageCelProperties, AseColor[] pixels)
        : base(celProperties, layer)
    {
        Width = imageCelProperties.Width;
        Height = imageCelProperties.Height;
        _pixels = pixels;
    }
}
