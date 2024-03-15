// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Drawing;
using System.Net;
using AsepriteDotNet.Helpers;

namespace AsepriteDotNet;

public static class AseColorBlending
{
    private const byte RGBA_R_SHIFT = 0;
    private const byte RGBA_G_SHIFT = 8;
    private const byte RGBA_B_SHIFT = 16;
    private const byte RGBA_A_SHIFT = 24;
    private const uint RGBA_R_MASK = 0x000000ff;
    private const uint RGBA_G_MASK = 0x0000ff00;
    private const uint RGBA_B_MASK = 0x00ff0000;
    private const uint RGBA_RGB_MASK = 0x00ffffff;
    private const uint RGBA_A_MASK = 0xff000000;

    public static AseColor Blend(this AseColor backdrop, AseColor source, int opacity, AsepriteBlendMode blendMode) =>
        backdrop.Blend(source, opacity, blendMode, (color) => color);

    public static T Blend<T>(this AseColor backdrop, AseColor source, int opacity, AsepriteBlendMode blendMode, Func<AseColor, T> converter)
    {
        //  Exit early depending on alpha
        if (backdrop.A == 0 && source.A == 0) { return converter(new AseColor(0, 0, 0, 0)); }
        if (backdrop.A == 0) { return converter(source); }
        if (source.A == 0) { return converter(backdrop); }

        AseColor blended = blendMode switch
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

        return converter(blended);
    }

    private static AseColor Normal(AseColor backdrop, AseColor source, int opacity)
    {
        if ((backdrop.PackedValue & RGBA_A_MASK) == 0)
        {
            source.A = Unsigned8Bit.Multiply(source.A, opacity);
            source.A <<= RGBA_A_SHIFT;
            return source;
        }
        else if ((source.PackedValue & RGBA_A_MASK) == 0)
        {
            return backdrop;
        }

        opacity = Unsigned8Bit.Multiply(source.A, opacity);

        int a = source.A + backdrop.A - Unsigned8Bit.Multiply(backdrop.A, source.A);
        int r = backdrop.R + (source.R - backdrop.R) * opacity / a;
        int g = backdrop.G + (source.G - backdrop.G) * opacity / a;
        int b = backdrop.B + (source.B - backdrop.B) * opacity / a;

        return new AseColor((byte)r, (byte)g, (byte)b, (byte)a);
    }

