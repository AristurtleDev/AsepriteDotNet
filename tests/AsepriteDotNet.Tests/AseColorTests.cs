// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Tests
{
    public sealed class AseColorTests
    {
        private static readonly AseColor _green = new AseColor(106, 190, 48, 255);
        private static readonly AseColor _orange = new AseColor(223, 113, 38, 255);
        private static readonly AseColor _transparent = new AseColor(0, 0, 0, 0);
        private static readonly AseColor _purple = new AseColor(63, 63, 116, 255);
        private static readonly AseColor _pink = new AseColor(215, 123, 186, 255);
        private static readonly AseColor _red = new AseColor(172, 50, 50, 255);

        [Theory]
        [InlineData(AsepriteBlendMode.Normal)]
        [InlineData(AsepriteBlendMode.Multiply)]
        [InlineData(AsepriteBlendMode.Screen)]
        [InlineData(AsepriteBlendMode.Overlay)]
        [InlineData(AsepriteBlendMode.Darken)]
        [InlineData(AsepriteBlendMode.Lighten)]
        [InlineData(AsepriteBlendMode.ColorDodge)]
        [InlineData(AsepriteBlendMode.ColorBurn)]
        [InlineData(AsepriteBlendMode.HardLight)]
        [InlineData(AsepriteBlendMode.SoftLight)]
        [InlineData(AsepriteBlendMode.Difference)]
        [InlineData(AsepriteBlendMode.Exclusion)]
        [InlineData(AsepriteBlendMode.Hue)]
        [InlineData(AsepriteBlendMode.Saturation)]
        [InlineData(AsepriteBlendMode.Color)]
        [InlineData(AsepriteBlendMode.Luminosity)]
        [InlineData(AsepriteBlendMode.Addition)]
        [InlineData(AsepriteBlendMode.Subtract)]
        [InlineData(AsepriteBlendMode.Divide)]
        public void AseColor_Blend_Color_On_Transparent(AsepriteBlendMode blendMode)
        {
            Assert.Equal(_orange, _transparent.Blend(_orange, 255, blendMode));
        }

        [Theory]
        [InlineData(AsepriteBlendMode.Normal)]
        [InlineData(AsepriteBlendMode.Multiply)]
        [InlineData(AsepriteBlendMode.Screen)]
        [InlineData(AsepriteBlendMode.Overlay)]
        [InlineData(AsepriteBlendMode.Darken)]
        [InlineData(AsepriteBlendMode.Lighten)]
        [InlineData(AsepriteBlendMode.ColorDodge)]
        [InlineData(AsepriteBlendMode.ColorBurn)]
        [InlineData(AsepriteBlendMode.HardLight)]
        [InlineData(AsepriteBlendMode.SoftLight)]
        [InlineData(AsepriteBlendMode.Difference)]
        [InlineData(AsepriteBlendMode.Exclusion)]
        [InlineData(AsepriteBlendMode.Hue)]
        [InlineData(AsepriteBlendMode.Saturation)]
        [InlineData(AsepriteBlendMode.Color)]
        [InlineData(AsepriteBlendMode.Luminosity)]
        [InlineData(AsepriteBlendMode.Addition)]
        [InlineData(AsepriteBlendMode.Subtract)]
        [InlineData(AsepriteBlendMode.Divide)]
        public void AseColor_Blend_Transparent_On_Color(AsepriteBlendMode blendMode)
        {
            Assert.Equal(_green, _green.Blend(_transparent, 255, blendMode));
        }

        [Theory]
        [InlineData(AsepriteBlendMode.Normal)]
        [InlineData(AsepriteBlendMode.Multiply)]
        [InlineData(AsepriteBlendMode.Screen)]
        [InlineData(AsepriteBlendMode.Overlay)]
        [InlineData(AsepriteBlendMode.Darken)]
        [InlineData(AsepriteBlendMode.Lighten)]
        [InlineData(AsepriteBlendMode.ColorDodge)]
        [InlineData(AsepriteBlendMode.ColorBurn)]
        [InlineData(AsepriteBlendMode.HardLight)]
        [InlineData(AsepriteBlendMode.SoftLight)]
        [InlineData(AsepriteBlendMode.Difference)]
        [InlineData(AsepriteBlendMode.Exclusion)]
        [InlineData(AsepriteBlendMode.Hue)]
        [InlineData(AsepriteBlendMode.Saturation)]
        [InlineData(AsepriteBlendMode.Color)]
        [InlineData(AsepriteBlendMode.Luminosity)]
        [InlineData(AsepriteBlendMode.Addition)]
        [InlineData(AsepriteBlendMode.Subtract)]
        [InlineData(AsepriteBlendMode.Divide)]
        public void AseColor_Blend_Transparent_On_Transparent(AsepriteBlendMode blendMode)
        {
            Assert.Equal(_transparent, _transparent.Blend(_transparent, 255, blendMode));
        }

        [Fact]
        public void AseColor_NormalBlend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_orange, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_MultiplyBlend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            AseColor expected = new AseColor(93, 84, 7, 255);
            Assert.Equal(expected, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_ScreenBlend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Screen;
            AseColor expected = new AseColor(236, 219, 79, 255);
            Assert.Equal(expected, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_OverlayBlend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
            AseColor expected = new AseColor(185, 183, 14, 255);
            Assert.Equal(expected, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_DarkenBlend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Darken;
            AseColor expected = new AseColor(106, 113, 38, 255);
            Assert.Equal(expected, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_LightenBlend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
            AseColor expected = new AseColor(223, 190, 48, 255);
            Assert.Equal(expected, _green.Blend(_orange, 255, mode));
        }
    }
}
