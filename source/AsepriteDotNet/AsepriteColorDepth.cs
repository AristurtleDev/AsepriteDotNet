//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
///	Defines the color depth mode used by an Aseprite sprite.
/// </summary>
public enum AsepriteColorDepth : ushort
{
    /// <summary>
    /// Defines that the Aseprite sprite uses an Indexed mode of 8-bits per pixel.
    /// </summary>
    Indexed = 8,

    /// <summary>
    ///	Defines that the Aseprite sprite uses a Grayscale mode of 16-bits per pixel.
    /// </summary>
    Grayscale = 16,

    /// <summary>
    ///	Defines that the Aseprite sprite uses an RGBA mode of 32-bits per pixel.
    /// </summary>
    RGBA = 32
}

