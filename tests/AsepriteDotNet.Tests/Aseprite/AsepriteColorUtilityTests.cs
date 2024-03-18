// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.


// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests.Aseprite
{
    public sealed class AsepriteColorUtilityTests
    {
        private static readonly Rgba32 _green = new Rgba32(106, 190, 48, 255);
        private static readonly Rgba32 _orange = new Rgba32(223, 113, 38, 255);
        private static readonly Rgba32 _transparent = new Rgba32(0, 0, 0, 0);
        private static readonly Rgba32 _purple = new Rgba32(63, 63, 116, 255);
        private static readonly Rgba32 _pink = new Rgba32(215, 123, 186, 255);
        private static readonly Rgba32 _red = new Rgba32(172, 50, 50, 255);

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
        public void AseColor_Blend_Color_On_Transparent_Test(AsepriteBlendMode blendMode)
        {
            Assert.Equal(_orange, AsepriteColorUtilities.Blend(_transparent, _orange, 255, blendMode));
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
        public void AseColor_Blend_Transparent_On_Color_Test(AsepriteBlendMode blendMode)
        {
            Assert.Equal(_green, AsepriteColorUtilities.Blend(_green, _transparent, 255, blendMode));
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
        public void AseColor_Blend_Transparent_On_Transparent_Test(AsepriteBlendMode blendMode)
        {
            Assert.Equal(_transparent, AsepriteColorUtilities.Blend(_transparent, _transparent, 255, blendMode));
        }

        [Fact]
        public void AseColor_Normal_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_orange, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Multiply_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            Rgba32 expected = new Rgba32(93, 84, 7, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Screen_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Screen;
            Rgba32 expected = new Rgba32(236, 219, 79, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Overlay_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
            Rgba32 expected = new Rgba32(185, 183, 14, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Darken_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Darken;
            Rgba32 expected = new Rgba32(106, 113, 38, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Lighten_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
            Rgba32 expected = new Rgba32(223, 190, 48, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_ColorDodge_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
            Rgba32 expected = new Rgba32(255, 255, 56, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_ColorBurn_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
            Rgba32 expected = new Rgba32(85, 108, 0, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_HardLight_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
            Rgba32 expected = new Rgba32(218, 168, 14, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_SoftLight_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
            Rgba32 expected = new Rgba32(150, 184, 21, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Difference_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Difference;
            Rgba32 expected = new Rgba32(117, 77, 10, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Exclusion_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
            Rgba32 expected = new Rgba32(143, 135, 72, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Hue_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Hue;
            Rgba32 expected = new Rgba32(214, 130, 72, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Saturation_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
            Rgba32 expected = new Rgba32(92, 202, 17, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Color_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Color;
            Rgba32 expected = new Rgba32(234, 124, 49, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Luminosity_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
            Rgba32 expected = new Rgba32(94, 178, 36, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Addition_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Addition;
            Rgba32 expected = new Rgba32(255, 255, 86, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Subtract_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
            Rgba32 expected = new Rgba32(0, 77, 10, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Divide_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Divide;
            Rgba32 expected = new Rgba32(121, 255, 255, 255);
            Assert.Equal(expected, AsepriteColorUtilities.Blend(_green, _orange, 255, mode));
        }
    }
}
