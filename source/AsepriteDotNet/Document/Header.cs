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
using System.Diagnostics.CodeAnalysis;

using AsepriteDotNet.Document.Native;

namespace AsepriteDotNet.Document;

public class Header
{
    required public int Frames { get; init; }
    required public int Width { get; init; }
    required public int Height { get; init; }
    required public ColorDepth ColorDepth { get; init; }
    required public HeaderFlags Flags { get; init; }
    required public int TransparentIndex { get; init; }
    required public int NumberOfColors { get; init; }

    public bool IsLayerOpacityVailid => (Flags & HeaderFlags.LayerOpacityValid) != 0;

    [SetsRequiredMembers]
    public Header(RawHeader header)
    {
        Frames = header.Frames;
        Width = header.Width;
        Height = header.Height;
        ColorDepth = (ColorDepth)header.ColorDepth;
        Flags = (HeaderFlags)header.Flags;
        TransparentIndex = header.TransparentIndex;
        NumberOfColors = header.NumberOfColors;
    }
}

// public sealed class Header
// {

//     /// <summary>
//     ///     Gets the total number of frames in the Aseprite file.
//     /// </summary>
//     public int Frames { get; }

//     /// <summary>
//     ///     Gets the width, in pixels, of the image (canvas).
//     /// </summary>
//     public int Width { get; }

//     /// <summary>
//     ///     Gets the height, in pixels, of the image (canvas).
//     /// </summary>
//     public int Height { get; }

//     /// <summary>
//     ///     Gets the color depth mode used by the Aseprite file that defines
//     ///     the number of bits-per-pixel used.
//     /// </summary>
//     public ColorDepth ColorDepth { get; }

//     /// <summary>
//     ///     Gets a value that indicates if the opacity value for layers if
//     ///     valid.
//     /// </summary>
//     public bool IsLayerOpacityVailid { get; }

//     /// <summary>
//     ///     Gets the index within the palette of the color that represents a
//     ///     transparent pixel. This value is only valid when the the color
//     ///     depth mode used is <see cref="ColorDepth.Indexed"/>.
//     /// </summary>
//     public int TransparentIndex { get; }

//     /// <summary>
//     ///     Gets the number of colors defined in the image.
//     /// </summary>
//     public int NumberOfColors { get; }

//     internal Header(RawHeader native)
//     {
//         if (native.MagicNumber != 0xA5E0)
//         {
//             throw new ArgumentException(nameof(native), $"Invalid magic number '0x{native.MagicNumber:X4}'");
//         }

//         if (native.Width == 0 || native.Height == 0)
//         {
//             throw new ArgumentException(nameof(native), $"Invalid image size '{native.Width}x{native.Height}");
//         }

//         if (native.ColorDepth != 8 || native.ColorDepth != 16 || native.ColorDepth != 32)
//         {
//             throw new ArgumentException(nameof(native), $"Invalid color depth '{native.ColorDepth}'");
//         }

//         Frames = native.Frames;
//         Width = native.Width;
//         Height = native.Height;
//         ColorDepth = (ColorDepth)native.ColorDepth;
//         IsLayerOpacityVailid = (native.Flags & 1) != 0;
//         TransparentIndex = native.TransparentIndex;
//         NumberOfColors = native.NumberOfColors;
//     }

// }
