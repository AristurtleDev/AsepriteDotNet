// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests.Common;

public sealed class PointTests
{
    [Theory]
    [InlineData(int.MinValue, int.MinValue)]
    [InlineData(0, 0)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Point_Vector2_Test(int x, int y)
    {
        Point expected = new Point(x, y);
        Vector2 actual = expected.ToVector2();
        Assert.Equal(expected.X, actual.X);
        Assert.Equal(expected.Y, actual.Y);
    }

    [Theory]
    [InlineData(int.MinValue, int.MinValue)]
    [InlineData(0, 0)]
    [InlineData(int.MaxValue, int.MaxValue)]
    public void Point_Equality_Test(int x, int y)
    {
        Point expected = new Point(x, y);
        Point actual = new Point(x, y);
        Assert.True(expected == actual);
        Assert.True(expected.Equals(actual));
        Assert.True(expected.Equals((object)actual));
    }

    [Fact]
    public void Point_Inequality_Test()
    {
        Point expected = new Point(int.MinValue, int.MinValue);
        Point actual = new Point(int.MaxValue, int.MaxValue);
        Assert.True(expected != actual);
        Assert.False(expected.Equals(actual));
        Assert.False(expected.Equals((object)actual));
    }
}
