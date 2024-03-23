// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using AsepriteDotNet.Common;

namespace AsepriteDotNet
{
    /// <summary>
    /// Provides common maths calculation utility functions.
    /// </summary>
    public static class Calc
    {
        private static readonly Vector4 s_maxBytes = Vector128.Create(255.0f).AsVector4();
        private static readonly Vector4 s_half = Vector128.Create(0.5f).AsVector4();

        /// <summary>
        /// Returns a value that indicates if the specified flag is set for the given value.
        /// </summary>
        /// <param name="value">The value to check for the flag.</param>
        /// <param name="flag">The flag to check against the value.</param>
        /// <returns>
        /// <see langword="true"/> if the flag is set in the value; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool HasFlag(this uint value, uint flag) => (value & flag) != 0;

        /// <summary>
        /// Returns a value that indicates if the specified flag is not set for the given value.
        /// </summary>
        /// <param name="value">The value to check for the flag.</param>
        /// <param name="flag">The flag to check against the value.</param>
        /// <returns>
        /// <see langword="true"/> if the flag is not set in the value otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool DoesNotHaveFlag(this uint value, uint flag) => !value.HasFlag(flag);

        /// <summary>
        /// Returns the reference to the value that is the maximum value between two double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <returns>The reference to the maximum value between <paramref name="a"/> and <paramref name="b"/></returns>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref double RefMax(ref double a, ref double b) => ref (a >= b ? ref a : ref b);

        /// <summary>
        /// Returns the reference to the value that is the minimum value between two double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <returns>The reference to the minimum value between <paramref name="a"/> and <paramref name="b"/></returns>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref double RefMin(ref double a, ref double b) => ref (a <= b ? ref a : ref b);

        /// <summary>
        /// Returns the reference to the value that is the middle value between the three double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <param name="c">The third double value.</param>
        /// <returns>
        /// The reference to the middle value between <paramref name="a"/>, <paramref name="b"/>, and
        /// <paramref name="c"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref double RefMid(ref double a, ref double b, ref double c)
        {
            double min = Math.Min(Math.Min(a, b), c);
            double max = Math.Max(Math.Max(a, b), c);

            if (a != min && a != max) { return ref a; }
            if (b != min && b != max) { return ref b; }
            return ref c;
        }

        /// <summary>
        /// Returns the result of multiplying two unsigned 8-bit values.
        /// </summary>
        /// <param name="a">The multiplicand</param>
        /// <param name="b">The multiplier</param>
        /// <returns>The result of multiplying two unsigned 8-bit values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte MultiplyUnsigned8Bit(byte a, int b)
        {
            int v = a * b + 0x80;
            return (byte)((v >> 8) + v >> 8);
        }

        /// <summary>
        /// Returns the result of multiplying two unsigned 8-bit values.
        /// </summary>
        /// <param name="a">The multiplicand</param>
        /// <param name="b">The multiplier</param>
        /// <returns>The result of multiplying two unsigned 8-bit values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte MultiplyUnsigned8Bit(int a, int b) => MultiplyUnsigned8Bit((byte)a, b);

        /// <summary>
        /// Returns the result of dividing two unsigned 8-bit values.
        /// </summary>
        /// <param name="a">The dividend</param>
        /// <param name="b">The divisor</param>
        /// <returns>The result of multiplying two unsigned 8-bit values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte DivideUnsigned8Bit(byte a, int b) => (byte)((a * 0xFF + b / 2) / b);

        /// <summary>
        /// Returns the result of dividing two unsigned 8-bit values.
        /// </summary>
        /// <param name="a">The dividend</param>
        /// <param name="b">The divisor</param>
        /// <returns>The result of multiplying two unsigned 8-bit values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte DivideUnsigned8Bit(int a, int b) => DivideUnsigned8Bit((byte)a, b);

        /// <summary>
        /// Unpacks the components of the given <see cref="Vector4"/> instance, which represents a color value with
        /// components in the range of 0 to 1, into a read-only span of bytes representing the color components in the
        /// range of 0 to 255.
        /// </summary>
        /// <param name="vector">
        /// The <see cref="Vector4"/> instance representing the color value to be unpacked, with components in the
        /// range of 0 to 1.
        /// </param>
        /// <returns>
        /// A read-only span of 4 bytes representing the unpacked color components (red, green, blue, alpha) in the
        /// range of 0 to 255.
        /// </returns>
        /// <remarks>
        /// This method is useful when working with graphics APIs or libraries that represent color values as
        /// floating-point values between 0 and 1, but you need to convert them to integer values between 0 and 255 for
        /// further processing or storage.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TColor UnpackColor<TColor>(Vector4 vector)
            where TColor : struct, IColor<TColor>
        {
            vector *= s_maxBytes;
            vector += s_half;
            vector = Vector4.Min(Vector4.Max(vector, Vector4.Zero), s_maxBytes);
            Vector128<byte> result = Vector128.ConvertToInt32(vector.AsVector128()).AsByte();
            TColor color = default(TColor);
            color.R = result.GetElement(0);
            color.G = result.GetElement(4);
            color.B = result.GetElement(8);
            color.A = result.GetElement(12);
            return color;
        }

        /// <summary>
        /// Packs the given red, green, blue, and alpha component values into a <see cref="System.Numerics.Vector4"/>
        /// instance, where each component value is represented as a floating-point value between 0 and 1.
        /// </summary>
        /// <param name="r">The red component value of the color, ranging from 0 to 255.</param>
        /// <param name="g">The green component value of the color, ranging from 0 to 255.</param>
        /// <param name="b">The blue component value of the color, ranging from 0 to 255.</param>
        /// <param name="a">The alpha (transparency) component value of the color, ranging from 0 to 255.</param>
        /// <returns>
        /// A <see cref="Vector4"/> instance representing the packed color value, where each component is a
        /// floating-point value between 0 and 1.
        /// </returns>
        /// <remarks>
        /// This method is useful when working with graphics APIs or libraries that expect color values to be
        /// represented as floating-point values between 0 and 1, rather than as integer values between 0 and 255.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 PackColor(byte r, byte g, byte b, byte a) => new Vector4(r, g, b, a) / s_maxBytes;
    }
}