    private static AseColor Multiply(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = Unsigned8Bit.Multiply(backdrop.R, source.R);
        source.G = Unsigned8Bit.Multiply(backdrop.G, source.G);
        source.B = Unsigned8Bit.Multiply(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Screen(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = (byte)(backdrop.R + source.R - Unsigned8Bit.Multiply(backdrop.R, source.R));
        source.G = (byte)(backdrop.G + source.G - Unsigned8Bit.Multiply(backdrop.G, source.G));
        source.B = (byte)(backdrop.B + source.B - Unsigned8Bit.Multiply(backdrop.B, source.B));
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Overlay(AseColor backdrop, AseColor source, int opacity)
    {
        static int overlay(int b, int s)
        {
            if (b < 128)
            {
                b <<= 1;
                return Unsigned8Bit.Multiply(s, b);
            }

            b = (b << 1) - 255;
            return s + b - Unsigned8Bit.Multiply(s, b);
        }

        source.R = (byte)overlay(backdrop.R, source.R);
        source.G = (byte)overlay(backdrop.G, source.G);
        source.B = (byte)overlay(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Darken(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = Math.Min(backdrop.R, source.R);
        source.G = Math.Min(backdrop.G, source.G);
        source.B = Math.Min(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Lighten(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = Math.Max(backdrop.R, source.R);
        source.G = Math.Max(backdrop.G, source.G);
        source.B = Math.Max(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor ColorDodge(AseColor backdrop, AseColor source, int opacity)
    {
        static int dodge(int b, int s)
        {
            if (b == 0) { return 0; }

            s = 255 - s;

            if (b >= s) { return 255; }

            return Unsigned8Bit.Divide(b, s);
        }

        source.R = (byte)dodge(backdrop.R, source.R);
        source.G = (byte)dodge(backdrop.G, source.G);
        source.B = (byte)dodge(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor ColorBurn(AseColor backdrop, AseColor source, int opacity)
    {
        static int burn(int b, int s)
        {
            if (b == 255) { return 255; }

            b = 255 - b;

            if (b >= s) { return 0; }

            return 255 - Unsigned8Bit.Divide(b, s);
        }

        source.R = (byte)burn(backdrop.R, source.R);
        source.G = (byte)burn(backdrop.G, source.G);
        source.B = (byte)burn(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor HardLight(AseColor backdrop, AseColor source, int opacity)
    {
        static int hardlight(int b, int s)
        {
            if (s < 128)
            {
                s <<= 1;
                return Unsigned8Bit.Multiply(b, s);
            }

            s = (s << 1) - 255;
            return b + s - Unsigned8Bit.Multiply(b, s);
        }

        source.R = (byte)hardlight(backdrop.R, source.R);
        source.G = (byte)hardlight(backdrop.G, source.G);
        source.B = (byte)hardlight(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor SoftLight(AseColor backdrop, AseColor source, int opacity)
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

    private static AseColor Difference(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = (byte)Math.Abs(backdrop.R - source.R);
        source.G = (byte)Math.Abs(backdrop.G - source.G);
        source.B = (byte)Math.Abs(backdrop.B - source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Exclusion(AseColor backdrop, AseColor source, int opacity)
    {
        static int exclusion(int b, int s) => b + s - 2 * Unsigned8Bit.Multiply(b, s);

        source.R = (byte)exclusion(backdrop.R, source.R);
        source.G = (byte)exclusion(backdrop.G, source.G);
        source.B = (byte)exclusion(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor HslHue(AseColor backdrop, AseColor source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double s = MathHelper.CalculateSaturation(r, g, b);
        double l = MathHelper.CalculateLuminance(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        MathHelper.AdjustSaturation(ref r, ref g, ref b, s);
        MathHelper.AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor HslSaturation(AseColor backdrop, AseColor source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double s = MathHelper.CalculateSaturation(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;
        double l = MathHelper.CalculateLuminance(r, g, b);

        MathHelper.AdjustSaturation(ref r, ref g, ref b, s);
        MathHelper.AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor HslColor(AseColor backdrop, AseColor source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double l = MathHelper.CalculateLuminance(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        MathHelper.AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor HslLuminosity(AseColor backdrop, AseColor source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double l = MathHelper.CalculateLuminance(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;

        MathHelper.AdjustLumanice(ref r, ref g, ref b, l);

        source.R = (byte)(r * 255.0);
        source.G = (byte)(g * 255.0);
        source.B = (byte)(b * 255.0);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Addition(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = (byte)Math.Min(backdrop.R + source.R, 255);
        source.G = (byte)Math.Min(backdrop.G + source.G, 255);
        source.B = (byte)Math.Min(backdrop.B + source.B, 255);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Subtract(AseColor backdrop, AseColor source, int opacity)
    {
        source.R = (byte)Math.Max(backdrop.R - source.R, 0);
        source.G = (byte)Math.Max(backdrop.G - source.G, 0);
        source.B = (byte)Math.Max(backdrop.B - source.B, 0);
        return Normal(backdrop, source, opacity);
    }

    private static AseColor Divide(AseColor backdrop, AseColor source, int opacity)
    {
        static int divide(int b, int s)
        {
            if (b == 0) { return 0; }

            if (b >= s) { return 255; }

            return Unsigned8Bit.Divide(b, s);
        }

        source.R = (byte)(divide(backdrop.R, source.R));
        source.G = (byte)(divide(backdrop.G, source.G));
        source.B = (byte)(divide(backdrop.B, source.B));
        return Normal(backdrop, source, opacity);
    }
}
