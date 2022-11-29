// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2022 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */
// using System.Diagnostics.CodeAnalysis;
// using System.Drawing;

// namespace AsepriteDotNet.Document;

// public struct Pixel : IEquatable<Pixel>
// {
//     private const int RGBA_R_SHIFT = 0;
//     private const int RGBA_G_SHIFT = 8;
//     private const int RGBA_B_SHIFT = 16;
//     private const int RGBA_A_SHIFT = 24;

//     private uint _value;

//     /// <summary>
//     ///     Gets the <see cref="System.Drawing.Color"/> value of this
//     ///     <see cref="Pixel"/>.
//     /// </summary>
//     public Color Color => Color.FromArgb(GetA(), GetR(), GetG(), GetB());


//     /// <summary>
//     ///     Creates a <see cref="Pixel"/> value from the four RGBA (red, green,
//     ///     blue, and alpha) values.
//     /// </summary>
//     /// <param name="red">
//     ///     The red componnet value.  Valid values are 0 through 255 inclusivly.
//     /// </param>
//     /// <param name="green">
//     ///     The green component value. Valid values are 0 through 255 
//     ///     inclusivly.
//     /// </param>
//     /// <param name="blue">
//     ///     The blue component value.  Valid values are 0 through 255 
//     ///     inclusivly.
//     /// </param>
//     /// <param name="alpha">
//     ///     The alpha component value.  Valid values are 0 through 255
//     ///     inclusivly.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="Pixel"/> value this method creates.
//     /// </returns>
//     /// <exception cref="ArgumentException">
//     ///     Thrown if <paramref name="red"/>, <paramref name="green"/>,
//     ///     <paramref name="blue"/>, or <paramref name="alpha"/> is less than 0
//     ///     or greater than 255.
//     /// </exception>
//     public static Pixel FromRgba(byte red, byte green, byte blue, byte alpha)
//     {
//         CheckByte(red, nameof(red));
//         CheckByte(green, nameof(green));
//         CheckByte(blue, nameof(blue));
//         CheckByte(alpha, nameof(alpha));

//         Pixel pixel;
//         pixel._value = (uint)((red << RGBA_R_SHIFT) |
//                               (green << RGBA_G_SHIFT) |
//                               (blue << RGBA_B_SHIFT) |
//                               (alpha << RGBA_A_SHIFT));

//         return pixel;
//     }

//     private static void CheckByte(byte value, string name)
//     {
//         if (value < 0 || value > 255)
//         {
//             throw new ArgumentException(name,
//             $"Value of '{name}' is not valid.  '{name}' should be greater than or equal to 0 and less than or equal to 255");
//         }
//     }


//     private byte GetR() => unchecked((byte)((_value >> RGBA_R_SHIFT) & 0xFF));
//     private byte GetG() => unchecked((byte)((_value >> RGBA_G_SHIFT) & 0xFF));
//     private byte GetB() => unchecked((byte)((_value >> RGBA_B_SHIFT) & 0xFF));
//     private byte GetA() => unchecked((byte)((_value >> RGBA_A_SHIFT) & 0xFF));

//     /// <summary>
//     ///     Returns whether the specified <paramref name="obj"/> is equal to
//     ///     this <see cref="Pixel"/> value.
//     /// </summary>
//     /// <param name="obj">
//     ///     The <see cref="object"/> to check for equality.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the specified <paramref name="obj"/> is
//     ///     equal to this <see cref="Pixel"/> value; otherwise,
//     ///     <see langword="false"/>.
//     /// </returns>
//     public override bool Equals([NotNullWhen(true)] object? obj) =>
//         obj is Pixel other && this == other;

//     /// <summary>
//     ///     Returns the hash code for this <see cref="Pixel"/> value.
//     /// </summary>
//     /// <returns>
//     ///     A 4-byte signed integer hash code.
//     /// </returns>
//     public override int GetHashCode() => _value.GetHashCode();

//     /// <summary>
//     ///     Returns whether the <paramref name="left"/> <see cref="Pixel"/> 
//     ///     value is equal to the <paramref name="right"/> <see cref="Pixel"/> 
//     ///     value.
//     /// </summary>
//     /// <param name="left">
//     ///     The <see cref="Pixel"/> value on the left side of the equality
//     ///     operator.
//     /// </param>
//     /// <param name="right">
//     ///     The <see cref="Pixel"/> value on the right side of the equality 
//     ///     operator.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the <paramref name="left"/> 
//     ///     <see cref="Pixel"/> value is equal to the <paramref name="right"/> 
//     ///     <see cref="Pixel"/> value; otherwise, <see langword="false"/>.
//     /// </returns>
//     public static bool operator ==(Pixel left, Pixel right) =>
//         left._value == right._value;

//     /// <summary>
//     ///     Returns whether the <paramref name="left"/> <see cref="Pixel"/> 
//     ///     value is not equal to the <paramref name="right"/> 
//     ///     <see cref="Pixel"/> value.
//     /// </summary>
//     /// <param name="left">
//     ///     The <see cref="Pixel"/> value on the left side of the non-equality
//     ///     operator.
//     /// </param>
//     /// <param name="right">
//     ///     The <see cref="Pixel"/> value on the right side of the non-equality 
//     ///     operator.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the <paramref name="left"/> 
//     ///     <see cref="Pixel"/> value is not equal to the 
//     ///     <paramref name="right"/> <see cref="Pixel"/> value; otherwise, 
//     ///     <see langword="false"/>.
//     /// </returns>
//     public static bool operator !=(Pixel left, Pixel right) =>
//         left._value != right._value;

//     /// <summary>
//     ///     Returns whether the specified <see cref="Pixel"/> value is equal to
//     ///     this <see cref="Pixel"/> value.
//     /// </summary>
//     /// <param name="other">
//     ///     The other <see cref="Pixel"/> value to check for equality.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if the specified <see cref="Pixel"/> value is
//     ///     equal to this <see cref="Pixel"/> value; otherwise,
//     ///     <see langword="false"/>.
//     /// </returns>
//     public bool Equals(Pixel other) => this._value == other._value;
// }