namespace AsepriteDotNet.Core.FileFormat;

public static class AsperiteFileFormat
{
    public const ushort HEADER_MAGIC = 0xA5E0;
    public const int HEADER_SIZE = 128;
    public const ushort FRAME_MAGIC = 0xF1FA;
    public const byte TILE_ID_SHIFT = 0;
    public const uint TILE_FLIP_X_MASK = 0x20000000;
    public const uint TILE_FLIP_Y_MASK = 0x40000000;
    public const uint TILE_90CW_ROTATION_MASK = 0x80000000;
}
