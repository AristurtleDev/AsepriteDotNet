//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a cel in an Aseprite file that contains image data.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteImageCel : AsepriteCel
{
    private readonly Rgba32[] _pixels;

    /// <summary>
    /// Gets the size of this image cel.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets the collection of color data that represents the pixels that make up the image for this image cel.
    /// The order of color elements starts with the top-left most pixel in the image and is read left-to-right from
    /// top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Rgba32> Pixels => _pixels;

    internal AsepriteImageCel(AsepriteCelProperties celProperties, AsepriteLayer layer, AsepriteImageCelProperties imageCelProperties, Rgba32[] pixels)
        : base(celProperties, layer)
    {
        Size = new Size(imageCelProperties.Width, imageCelProperties.Height);
        _pixels = pixels;
    }
}
