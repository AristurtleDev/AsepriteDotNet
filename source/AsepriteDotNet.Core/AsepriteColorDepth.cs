//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core;

/// <summary>
/// Defines the color depth and pixel format used by Aseprite sprites for color representation.
/// </summary>
/// <remarks>
/// Color depth determines both the bits per pixel and the color format used throughout the sprite.
/// </remarks>
public enum AsepriteColorDepth
{
    /// <summary>
    /// 8-bit indexed color mode using palette references for color representation.
    /// </summary>
    Indexed = 8,

    /// <summary>
    /// 16-bit grayscale mode storing luminance and alpha channel information.
    /// </summary>
    Grayscale = 16,

    /// <summary>
    /// 32-bit RGBA color mode with full color and alpha channel support.
    /// </summary>
    RGBA = 32
}

