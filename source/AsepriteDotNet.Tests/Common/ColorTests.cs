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
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests;

public sealed class ColorTests
{
    private static readonly Color _green = Color.FromRGBA(106, 190, 48, 255);
    private static readonly Color _purple = Color.FromRGBA(63, 63, 116, 255);
    private static readonly Color _pink = Color.FromRGBA(215, 123, 186, 255);
    private static readonly Color _red = Color.FromRGBA(172, 50, 50, 255);
    private static readonly Color _orange = Color.FromRGBA(223, 113, 38, 255);
    private static readonly Color _transparent = Color.FromRGBA(0, 0, 0, 0);


    [Fact]
    public void Color_FromRGBATest()
    {
        byte red = 10;
        byte green = 20;
        byte blue = 30;
        byte alpha = 40;

        Color color = Color.FromRGBA(red, green, blue, alpha);

        Assert.Equal(red, color.R);
        Assert.Equal(green, color.G);
        Assert.Equal(blue, color.B);
        Assert.Equal(alpha, color.A);
    }



    /*
        In order to test the blend modes, I needed to have hard factual values to
        assert as the expected values after performing a blend.  Since the blend
        functions were ported form Aseprite, we'll use Aseprite itself as the source
        of truth for what the values should be.

        To get these values, a 4x4 sprite was created with the following colors

        * orange      = rgba(223, 113, 38, 255)
        * green       = rgba(106, 190, 48, 255)
        * purple      = rgba(63, 63, 116, 255)
        * pink        = rgba(215, 123, 186, 255)
        * red         = rgba(172, 50, 50, 255)
        * transparent = rgba(0, 0, 0, 0)

        Next two layers were added to the sprite. The bottom layer consists of the
        following pixels, in order from top-to-bottom, read left-to-right

        [Layer 1]
        green, green, purple, purple,
        green, green, purple, purple,
        pink,  pink,  red,    red,
        pink,  pink,  red,    red,

        The top layers consists fo the following pixles, in order from 
        top-to-bottom, read left-to-right

        [Layer 2]
        transparent, transparent, transparent, transparent,
        transparent, orange,      orange,      transparent,
        transparent, orange,      orange,      transparent,
        transparent, transparent, transparent, transparent,

        There is much simpler ways of doing this (like making it a 2x2 image and
        trimming off the transparent border), but this is what i did.

        Anyway, the next steps was to set the Blend Mode used by the top layer, then
        copy the RGBA value of each pixel where it overlapped one of the base layer
        colors into the test for that blend mode.

        It was a manual process, but the point of the tests is to ensure that the
        ported code blends colors the same way that Aseprite does to match it 1:1.
    */
    [Fact]
    public void Color_NormalBlendTeset()
    {
        BlendMode mode = BlendMode.Normal;

        //  Color on Color
        Assert.Equal(_orange, Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(_orange, Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(_orange, Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(_orange, Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_DarkenBlendTest()
    {
        BlendMode mode = BlendMode.Darken;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(106, 113, 38, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(63, 63, 38, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(215, 113, 38, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(172, 50, 38, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_MultiplyBlendTest()
    {
        BlendMode mode = BlendMode.Multiply;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(93, 84, 7, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(55, 28, 17, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(188, 55, 28, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(150, 22, 7, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ColorBurnBlendTest()
    {
        BlendMode mode = BlendMode.ColorBurn;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(85, 108, 0, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(35, 0, 0, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(209, 0, 0, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(160, 0, 0, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_LightenBlendTest()
    {
        BlendMode mode = BlendMode.Lighten;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(223, 190, 48, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(223, 113, 116, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(223, 123, 186, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(223, 113, 50, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ScreenBlendTest()
    {
        BlendMode mode = BlendMode.Screen;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(236, 219, 79, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(231, 148, 137, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(250, 181, 196, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(245, 141, 81, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ColorDodgeBlendTest()
    {
        BlendMode mode = BlendMode.ColorDodge;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(255, 255, 56, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 113, 136, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 221, 219, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 90, 59, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_AdditionBlendTest()
    {
        BlendMode mode = BlendMode.Addition;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(255, 255, 86, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 176, 154, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 236, 224, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 163, 88, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_OverlayBlendTest()
    {
        BlendMode mode = BlendMode.Overlay;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(185, 183, 14, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(110, 56, 35, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(245, 109, 138, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(234, 44, 15, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_SoftLightBlendTest()
    {
        BlendMode mode = BlendMode.SoftLight;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(150, 184, 21, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(111, 58, 72, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(229, 116, 151, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(200, 45, 22, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_HardLightBlendTest()
    {
        BlendMode mode = BlendMode.HardLight;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(218, 168, 14, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(207, 56, 35, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(245, 109, 55, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(234, 44, 15, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_DifferenceBlendTest()
    {
        BlendMode mode = BlendMode.Difference;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(117, 77, 10, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(160, 50, 78, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(8, 10, 148, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(51, 63, 12, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ExclusionBlendTest()
    {
        BlendMode mode = BlendMode.Exclusion;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(143, 135, 72, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(176, 120, 120, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(62, 126, 168, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(95, 119, 74, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_SubtractBlendTest()
    {
        BlendMode mode = BlendMode.Subtract;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(0, 77, 10, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(0, 0, 78, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(0, 10, 148, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(0, 0, 12, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_DivideBlendTest()
    {
        BlendMode mode = BlendMode.Divide;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(121, 255, 255, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(72, 142, 255, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(246, 255, 255, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(197, 113, 255, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_HueBlendTest()
    {
        BlendMode mode = BlendMode.Hue;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(214, 130, 72, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(93, 61, 40, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(199, 145, 107, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(142, 70, 20, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_SaturationBlendTest()
    {
        BlendMode mode = BlendMode.Saturation;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(92, 202, 17, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(92, 29, 214, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(255, 98, 205, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(186, 51, 1, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ColorBlendTest()
    {
        BlendMode mode = BlendMode.Color;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(234, 124, 49, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(127, 51, 0, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(242, 132, 57, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(160, 65, 0, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_LuminosityBlendTest()
    {
        BlendMode mode = BlendMode.Luminosity;

        //  Color on Color
        Assert.Equal(Color.FromRGBA(94, 178, 36, 255), Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromRGBA(131, 131, 184, 255), Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromRGBA(195, 103, 166, 255), Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromRGBA(223, 101, 101, 255), Color.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, Color.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, Color.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, Color.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, Color.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, Color.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, Color.Blend(mode, _transparent, _transparent, 255));
    }
}