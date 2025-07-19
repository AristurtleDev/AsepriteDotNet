// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core
{
    /// <summary>
    /// Provides mathematical utility functions for bit manipulation and 8-bit color calculations.
    /// </summary>
    internal static class Calc
    {
        /// <summary>
        /// Determines whether the specified flag bits are set in the value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="flag">The flag bits to check for.</param>
        /// <returns><c>true</c> if all bits in <paramref name="flag"/> are set in <paramref name="value"/>; otherwise, <c>false</c>.</returns>
        internal static bool HasFlag(this uint value, uint flag) => (value & flag) != 0;

        /// <summary>
        /// Determines whether the specified flag bits are not set in the value.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="flag">The flag bits to check for absence.</param>
        /// <returns><c>true</c> if any bits in <paramref name="flag"/> are not set in <paramref name="value"/>; otherwise, <c>false</c>.</returns>
        internal static bool DoesNotHaveFlag(this uint value, uint flag) => !value.HasFlag(flag);

        /// <summary>
        /// Returns a reference to the larger of two double values.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        /// <returns>A reference to <paramref name="a"/> if it is greater than or equal to <paramref name="b"/>; otherwise, a reference to <paramref name="b"/>.</returns>
        /// <remarks>
        /// Enables in-place modification of the maximum value without copying.
        /// </remarks>
        internal static ref double RefMax(ref double a, ref double b) => ref (a >= b ? ref a : ref b);

        /// <summary>
        /// Returns a reference to the smaller of two double values.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        /// <returns>A reference to <paramref name="a"/> if it is less than or equal to <paramref name="b"/>; otherwise, a reference to <paramref name="b"/>.</returns>
        /// <remarks>
        /// Enables in-place modification of the minimum value without copying.
        /// </remarks>
        internal static ref double RefMin(ref double a, ref double b) => ref (a <= b ? ref a : ref b);

        /// <summary>
        /// Returns a reference to the middle value among three double values.
        /// </summary>
        /// <param name="a">The first value to compare.</param>
        /// <param name="b">The second value to compare.</param>
        /// <param name="c">The third value to compare.</param>
        /// <returns>A reference to whichever parameter contains the median value.</returns>
        /// <remarks>
        /// Efficiently finds the median without sorting by excluding the minimum and maximum values.
        /// Enables in-place modification of the median value without copying.
        /// </remarks>
        internal static ref double RefMid(ref double a, ref double b, ref double c)
        {
            double min = Math.Min(Math.Min(a, b), c);
            double max = Math.Max(Math.Max(a, b), c);

            if (a != min && a != max) { return ref a; }
            if (b != min && b != max) { return ref b; }
            return ref c;
        }

        /// <summary>
        /// Multiplies two values using fixed-point arithmetic optimized for 8-bit color calculations.
        /// </summary>
        /// <param name="a">The first value (0-255).</param>
        /// <param name="b">The second value as an integer multiplier.</param>
        /// <returns>The product clamped to the range [0, 255].</returns>
        internal static byte MultiplyUnsigned8Bit(byte a, int b)
        {
            int v = a * b + 0x80;
            return (byte)((v >> 8) + v >> 8);
        }

        /// <summary>
        /// Multiplies two values using fixed-point arithmetic optimized for 8-bit color calculations.
        /// </summary>
        /// <param name="a">The first value cast to byte range.</param>
        /// <param name="b">The second value as an integer multiplier.</param>
        /// <returns>The product clamped to the range [0, 255].</returns>
        internal static byte MultiplyUnsigned8Bit(int a, int b) => MultiplyUnsigned8Bit((byte)a, b);

        /// <summary>
        /// Divides two values using fixed-point arithmetic optimized for 8-bit color calculations.
        /// </summary>
        /// <param name="a">The dividend (0-255).</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The quotient clamped to the range [0, 255].</returns>
        internal static byte DivideUnsigned8Bit(byte a, int b) => (byte)((a * 0xFF + b / 2) / b);

        /// <summary>
        /// Divides two values using fixed-point arithmetic optimized for 8-bit color calculations.
        /// </summary>
        /// <param name="a">The dividend cast to byte range.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The quotient clamped to the range [0, 255].</returns>
        internal static byte DivideUnsigned8Bit(int a, int b) => DivideUnsigned8Bit((byte)a, b);
    }
}
