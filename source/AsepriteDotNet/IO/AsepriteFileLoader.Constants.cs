//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.IO;

public static partial class AsepriteFileLoader
{
    private const ushort ASE_HEADER_MAGIC = 0xA5E0;
    private const int ASE_HEADER_SIZE = 128;
    private const uint ASE_HEADER_FLAG_LAYER_OPACITY_VALID = 1;

    private const ushort ASE_FRAME_MAGIC = 0xF1FA;

    private const ushort ASE_CHUNK_OLD_PALETTE1 = 0x0004;
    private const ushort ASE_CHUNK_OLD_PALETTE2 = 0x0011;
    private const ushort ASE_CHUNK_LAYER = 0x2004;
    private const ushort ASE_CHUNK_CEL = 0x2005;
    private const ushort ASE_CHUNK_CEL_EXTRA = 0x2006;
    private const ushort ASE_CHUNK_COLOR_PROFILE = 0x2007;
    private const ushort ASE_CHUNK_EXTERNAL_FILES = 0x2008;
    private const ushort ASE_CHUNK_MASK = 0x2016;
    private const ushort ASE_CHUNK_PATH = 0x2017;
    private const ushort ASE_CHUNK_TAGS = 0x2018;
    private const ushort ASE_CHUNK_PALETTE = 0x2019;
    private const ushort ASE_CHUNK_USER_DATA = 0x2020;
    private const ushort ASE_CHUNK_SLICE = 0x2022;
    private const ushort ASE_CHUNK_TILESET = 0x2023;

    private const ushort ASE_LAYER_TYPE_NORMAL = 0;
    private const ushort ASE_LAYER_TYPE_GROUP = 1;
    private const ushort ASE_LAYER_TYPE_TILEMAP = 2;

    private const ushort ASE_LAYER_FLAG_VISIBLE = 1;
    private const ushort ASE_LAYER_FLAG_EDITABLE = 2;
    private const ushort ASE_LAYER_FLAG_LOCKED = 4;
    private const ushort ASE_LAYER_FLAG_BACKGROUND = 8;
    private const ushort ASE_LAYER_FLAG_PREFERS_LINKED = 16;
    private const ushort ASE_LAYER_FLAG_COLLAPSED = 32;
    private const ushort ASE_LAYER_FLAG_REFERENCE = 64;

    private const ushort ASE_CEL_TYPE_RAW_IMAGE = 0;
    private const ushort ASE_CEL_TYPE_LINKED = 1;
    private const ushort ASE_CEL_TYPE_COMPRESSED_IMAGE = 2;
    private const ushort ASE_CEL_TYPE_COMPRESSED_TILEMAP = 3;

    private const uint ASE_CEL_EXTRA_FLAG_PRECISE_BOUNDS_SET = 1;

    private const ushort ASE_PALETTE_FLAG_HAS_NAME = 1;

    private const uint ASE_USER_DATA_FLAG_HAS_TEXT = 1;
    private const uint ASE_USER_DATA_FLAG_HAS_COLOR = 2;

    private const uint ASE_SLICE_FLAGS_IS_NINE_PATCH = 1;
    private const uint ASE_SLICE_FLAGS_HAS_PIVOT = 2;

    private const uint ASE_TILESET_FLAG_EXTERNAL_FILE = 1;
    private const uint ASE_TILESET_FLAG_EMBEDDED = 2;

    private const byte TILE_ID_SHIFT = 0;
    private const uint TILE_ID_MASK = 0x1fffffff;
    private const uint TILE_FLIP_X_MASK = 0x20000000;
    private const uint TILE_FLIP_Y_MASK = 0x40000000;
    private const uint TILE_90CW_ROTATION_MASK = 0x80000000;
}