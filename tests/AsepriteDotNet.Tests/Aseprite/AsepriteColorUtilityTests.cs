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
        private static readonly SystemColor _green = new SystemColor(106, 190, 48, 255);
        private static readonly SystemColor _orange = new SystemColor(223, 113, 38, 255);
        private static readonly SystemColor _transparent = new SystemColor(0, 0, 0, 0);

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
            Assert.Equal(_orange, AsepriteBlendFunctions.Blend(_transparent, _orange, 255, blendMode));
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
            Assert.Equal(_green, AsepriteBlendFunctions.Blend(_green, _transparent, 255, blendMode));
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
            Assert.Equal(_transparent, AsepriteBlendFunctions.Blend(_transparent, _transparent, 255, blendMode));
        }

        [Fact]
        public void AseColor_Normal_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_orange, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Multiply_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            SystemColor expected = new SystemColor(93, 84, 7, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Screen_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Screen;
            SystemColor expected = new SystemColor(236, 219, 79, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Overlay_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
            SystemColor expected = new SystemColor(185, 183, 14, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Darken_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Darken;
            SystemColor expected = new SystemColor(106, 113, 38, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Lighten_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
            SystemColor expected = new SystemColor(223, 190, 48, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_ColorDodge_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
            SystemColor expected = new SystemColor(255, 255, 56, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_ColorBurn_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
            SystemColor expected = new SystemColor(85, 108, 0, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_HardLight_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
            SystemColor expected = new SystemColor(218, 168, 14, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_SoftLight_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
            SystemColor expected = new SystemColor(150, 184, 21, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Difference_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Difference;
            SystemColor expected = new SystemColor(117, 77, 10, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Exclusion_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
            SystemColor expected = new SystemColor(143, 135, 72, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Hue_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Hue;
            SystemColor expected = new SystemColor(214, 130, 72, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Saturation_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
            SystemColor expected = new SystemColor(92, 202, 17, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Color_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Color;
            SystemColor expected = new SystemColor(234, 124, 49, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Luminosity_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
            SystemColor expected = new SystemColor(94, 178, 36, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Addition_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Addition;
            SystemColor expected = new SystemColor(255, 255, 86, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Subtract_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
            SystemColor expected = new SystemColor(0, 77, 10, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }

        [Fact]
        public void AseColor_Divide_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Divide;
            SystemColor expected = new SystemColor(121, 255, 255, 255);
            Assert.Equal(expected, AsepriteBlendFunctions.Blend(_green, _orange, 255, mode));
        }
    }
}
