//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Describes the color depth mode (bits per pixel) used.
/// </summary>
public enum ColorDepth
{
    /// <summary>
    /// Defines that Indexed mode (8-bits per pixel) was used.
    /// </summary>
    Indexed = 8,

    /// <summary>
    /// Defines that Grayscale mode (16-bits per pixel) was used.
    /// </summary>
    Grayscale = 16,

    /// <summary>
    /// Defines that RGBA mode (32-bits per pixel) was used.
    /// </summary>
    RGBA = 32
}