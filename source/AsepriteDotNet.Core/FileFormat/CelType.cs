// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.FileFormat;

/// <summary>
/// Defines the data storage format for cel content in the Aseprite file format.
/// </summary>
internal enum CelType : ushort
{
    /// <summary>
    /// Raw uncompressed image data stored directly in the file.
    /// </summary>
    RawImage = 0,

    /// <summary>
    /// Reference to another cel's data instead of storing pixel information.
    /// </summary>
    Linked = 1,

    /// <summary>
    /// ZLIB-compressed image data using the DEFLATE algorithm.
    /// </summary>
    CompressedImage = 2,

    /// <summary>
    /// ZLIB-compressed tilemap data for tilemap layers.
    /// </summary>
    CompressedTilemap = 3
}
