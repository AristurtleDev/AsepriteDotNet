// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core
{
    internal static class Calc
    {
        /// <summary>
        /// Returns a value that indicates if the specified flag is set for the given value.
        /// </summary>
        /// <param name="value">The value to check for the flag.</param>
        /// <param name="flag">The flag to check against the value.</param>
        /// <returns>
        /// <see langword="true"/> if the flag is set in the value; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool HasFlag(this uint value, uint flag) => (value & flag) != 0;

        /// <summary>
        /// Returns a value that indicates if the specified flag is not set for the given value.
        /// </summary>
        /// <param name="value">The value to check for the flag.</param>
        /// <param name="flag">The flag to check against the value.</param>
        /// <returns>
        /// <see langword="true"/> if the flag is not set in the value otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool DoesNotHaveFlag(this uint value, uint flag) => !value.HasFlag(flag);

        /// <summary>
        /// Returns the reference to the value that is the maximum value between two double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <returns>The reference to the maximum value between <paramref name="a"/> and <paramref name="b"/></returns>.
        internal static ref double RefMax(ref double a, ref double b) => ref (a >= b ? ref a : ref b);

        /// <summary>
        /// Returns the reference to the value that is the minimum value between two double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <returns>The reference to the minimum value between <paramref name="a"/> and <paramref name="b"/></returns>.
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
        public static byte MultiplyUnsigned8Bit(int a, int b) => MultiplyUnsigned8Bit((byte)a, b);

        /// <summary>
        /// Returns the result of dividing two unsigned 8-bit values.
        /// </summary>
        /// <param name="a">The dividend</param>
        /// <param name="b">The divisor</param>
        /// <returns>The result of multiplying two unsigned 8-bit values.</returns>
        internal static byte DivideUnsigned8Bit(byte a, int b) => (byte)((a * 0xFF + b / 2) / b);

        /// <summary>
        /// Returns the result of dividing two unsigned 8-bit values.
        /// </summary>
        /// <param name="a">The dividend</param>
        /// <param name="b">The divisor</param>
        /// <returns>The result of multiplying two unsigned 8-bit values.</returns>
        internal static byte DivideUnsigned8Bit(int a, int b) => DivideUnsigned8Bit((byte)a, b);
    }
}
