/* -----------------------------------------------------------------------------
Copyright 2022 Aristurtle

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
namespace AsepriteDotNet.Document.Native;

public struct RawHeader
{
    public uint FileSize;
    public ushort MagicNumber;
    public ushort Frames;
    public ushort Width;
    public ushort Height;
    public ushort ColorDepth;
    public uint Flags;
    public ushort Speed;
    public uint Ignore1;
    public uint Ignore2;
    public byte TransparentIndex;
    public byte[] Ignore3;
    public ushort NumberOfColors;
    public byte PixelWidth;
    public byte PixelHeight;
    public short GridX;
    public short GridY;
    public ushort GridWidth;
    public ushort GridHeight;
    public byte[] Ignore4;
}