// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.FileFormat;

/// <summary>
/// Defines the content type and behavior of layers in the Layer Chunk (0x2004) of the Aseprite file format.
/// </summary>
internal enum LayerType : ushort
{
    /// <summary>
    /// Standard image layer containing pixel data for traditional sprite artwork.
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Organizational layer that groups other layers without containing pixel data.
    /// </summary>
    Group = 1,

    /// <summary>
    /// Specialized layer for tile-based content using tileset references.
    /// </summary>
    Tilemap = 2
}
