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

        #region Normal Blend

        [Fact]
        public void AseColor_NormalBlend_Color_On_Color_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_orange, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_NormalBlend_Color_On_Transparent_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_orange, _transparent.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_NormalBLend_Transparent_On_Color_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_green, _green.Blend(_transparent, 255, mode));
        }

        [Fact]
        public void AseColor_NormalBlend_Transparent_On_Transparent_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Normal;
            Assert.Equal(_transparent, _transparent.Blend(_transparent, 255, mode));
        }

        #endregion Normal Blend

        #region Multiply

        [Fact]
        public void AseColor_MultiplyBlend_Color_On_Color_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            AseColor expected = new AseColor(93, 84, 7, 255);
            Assert.Equal(expected, _green.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_MultiplyBlend_Color_On_Transparent_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            Assert.Equal(_orange, _transparent.Blend(_orange, 255, mode));
        }

        [Fact]
        public void AseColor_MultiplyBLend_Transparent_On_Color_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            Assert.Equal(_green, _green.Blend(_transparent, 255, mode));
        }

        [Fact]
        public void AseColor_MultiplyBlend_Transparent_On_Transparent_Test()
        {
            AsepriteBlendMode mode = AsepriteBlendMode.Multiply;
            Assert.Equal(_transparent, _transparent.Blend(_transparent, 255, mode));
        }

        #endregion Multiply



    }
}
