//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Document;

namespace AsepriteDotNet.IO;

public static partial class AsepriteFileLoader
{
    private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;
    private static bool DoesNotHaveFlag(uint value, uint flag) => !HasFlag(value, flag);


    private static AseColor[] PixelsToColor(byte[] pixels, AsepriteColorDepth depth, Palette palette)
    {
        return depth switch
        {
            AsepriteColorDepth.Indexed => IndexedPixelsToColor(pixels, palette),
            AsepriteColorDepth.Grayscale => GrayscalePixelsToColor(pixels),
            AsepriteColorDepth.RGBA => RgbaPixelsToColor(pixels),
            _ => throw new InvalidOperationException($"Unknown Color Depth: {depth}"),
        };
    }

    private static AseColor[] RgbaPixelsToColor(byte[] pixels)
    {
        int bpp = (int)AsepriteColorDepth.RGBA / 8;
        AseColor[] result = new AseColor[pixels.Length / bpp];

        for (int i = 0, b = 0; i < result.Length; i++, b += bpp)
        {
            byte red = pixels[b];
            byte green = pixels[b + 1];
            byte blue = pixels[b + 2];
            byte alpha = pixels[b + 3];
            result[i] = new AseColor(red, green, blue, alpha);
        }

        return result;
    }

    private static AseColor[] GrayscalePixelsToColor(byte[] pixels)
    {
        int bpp = (int)AsepriteColorDepth.Grayscale / 8;
        AseColor[] result = new AseColor[pixels.Length / bpp];

        for (int i = 0, b = 0; i < result.Length; i++, b += bpp)
        {
            byte red = pixels[b];
            byte green = pixels[b];
            byte blue = pixels[b];
            byte alpha = pixels[b + 1];
            result[i] = new AseColor(red, green, blue, alpha);
        }

        return result;
    }

    private static AseColor[] IndexedPixelsToColor(byte[] pixels, Palette palette)
    {
        int bpp = (int)AsepriteColorDepth.Indexed / 8;
        AseColor[] result = new AseColor[pixels.Length / bpp];

        for (int i = 0; i < pixels.Length; i++)
        {
            int index = pixels[i];

            if (index == palette.TransparentIndex)
            {
                result[i] = new AseColor(0, 0, 0, 0);
            }
            else
            {
                result[i] = palette.Colors[index];
            }
        }

        return result;
    }
}
