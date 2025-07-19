// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.FileFormat;

/// <summary>
/// Defines flags for tileset storage and referencing in the Tileset Chunk (0x2023) of the Aseprite file format.
/// </summary>
[Flags]
internal enum TilesetFlags : uint
{
    /// <summary>
    /// Tileset references an external file rather than embedding tile data.
    /// </summary>
    ExternalFile = 1,

    /// <summary>
    /// Tileset includes compressed tile image data within the file.
    /// </summary>
    Embedded = 2
}
