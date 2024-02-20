//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace AsepriteDotNet.IO;

public partial class AsepriteFileReader
{
    private Stream _stream;
    private BinaryReader _reader;
    private bool _isDisposed;

    public AsepriteFileReader(Stream stream, bool leaveOpen = false)
    {
        ValidateStream(stream);
        _stream = stream;
        _reader = new BinaryReader(_stream, Encoding.UTF8, leaveOpen);
    }

    public AsepriteFile ReadFile()
    {
        ValidateDisposed(_isDisposed);
        _stream.Seek(0, SeekOrigin.Begin);

        FileHeader fileHeader;

        Span<byte> fileHeaderData = _reader.ReadBytes(128);
        unsafe
        {
            fixed (byte* ptr = fileHeaderData)
            {
                fileHeader = Marshal.PtrToStructure<FileHeader>((IntPtr)ptr);
            }
        }

        AsepriteFileBuilder builder = new AsepriteFileBuilder(fileHeader);

        //  Read frame-by-frame until all frames read
        for (int frameNum = 0; frameNum < fileHeader.FrameCount; frameNum++)
        {
            ReadFrame(builder);
        }
    }

    private void ReadFrame(AsepriteFileBuilder builder)
    {
        FrameHeader frameHeader;
        Span<byte> frameHeaderData = _reader.ReadBytes(16);
        unsafe
        {
            fixed (byte* ptr = frameHeaderData)
            {
                frameHeader = Marshal.PtrToStructure<FrameHeader>((IntPtr)ptr);
            }
        }

        //  Determine the correct number of chunks to read
        int chunkCount = frameHeader.OldChunkCount;
        if (chunkCount == 0xFFFF && chunkCount < frameHeader.NewChunkCount)
        {
            chunkCount = (int)frameHeader.NewChunkCount;
        }

        //  Read chunk-by-chunk until all chunks are read
        for (int chunkNum = 0; chunkNum < chunkCount; chunkNum++)
        {
            ReadChunk(builder);
        }
    }

    private void ReadChunk(AsepriteFileBuilder builder)
    {
        const int aseChunkTypeOldPalette1 = 0x0004;
        const int aseChunkTypeOldPalette2 = 0x0011;
        const int aseChunkTypeLayer = 0x2004;
        const int aseChunkTypeCel = 0x2005;
        const int aseChunkTypeCelExtra = 0x2006;
        const int aseChunkTypeColorProfile = 0x2007;
        const int aseChunkTypeExternalFiles = 0x2008;
        const int aseChunkTypeMask = 0x2016;
        const int aseChunkTypePath = 0x2017;
        const int aseChunkTypeTags = 0x2018;
        const int aseChunkTypePalette = 0x2019;
        const int aseChunkTypeUserData = 0x2020;
        const int aseChunkTypeSlice = 0x2022;
        const int aseChunkTypeTileset = 0x2023;


        long start = _stream.Position;
        int len = _reader.ReadDword();
        int type = _reader.ReadWord();
        long end = start + len;

        switch (type)
        {
            case aseChunkTypeLayer:
                ReadLayerChunk(builder);
                break;
            case aseChunkTypeCel:
                ReadCelChunk(builder, end);
                break;
            case aseChunkTypeTags:
                break;
            case aseChunkTypePalette:
                break;
            case aseChunkTypeUserData:
                break;
            case aseChunkTypeSlice:
                ReadSliceChunk(builder);
                break;
            case aseChunkTypeTileset:
                ReadTilesetChunk(builder);
                break;
            case aseChunkTypeOldPalette1:
            case aseChunkTypeOldPalette2:
            case aseChunkTypeCelExtra:
            case aseChunkTypeColorProfile:
            case aseChunkTypeExternalFiles:
            case aseChunkTypeMask:
            case aseChunkTypePath:
            default:
                goto SEEK_TO_CHUNK_END;
        }

    SEEK_TO_CHUNK_END:
        _stream.Seek(end, SeekOrigin.Begin);
    }

