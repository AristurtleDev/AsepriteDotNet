// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet;

internal static class AsepriteColorUtilities
{
    /// <summary>
    /// Converts an array of <see cref="byte"/> data to an array of <see cref="Rgba32"/> values based on the
    /// specified <see cref="AsepriteColorDepth"/>.
    /// </summary>
    /// <param name="pixels">The array of <see cref="byte"/> data that represents the color data.</param>
    /// <param name="depth">The color depth.</param>
    /// <param name="palette">
    /// The palette used for <see cref="AsepriteColorDepth.Indexed">ColorDepth.Index</see>.  Optional, only required when
    /// <paramref name="depth"/> is equal to <see cref="AsepriteColorDepth.Indexed">ColorDepth.Indexed</see>.
    /// </param>
    /// <returns>An array of <see cref="Rgba32"/> values converted from the data.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="pixels"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="depth"/> is an unknown <see cref="AsepriteColorDepth"/> value.
    /// </exception>
    internal static Rgba32[] PixelsToColor(byte[] pixels, AsepriteColorDepth depth, AsepritePalette? palette = null)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        int bpp = (int)depth / 8;
        Rgba32[] result = new Rgba32[pixels.Length / bpp];

        for (int i = 0, b = 0; i < result.Length; i++, b += bpp)
        {
            Rgba32 color = new Rgba32();

            switch (depth)
            {
                case AsepriteColorDepth.RGBA:
                    color.R = pixels[b];
                    color.G = pixels[b + 1];
                    color.B = pixels[b + 2];
                    color.A = pixels[b + 3];
                    break;

                case AsepriteColorDepth.Grayscale:
                    color.R = color.G = color.B = pixels[b];
                    color.A = pixels[b + 1];
                    break;

                case AsepriteColorDepth.Indexed:
                    int index = pixels[i];
                    if (index != palette?.TransparentIndex)
                    {
                        if(palette is not null)
                        {
                            color = palette.Colors[index];
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException($"Unknown Color Depth: {depth}");
            }
            result[i] = color;
        }

        return result;
    }

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
        ref double min = ref Calc.RefMin(ref Calc.RefMin(ref r, ref g), ref b);
        ref double max = ref Calc.RefMax(ref Calc.RefMax(ref r, ref g), ref b);
        ref double mid = ref Calc.RefMid(ref r, ref g, ref b);

        if (max > min)
        {
            mid = (mid - min) * s / (max - min);
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
    internal static void NormalizeColor(ref double r, ref double g, ref double b)
    {
        double luminosity = CalculateLuminance(r, g, b);
        double rgbMin = Math.Min(Math.Min(r, g), b);
        double rgbMax = Math.Max(Math.Max(r, g), b);

        if (rgbMin < 0)
        {
            double scale = luminosity / (luminosity - rgbMin);
            r = luminosity + (r - luminosity) * scale;
            g = luminosity + (g - luminosity) * scale;
            b = luminosity + (b - luminosity) * scale;
        }

        if (rgbMax > 1)
        {
            double scale = (1 - luminosity) / (rgbMax - luminosity);
            r = luminosity + (r - luminosity) * scale;
            g = luminosity + (g - luminosity) * scale;
            b = luminosity + (b - luminosity) * scale;
        }
    }

    /// <summary>
    /// Blends two <see cref="Rgba32"/> values using the specified <see cref="AsepriteBlendMode"/> and opacity.
    /// </summary>
    /// <param name="backdrop">The backdrop color.</param>
    /// <param name="source">The source color to be blended onto the <paramref name="backdrop"/>.</param>
    /// <param name="opacity">The opacity of the blending operation.</param>
    /// <param name="blendMode">The <see cref="AsepriteBlendMode"/> to use for the blending operation.</param>
    /// <returns>The resulting <see cref="Rgba32"/> value created from the blending.</returns>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="blendMode"/> is an unknown <see cref="AsepriteBlendMode"/> value.
    /// </exception>
    internal static Rgba32 Blend(Rgba32 backdrop, Rgba32 source, int opacity, AsepriteBlendMode blendMode)
    {
        //  Exit early depending on alpha
        if (backdrop.A == 0 && source.A == 0) { return new Rgba32(0, 0, 0, 0); }
        if (backdrop.A == 0) { return source; }
        if (source.A == 0) { return backdrop; }

        Rgba32 blended = blendMode switch
        {
            #pragma warning disable format
            AsepriteBlendMode.Normal        => Normal(backdrop, source, opacity),
            AsepriteBlendMode.Multiply      => Multiply(backdrop, source, opacity),
            AsepriteBlendMode.Screen        => Screen(backdrop, source, opacity),
            AsepriteBlendMode.Overlay       => Overlay(backdrop, source, opacity),
            AsepriteBlendMode.Darken        => Darken(backdrop, source, opacity),
            AsepriteBlendMode.Lighten       => Lighten(backdrop, source, opacity),
            AsepriteBlendMode.ColorDodge    => ColorDodge(backdrop, source, opacity),
            AsepriteBlendMode.ColorBurn     => ColorBurn(backdrop, source, opacity),
            AsepriteBlendMode.HardLight     => HardLight(backdrop, source, opacity),
            AsepriteBlendMode.SoftLight     => SoftLight(backdrop, source, opacity),
            AsepriteBlendMode.Difference    => Difference(backdrop, source, opacity),
            AsepriteBlendMode.Exclusion     => Exclusion(backdrop, source, opacity),
            AsepriteBlendMode.Hue           => HslHue(backdrop, source, opacity),
            AsepriteBlendMode.Saturation    => HslSaturation(backdrop, source, opacity),
            AsepriteBlendMode.Color         => HslColor(backdrop, source, opacity),
            AsepriteBlendMode.Luminosity    => HslLuminosity(backdrop, source, opacity),
            AsepriteBlendMode.Addition      => Addition(backdrop, source, opacity),
            AsepriteBlendMode.Subtract      => Subtract(backdrop, source, opacity),
            AsepriteBlendMode.Divide        => Divide(backdrop, source, opacity),
            _                               => throw new InvalidOperationException($"Unknown blend mode '{blendMode}'")
            #pragma warning restore format           
        };

        return blended;
    }

    private static Rgba32 Normal(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        if (backdrop.A == 0)
        {
            source.A = Calc.MultiplyUnsigned8Bit(source.A, opacity);
            return source;
        }
        else if (source.A == 0)
        {
            return backdrop;
        }

        opacity = Calc.MultiplyUnsigned8Bit(source.A, opacity);

        int a = source.A + backdrop.A - Calc.MultiplyUnsigned8Bit(backdrop.A, source.A);
        int r = backdrop.R + (source.R - backdrop.R) * opacity / a;
        int g = backdrop.G + (source.G - backdrop.G) * opacity / a;
        int b = backdrop.B + (source.B - backdrop.B) * opacity / a;

        return new Rgba32((byte)r, (byte)g, (byte)b, (byte)a);
    }

    private static Rgba32 Multiply(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = Calc.MultiplyUnsigned8Bit(backdrop.R, source.R);
        source.G = Calc.MultiplyUnsigned8Bit(backdrop.G, source.G);
        source.B = Calc.MultiplyUnsigned8Bit(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Screen(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = (byte)(backdrop.R + source.R - Calc.MultiplyUnsigned8Bit(backdrop.R, source.R));
        source.G = (byte)(backdrop.G + source.G - Calc.MultiplyUnsigned8Bit(backdrop.G, source.G));
        source.B = (byte)(backdrop.B + source.B - Calc.MultiplyUnsigned8Bit(backdrop.B, source.B));
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Overlay(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int overlay(int b, int s)
        {
            if (b < 128)
            {
                b <<= 1;
                return Calc.MultiplyUnsigned8Bit(s, b);
            }

            b = (b << 1) - 255;
            return s + b - Calc.MultiplyUnsigned8Bit(s, b);
        }

        source.R = (byte)overlay(backdrop.R, source.R);
        source.G = (byte)overlay(backdrop.G, source.G);
        source.B = (byte)overlay(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Darken(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = Math.Min(backdrop.R, source.R);
        source.G = Math.Min(backdrop.G, source.G);
        source.B = Math.Min(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Lighten(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = Math.Max(backdrop.R, source.R);
        source.G = Math.Max(backdrop.G, source.G);
        source.B = Math.Max(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 ColorDodge(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int dodge(int b, int s)
        {
            if (b == 0) { return 0; }

            s = 255 - s;

            if (b >= s) { return 255; }

            return Calc.DivideUnsigned8Bit(b, s);
        }

        source.R = (byte)dodge(backdrop.R, source.R);
        source.G = (byte)dodge(backdrop.G, source.G);
        source.B = (byte)dodge(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 ColorBurn(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int burn(int b, int s)
        {
            if (b == 255) { return 255; }

            b = 255 - b;

            if (b >= s) { return 0; }

            return 255 - Calc.DivideUnsigned8Bit(b, s);
        }

        source.R = (byte)burn(backdrop.R, source.R);
        source.G = (byte)burn(backdrop.G, source.G);
        source.B = (byte)burn(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 HardLight(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int hardlight(int b, int s)
        {
            if (s < 128)
            {
                s <<= 1;
                return Calc.MultiplyUnsigned8Bit(b, s);
            }

            s = (s << 1) - 255;
            return b + s - Calc.MultiplyUnsigned8Bit(b, s);
        }

        source.R = (byte)hardlight(backdrop.R, source.R);
        source.G = (byte)hardlight(backdrop.G, source.G);
        source.B = (byte)hardlight(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 SoftLight(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int softlight(int _b, int _s)
        {
            double b = _b / 255.0;
            double s = _s / 255.0;
            double r, d;

            if (b <= 0.25)
            {
                d = ((16 * b - 12) * b + 4) * b;
            }
            else
            {
                d = Math.Sqrt(b);
            }

            if (s <= 0.5)
            {
                r = b - (1.0 - 2.0 * s) * b * (1.0 - b);
            }
            else
            {
                r = b + (2.0 * s - 1.0) * (d - b);
            }

            return (int)(r * 255 + 0.5);
        }

        source.R = (byte)softlight(backdrop.R, source.R);
        source.G = (byte)softlight(backdrop.G, source.G);
        source.B = (byte)softlight(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Difference(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = (byte)Math.Abs(backdrop.R - source.R);
        source.G = (byte)Math.Abs(backdrop.G - source.G);
        source.B = (byte)Math.Abs(backdrop.B - source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Exclusion(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int exclusion(int b, int s) => b + s - 2 * Calc.MultiplyUnsigned8Bit(b, s);

        source.R = (byte)exclusion(backdrop.R, source.R);
        source.G = (byte)exclusion(backdrop.G, source.G);
        source.B = (byte)exclusion(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 HslHue(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double s = CalculateSaturation(r, g, b);
        double l = CalculateLuminance(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        AdjustSaturation(ref r, ref g, ref b, s);
        AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 HslSaturation(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double s = CalculateSaturation(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;
        double l = CalculateLuminance(r, g, b);

        AdjustSaturation(ref r, ref g, ref b, s);
        AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 HslColor(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double l = CalculateLuminance(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 HslLuminosity(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double l = CalculateLuminance(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;

        AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Addition(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = (byte)Math.Min(backdrop.R + source.R, 255);
        source.G = (byte)Math.Min(backdrop.G + source.G, 255);
        source.B = (byte)Math.Min(backdrop.B + source.B, 255);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Subtract(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        source.R = (byte)Math.Max(backdrop.R - source.R, 0);
        source.G = (byte)Math.Max(backdrop.G - source.G, 0);
        source.B = (byte)Math.Max(backdrop.B - source.B, 0);
        return Normal(backdrop, source, opacity);
    }

    private static Rgba32 Divide(Rgba32 backdrop, Rgba32 source, int opacity)
    {
        static int divide(int b, int s)
        {
            if (b == 0) { return 0; }

            if (b >= s) { return 255; }

            return Calc.DivideUnsigned8Bit(b, s);
        }

        source.R = (byte)(divide(backdrop.R, source.R));
        source.G = (byte)(divide(backdrop.G, source.G));
        source.B = (byte)(divide(backdrop.B, source.B));
        return Normal(backdrop, source, opacity);
    }
}
