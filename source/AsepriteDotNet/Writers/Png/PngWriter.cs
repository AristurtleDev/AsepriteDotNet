/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Buffers.Binary;

using AsepriteDotNet.Document;

namespace AsepriteDotNet.Encoding;

//  Reference: https://www.w3.org/TR/2003/REC-PNG-20031110/#5DataRep
public sealed class PngWriter
{
    private static readonly byte[] _header = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private static readonly uint[] _crcTable = new uint[256];


    [Flags]
    private enum ColorFlags
    {
        Palette = 1,
        TrueColor = 2,
        Alpha = 4
    };

    private enum ImageType
    {
        Grayscale = 0,
        TrueColor = ColorFlags.TrueColor,
        IndexedColor = ColorFlags.Palette | ColorFlags.TrueColor,
        GrayscaleWithAlpha = ColorFlags.Alpha,
        TrueColorWithAlpha = ColorFlags.TrueColor | ColorFlags.Alpha
    }




    static PngWriter()
    {
        //  Make the table for a fast crc
        //  https://www.w3.org/TR/2003/REC-PNG-20031110/#D-CRCAppendix
        uint c;
        for (uint n = 0; n < 256; n++)
        {
            c = n;
            for (int k = 0; k < 8; k++)
            {
                if ((c & 1) != 0)
                {
                    c = 0xEDB88320 ^ (c >> 1);
                }
                else
                {
                    c = c >> 1;
                }
            }
            _crcTable[n] = c;
        }
    }

    private const int MAX_IDAT_CHUNK_LEN = 8192;

    public static void Write(AsepriteFile doc)
    {
        
    }


    private static void WriteIHDR(int width, int height, byte depth, ImageType type, byte cmethod, byte fmethod, byte imethod)
    {

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





    // private static void WriteChunk(BinaryWriter writer, string title, Span<byte> buffer)
    // {
    //     writer.Write(GetCorrectEndian(buffer.Length));
    //     for (int i = 0; i < title.Length; i++)
    //     {
    //         writer.Write((byte)title[i]);
    //     }
    //     writer.Write(buffer);

    //     uint crc = 0xFFFFFFFFU;
    //     for (int i = 0; i < title.Length; i++)
    //     {
    //         crc = crcTable[crc ^ (byte)title[i] & 0xFF] ^ (crc >> 8);
    //     }

    //     for (int i = 0; i < buffer.Length; i++)
    //     {
    //         crc = crcTable[(crc ^ buffer[i]) & 0xFF] ^ (crc >> 8);
    //     }

    //     writer.Write(GetCorrectEndian((int)(crc ^ 0xFFFFFFFFU)));
    // }

    private static void WriteIDAT(BinaryWriter writer, MemoryStream stram)
    {

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
}