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
using System.Drawing;

using AsepriteDotNet.Document;

namespace AsepriteDotNet.Tests;

public sealed class BlendFunctionsTests
{
    private static readonly Color _green = Color.FromArgb(255, 106, 190, 48);
    private static readonly Color _purple = Color.FromArgb(255, 63, 63, 116);
    private static readonly Color _pink = Color.FromArgb(255, 215, 123, 186);
    private static readonly Color _red = Color.FromArgb(255, 172, 50, 50);
    private static readonly Color _orange = Color.FromArgb(255, 223, 113, 38);
    private static readonly Color _transparent = Color.Transparent;

    /*
        In order to test the blend modes, I needed to have hard factual values to
        assert as the expected values after performing a blend.  Since the blend
        functions were ported form Aseprite, we'll use Aseprite itself as the source
        of truth for what the values should be.

        To get these values, a 4x4 sprite was created with the following colors

        * orange      = argb(255, 223, 113, 38)
        * green       = argb(255, 106, 190, 48)
        * purple      = argb(255, 63, 63, 116)
        * pink        = argb(255, 215, 123, 186)
        * red         = argb(255, 172, 50, 50)
        * transparent = argb(0, 0, 0, 0)

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
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_DarkenBlendTest()
    {
        BlendMode mode = BlendMode.Darken;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 106, 113, 38), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 63, 63, 38), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 215, 113, 38), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 172, 50, 38), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_MultiplyBlendTest()
    {
        BlendMode mode = BlendMode.Multiply;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 93, 84, 7), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 55, 28, 17), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 188, 55, 28), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 150, 22, 7), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ColorBurnBlendTest()
    {
        BlendMode mode = BlendMode.ColorBurn;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 85, 108, 0), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 35, 0, 0), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 209, 0, 0), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 160, 0, 0), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_LightenBlendTest()
    {
        BlendMode mode = BlendMode.Lighten;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 223, 190, 48), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 223, 113, 116), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 223, 123, 186), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 223, 113, 50), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ScreenBlendTest()
    {
        BlendMode mode = BlendMode.Screen;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 236, 219, 79), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 231, 148, 137), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 250, 181, 196), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 245, 141, 81), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ColorDodgeBlendTest()
    {
        BlendMode mode = BlendMode.ColorDodge;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 255, 255, 56), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 113, 136), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 221, 219), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 90, 59), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_AdditionBlendTest()
    {
        BlendMode mode = BlendMode.Addition;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 255, 255, 86), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 176, 154), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 236, 224), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 163, 88), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_OverlayBlendTest()
    {
        BlendMode mode = BlendMode.Overlay;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 185, 183, 14), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 110, 56, 35), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 245, 109, 138), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 234, 44, 15), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_SoftLightBlendTest()
    {
        BlendMode mode = BlendMode.SoftLight;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 150, 184, 21), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 111, 58, 72), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 229, 116, 151), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 200, 45, 22), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_HardLightBlendTest()
    {
        BlendMode mode = BlendMode.HardLight;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 218, 168, 14), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 207, 56, 35), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 245, 109, 55), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 234, 44, 15), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_DifferenceBlendTest()
    {
        BlendMode mode = BlendMode.Difference;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 117, 77, 10), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 160, 50, 78), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 8, 10, 148), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 51, 63, 12), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ExclusionBlendTest()
    {
        BlendMode mode = BlendMode.Exclusion;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 143, 135, 72), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 176, 120, 120), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 62, 126, 168), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 95, 119, 74), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_SubtractBlendTest()
    {
        BlendMode mode = BlendMode.Subtract;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 0, 77, 10), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 0, 0, 78), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 0, 10, 148), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 0, 0, 12), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_DivideBlendTest()
    {
        BlendMode mode = BlendMode.Divide;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 121, 255, 255), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 72, 142, 255), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 246, 255, 255), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 197, 113, 255), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_HueBlendTest()
    {
        BlendMode mode = BlendMode.Hue;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 214, 130, 72), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 93, 61, 40), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 199, 145, 107), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 142, 70, 20), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_SaturationBlendTest()
    {
        BlendMode mode = BlendMode.Saturation;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 92, 202, 17), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 92, 29, 214), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 255, 98, 205), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 186, 51, 1), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_ColorBlendTest()
    {
        BlendMode mode = BlendMode.Color;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 234, 124, 49), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 127, 51, 0), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 242, 132, 57), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 160, 65, 0), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }

    [Fact]
    public void Color_LuminosityBlendTest()
    {
        BlendMode mode = BlendMode.Luminosity;

        //  Color on Color
        Assert.Equal(Color.FromArgb(255, 94, 178, 36), BlendFunctions.Blend(mode, _green, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 131, 131, 184), BlendFunctions.Blend(mode, _purple, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 195, 103, 166), BlendFunctions.Blend(mode, _pink, _orange, 255));
        Assert.Equal(Color.FromArgb(255, 223, 101, 101), BlendFunctions.Blend(mode, _red, _orange, 255));

        //  Color on Transparent
        Assert.Equal(_orange, BlendFunctions.Blend(mode, _transparent, _orange, 255));

        //  Transparent on Color
        Assert.Equal(_green, BlendFunctions.Blend(mode, _green, _transparent, 255));
        Assert.Equal(_purple, BlendFunctions.Blend(mode, _purple, _transparent, 255));
        Assert.Equal(_pink, BlendFunctions.Blend(mode, _pink, _transparent, 255));
        Assert.Equal(_red, BlendFunctions.Blend(mode, _red, _transparent, 255));

        //  Transparent on Transparent
        Assert.Equal(_transparent, BlendFunctions.Blend(mode, _transparent, _transparent, 255));
    }
}