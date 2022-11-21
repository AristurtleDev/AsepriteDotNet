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
using AsepriteDotNet.Document;
using AsepriteDotNet.InternalStructs;

namespace AsepriteDotNet.IO;

public static class AsepriteFileReader
{

    public static void ReadFile(string path)
    {
        using AsepriteBinaryReader reader = new(File.OpenRead(path));

        AseHeader header;
        header.FileSize = reader.ReadDword();
        header.MagicNumber = reader.ReadWord();
        header.Frames = reader.ReadWord();
        header.Width = reader.ReadWord();
        header.Height = reader.ReadWord();
        header.ColorDepth = reader.ReadWord();
        header.Flags = reader.ReadDword();
        header.Speed = reader.ReadWord();
        header.Ignore1 = reader.ReadDword();
        header.Ignore2 = reader.ReadDword();
        header.TransparentIndex = reader.ReadByte();
        header.Ignore3 = reader.ReadBytes(3);
        header.NumberOfColors = reader.ReadWord();
        header.PixelWidth = reader.ReadByte();
        header.PixelHeight = reader.ReadByte();
        header.GridX = reader.ReadShort();
        header.GridY = reader.ReadShort();
        header.GridWidth = reader.ReadWord();
        header.GridHeight = reader.ReadWord();
        header.Ignore4 = reader.ReadBytes(84);

        for (int i = 0; i < header.Frames; i++)
        {
            AseFrame frame;
            frame.Length = reader.ReadDword();
            frame.Magic = reader.ReadWord();
            frame.OldCount = reader.ReadWord();
            frame.Duration = reader.ReadWord();
            frame.Ignore1 = reader.ReadBytes(2);
            frame.NewCount = reader.ReadDword();

        }

    }
}