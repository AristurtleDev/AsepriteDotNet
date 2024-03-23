// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a cel in an Aseprite file that contains image data.  This class cannot be inherited.
/// </summary>
public class AsepriteImageCel<TColor> : AsepriteCel<TColor> where TColor : struct, IColor<TColor>
{
    private readonly TColor[] _pixels;

    /// <summary>
    /// Gets the size of this image cel.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of color data that represents the pixels that make up the image for this
    /// image cel. The order of color elements start with the top-left most pixel in the image and is read from
    /// left-to-right, top-to-bottom.
    /// </summary>
    public ReadOnlySpan<TColor> Pixels => _pixels;

    internal AsepriteImageCel(AsepriteCelProperties celProperties, AsepriteLayer<TColor> layer, AsepriteImageCelProperties imageCelProperties, TColor[] pixels)
    : base(celProperties, layer)
    {
        Size = new Size(imageCelProperties.Width, imageCelProperties.Height);
        _pixels = pixels;
    }
}
