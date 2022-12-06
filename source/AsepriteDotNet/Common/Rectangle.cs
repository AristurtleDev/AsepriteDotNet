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
///     Represents a rectangular boundry.
/// </summary>
public struct Rectangle
{
    public static readonly Rectangle Empty = new Rectangle(0, 0, 0, 0);

    private int _x;
    private int _y;
    private int _width;
    private int _height;

    /// <summary>
    ///     Gets or Sets the top-left x-coordinate point element of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public int X
    {
        readonly get => _x;
        set => _x = value;
    }

    /// <summary>
    ///     Getsor Sets the top-left y-coordinate point element of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public int Y
    {
        readonly get => _y;
        set => _y = value;
    }

    /// <summary>
    ///     Gets or Sets the top-left xy-coordainte point elements of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public Point Location
    {
        readonly get => new Point(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    /// <summary>
    ///     Gets or Sets the width element of this <see cref="Rectangle"/>
    ///     instance.
    /// </summary>
    public int Width
    {
        readonly get => _width;
        set => _width = value;
    }

    /// <summary>
    ///     Gets or Sets the height element of this <see cref="Rectangle"/>
    ///     instance.
    /// </summary>
    public int Height
    {
        readonly get => _height;
        set => _height = value;
    }

    /// <summary>
    ///     Gets or Sets the width and height elements of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public Size Size
    {
        readonly get => new Size(Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    ///     Gets the y-coordiante of the upper-left corner of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public readonly int Top => Y;

    /// <summary>
    ///     Gets the y-coordinate of the lower-right corner of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public readonly int Bottom => unchecked(Y + Height);

    /// <summary>
    ///     Gets the x-coordinate of the upper-left corner of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public readonly int Left => X;

    /// <summary>
    ///     Gets the y-coordinate of the lower-right corner of this
    ///     <see cref="Rectangle"/> instance.
    /// </summary>
    public readonly int Right => unchecked(X + Width);

    /// <summary>
    ///     Returns a value that indicates whether this <see cref="Rectangle"/>
    ///     instance is empyt.
    /// </summary>
    /// <remarks>
    ///     An empty <see cref="Rectangle"/> is one where the 
    ///     <see cref="Rectangle.X"/>, <see cref="Rectangle.Y"/>,
    ///     <see cref="Rectangle.Width"/>, and <see cref="Rectangle.Height"/>
    ///     elements are all equal to zero.
    /// </remarks>
    public readonly bool IsEmpty => _x == 0 && _y == 0 && _width == 0 && _height == 0;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    public Rectangle() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="x">
    ///     The top-left x-coordinate location of this <see cref="Rectangle"/>
    ///     instance.
    /// </param>
    /// <param name="y">
    ///     THe top-left y-coordinate location of this <see cref="Rectangle"/>
    ///     instance.
    /// </param>
    /// <param name="width">
    ///     The width of this <see cref="Rectangle"/> instance.
    /// </param>
    /// <param name="height">
    ///     The height of this <see cref="Rectangle"/> instance.
    /// </param>
    public Rectangle(int x, int y, int width, int height) => (_x, _y, _width, _height) = (x, y, width, height);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Rectangle"/> struct.
    /// </summary>
    /// <param name="location">
    ///     The top-left xy-coordinate location of this <see cref="Rectangle"/>
    ///     instance.
    /// </param>
    /// <param name="size">
    ///     The width and height of this <see cref="Rectangle"/> instance.
    /// </param>
    public Rectangle(Point location, Size size) => (_x, _y, _width, _height) = (location.X, location.Y, size.Width, size.Height);

    /// <summary>
    ///     Returns a new <see cref="Rectangle"/> value whos 
    ///     <see cref="Rectangle.X"/>, <see cref="Rectangle.Y"/>,
    ///     <see cref="Rectangle.Width"/>, and <see cref="Rectangle.Height"/>
    ///     compoennts are qual to the sum of the components from
    ///     <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">
    ///     A <see cref="Rectangle"/> value.
    /// </param>
    /// <param name="b">
    ///     Another <see cref="Rectangle"/> value.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> components are equal to the sum of
    ///     the components from <paramref name="a"/> and <paramref name="b"/>.
    /// </returns>
    public static Rectangle Add(Rectangle a, Rectangle b) =>
        new Rectangle(unchecked(a.X + b.X), unchecked(a.Y + b.Y), unchecked(a.Width + b.Width), unchecked(a.Height + b.Height));

    /// <summary>
    ///     Returns a new <see cref="Rectangle"/> value whos 
    ///     <see cref="Rectangle.X"/> and <see cref="Rectangle.Y"/> values are
    ///     equal to the sum of the components from <paramref name="rect"/>
    ///     and <paramref name="location"/>, while retaining the original
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/>
    ///     values from <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">
    ///     A <see cref="Rectangle"/> value.
    /// </param>
    /// <param name="location">
    ///     A <see cref="Location"/> value to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos <see cref="Rectangle.X"/>
    ///     and <see cref="Rectangle.Y"/> vlaues are equal to the sum of the
    ///     components from <paramref name="rect"/> and 
    ///     <paramref name="location"/>, while retaining the original
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     from <paramref name="rect"/>.
    /// </returns>
    public static Rectangle Add(Rectangle rect, Point location) =>
        new Rectangle(unchecked(rect.X + location.X), unchecked(rect.Y + location.Y), rect.Width, rect.Height);

    /// <summary>
    ///     Returns a new <see cref="Rectangle"/> value whos 
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     values are equal to the sum of the components from 
    ///     <paramref name="rect"/> and <paramref name="size"/>, while retaining
    ///     the original <see cref="Rectangle.X"/> and <see cref="Rectangle.Y"/>
    ///     values from <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">
    ///     A <see cref="Rectangle"/> value.
    /// </param>
    /// <param name="location">
    ///     A <see cref="Size"/> value to add.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     values are equal to the sum of the components from 
    ///     <paramref name="rect"/> and <paramref name="size"/>, while retaining
    ///     the original <see cref="Rectangle.X"/> and <see cref="Rectangle.Y"/>
    ///     values from <paramref name="rect"/>.
    /// </returns>
    public static Rectangle Add(Rectangle rect, Size size) =>
        new Rectangle(rect.X, rect.Y, unchecked(rect.Width + size.Width), unchecked(rect.Height + size.Height));

    /// <summary>
    ///     Returns a new <see cref="Rectangle"/> value whos 
    ///     <see cref="Rectangle.X"/>, <see cref="Rectangle.Y"/>,
    ///     <see cref="Rectangle.Width"/>, and <see cref="Rectangle.Height"/>
    ///     compoennts are equal to the difference of the components from
    ///     <paramref name="a"/> and <paramref name="b"/>.
    /// </summary>
    /// <param name="a">
    ///     A <see cref="Rectangle"/> value.
    /// </param>
    /// <param name="b">
    ///     Another <see cref="Rectangle"/> value.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> components are equal to the 
    ///     difference of the components from <paramref name="a"/> and 
    ///     <paramref name="b"/>.
    /// </returns>
    public static Rectangle Subtract(Rectangle a, Rectangle b) =>
        new Rectangle(unchecked(a.X - b.X), unchecked(a.Y - b.Y), unchecked(a.Width - b.Width), unchecked(a.Height - b.Height));

    /// <summary>
    ///     Returns a new <see cref="Rectangle"/> value whos 
    ///     <see cref="Rectangle.X"/> and <see cref="Rectangle.Y"/> values are
    ///     equal to the difference of the components from 
    ///     <paramref name="rect"/> and <paramref name="location"/>, while 
    ///     retaining the original <see cref="Rectangle.Width"/> and 
    ///     <see cref="Rectangle.Height"/> values from <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">
    ///     A <see cref="Rectangle"/> value.
    /// </param>
    /// <param name="location">
    ///     A <see cref="Location"/> value to subtract.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos <see cref="Rectangle.X"/>
    ///     and <see cref="Rectangle.Y"/> vlaues are equal to the sum of the
    ///     components from <paramref name="rect"/> and 
    ///     <paramref name="location"/>, while retaining the original
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     from <paramref name="rect"/>.
    /// </returns>
    public static Rectangle Subtract(Rectangle rect, Point location) =>
        new Rectangle(unchecked(rect.X - location.X), unchecked(rect.Y - location.Y), rect.Width, rect.Height);

    /// <summary>
    ///     Returns a new <see cref="Rectangle"/> value whos 
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     values are equal to the difference of the components from 
    ///     <paramref name="rect"/> and <paramref name="size"/>, while retaining
    ///     the original <see cref="Rectangle.X"/> and <see cref="Rectangle.Y"/>
    ///     values from <paramref name="rect"/>.
    /// </summary>
    /// <param name="rect">
    ///     A <see cref="Rectangle"/> value.
    /// </param>
    /// <param name="location">
    ///     A <see cref="Size"/> value to subtract.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     values are equal to the difference of the components from 
    ///     <paramref name="rect"/> and <paramref name="size"/>, while retaining
    ///     the original <see cref="Rectangle.X"/> and <see cref="Rectangle.Y"/>
    ///     values from <paramref name="rect"/>.
    /// </returns>
    public static Rectangle Subtract(Rectangle rect, Size size) =>
        new Rectangle(rect.X, rect.Y, unchecked(rect.Width - size.Width), unchecked(rect.Height - size.Height));

    /// <summary>
    ///     Returns a value taht indicates wehther the specified 
    ///     <paramref name="obj"/> is equal to this <see cref="Rectangle"/>
    ///     value.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/> to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <paramref name="obj"/> is
    ///     equal to this <see cref="Rectangle"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Rectangle other && Equals(other);

    /// <summary>
    ///     Returns a hash code for this <see cref="Rectangle"/> instance.
    /// </summary>
    /// <returns>
    ///     A 32-bit signed integer that is the has code for this
    ///     <see cref="Rectangle"/> instance.
    /// </returns>
    public override int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    /// <summary>
    ///     Returns a value that indicates whether the given 
    ///     <see cref="Rectangle"/> value is equal to this
    ///     <see cref="Rectangle"/> value.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="Rectangle"/> value to check for equaility.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the other <see cref="Rectangle"/> value is
    ///     equal to this <see cref="Rectangle"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public bool Equals(Rectangle other) => this == other;

    /// <summary>
    ///     Compare two <see cref="Rectangle"/> values for equaility.  The
    ///     result specifies whether the <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> elements of the two
    ///     <see cref="Rectangle"/> values are equal.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the equaility
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Rectangle"/> value on the right side of the equaility
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Rectangle"/> values are
    ///     equal to each other; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Rectangle left, Rectangle right) =>
        left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

    /// <summary>
    ///     Compare two <see cref="Rectangle"/> values for inequaility.  The
    ///     result specifies whether the <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> elements of the two
    ///     <see cref="Rectangle"/> values are not equal.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the 
    ///     inequaility operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Rectangle"/> value on the right side of the
    ///     inequaility operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Rectangle"/> values are
    ///     not equal to each other; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Rectangle left, Rectangle right) => !(left == right);

    /// <summary>
    ///     Adds two <see cref="Rectangle"/> values.  The result is a new
    ///     <see cref="Rectangle"/> value where the <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> elements from the two
    ///     <see cref="Rectangle"/> values are summed.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the addition
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Rectangle"/> value on the right side of the addition
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> elements are the sum of the two
    ///     <see cref="Rectangle"/> values.
    /// </returns>
    public static Rectangle operator +(Rectangle left, Rectangle right) => Add(left, right);

    /// <summary>
    ///     Adds a <see cref="Point"/> location to a <see cref="Rectangle"/>
    ///     value.  The result is a new <see cref="Rectangle"/> value where the 
    ///     <see cref="Rectangle.X"/>, <see cref="Rectangle.Y"/> values are the
    ///     sum of the elements from the given <see cref="Rectangle"/> and
    ///     <see cref="Point"/> values.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the addition
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Point"/> value on the right side of the addition
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> values whos <see cref="Rectangle.X"/>
    ///     and <see cref="Rectangle.Y"/> values are the sum of the elements
    ///     from the given <see cref="Rectangle"/> and <see cref="Point"/>
    ///     values.
    /// </returns>
    public static Rectangle operator +(Rectangle left, Point right) => Add(left, right);

    /// <summary>
    ///     Adds a <see cref="Size"/> size to a <see cref="Rectangle"/> value
    ///     The result is a new <see cref="Rectangle"/> value where the 
    ///     <see cref="Rectangle.Width"/>, <see cref="Rectangle.Height"/> values 
    ///     are the sum of the elements from the given <see cref="Rectangle"/>
    ///     and <see cref="Size"/> values.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the addition
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Size"/> value on the right side of the addition
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> values whos 
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     values are the sum of the elements from the given <see cref="Rectangle"/>
    ///     and <see cref="Size"/> values.
    /// </returns>
    public static Rectangle operator +(Rectangle left, Size right) => Add(left, right);

    /// <summary>
    ///     Subtracts two <see cref="Rectangle"/> values.  The result is a new
    ///     <see cref="Rectangle"/> value where the <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> elements are the difference of the
    ///     two <see cref="Rectangle"/> values given..
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the addition
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Rectangle"/> value on the right side of the addition
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> value whos <see cref="Rectangle.X"/>,
    ///     <see cref="Rectangle.Y"/>, <see cref="Rectangle.Width"/>, and
    ///     <see cref="Rectangle.Height"/> elements are the difference of the
    ///     two <see cref="Rectangle"/> values given.
    /// </returns>
    public static Rectangle operator -(Rectangle left, Rectangle right) => Subtract(left, right);

    /// <summary>
    ///     Subtracts a <see cref="Point"/> value from a <see cref="Rectangle"/>
    ///     value.  The result is a new <see cref="Rectangle"/> value where the 
    ///     <see cref="Rectangle.X"/>, <see cref="Rectangle.Y"/> values are the
    ///     difference of the elements from the given <see cref="Rectangle"/>
    ///     and <see cref="Point"/> values.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the minus
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Point"/> value on the right side of the minus
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> values whos <see cref="Rectangle.X"/>
    ///     and <see cref="Rectangle.Y"/> values are the difference of the
    ///     elements from the given <see cref="Rectangle"/> and 
    ///     <see cref="Point"/> values.
    /// </returns>
    public static Rectangle operator -(Rectangle left, Point right) => Subtract(left, right);

    /// <summary>
    ///     Subtracts a <see cref="Size"/> value from a <see cref="Rectangle"/>
    ///     value.  The result is a new <see cref="Rectangle"/> value where the 
    ///     <see cref="Rectangle.Width"/>, <see cref="Rectangle.Height"/> values 
    ///     are the difference of the elements from the given 
    ///     <see cref="Rectangle"/> and <see cref="Size"/> values.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Rectangle"/> value on the left side of the minus
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Size"/> value on the right side of the minus
    ///     operator.
    /// </param>
    /// <returns>
    ///     A new <see cref="Rectangle"/> values whos 
    ///     <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> 
    ///     values are the difference of the elements from the given
    ///     <see cref="Rectangle"/> and <see cref="Size"/> values.
    /// </returns>
    public static Rectangle operator -(Rectangle left, Size right) => Subtract(left, right);



}