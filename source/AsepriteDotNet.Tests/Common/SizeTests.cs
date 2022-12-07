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

// public sealed class SizeTests
// {
//     [Fact]
//     public void Size_AddTest()
//     {
//         Size left = new Size(1, 2);
//         Size right = new Size(3, 4);

//         Size expected = new Size(4, 6);

//         Assert.Equal(expected, Size.Add(left, right));
//         Assert.Equal(expected, left + right);
//     }

//     [Fact]
//     public void Size_SubtractTest()
//     {
//         Size left = new Size(1, 2);
//         Size right = new Size(3, 4);

//         Size expected = new Size(-2, -2);

//         Assert.Equal(expected, Size.Subtract(left, right));
//         Assert.Equal(expected, left - right);
//     }

//     [Fact]
//     public void Size_EqualTest()
//     {
//         Size a = new Size(1, 2);
//         Size b = new Size(1, 2);

//         Assert.True(a == b);
//         Assert.True(a.Equals(b));
//         Assert.True(a.Equals((object)b));
//         Assert.False(a == Size.Empty);
//         Assert.False(a.Equals(Size.Empty));
//         Assert.False(a.Equals((object)Size.Empty));
//     }

//     [Fact]
//     public void Size_NotEqualTest()
//     {
//         Assert.True(Size.Empty != new Size(1, 2));
//         Assert.False(Size.Empty != new Size(0, 0));
//     }
// }