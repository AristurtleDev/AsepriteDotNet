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
namespace AsepriteDotNet.Document;

public sealed class Header
{
    /// <summary>
    ///     Gets the size, in bytes, of the Aseprite file.
    /// </summary>
    public uint FileSize { get; internal set; }

    /// <summary>
    ///     Gets the magic number value of this <see cref="Header"/>
    ///     instance. This should always be 0xA5E0.
    /// </summary>
    public ushort Magic { get; internal set; }

    /// <summary>
    ///     Gets the total number of frames in the Aseprite file.
    /// </summary>
    public ushort Frames { get; internal set; }

    /// <summary>
    ///     Gets the width, in pixels, of the image (canvas).
    /// </summary>
    public ushort Width { get; internal set; }

    /// <summary>
    ///     Gets the height, in pixels, of the image (canvas).
    /// </summary>
    public ushort Height { get; internal set; }

    /// <summary>
    ///     Gets the color depth mode used by the Aseprite file that defines
    ///     the number of bits-per-pixel used.
    /// </summary>
    public AseColorDepth ColorDepth { get; internal set; }

    /// <summary>
    ///     Gets the flags set for this <see cref="Header"/> instance.
    /// </summary>
    public AseHeaderFlags Flags { get; internal set; }

    /// <summary>
    ///     Gets the default duration, in milliseconds, between frames.
    /// </summary>
    /// <remarks>
    ///     Per aseprite file spec, this value is deprecated.  You should
    ///     instead use the frame duration field.
    /// </remarks>
    public ushort Speed { get; internal set; }

    /// <summary>
    ///     Gets the index within the palette of the color that represents a
    ///     transparent pixel. This value is only valid when the the color
    ///     depth mode used is <see cref="AseColorDepth.Indexed"/>.
    /// </summary>
    public byte TransparentIndex { get; internal set; }

    /// <summary>
    ///     Gets the number of colors defined in the image.
    /// </summary>
    public ushort NumberOfColors { get; internal set; }

    /// <summary>
    ///     Gets the width of a pixel.
    /// </summary>
    public byte PixelWidth { get; internal set; }

    /// <summary>
    ///     Gets the height of a pixel.
    /// </summary>
    public byte PixelHeight { get; internal set; }

    /// <summary>
    ///     Gets the top-left x-coordiante position of the grid displayed in the
    ///     Aseprite editor.
    /// </summary>
    public short GridX { get; internal set; }

    /// <summary>
    ///     Get the top-left y-coordinate position of the grid displayed in the
    ///     Asperite editor.
    /// </summary>
    public short GridY { get; internal set; }

    /// <summary>
    ///     Gets the width of the grid displayed in the Aseprite editor. 0 if
    ///     there is no grid.
    /// </summary>
    public ushort GridWidth { get; internal set; }

    /// <summary>
    ///     Gets the height of the grid displed in the Aseprite editor. 0 if
    ///     there i sno grid.
    /// </summary>
    public ushort GridHeight { get; internal set; }

    internal Header() { }
}