    private void ReadLayerChunk(AsepriteFileBuilder builder)
    {
        const int normalLayerType = 0;
        const int groupLayerType = 1;
        const int tilemapLayerType = 2;

        LayerChunkHeader layerChunkHeader;
        Span<byte> layerChunkHeaderData = _reader.ReadBytes(LayerChunkHeader.SizeOf);
        unsafe
        {
            fixed (byte* ptr = layerChunkHeaderData)
            {
                layerChunkHeader = Marshal.PtrToStructure<LayerChunkHeader>((IntPtr)ptr);
            }
        }

        string layerName = Encoding.UTF8.GetString(_reader.ReadBytes(layerChunkHeader.NameLen));

        switch (layerChunkHeader.Type)
        {
            case normalLayerType:
                builder.AddImageLayer(layerChunkHeader, layerName);
                break;
            case groupLayerType:
                builder.AddGroupLayer(layerChunkHeader, layerName);
                break;
            case tilemapLayerType:
                int tilesetIndex = _reader.ReadDword();
                builder.AddTilemapLayer(layerChunkHeader, layerName, tilesetIndex);
                break;
        }
    }

    private void ReadCelChunk(AsepriteFileBuilder builder, long chunkEnd)
    {
        const int celTypeRawImage = 0;
        const int celTypeLinked = 1;
        const int celTypeCompressedImage = 2;
        const int celTypeCompressedTilemap = 3;

        CelProperties celProperties;
        Span<byte> celPropertiesData = _reader.ReadBytes(CelProperties.SizeOf);
        unsafe
        {
            fixed (byte* ptr = celPropertiesData)
            {
                celProperties = Marshal.PtrToStructure<CelProperties>((IntPtr)ptr);
            }
        }

        switch (celProperties.Type)
        {
            case celTypeRawImage:
            case celTypeCompressedImage:
                {
                    ImageCelProperties imageCelProperties;
                    Span<byte> imageCelPropertiesData = _reader.ReadBytes(ImageCelProperties.SizeOf);
                    unsafe
                    {
                        fixed (byte* ptr = imageCelPropertiesData)
                        {
                            imageCelProperties = Marshal.PtrToStructure<ImageCelProperties>((IntPtr)ptr);
                        }
                    }

                    int dataLen = (int)(chunkEnd - _stream.Position);
                    byte[] data = _reader.ReadBytes(dataLen);
                    if (celProperties.Type == celTypeCompressedImage)
                    {
                        data = Deflate(data);
                    }
                    builder.AddImageCel(celProperties, imageCelProperties, data);
                }
                break;
            case celTypeLinked:
                int linkedFrameIndex = _reader.ReadWord();
                builder.AddLinkedCel(celProperties, linkedFrameIndex);
                break;
            case celTypeCompressedTilemap:
                {
                    TilemapCelProperties tilemapCelProperties;
                    Span<byte> tilemapCelPropertiesData = _reader.ReadBytes(TilemapCelProperties.SizeOf);
                    unsafe
                    {
                        fixed (byte* ptr = tilemapCelPropertiesData)
                        {
                            tilemapCelProperties = Marshal.PtrToStructure<TilemapCelProperties>((IntPtr)ptr);
                        }
                    }

                    int dataLen = (int)(chunkEnd - _stream.Position);
                    byte[] data = _reader.ReadBytes(dataLen);
                    data = Deflate(data);
                    builder.AddTilemapCel(celProperties, tilemapCelProperties, data);
                }
                break;
        }
    }

    private void ReadUserDataChunk(AsepriteFileBuilder builder)
    {
        uint flags = _reader.ReadDword();

        string? text = null;
        AseColor? color = null;

        if (flags.HasFlag(1))
        {
            int len = _reader.ReadWord();
            text = Encoding.UTF8.GetString(_reader.ReadBytes(len));
        }

        if (flags.HasFlag(2))
        {
            color = ReadPropertiesUnsafe<AseColor>(AseColor.StructSize);
        }

        //  Support for reading custom properties is not implemented at this time.  It is not implemented in the
        //  Aseprite UI and is only available through the LUA scripting extensions. If users ask for this, I'll consider
        //  implementing it, otherwise, it will remain unimplemented until such time that it is added as part of the
        //  AsepriteUI.
        //
        //  HERE IS WHERE I WOULD READ PROPERTIES

        builder.AddUserData(text, color);
    }

