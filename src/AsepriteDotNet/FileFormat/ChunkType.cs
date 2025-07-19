// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.FileFormat;

/// <summary>
/// Defines the chunk types used in the Aseprite file format for organizing different data sections.
/// </summary>
internal enum ChunkType : ushort
{
    /// <summary>
    /// Undefined or invalid chunk type.
    /// </summary>
    None = 0x0000,

    /// <summary>
    /// Legacy palette chunk with RGB values in 0-255 range.
    /// </summary>
    OldPalette1 = 0x0004,

    /// <summary>
    /// Legacy palette chunk with RGB values in 0-63 range.
    /// </summary>
    OldPalette2 = 0x0011,

    /// <summary>
    /// Layer properties including visibility, type, blending, and hierarchy.
    /// </summary>
    Layer = 0x2004,

    /// <summary>
    /// Cel data including position, opacity, and pixel or tile information.
    /// </summary>
    Cel = 0x2005,

    /// <summary>
    /// Additional cel properties beyond basic positioning and content.
    /// </summary>
    CelExtra = 0x2006,

    /// <summary>
    /// Color profile information for accurate color reproduction.
    /// </summary>
    ColorProfile = 0x2007,

    /// <summary>
    /// External files referenced by the sprite for palettes, tilesets, or extensions.
    /// </summary>
    ExternalFiles = 0x2008,

    /// <summary>
    /// Deprecated mask chunk for selection areas.
    /// </summary>
    Mask = 0x2016,

    /// <summary>
    /// Reserved path chunk that was never implemented.
    /// </summary>
    Path = 0x2017,

    /// <summary>
    /// Animation tags with frame ranges, loop directions, and repeat counts.
    /// </summary>
    Tags = 0x2018,

    /// <summary>
    /// Modern palette chunk with full RGBA color support and named color entries.
    /// </summary>
    Palette = 0x2019,

    /// <summary>
    /// Custom data associated with the previously read chunk including text, colors, and properties.
    /// </summary>
    UserData = 0x2020,

    /// <summary>
    /// Named regions within sprites with optional 9-patch scaling and pivot points.
    /// </summary>
    Slice = 0x2022,

    /// <summary>
    /// Tileset definitions with tile dimensions and image data for tilemap layers.
    /// </summary>
    Tileset = 0x2023
}
