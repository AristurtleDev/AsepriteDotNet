// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.FileFormat;

/// <summary>
/// Defines flags for individual palette entries in the Palette Chunk (0x2019) of the Aseprite file format.
/// </summary>
[Flags]
internal enum PaletteFlags : ushort
{
    /// <summary>
    /// Palette entry includes a name string after the RGBA color data.
    /// </summary>
    HasName = 1
}
