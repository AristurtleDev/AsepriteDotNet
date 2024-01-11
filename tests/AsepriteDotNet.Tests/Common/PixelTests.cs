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

public sealed class PixelTests
{
    private static readonly Pixel _green = new Pixel(106, 190, 48, 255);
    private static readonly Pixel _orange = new Pixel(223, 113, 38, 255);
    private static readonly Pixel _transparent = new Pixel(0, 0, 0, 0);

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

        The top layers consists fo the following pixels, in order from
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
    public void Always_Return_Backdrop_If_Source_Transparent_All_BlendModes()
    {
        foreach (BlendMode mode in (BlendMode[])Enum.GetValues(typeof(BlendMode)))
        {
            Pixel background = _orange;
            Pixel source = _transparent;
            Pixel expected = _orange;
            background.Blend(source, 255, mode);
            Assert.Equal(expected, background);
        }
    }

    [Fact]
    public void Always_Return_Source_If_Backdrop_Transparent_All_BlendModes()
    {
        foreach (BlendMode mode in (BlendMode[])Enum.GetValues(typeof(BlendMode)))
        {
            Pixel background = _transparent;
            Pixel source = _orange;
            Pixel expected = _orange;
            background.Blend(source, 255, mode);
            Assert.Equal(expected, background);
        }
    }

    [Fact]
    public void Always_Return_Transparent_If_Source_And_Backdrop_Transparent_All_BlendModes()
    {
        foreach (BlendMode mode in (BlendMode[])Enum.GetValues(typeof(BlendMode)))
        {
            Pixel background = _transparent;
            Pixel source = _transparent;
            Pixel expected = _transparent;
            background.Blend(source, 255, mode);
            Assert.Equal(expected, background);
        }
    }

    [Fact]
    public void Normal_Blend_Color_on_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Normal);
        Assert.Equal(_orange, backdrop);
    }

    [Fact]
    public void Darken_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Darken);
        Pixel expected = new Pixel(106, 113, 38, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Multiply_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Multiply);
        Pixel expected = new Pixel(93, 84, 7, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void ColorBurn_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.ColorBurn);
        Pixel expected = new Pixel(85, 108, 0, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void ColorLighten_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Lighten);
        Pixel expected = new Pixel(223, 190, 48, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Screen_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Screen);
        Pixel expected = new Pixel(236, 219, 79, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void ColorDodge_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.ColorDodge);
        Pixel expected = new Pixel(255, 255, 56, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Addition_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Addition);
        Pixel expected = new Pixel(255, 255, 86, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Overlay_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Overlay);
        Pixel expected = new Pixel(185, 183, 14, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void SoftLight_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.SoftLight);
        Pixel expected = new Pixel(150, 184, 21, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void HardLight_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.HardLight);
        Pixel expected = new Pixel(218, 168, 14, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Difference_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Difference);
        Pixel expected = new Pixel(117, 77, 10, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Exclusion_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Exclusion);
        Pixel expected = new Pixel(143, 135, 72, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Subtract_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Subtract);
        Pixel expected = new Pixel(0, 77, 10, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Divide_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Divide);
        Pixel expected = new Pixel(121, 255, 255, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Hue_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Hue);
        Pixel expected = new Pixel(214, 130, 72, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Saturation_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Saturation);
        Pixel expected = new Pixel(92, 202, 17, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Color_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Color);
        Pixel expected = new Pixel(234, 124, 49, 255);
        Assert.Equal(expected, backdrop);
    }

    [Fact]
    public void Luminosity_Blend_Color_On_Color_Test()
    {
        Pixel backdrop = _green;
        backdrop.Blend(_orange, 255, BlendMode.Luminosity);
        Pixel expected = new Pixel(94, 178, 36, 255);
        Assert.Equal(expected, backdrop);
    }

}



