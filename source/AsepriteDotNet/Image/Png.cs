/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
using System.Buffers.Binary;
using System.Drawing;
using System.IO.Compression;

namespace AsepriteDotNet.Image;

//  Reference: https://www.w3.org/TR/2003/REC-PNG-20031110/#5DataRep
public static class Png
{
    //  Common IDAT chunk sizes are between 8 and 32 Kib.  Opting to use
    //  8Kib for this project.
    private const int MAX_IDAT_LEN = 8192;

    //  Each horizontal scanline will be 1Kib. Combining this with the max IDAT
    //  length above, this means each IDAT chunk will be 8 scanlines
    private const int SCANLINE_LEN = 1024;

    private static readonly byte[] _header = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private static readonly uint[] _crcTable = new uint[256];

    public static void SaveTo(string path, Size size, Color[] data)
    {

    }

    //  IHDR
    //   -----------------------------------
    //  | Width                 |   4-bytes |
    //  | Height                |   4-bytes |
    //  | Bit Depth             |   1-byte  |
    //  | Color Type            |   1-byte  |
    //  | Compressions Method   |   1-byte  |
    //  | Filter Method         |   1-byte  |
    //  | Interlace Method      |   1-byte  |
    //   -----------------------------------
    //
    //  Width:
    //      4-byte integer (Network Byte Order) that defines the width, in
    //      pixels of the final image
    //
    //  Height:
    //      4-byte integer (Network Byte Order) that defines the height, in
    //      pixels of the final image
    //
    //  Bit Depth:
    //      1-byte integer that indicates the number of bits per sample. Valid
    //      values depend on the color type used
    //      
    //       -----------------------------------------------
    //      | Color Type            |   Allowed bit depths  |
    //       -----------------------------------------------
    //      | Grayscale             |   1, 2, 4, 8, 16      |
    //      | Truecolor             |   8, 16               |
    //      | Index-color           |   1, 2, 4, 8          |
    //      | Grayscale with alpha  |   8, 16               |
    //      | Truecolor with alpha  |   8, 16               |
    //       -----------------------------------------------
    //
    //  Color Type:
    //      1-byte integer the indicates the color type used
    //
    //       -----------------------------------
    //      | Color Type            |   Value   |
    //       -----------------------------------
    //      | Grayscale             |   0       |
    //      | Truecolor             |   2       |
    //      | Index-color           |   3       |
    //      | Grayscale with alpha  |   4       |
    //      | Truecolor with alpha  |   6       |
    //       -----------------------------------
    //
    //  Compression Method:
    //      1-byte integer that indicates the compression method used. Only
    //      compression method 0 (deflate/inflate compression with a sliding
    //      window of 32768 bytes) is defined
    //
    //  Filter Method:
    //      1-byte integer that indicates the preprocessing method applied to
    //      the image data before compression.  Only filter method 0 (adaptive
    //      filtering with five basic filter types) is defined.
    //
    //  Interlace Method:
    //      1-byte integer that indicates the transmission order of the image
    //      data.  
    //  
    //       ---------------------------------------
    //      | Interlace Method          |   Value   |
    //       ---------------------------------------
    //      | Standard (no interlace)   |   0       |
    //      | Adam7                     |   1       |
    //       ---------------------------------------
    //
    //  Reference: https://www.w3.org/TR/2003/REC-PNG-20031110/#11IHDR
    private static void WriteIHDR(BinaryWriter writer, Size size)
    {
        Span<byte> ihdr = stackalloc byte[13];
        ihdr.Clear();
        BinaryPrimitives.WriteInt32BigEndian(ihdr.Slice(0), size.Width);
        BinaryPrimitives.WriteInt32BigEndian(ihdr.Slice(4), size.Height);
        ihdr[8] = 8;    //  Bit-depth
        ihdr[9] = 6;    //  Color Type
        ihdr[10] = 0;   //  Compression Method
        ihdr[11] = 0;   //  Filter Method
        ihdr[12] = 0;   //  Interlace Method

        WriteChunk(writer, "IHDR", ihdr);
    }

    private static void WriteIDAT(BinaryWriter writer, Color[] data)
    {
        using MemoryStream ms = new();

        //  Zlib deflate header for Default Compression
        ms.WriteByte(0x78);
        ms.WriteByte(0x9C);

        using DeflateStream deflate = new DeflateStream(ms, CompressionMode.Compress, leaveOpen: true);


    }

