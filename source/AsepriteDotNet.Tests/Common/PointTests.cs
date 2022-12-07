// /* -----------------------------------------------------------------------------
// Copyright 2022 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ----------------------------------------------------------------------------- */
// using AsepriteDotNet.Common;

// namespace AsepriteDotNet.Tests;

// public sealed class PointTests
// {
//     [Fact]
//     public void Point_AddTest()
//     {
//         Point left = new Point(1, 2);
//         Point right = new Point(3, 4);

//         Point expected = new Point(4, 6);

//         Assert.Equal(expected, Point.Add(left, right));
//         Assert.Equal(expected, left + right);
//     }

//     [Fact]
//     public void Point_SubtractTest()
//     {
//         Point left = new Point(1, 2);
//         Point right = new Point(3, 4);

//         Point expected = new Point(-2, -2);

//         Assert.Equal(expected, Point.Subtract(left, right));
//         Assert.Equal(expected, left - right);
//     }

//     [Fact]
//     public void Point_EqualTest()
//     {
//         Point a = new Point(1, 2);
//         Point b = new Point(1, 2);

//         Assert.True(a == b);
//         Assert.True(a.Equals(b));
//         Assert.True(a.Equals((object)b));
//         Assert.False(a == Point.Empty);
//         Assert.False(a.Equals(Point.Empty));
//         Assert.False(a.Equals((object)Point.Empty));
//     }

//     [Fact]
//     public void Point_NotEqualTest()
//     {
//         Assert.True(Point.Empty != new Point(1, 2));
//         Assert.False(Point.Empty != new Point(0, 0));
//     }
// }