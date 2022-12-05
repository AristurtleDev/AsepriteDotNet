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
using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet.Common;

public struct Point : IEquatable<Point>
{
    /// <summary>
    ///     Creates a new <see cref="Point"/> value with the 
    ///     <see cref="Point.X"/> and <see cref="Point.Y"/> initialized to
    ///     zero.
    /// </summary>
    public static readonly Point Empty = new Point(0, 0);

    private int _x;
    private int _y;

    /// <summary>
    ///     Gets the x-coordinate element of this <see cref="Point"/> instance.
    /// </summary>
    public int X
    {
        readonly get => _x;
        set => _x = value;
    }

    /// <summary>
    ///     Gets the y-coordinate element of this <see cref="Point"/> instance.
    /// </summary>
    public int Y
    {
        readonly get => _y;
        set => _y = value;
    }

    /// <summary>
    ///     Returns a value that indicates whether this <see cref="Point"/>
    ///     instance is empty.
    /// </summary>
    /// <remarks>
    ///     An empty <see cref="Point"/> is one where both the 
    ///     <see cref="Point.X"/> and <see cref="Point.Y"/> values are equal to
    ///     zero.
    /// </remarks>
    public readonly bool IsEmpty => _x == 0 && _y == 0;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Point"/> struct.
    /// </summary>
    public Point() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Point"/> struct.
    /// </summary>
    /// <param name="x">
    ///     The x-coordinate element of this <see cref="Point"/> instnace.
    /// </param>
    /// <param name="y">
    ///     The y-coordinate element of this <see cref="Point"/> instance.
    /// </param>
    public Point(int x, int y) => (_x, _y) = (x, y);

    /// <summary>
    ///     Returns a new <see cref="Point"/> value whos <see cref="Point.X"/>
    ///     and <see cref="Point.Y"/> components are equal to the sum of the
    ///     components from <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">
    ///     A <see cref="Point"/> value.
    /// </param>
    /// <param name="b">
    ///     Another <see cref="Point"/> value.
    /// </param>
    /// <returns>
    ///     A new <see cref="Point"/> value whos <see cref="Point.X"/> and
    ///     <see cref="Point.Y"/> components are equal to the sum of the 
    ///     components from <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    public static Point Add(Point a, Point b) => new Point(unchecked(a.X + b.X), unchecked(a.Y + b.Y));

    /// <summary>
    ///     Returns a new <see cref="Point"/> value whos <see cref="Point.X"/>
    ///     and <see cref="Point.Y"/> components are equal to the difference of
    ///     the components from <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">
    ///     A <see cref="Point"/> value.
    /// </param>
    /// <param name="b">
    ///     Another <see cref="Point"/> value.
    /// </param>
    /// <returns>
    ///     A new <see cref="Point"/> value whos <see cref="Point.X"/> and
    ///     <see cref="Point.Y"/> components are equal to the difference of the
    ///     components from <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    public static Point Subtract(Point a, Point b) => new Point(unchecked(a.X - b.X), unchecked(a.Y - b.Y));

    /// <summary>
    ///     Returns a value that indicates whether the specified
    ///     <paramref name="obj"/> is equal to this <see cref="Point"/> value.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/> to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the the specified <paramref name="obj"/>
    ///     is equal to this <see cref="Point"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point && Equals((Point)obj);

    /// <summary>
    ///     Returns a hash code for this <see cref="Point"/> instance.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer that is the hash code for this
    ///     <see cref="Point"/> instance.
    /// </returns>
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    ///     Returns a value that indicates whether the given <see cref="Point"/>
    ///     value is equal to this <see cref="Point"/> value.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="Point"/> value to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the other <see cref="Point"/> value is
    ///     equal to this <see cref="Point"/> value; otherwise, 
    ///     <see langword="false"/>
    /// </returns>
    public bool Equals(Point other) => this == other;

    /// <summary>
    ///     Compres two <see cref="Point"/> values for equaility.  The result
    ///     specifies whether the <see cref="Point.X"/> and 
    ///     <see cref="Point.Y"/> elements of the two <see cref="Point"/> values
    ///     are equal.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Point"/> value on the left side of the equality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Point"/> value on the right side of the equaility
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Point"/> values are
    ///     equal to each other; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y;

    /// <summary>
    ///     Compres two <see cref="Point"/> values for inequaility.  The result
    ///     specifies whether the <see cref="Point.X"/> and 
    ///     <see cref="Point.Y"/> elements of the two <see cref="Point"/> values
    ///     are not equal.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Point"/> value on the left side of the inequality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Point"/> value on the right side of the inequaility
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Point"/> values are
    ///     not equal to each other; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Point left, Point right) => !(left == right);

    /// <summary>
    ///     Adds two <see cref="Point"/> values. The result is a new
    ///     <see cref="Point"/> value where the <see cref="Point.X"/> and
    ///     <see cref="Point.Y"/> elements from the two <see cref="Point"/> 
    ///     values are summed.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Point"/> value on the left side of the addition
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Point"/> value on the right side of the addition
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Point"/> values whos <see cref="Point.X"/> and
    ///     <see cref="Point.Y"/> elements are the sum of the two 
    ///     <see cref="Point"/> values.
    /// </returns>
    public static Point operator +(Point left, Point right) => Add(left, right);

    /// <summary>
    ///     Subtracts one <see cref="Point"/> value from another
    ///     <see cref="Point"/> value.  The result is a new <see cref="Point"/>
    ///     value where the <see cref="Point.X"/> and <see cref="Point.Y"/>
    ///     elements are the difference of the two <see cref="Point"/> values
    ///     given.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Point"/> value on the left side of the minus
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Point"/> value on the right side of the minus
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Point"/> value whos <see cref="Point.X"/> and
    ///     <see cref="Point.Y"/> elemnts are the difference of the two
    ///     <see cref="Point"/> values.
    /// </returns>
    public static Point operator -(Point left, Point right) => Subtract(left, right);
}