    //  Chunks consist of three or four parts
    //
    //   ------------    ----------------    ----------------    ---------
    //  |   Length   |  |   Chunk Type   |  |   Chunk Data   |  |   CRC   |
    //   ------------    ----------------    ----------------    ---------
    //
    //  If there is no data (Length = 0), then the data chunk is not written
    //
    //  Length: 
    //      A 4-byte unsigned integer giving the number of bytes in the chunk's
    //      data field. Only the data field. Do not include the length field
    //      itself, the chunk type field, or the crc field
    //
    //  Chunk Type:
    //      A sequence of 4-bytes that represent the lowercase and/or uppercase
    //      ISO-646 letters (A-Z and a-z) of the chunk type (e.g. IHDR, IDAT)
    //
    //  Chunk Data:
    //      The data bytes of the chunk. Can be zero length (empty)
    //
    //  CRC:
    //      A 4-byte CRC calculated on the preceding bytes in the chunk
    //      including the chunk type field and the chunk data field, but not the
    //      chunk length field. CRC is always present, even if the chunk data
    //      field is excluded.
    //
    //  Reference: https://www.w3.org/TR/2003/REC-PNG-20031110/#11Chunks
    private static void WriteChunk(BinaryWriter writer, string chunkType, ReadOnlySpan<byte> data)
    {
        //  Write the length
        writer.Write(ToBigEndian(data.Length));

        //  Create a byte array that will contain the chunk type and the data
        byte[] typeAndData = new byte[4 + data.Length];

        //  First insert the chunk type bytes
        typeAndData[0] = (byte)chunkType[0];
        typeAndData[1] = (byte)chunkType[1];
        typeAndData[2] = (byte)chunkType[2];
        typeAndData[3] = (byte)chunkType[3];

        //  Copy the chunk data into it
        for (int i = 0; i < data.Length; i++)
        {
            typeAndData[4 + i] = data[i];
        }

        //  Write the type and data
        writer.Write(typeAndData);

        //  Calcluate the CRC of the type and data
        int crc = CRC(typeAndData);

        //  Write the crc
        writer.Write(ToBigEndian(crc));
    }

    //  CRC fields are calculated using standardized CRC methods with pre and
    //  post conditioning, as defined in ISO-3309 and ITU-T V.42
    //
    //  In PNG, the 32-bit CRC is initialized to all 1's, and then data from
    //  each byte is processed from the least significant bit (1) to the most
    //  significant bit (128).  After all data is processed, the CRC is
    //  inverted (its ones complement is taken)
    //
    //  Refernece: https://www.w3.org/TR/2003/REC-PNG-20031110/#5CRC-algorithm
    //  Reference: https://www.w3.org/TR/2003/REC-PNG-20031110/#D-CRCAppendix
    private static int CRC(ReadOnlySpan<byte> buffer)
    {
        //  Initialize all bits to 1
        uint crc = 0xFFFFFFFF;

        //  Process each bit
        for (int n = 0; n < buffer.Length; n++)
        {
            crc = _crcTable[(crc ^ buffer[n]) & 0xFF] ^ (crc >> 8);
        }

        //  Invert the crc
        crc ^= 0xffffffff;

        return (int)crc;
    }

    //  Per https://www.w3.org/TR/2003/REC-PNG-20031110/#7Integers-and-byte-order
    //      
    //      "All integers that require more than one byte shall be in network 
    //      byte order"
    //
    //  Basically, we have to ensure that all interger type values are in 
    //  BigEndian.  
    //
    //  This method will check for endianess and convert to BigEndian if needed.
    private static int ToBigEndian(int value)
    {
        //  https://stackoverflow.com/a/19276259

        //  If already BigEndian just return the value back
        if (!BitConverter.IsLittleEndian) { return value; }

        uint b = (uint)value;
        uint b0 = (b & 0x000000FF) << 24;   //  Least significant to most significant
        uint b1 = (b & 0x0000FF00) << 8;    //  2nd least significant to 2nd most significant
        uint b2 = (b & 0x00FF0000) >> 8;    //  2nd most significant to 2nd least significant
        uint b3 = (b & 0xFF000000) >> 24;   //  Most significant to least significant.

        return (int)(b0 | b1 | b2 | b3);

    }

    /*
         Update a running Adler-32 checksum with the bytes buf[0..len-1]
       and return the updated checksum. The Adler-32 checksum should be
       initialized to 1.

       Usage example:

         unsigned long adler = 1L;

         while (read_buffer(buffer, length) != EOF) {
           adler = update_adler32(adler, buffer, length);
         }
         if (adler != original_adler) error();
      */

    private const uint BASE = 65521; /* largest prime smaller than 65536 */
    private static uint update_adler32(uint adler, ReadOnlySpan<byte> buf, int len)
    {
        uint s1 = adler & 0xffff;
        uint s2 = (adler >> 16) & 0xffff;
        int n;

        for (n = 0; n < len; n++)
        {
            s1 = (s1 + buf[n]) % BASE;
            s2 = (s2 + s1) % BASE;
        }
        return (s2 << 16) + s1;
    }

    /* Return the adler32 of the bytes buf[0..len-1] */
    private static uint adler32(ReadOnlySpan<byte> buf, int len)
    {
        return update_adler32(1, buf, len);
    }

}