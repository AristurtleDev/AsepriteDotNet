namespace AsepriteDotNet.Core.FileFormat;

[Flags]
internal enum LayerFlags : ushort
{
    None = 0,
    Visible = 1,
    Editable = 2,
    Locked = 4,
    Background = 8,
    PrefersLinked = 16,
    Collapsed = 32,
    Reference = 64
}