    private void ReadSliceChunk(AsepriteFileBuilder builder)
    {
        SliceProperties sliceProperties = ReadPropertiesUnsafe<SliceProperties>(SliceProperties.StructSize);
        string name = Encoding.UTF8.GetString(_reader.ReadBytes(sliceProperties.NameLen));
        AsepriteSliceKey[] keys = new AsepriteSliceKey[sliceProperties.KeyCount];
        for (int i = 0; i < sliceProperties.KeyCount; i++)
        {
            keys[i] = ReadSliceKey(sliceProperties.Flags);
        }

        builder.AddSlice(sliceProperties, name, keys);
    }

    private AsepriteSliceKey ReadSliceKey(uint flags)
    {
        const int flagNinePatch = 1;
        const int flagPivot = 2;

        bool isNinePatch = flags.HasFlag(flagNinePatch);
        bool hasPivot = flags.HasFlag(flagPivot);

        SliceKeyProperties sliceKeyProperties = ReadPropertiesUnsafe<SliceKeyProperties>(SliceKeyProperties.StructSize);
        NinePatchProperties? ninePatchProperties = isNinePatch ? ReadPropertiesUnsafe<NinePatchProperties>(NinePatchProperties.StructSize) : null;
        PivotProperties? pivotProperties = hasPivot ? ReadPropertiesUnsafe<PivotProperties>(PivotProperties.StructSize) : null;
        return new AsepriteSliceKey(sliceKeyProperties, ninePatchProperties, pivotProperties);
    }

    private T ReadPropertiesUnsafe<T>(int size) where T : struct
    {
        T properties;
        Span<byte> data = _reader.ReadBytes(size);
        unsafe
        {
            fixed (byte* ptr = data)
            {
                properties = Marshal.PtrToStructure<T>((IntPtr)ptr);
            }
        }

        return properties;
    }


    private void ReadTilesetChunk(AsepriteFileBuilder builder)
    {
        const int flagExternalFile = 1;
        const int flagEmbedded = 2;

        TilesetProperties tilesetProperties;
        Span<byte> tilesetPropertiesData = _reader.ReadBytes(TilesetProperties.StructSize);
        unsafe
        {
            fixed (byte* ptr = tilesetPropertiesData)
            {
                tilesetProperties = Marshal.PtrToStructure<TilesetProperties>((IntPtr)ptr);
            }
        }

        string name = Encoding.UTF8.GetString(_reader.ReadBytes(tilesetProperties.NameLen));

        if (tilesetProperties.Flags.HasFlag(flagExternalFile))
        {
            //  No support for external files at this time. To my knowledge, Aseprite
            //  doesn't support this directly in the UI and is only something that
            //  can be added through the LUA scripting extensions.  So unless someone
            //  opens an issue and needs this, not implementing it.
            //
            throw new InvalidOperationException($"Tileset '{name}' includes the tileset in an external file. This is not supported at this time.");
        }

        if (tilesetProperties.Flags.DoesNotHaveFlag(flagEmbedded))
        {
            //  Only support at this time for tileset data that is embedded in the file.
            throw new InvalidOperationException($"Tileset '{name}' does not include tileset image embedded in file.");
        }

        uint len = _reader.ReadDword();
        byte[] pixelData = _reader.ReadBytes((int)len);
        pixelData = Deflate(pixelData);
        builder.AddTileset(tilesetProperties, name, pixelData);
    }

    private static byte[] Deflate(byte[] data)
    {
        using (MemoryStream compressedStream = new MemoryStream(data))
        {
            //  First 2 bytes are the zlib header information, skip it, we don't need it.
            compressedStream.Seek(2, SeekOrigin.Begin);

            using (MemoryStream decompressedStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    deflateStream.CopyTo(decompressedStream);
                    return decompressedStream.ToArray();
                }
            }
        }
    }
}
