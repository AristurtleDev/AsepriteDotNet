//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents an image cel containing RGBA pixel data for traditional sprite artwork.
/// </summary>
/// <remarks>
/// Image cels store pixel data that can be painted, transformed, and composited using various blend modes.
/// The pixel data is stored in row-major order from top-left to bottom-right, with each pixel containing
/// red, green, blue, and alpha components as specified in the Aseprite file format.
/// </remarks>
public sealed class AsepriteImageCel : AsepriteCel
{
    internal Rgba32[] InternalPixels { get; set; }

    /// <summary>
    /// Gets the dimensions of the cel's pixel data in pixels.
    /// </summary>
    /// <remarks>
    /// The size represents the actual dimensions of the pixel array and may differ from the layer's
    /// canvas size. Cel positioning is handled separately through the inherited Location property.
    /// </remarks>
    public Size Size { get; internal set; }

    /// <summary>
    /// Gets the pixel data contained in this cel.
    /// </summary>
    /// <remarks>
    /// Pixels are stored from top-left to bottom-right, with each row containing pixels from left to right.
    /// The total number of pixels equals Size.Width × Size.Height. Each pixel contains 8-bit red, green,
    /// blue, and alpha components as defined in the RGBA color format specification.
    /// </remarks>
    public ReadOnlySpan<Rgba32> Pixels => InternalPixels;

    internal AsepriteImageCel() { }
}
