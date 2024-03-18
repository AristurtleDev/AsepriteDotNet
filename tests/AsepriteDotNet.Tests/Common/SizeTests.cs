// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests.Common;

public sealed class SizeTests
{
    [Theory]
    [InlineData(int.MinValue, int.MinValue)]
    [InlineData(0, 0)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Size_Vector2_Test(int width, int height)
    {
        Size expected = new Size(width, height);
        Vector2 actual = expected.ToVector2();
        Assert.Equal(expected.Width, actual.X);
        Assert.Equal(expected.Height, actual.Y);
    }

    [Theory]
    [InlineData(int.MinValue, int.MinValue)]
    [InlineData(0, 0)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Size_Equality_Test(int width, int height)
    {
        Size expected = new Size(width, height);
        Size actual = new Size(width, height);
        Assert.True(expected == actual);
        Assert.True(expected.Equals(actual));
        Assert.True(expected.Equals((object)actual));
    }

    [Fact]
    public void Point_Inequality_Test()
    {
        Size expected = new Size(int.MinValue, int.MinValue);
        Size actual = new Size(int.MaxValue, int.MaxValue);
        Assert.True(expected != actual);
        Assert.False(expected.Equals(actual));
        Assert.False(expected.Equals((object)actual));
    }
}
