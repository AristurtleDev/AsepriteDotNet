// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests.Common
{
    public sealed class Rgba32Tests
    {
        [Theory]
        [InlineData(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue)]
        [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
        public void Rgba32_Vector4_Test(byte r, byte g, byte b, byte a)
        {
            Rgba32 expected = new Rgba32(r, g, b, a);
            Vector4 vector = expected.ToVector4();
            Rgba32 actual = new Rgba32(vector);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue)]
        [InlineData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)]
        public void Rgba32_Equality_Test(byte r, byte g, byte b, byte a)
        {
            Rgba32 expected = new Rgba32(r, g, b, a);
            Rgba32 actual = new Rgba32(r, g, b, a);
            Assert.True(expected == actual);
            Assert.True(expected.Equals(actual));
            Assert.True(expected.Equals((object)actual));
        }

        [Fact]
        public void Rgba32_Inequality_Test()
        {
            Rgba32 expected = new Rgba32(byte.MinValue, byte.MinValue, byte.MinValue, byte.MinValue);
            Rgba32 actual = new Rgba32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            Assert.True(expected != actual);
            Assert.False(expected.Equals(actual));
            Assert.False(expected.Equals((object)actual));
        }
    }
}
