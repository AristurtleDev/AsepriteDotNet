// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Helpers
{
    internal static class MathHelper
    {
        /// <summary>
        /// Calculates the saturation value based on the given RGB color component values.
        /// </summary>
        /// <param name="r">The red color component value (0 to 1).</param>
        /// <param name="g">The green color component value (0 to 1).</param>
        /// <param name="b">The blue color component value (0 to 1).</param>
        /// <returns>The saturation value calculated.</returns>
        internal static double CalculateSaturation(double r, double g, double b)
        {
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            return max - min;
        }

        /// <summary>
        /// Calculates the luminance value based on the given RGB color component values.
        /// </summary>
        /// <param name="r">The red color component value (0 to 1).</param>
        /// <param name="g">The green color component value (0 to 1).</param>
        /// <param name="b">The blue color component value (0 to 1).</param>
        /// <returns></returns>
        internal static double CalculateLuminance(double r, double g, double b)
        {
            //  Primary coefficients for Rec. 601
            //  however Aseprite internally uses these exact values.
            const double redCoefficient = 0.3;
            const double greenCoefficient = 0.59;
            const double blueCoefficient = 0.11;

            r *= redCoefficient;
            g *= greenCoefficient;
            b *= blueCoefficient;

            return r + g + b;
        }

        /// <summary>
        /// Modifies the saturation of the specified RGB color component values.
        /// </summary>
        /// <param name="r">The red color component value (0 to 1).</param>
        /// <param name="g">The green color component value (0 to 1).</param>
        /// <param name="b">The blue color component value (0 to 1).</param>
        /// <param name="s">The saturation factor to adjust the color components by.</param>
        internal static void AdjustSaturation(ref double r, ref double g, ref double b, double s)
        {
            ref double min = ref Min(ref Min(ref r, ref g), ref b);
            ref double max = ref Max(ref Max(ref r, ref g), ref b);
            ref double mid = ref Mid(ref r, ref g, ref b);

            if (max > min)
            {
                mid = ((mid - min) * s) / (max - min);
                max = s;
            }
            else
            {
                mid = max = 0;
            }

            min = 0;
        }

        /// <summary>
        /// Modifies the luminosity of the specified RGB color component values.
        /// </summary>
        /// <param name="r">The red color component value (0 to 1).</param>
        /// <param name="g">The green color component value (0 to 1).</param>
        /// <param name="b">The blue color component value (0 to 1).</param>
        /// <param name="l">The desired luminosity value to apply.</param>
        internal static void AdjustLumanice(ref double r, ref double g, ref double b, double l)
        {
            double current = CalculateLuminance(r, g, b);
            double difference = l - current;
            r += difference;
            g += difference;
            b += difference;
            NormalizeColor(ref r, ref g, ref b);
        }

        /// <summary>
        /// Normalizes the specified RGB color component values to ensure they are within the valid range of 0 to 1.
        /// Clips the values of the specified RGB color
        /// </summary>
        /// <param name="r">The red color component value.</param>
        /// <param name="g">The green color component value.</param>
        /// <param name="b">The blue color component value.</param>
        private static void NormalizeColor(ref double r, ref double g, ref double b)
        {
            double luminosity = CalculateLuminance(r, g, b);
            double rgbMin = Math.Min(Math.Min(r, g), b);
            double rgbMax = Math.Max(Math.Max(r, g), b);

            if (rgbMin < 0)
            {
                double scale = luminosity / (luminosity - rgbMin);
                r = luminosity + ((r - luminosity) * scale);
                g = luminosity + ((g - luminosity) * scale);
                b = luminosity + ((b - luminosity) * scale);
            }

            if (rgbMax > 1)
            {
                double scale = (1 - luminosity) / (rgbMax - luminosity);
                r = luminosity + ((r - luminosity) * scale);
                g = luminosity + ((g - luminosity) * scale);
                b = luminosity + ((b - luminosity) * scale);
            }
        }

        /// <summary>
        /// Returns the reference to the value that is the maximum value between two double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <returns>The reference to the maximum value between <paramref name="a"/> and <paramref name="b"/></returns>.
        internal static ref double Max(ref double a, ref double b) => ref (a >= b ? ref a : ref b);

        /// <summary>
        /// Returns the reference to the value that is the minimum value between two double values specified.
        /// </summary>
        /// <param name="a">The first double value.</param>
        /// <param name="b">The second double value.</param>
        /// <returns>The reference to the minimum value between <paramref name="a"/> and <paramref name="b"/></returns>.
        internal static ref double Min(ref double a, ref double b) => ref (a <= b ? ref a : ref b);

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
        internal static ref double Mid(ref double a, ref double b, ref double c)
        {
            double min = Math.Min(Math.Min(a, b), c);
            double max = Math.Max(Math.Max(a, b), c);

            if (a != min && a != max) { return ref a; }
            if (b != min && b != max) { return ref b; }
            return ref c;
        }
    }
}