// [Fact]
// public void Color_LightenBlendTest()
// {
//     BlendMode mode = BlendMode.Lighten;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(223, 190, 48, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(223, 113, 116, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(223, 123, 186, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(223, 113, 50, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_ScreenBlendTest()
// {
//     BlendMode mode = BlendMode.Screen;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(236, 219, 79, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(231, 148, 137, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(250, 181, 196, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(245, 141, 81, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_ColorDodgeBlendTest()
// {
//     BlendMode mode = BlendMode.ColorDodge;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(255, 255, 56, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 113, 136, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 221, 219, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 90, 59, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_AdditionBlendTest()
// {
//     BlendMode mode = BlendMode.Addition;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(255, 255, 86, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 176, 154, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 236, 224, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 163, 88, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_OverlayBlendTest()
// {
//     BlendMode mode = BlendMode.Overlay;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(185, 183, 14, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(110, 56, 35, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(245, 109, 138, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(234, 44, 15, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_SoftLightBlendTest()
// {
//     BlendMode mode = BlendMode.SoftLight;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(150, 184, 21, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(111, 58, 72, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(229, 116, 151, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(200, 45, 22, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_HardLightBlendTest()
// {
//     BlendMode mode = BlendMode.HardLight;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(218, 168, 14, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(207, 56, 35, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(245, 109, 55, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(234, 44, 15, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_DifferenceBlendTest()
// {
//     BlendMode mode = BlendMode.Difference;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(117, 77, 10, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(160, 50, 78, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(8, 10, 148, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(51, 63, 12, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_ExclusionBlendTest()
// {
//     BlendMode mode = BlendMode.Exclusion;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(143, 135, 72, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(176, 120, 120, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(62, 126, 168, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(95, 119, 74, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_SubtractBlendTest()
// {
//     BlendMode mode = BlendMode.Subtract;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(0, 77, 10, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(0, 0, 78, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(0, 10, 148, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(0, 0, 12, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_DivideBlendTest()
// {
//     BlendMode mode = BlendMode.Divide;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(121, 255, 255, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(72, 142, 255, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(246, 255, 255, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(197, 113, 255, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_HueBlendTest()
// {
//     BlendMode mode = BlendMode.Hue;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(214, 130, 72, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(93, 61, 40, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(199, 145, 107, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(142, 70, 20, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_SaturationBlendTest()
// {
//     BlendMode mode = BlendMode.Saturation;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(92, 202, 17, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(92, 29, 214, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(255, 98, 205, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(186, 51, 1, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_ColorBlendTest()
// {
//     BlendMode mode = BlendMode.Color;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(234, 124, 49, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(127, 51, 0, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(242, 132, 57, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(160, 65, 0, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

// [Fact]
// public void Color_LuminosityBlendTest()
// {
//     BlendMode mode = BlendMode.Luminosity;

//     //  Color on Color
//     Assert.Equal(Pixel.FromRGBA(94, 178, 36, 255), Pixel.Blend(mode, _green, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(131, 131, 184, 255), Pixel.Blend(mode, _purple, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(195, 103, 166, 255), Pixel.Blend(mode, _pink, _orange, 255));
//     Assert.Equal(Pixel.FromRGBA(223, 101, 101, 255), Pixel.Blend(mode, _red, _orange, 255));

//     //  Color on Transparent
//     Assert.Equal(_orange, Pixel.Blend(mode, _transparent, _orange, 255));

//     //  Transparent on Color
//     Assert.Equal(_green, Pixel.Blend(mode, _green, _transparent, 255));
//     Assert.Equal(_purple, Pixel.Blend(mode, _purple, _transparent, 255));
//     Assert.Equal(_pink, Pixel.Blend(mode, _pink, _transparent, 255));
//     Assert.Equal(_red, Pixel.Blend(mode, _red, _transparent, 255));

//     //  Transparent on Transparent
//     Assert.Equal(_transparent, Pixel.Blend(mode, _transparent, _transparent, 255));
// }

