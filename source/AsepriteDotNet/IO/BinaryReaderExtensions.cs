//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.IO;

internal static class BinaryReaderExtensions
{
    internal static ushort ReadWord(this BinaryReader reader) => reader.ReadUInt16();
    internal static short ReadShort(this BinaryReader reader) => reader.ReadInt16();
    internal static uint ReadDword(this BinaryReader reader) => reader.ReadUInt32();
    internal static int ReadLong(this BinaryReader reader) => reader.ReadInt32();
    internal static string ReadString(this BinaryReader reader) => System.Text.Encoding.UTF8.GetString(reader.ReadBytes(reader.ReadWord()));

    internal static void IgnoreBytes(this BinaryReader reader, int count) => reader.BaseStream.Seek(count, SeekOrigin.Current);
    internal static void IgnoreByte(this BinaryReader reader) => reader.IgnoreBytes(sizeof(byte));
    internal static void IgnoreWord(this BinaryReader reader) => reader.IgnoreBytes(sizeof(ushort));
    internal static void IgnoreShort(this BinaryReader reader) => reader.IgnoreBytes(sizeof(ushort));
    internal static void IgnoreDword(this BinaryReader reader) => reader.IgnoreBytes(sizeof(uint));
    internal static void IgnoreLong(this BinaryReader reader) => reader.IgnoreBytes(sizeof(int));
    internal static void IgnoreString(this BinaryReader reader) => reader.IgnoreBytes(reader.ReadWord());
}
