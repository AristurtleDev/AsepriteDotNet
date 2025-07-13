namespace AsepriteDotNet.Core.FileFormat;

[Flags]
internal enum TilesetFlags : uint
{
    ExternalFile = 1,
    Embedded = 2
}
