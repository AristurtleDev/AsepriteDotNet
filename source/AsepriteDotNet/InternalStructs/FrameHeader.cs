using System.Runtime.InteropServices;

namespace AsepriteDotNet;

[StructLayout(LayoutKind.Explicit)]
internal struct FrameHeader
{
    [FieldOffset(0)]
    public uint Length;

    [FieldOffset(sizeof(uint))]
    public ushort MagicNumber;

    [FieldOffset(sizeof(ushort))]
    public ushort OldChunkCount;

    [FieldOffset(sizeof(ushort))]
    public ushort Duration;

    [FieldOffset(sizeof(ushort))]
    public byte[] Ignore1;

    [FieldOffset(sizeof(byte) * 2)]
    public uint NewChunkCount;
}