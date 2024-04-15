// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests.Aseprite
{
    public sealed class AsepriteColorUtilityVectorTests
    {
        private static readonly Rgba32 _green = new Rgba32(106, 190, 48, 255);
        private static readonly Rgba32 _orange = new Rgba32(223, 113, 38, 255);
        private static readonly Rgba32 _transparent = new Rgba32(0, 0, 0, 0);

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
            Vector4[] backdrop = new Vector4[]
            {
                _orange.ToVector4(),
                _orange.ToVector4(),
                _orange.ToVector4(),
                _orange.ToVector4(),
            };

            Vector4[] source = new Vector4[]
            {
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
            };

            Vector4[] expected = new Vector4[]
            {
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
            };

            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, blendMode);
            Assert.Equal(expected, actual);
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
            Vector4[] backdrop = new Vector4[]
            {
                _green.ToVector4(),
                _green.ToVector4(),
                _green.ToVector4(),
                _green.ToVector4(),
            };

            Vector4[] source = new Vector4[]
            {
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
            };

            Vector4[] expected = new Vector4[]
            {
                _green.ToVector4(),
                _green.ToVector4(),
                _green.ToVector4(),
                _green.ToVector4(),
            };

            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, blendMode);
            Assert.Equal(expected, actual);
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
            Vector4[] transparent = new Vector4[]
            {
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
                _transparent.ToVector4(),
            };

            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(transparent, transparent, 255, blendMode);
            Assert.Equal(transparent, actual);
        }

        [Fact]
        public void AseColor_Normal_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { _orange.ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Multiply_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(93, 84, 7, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Screen_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Screen;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(236, 219, 79, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Overlay_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Overlay;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(185, 183, 14, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Darken_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Darken;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(106, 113, 38, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Lighten_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Lighten;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(223, 190, 48, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_ColorDodge_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.ColorDodge;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(255, 255, 56, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_ColorBurn_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.ColorBurn;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(85, 108, 0, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_HardLight_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.HardLight;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(218, 168, 14, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_SoftLight_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.SoftLight;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(150, 184, 21, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Difference_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Difference;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(117, 77, 10, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Exclusion_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Exclusion;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(143, 135, 72, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Hue_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Hue;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(214, 130, 72, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Saturation_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Saturation;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(92, 202, 17, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Color_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Color;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(234, 124, 49, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Luminosity_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Luminosity;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(94, 178, 36, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Addition_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Addition;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(255, 255, 86, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Subtract_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Subtract;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(0, 77, 10, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AseColor_Divide_Blend_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Divide;
            Vector4[] backdrop = new Vector4[] { _green.ToVector4() };
            Vector4[] source = new Vector4[] { _orange.ToVector4() };

            Vector4[] expected = new Vector4[] { new Rgba32(121, 255, 255, 255).ToVector4() };
            Vector4[] actual = AsepriteColorUtilitiesVectors.Blend(backdrop, source, 255, mode);
            Assert.Equal(expected, actual);
        }
    }
}
