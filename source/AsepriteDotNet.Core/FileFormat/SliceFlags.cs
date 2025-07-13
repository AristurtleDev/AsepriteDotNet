namespace AsepriteDotNet.Core.FileFormat;

[Flags]
internal enum SliceFlags : uint
{
    IsNinePatch = 1,
    HasPivot = 2
}
