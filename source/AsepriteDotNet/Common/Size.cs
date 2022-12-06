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

/// <summary>
///     Represents the width and height of something.
/// </summary>
public struct Size : IEquatable<Size>
{
    /// <summary>
    ///     Creates a new <see cref="Size"/> value with the 
    ///     <see cref="Size.Width"/> and <see cref="Size.Height"/> initialized to
    ///     zero.
    /// </summary>
    public static readonly Size Empty = new Size(0, 0);

    private int _width;
    private int _height;

    /// <summary>
    ///     Gets or Sets the width element of this <see cref="Size"/> instance.
    /// </summary>
    public int Width
    {
        readonly get => _width;
        set => _width = value;
    }

    /// <summary>
    ///     Gets or Sets the Height element of this <see cref="Size"/> instance.
    /// </summary>
    public int Height
    {
        readonly get => _height;
        set => _height = value;
    }

    /// <summary>
    ///     Returns a value that indicates whether this <see cref="Size"/>
    ///     instance is empty.
    /// </summary>
    /// <remarks>
    ///     An empty <see cref="Size"/> is one where both the 
    ///     <see cref="Size.Width"/> and <see cref="Size.Height"/> values are 
    ///     equal to zero.
    /// </remarks>
    public readonly bool IsEmpty => _width == 0 && _height == 0;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size"/> struct.
    /// </summary>
    public Size() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Size"/> struct.
    /// </summary>
    /// <param name="width">
    ///     The width element of this <see cref="Size"/> instance.
    /// </param>
    /// <param name="height">
    ///     The height element of this <see cref="Size"/> instance.
    /// </param>
    public Size(int width, int height) => (_width, _height) = (width, height);

    /// <summary>
    ///     Returns a new <see cref="Size"/> value whos <see cref="Size.Width"/>
    ///     and <see cref="Size.Height"/> components are equal to the sum of the
    ///     components from <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">
    ///     A <see cref="Size"/> value.
    /// </param>
    /// <param name="b">
    ///     Another <see cref="Size"/> value.
    /// </param>
    /// <returns>
    ///     A new <see cref="Size"/> value whos <see cref="Size.Width"/> and
    ///     <see cref="Size.Height"/> components are equal to the sum of the 
    ///     components from <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    public static Size Add(Size a, Size b) => new Size(unchecked(a.Width + b.Width), unchecked(a.Height + b.Height));

    /// <summary>
    ///     Returns a new <see cref="Size"/> value whos <see cref="Size.Width"/>
    ///     and <see cref="Size.Height"/> components are equal to the difference
    ///     of the components from <paramref name="a"/> and 
    ///     <paramref name="b"/>.
    /// </summary>
    /// <param name="a">
    ///     A <see cref="Size"/> value.
    /// </param>
    /// <param name="b">
    ///     Another <see cref="Size"/> value.
    /// </param>
    /// <returns>
    ///     A new <see cref="Size"/> value whos <see cref="Size.Width"/> and
    ///     <see cref="Size.Height"/> components are equal to the difference of
    ///     the components from <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    public static Size Subtract(Size a, Size b) => new Size(unchecked(a.Width - b.Width), unchecked(a.Height - b.Height));

    /// <summary>
    ///     Returns a value that indicates whether the specified
    ///     <paramref name="obj"/> is equal to this <see cref="Size"/> value.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/> to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the the specified <paramref name="obj"/>
    ///     is equal to this <see cref="Size"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Size && Equals((Size)obj);

    /// <summary>
    ///     Returns a hash code for this <see cref="Size"/> instance.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer that is the hash code for this
    ///     <see cref="Size"/> instance.
    /// </returns>
    public override int GetHashCode() => HashCode.Combine(Width, Height);

    /// <summary>
    ///     Returns a value that indicates whether the given <see cref="Size"/>
    ///     value is equal to this <see cref="Size"/> value.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="Size"/> value to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the other <see cref="Size"/> value is
    ///     equal to this <see cref="Size"/> value; otherwise, 
    ///     <see langword="false"/>
    /// </returns>
    public bool Equals(Size other) => this == other;

    /// <summary>
    ///     Compares two <see cref="Size"/> values for equaility.  The result
    ///     specifies whether the <see cref="Size.Width"/> and 
    ///     <see cref="Size.Height"/> elements of the two <see cref="Size"/> 
    ///     values are equal.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Size"/> value on the left side of the equality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Size"/> value on the right side of the equaility
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Size"/> values are
    ///     equal to each other; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Size left, Size right) => left.Width == right.Width && left.Height == right.Height;

    /// <summary>
    ///     Compres two <see cref="Size"/> values for inequaility.  The result
    ///     specifies whether the <see cref="Size.Width"/> and 
    ///     <see cref="Size.Height"/> elements of the two <see cref="Size"/> 
    ///     values are not equal.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Size"/> value on the left side of the inequality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Size"/> value on the right side of the inequaility
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Size"/> values are
    ///     not equal to each other; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Size left, Size right) => !(left == right);

    /// <summary>
    ///     Adds two <see cref="Size"/> values. The result is a new
    ///     <see cref="Size"/> value where the <see cref="Size.Width"/> and
    ///     <see cref="Size.Height"/> elements from the two <see cref="Size"/> 
    ///     values are summed.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Size"/> value on the left side of the addition
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Size"/> value on the right side of the addition
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Size"/> values whos <see cref="Size.Width"/> and
    ///     <see cref="Size.Height"/> elements are the sum of the two 
    ///     <see cref="Size"/> values.
    /// </returns>
    public static Size operator +(Size left, Size right) => Add(left, right);

    /// <summary>
    ///     Subtracts one <see cref="Size"/> value from another 
    ///     <see cref="Size"/> value.  The result is a new <see cref="Size"/>
    ///     value where the <see cref="Size.Width"/> and 
    ///     <see cref="Size.Height"/> elements are the difference of the two 
    ///     <see cref="Size"/> values given.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Size"/> value on the left side of the minus
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Size"/> value on the right side of the minus
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Size"/> value whos <see cref="Size.Width"/> and
    ///     <see cref="Size.Height"/> elemnts are the difference of the two
    ///     <see cref="Size"/> values.
    /// </returns>
    public static Size operator -(Size left, Size right) => Subtract(left, right);
}