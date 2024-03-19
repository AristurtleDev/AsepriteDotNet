// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests.Common;

public sealed class RectangleTests
{
    [Theory]
    [InlineData(int.MinValue, int.MinValue, int.MinValue, int.MinValue)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    public void Rectangle_Vector4_Test(int x, int y, int width, int height)
    {
        Rectangle expected = new Rectangle(x, y, width, height);
        Vector4 actual = expected.ToVector4();
        Assert.Equal(expected.X, actual.X);
        Assert.Equal(expected.Y, actual.Y);
        Assert.Equal(expected.Width, actual.Z);
        Assert.Equal(expected.Height, actual.W);
    }

    [Theory]
    [InlineData(int.MinValue, int.MinValue, int.MinValue, int.MinValue)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue)]
    public void Rectangle_Equality_Test(int x, int y, int width, int height)
    {
        Rectangle expected = new Rectangle(x, y, width, height);
        Rectangle actual = new Rectangle(x, y, width, height);
        Assert.True(expected == actual);
        Assert.True(expected.Equals(actual));
        Assert.True(expected.Equals((object)actual));
    }

    [Fact]
    public void Rectangle_Inequality_Test()
    {
        Rectangle expected = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
        Rectangle actual = new Rectangle(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
        Assert.True(expected != actual);
        Assert.False(expected.Equals(actual));
        Assert.False(expected.Equals((object)actual));
    }
}
