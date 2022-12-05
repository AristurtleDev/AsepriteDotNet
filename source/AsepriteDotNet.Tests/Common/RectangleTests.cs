/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Tests;

public sealed class RectangleTests
{
    [Fact]
    public void Rectangle_AddRectanagleTest()
    {
        Rectangle left = new Rectangle(1, 2, 3, 4);
        Rectangle right = new Rectangle(5, 6, 7, 8);

        Rectangle expected = new Rectangle(6, 8, 10, 12);

        Assert.Equal(expected, Rectangle.Add(left, right));
        Assert.Equal(expected, left + right);
    }

    [Fact]
    public void Rectangle_AddPointTest()
    {
        Rectangle left = new Rectangle(1, 2, 3, 4);
        Point right = new Point(5, 6);

        Rectangle expected = new Rectangle(6, 8, 3, 4);

        Assert.Equal(expected, Rectangle.Add(left, right));
        Assert.Equal(expected, left + right);
    }

    [Fact]
    public void Rectangle_AddSizeTest()
    {
        Rectangle left = new Rectangle(1, 2, 3, 4);
        Size right = new Size(5, 6);

        Rectangle expected = new Rectangle(1, 2, 8, 10);

        Assert.Equal(expected, Rectangle.Add(left, right));
        Assert.Equal(expected, left + right);
    }

    [Fact]
    public void Rectangle_SubtractRectanagleTest()
    {
        Rectangle left = new Rectangle(1, 2, 3, 4);
        Rectangle right = new Rectangle(5, 6, 7, 8);

        Rectangle expected = new Rectangle(-4, -4, -4, -4);

        Assert.Equal(expected, Rectangle.Subtract(left, right));
        Assert.Equal(expected, left - right);
    }

    [Fact]
    public void Rectangle_SubtractPointTest()
    {
        Rectangle left = new Rectangle(1, 2, 3, 4);
        Point right = new Point(5, 6);

        Rectangle expected = new Rectangle(-4, -4, 3, 4);

        Assert.Equal(expected, Rectangle.Subtract(left, right));
        Assert.Equal(expected, left - right);
    }

    [Fact]
    public void Rectangle_SubtractSizeTest()
    {
        Rectangle left = new Rectangle(1, 2, 3, 4);
        Size right = new Size(5, 6);

        Rectangle expected = new Rectangle(1, 2, -2, -2);

        Assert.Equal(expected, Rectangle.Subtract(left, right));
        Assert.Equal(expected, left - right);
    }


    [Fact]
    public void Rectangle_EqualTest()
    {
        Rectangle a = new Rectangle(1, 2, 3, 4);
        Rectangle b = new Rectangle(1, 2, 3, 4);

        Assert.True(a == b);
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.False(a == Rectangle.Empty);
        Assert.False(a.Equals(Rectangle.Empty));
        Assert.False(a.Equals((object)Rectangle.Empty));
    }

    [Fact]
    public void Point_NotEqualTest()
    {
        Assert.True(Rectangle.Empty != new Rectangle(1, 2, 3, 4));
        Assert.False(Rectangle.Empty != new Rectangle(0, 0, 0, 0));
    }
}