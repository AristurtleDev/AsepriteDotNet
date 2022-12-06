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

    [Fact]
    public void Color_NormalBlendTest()
    {
        Assert.Equal(_orange, Color.Blend(BlendMode.Normal, _green, _orange, 255));
        Assert.Equal(_orange, Color.Blend(BlendMode.Normal, _purple, _orange, 255));
        Assert.Equal(_orange, Color.Blend(BlendMode.Normal, _pink, _orange, 255));
        Assert.Equal(_orange, Color.Blend(BlendMode.Normal, _red, _orange, 255));
    }

    [Fact]
    public void Color_DarkenBlendTest()
    {
        Color greenToOrange = Color.FromRGBA(106, 113, 38, 255);
        Color purpleToOrange = Color.FromRGBA(63, 63, 38, 255);
        Color pinkToOrange = Color.FromRGBA(215, 113, 38, 255);
        Color redToOrange = Color.FromRGBA(172, 50, 38, 255);

        Assert.Equal(greenToOrange, Color.Blend(BlendMode.Darken, _green, _orange, 255));
        Assert.Equal(purpleToOrange, Color.Blend(BlendMode.Darken, _purple, _orange, 255));
        Assert.Equal(pinkToOrange, Color.Blend(BlendMode.Darken, _pink, _orange, 255));
        Assert.Equal(redToOrange, Color.Blend(BlendMode.Darken, _red, _orange, 255));
    }

    [Fact]
    public void Color_MultiplylendTest()
    {
        BlendMode mode = BlendMode.Multiply;

        Color greenToOrange = Color.FromRGBA(93, 84, 7, 255);
        Color purpleToOrange = Color.FromRGBA(55, 28, 17, 255);
        Color pinkToOrange = Color.FromRGBA(188, 55, 28, 255);
        Color redToOrange = Color.FromRGBA(150, 22, 7, 255);

        Assert.Equal(greenToOrange, Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(purpleToOrange, Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(pinkToOrange, Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(redToOrange, Color.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void Color_ColorBurnBlendTest()
    {
        BlendMode mode = BlendMode.ColorBurn;

        Color greenToOrange = Color.FromRGBA(85, 108, 0, 255);
        Color purpleToOrange = Color.FromRGBA(35, 0, 0, 255);
        Color pinkToOrange = Color.FromRGBA(209, 0, 0, 255);
        Color redToOrange = Color.FromRGBA(160, 0, 0, 255);

        Assert.Equal(greenToOrange, Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(purpleToOrange, Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(pinkToOrange, Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(redToOrange, Color.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void Color_LightenBlendTest()
    {
        BlendMode mode = BlendMode.Lighten;

        Color greenToOrange = Color.FromRGBA(223, 190, 48, 255);
        Color purpleToOrange = Color.FromRGBA(223, 113, 116, 255);
        Color pinkToOrange = Color.FromRGBA(223, 123, 186, 255);
        Color redToOrange = Color.FromRGBA(223, 113, 50, 255);

        Assert.Equal(greenToOrange, Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(purpleToOrange, Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(pinkToOrange, Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(redToOrange, Color.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void Color_ScreenBlendTest()
    {
        BlendMode mode = BlendMode.Screen;

        Color greenToOrange = Color.FromRGBA(236, 219, 79, 255);
        Color purpleToOrange = Color.FromRGBA(231, 148, 137, 255);
        Color pinkToOrange = Color.FromRGBA(250, 181, 196, 255);
        Color redToOrange = Color.FromRGBA(245, 141, 81, 255);

        Assert.Equal(greenToOrange, Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(purpleToOrange, Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(pinkToOrange, Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(redToOrange, Color.Blend(mode, _red, _orange, 255));
    }

    [Fact]
    public void Color_ColorDodgeTest()
    {
        BlendMode mode = BlendMode.ColorDodge;

        Color greenToOrange = Color.FromRGBA(255, 255, 56, 255);
        Color purpleToOrange = Color.FromRGBA(255, 113, 136, 255);
        Color pinkToOrange = Color.FromRGBA(255, 221, 219, 255);
        Color redToOrange = Color.FromRGBA(255, 90, 59, 255);

        Assert.Equal(greenToOrange, Color.Blend(mode, _green, _orange, 255));
        Assert.Equal(purpleToOrange, Color.Blend(mode, _purple, _orange, 255));
        Assert.Equal(pinkToOrange, Color.Blend(mode, _pink, _orange, 255));
        Assert.Equal(redToOrange, Color.Blend(mode, _red, _orange, 255));
    }
}