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
using System.Collections.ObjectModel;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO.Image;

namespace AsepriteDotNet.Image;

/// <summary>
///     Represents a generated spritesheet from all frames in an Aseprite file,
///     including slice and tag animation data.
/// </summary>
public sealed class Spritesheet
{
    private Pixel[] _pixels;
    private List<SpritesheetFrame> _frames;
    private List<SpritesheetAnimation> _animations;

    /// <summary>
    ///     Gets the width and height of this <see cref="Spritesheet"/>.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="SpritesheetFrame"/>
    ///     elements within this <see cref="Spritesheet"/>.
    /// </summary>
    public ReadOnlyCollection<SpritesheetFrame> Frames { get; }

    /// <summary>
    ///     Gets a read-only collection of all
    ///     <see cref="SpritesheetAnimation"/> elements within this
    ///     <see cref="Spritesheet"/>.
    /// </summary>
    public ReadOnlyCollection<SpritesheetAnimation> Animations { get; }

    /// <summary>
    ///     Gets a read-only collection of all pixels that make up the image
    ///     for this <see cref="Spritesheet"/>.  Order of pixels is from
    ///     top-to-bottom, read left-to-right.
    /// </summary>
    public ReadOnlyCollection<Pixel> Pixels { get; }

    internal Spritesheet(Size size, List<SpritesheetFrame> frames, List<SpritesheetAnimation> animations, Pixel[] pixels)
    {
        Size = size;
        _frames = frames;
        Frames = _frames.AsReadOnly();
        _animations = animations;
        Animations = _animations.AsReadOnly();
        _pixels = pixels;
        Pixels = Array.AsReadOnly<Pixel>(_pixels);
    }

    /// <summary>
    ///     Writes the pixel data for this <see cref="Spritesheet"/> to disk
    ///     as a .png file.
    /// </summary>
    /// <param name="path">
    ///     The absolute file path to save the generated .png file to.
    /// </param>
    public void ToPng(string path)
    {
        PngWriter.SaveTo(path, Size, _pixels);
    }
}
