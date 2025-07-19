// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.FileFormat;

/// <summary>
/// Defines constants and magic numbers for the Aseprite file format (.ase/.aseprite) specification.
/// </summary>
public static class AsperiteFileFormat
{
    /// <summary>
    /// Magic number that identifies a valid Aseprite file header.
    /// </summary>
    public const ushort HEADER_MAGIC = 0xA5E0;

    /// <summary>
    /// Fixed size of the Aseprite file header in bytes.
    /// </summary>
    public const int HEADER_SIZE = 128;

    /// <summary>
    /// Magic number that identifies the start of each frame header.
    /// </summary>
    public const ushort FRAME_MAGIC = 0xF1FA;

    /// <summary>
    /// Bit shift amount for extracting tile ID from tilemap data.
    /// </summary>
    public const byte TILE_ID_SHIFT = 0;

    /// <summary>
    /// Bitmask for horizontal flip flag in tilemap tile data.
    /// </summary>
    public const uint TILE_FLIP_X_MASK = 0x20000000;

    /// <summary>
    /// Bitmask for vertical flip flag in tilemap tile data.
    /// </summary>
    public const uint TILE_FLIP_Y_MASK = 0x40000000;

    /// <summary>
    /// Bitmask for 90-degree clockwise rotation flag in tilemap tile data.
    /// </summary>
    public const uint TILE_90CW_ROTATION_MASK = 0x80000000;
}
