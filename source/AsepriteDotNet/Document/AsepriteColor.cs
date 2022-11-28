/* -----------------------------------------------------------------------------
Copyright 2022 Aristurtle

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
using System.Drawing;

namespace AsepriteDotNet.Document;

public struct AsepriteColor : IEquatable<AsepriteColor>, IEquatable<Color>
{
    private const int RGBA_R_SHIFT = 0;
    private const int RGBA_G_SHIFT = 8;
    private const int RGBA_B_SHIFT = 16;
    private const int RGBA_A_SHIFT = 24;

    private uint _value;

    public byte R => unchecked((byte)((_value >> RGBA_R_SHIFT) & 0xFF));
    public byte G => unchecked((byte)((_value >> RGBA_G_SHIFT) & 0xFF));
    public byte B => unchecked((byte)((_value >> RGBA_B_SHIFT) & 0xFF));
    public byte A => unchecked((byte)((_value >> RGBA_A_SHIFT) & 0xFF));


    /// <summary>
    ///     Creates a <see cref="AsepriteColor"/> value from the four RGBA
    ///     (red, green, blue, alpha) values.
    /// </summary>
    /// <param name="red">
    ///     The red componnet value.  Valid values are 0 through 255 inclusivly.
    /// </param>
    /// <param name="green">
    ///     The green component value. Valid values are 0 through 255 
    ///     inclusivly.
    /// </param>
    /// <param name="blue">
    ///     The blue component value.  Valid values are 0 through 255 
    ///     inclusivly.
    /// </param>
    /// <param name="alpha">
    ///     The alpha component value.  Valid values are 0 through 255
    ///     inclusivly.
    /// </param>
    /// <returns>
    ///     The <see cref="AsepriteColor"/> value this method creates.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if <paramref name="red"/>, <paramref name="green"/>,
    ///     <paramref name="blue"/>, or <paramref name="alpha"/> is less than 0
    ///     or greater than 255.
    /// </exception>
    public static AsepriteColor FromRgba(byte red, byte green, byte blue, byte alpha)
    {
        CheckByte(red, nameof(red));
        CheckByte(green, nameof(green));
        CheckByte(blue, nameof(blue));
        CheckByte(alpha, nameof(alpha));

        AsepriteColor color;
        color._value = (uint)((red << RGBA_R_SHIFT) |
                              (green << RGBA_G_SHIFT) |
                              (blue << RGBA_B_SHIFT) |
                              (alpha << RGBA_A_SHIFT));

        return color;
    }

    /// <summary>
    ///     Gets the 4-byte unsigned integer value of this
    ///     <see cref="AsepriteColor"/>.
    /// </summary>
    public uint Value => unchecked((uint)_value);

    private static void CheckByte(byte value, string name)
    {
        if (value < 0 || value > 255)
        {
            throw new ArgumentException(name,
            $"Value of '{name}' is not valid.  '{name}' should be greater than or equal to 0 and less than or equal to 255");
        }
    }

    /// <summary>
    ///     Returns whether the specified <paramref name="obj"/> is equal to
    ///     this <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="obj">
    ///     The <see cref="object"/> to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <paramref name="obj"/> is
    ///     equal to this <see cref="AsepriteColor"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is AsepriteColor ase)
        {
            return Equals(ase);
        }
        else if (obj is Color color)
        {
            return Equals(color);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///     Returns whether the <paramref name="left"/> 
    ///     <see cref="AsepriteColor"/> value is equal to the 
    ///     <paramref name="right"/> <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="AsepriteColor"/> value on the left side of the
    ///     equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="AsepriteColor"/> value on the right side of the
    ///     equality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="left"/>
    ///     <see cref="AsepriteColor"/> value is equal to the 
    ///     <paramref name="right"/> <see cref="AsepriteColor"/> value;
    ///     otherwise; <see langword="false"/>.
    /// </returns>
    public static bool operator ==(AsepriteColor left, AsepriteColor right) =>
        left._value == right._value;

    /// <summary>
    ///     Returns whether the <paramref name="left"/> 
    ///     <see cref="AsepriteColor"/> value is equal to the 
    ///     <paramref name="right"/> <see cref="Color"/> value.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="AsepriteColor"/> value on the left side of the
    ///     equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Color"/> value on the right side of the
    ///     equality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="left"/>
    ///     <see cref="AsepriteColor"/> value is equal to the 
    ///     <paramref name="right"/> <see cref="Color"/> value; otherwise, 
    ///     <see langword="false"/>.
    /// </returns>
    public static bool operator ==(AsepriteColor left, Color right) =>
        left.R == right.R &&
        left.G == right.G &&
        left.B == right.B &&
        left.A == right.A;

    /// <summary>
    ///     Returns whether the <paramref name="left"/> <see cref="Color"/> 
    ///     value is equal to the <paramref name="right"/> 
    ///     <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Color"/> value on the left side of the
    ///     equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="AsepriteColor"/> value on the right side of the
    ///     equality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="left"/>
    ///     <see cref="Color"/> value is equal to the <paramref name="right"/> 
    ///     <see cref="AsepriteColor"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Color left, AsepriteColor right) => right == left;

    /// <summary>
    ///     Returns whether the <paramref name="left"/>
    ///     <see cref="AsepriteColor"/> value is not equal to the 
    ///     <paramref name="right"/> <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="AsepriteColor"/> value on the left side of the
    ///     non-equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="AsepriteColor"/> value on the right side of the
    ///     non-equality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="left"/>
    ///     <see cref="AsepriteColor"/> value is not equal to the 
    ///     <paramref name="right"/> <see cref="AsepriteColor"/> value; 
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(AsepriteColor left, AsepriteColor right) => !(left == right);

    /// <summary>
    ///     Returns whether the <paramref name="left"/>
    ///     <see cref="AsepriteColor"/> value is not equal to the 
    ///     <paramref name="right"/> <see cref="Color"/> value.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="AsepriteColor"/> value on the left side of the
    ///     non-equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Color"/> value on the right side of the non-equality
    ///     operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="left"/>
    ///     <see cref="AsepriteColor"/> value is not equal to the 
    ///     <paramref name="right"/> <see cref="Color"/> value; otherwise, 
    ///     <see langword="false"/>.
    /// </returns>
    public static bool operator !=(AsepriteColor left, Color right) => !(left == right);

    /// <summary>
    ///     Returns whether the <paramref name="left"/> <see cref="Color"/> 
    ///     value is not equal to the <paramref name="right"/> 
    ///     <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Color"/> value on the left side of the non-equality
    ///     operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="AsepriteColor"/> value on the right side of the
    ///     non-equality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="left"/>
    ///     <see cref="Color"/> value is not equal to the 
    ///     <paramref name="right"/> <see cref="AsepriteColor"/> value; 
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Color left, AsepriteColor right) => right != left;

    /// <summary>
    ///     Returns the hash code for this <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <returns>
    ///     A 4-byte signed integer hash code.
    /// </returns>
    public override int GetHashCode() => _value.GetHashCode();

    /// <summary>
    ///     Returns whether the specified <see cref="AsepriteColor"/> value is
    ///     euqal to this <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="AsepriteColor"/> value to check for quality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <see cref="AsepriteColor"/>
    ///     value is equal to this <see cref="AsepriteColor"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public bool Equals(AsepriteColor other) => this._value == other._value;

    /// <summary>
    ///     Returns whether the specified <see cref="Color"/> value is equal
    ///     to this <see cref="AsepriteColor"/> value.
    /// </summary>
    /// <param name="color">
    ///     The <see cref="Color"/> value to check for equality.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified <see cref="Color"/> value
    ///     is equal to this <see cref="AsepriteColor"/> value; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public bool Equals(Color color) => this.R == color.R &&
                                       this.G == color.G &&
                                       this.B == color.B &&
                                       this.A == color.A;


    public static explicit operator Color(AsepriteColor c) => Color.FromArgb(c.A, c.R, c.G, c.B);
    public static explicit operator AsepriteColor(Color c) => AsepriteColor.FromRgba(c.R, c.G, c.B, c.A);
}