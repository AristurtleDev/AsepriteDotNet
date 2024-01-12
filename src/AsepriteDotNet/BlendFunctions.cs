using System.ComponentModel;
using AsepriteDotNet.Common;
using AsepriteDotNet.Pixman;

namespace AsepriteDotNet;

public static class BlendFunctions
{
    public static void Blend(ref this Pixel backdrop, Pixel source, int opacity, BlendMode mode)
    {
        if (backdrop.A == 0 && source.A == 0)
        {
            backdrop.Rgba = 0;
            return;
        }
        else if (backdrop.A == 0)
        {
            backdrop = source;
            return;
        }
        else if (source.A == 0)
        {
            return;
        }

        switch (mode)
        {
            case BlendMode.Normal:
                Normal(ref backdrop, in source, opacity);
                break;
            case BlendMode.Multiply:
                Multiply(ref backdrop, in source, opacity);
                break;
            case BlendMode.Screen:
                Screen(ref backdrop, in source, opacity);
                break;
            case BlendMode.Overlay:
                Overlay(ref backdrop, in source, opacity);
                break;
            case BlendMode.Darken:
                Darken(ref backdrop, in source, opacity);
                break;
            case BlendMode.Lighten:
                Lighten(ref backdrop, in source, opacity);
                break;
            case BlendMode.ColorDodge:
                ColorDodge(ref backdrop, in source, opacity);
                break;
            case BlendMode.ColorBurn:
                ColorBurn(ref backdrop, in source, opacity);
                break;
            case BlendMode.HardLight:
                HardLight(ref backdrop, in source, opacity);
                break;
            case BlendMode.SoftLight:
                SoftLight(ref backdrop, source, opacity);
                break;
            case BlendMode.Difference:
                Difference(ref backdrop, in source, opacity);
                break;
            case BlendMode.Exclusion:
                Exclusion(ref backdrop, in source, opacity);
                break;
            case BlendMode.Hue:
                HslHue(ref backdrop, in source, opacity);
                break;
            case BlendMode.Saturation:
                HslSaturation(ref backdrop, in source, opacity);
                break;
            case BlendMode.Color:
                HslColor(ref backdrop, in source, opacity);
                break;
            case BlendMode.Luminosity:
                HslLuminosity(ref backdrop, in source, opacity);
                break;
            case BlendMode.Addition:
                Addition(ref backdrop, in source, opacity);
                break;
            case BlendMode.Subtract:
                Subtract(ref backdrop, in source, opacity);
                break;
            case BlendMode.Divide:
                Divide(ref backdrop, in source, opacity);
                break;
            default:
                throw new InvalidOperationException($"Unknown blend mode '{mode}'");
        }
    }

    private static double Sat(double r, double g, double b) => Math.Max(r, Math.Max(g, b)) - Math.Min(r, Math.Min(g, b));

    private static double Lum(double r, double g, double b) => 0.3 * r + 0.59 * g + 0.11 * b;

    private static void SetSat(ref double r, ref double g, ref double b, double s)
    {
        ref double MIN(ref double x, ref double y) => ref (x < y ? ref x : ref y);
        ref double MAX(ref double x, ref double y) => ref (x > y ? ref x : ref y);
        ref double MID(ref double x, ref double y, ref double z) =>
            ref (x > y ? ref (y > z ? ref y : ref (x > z ? ref z : ref x)) : ref (y > z ? ref (z > x ? ref z : ref x) : ref y));

        ref double min = ref MIN(ref r, ref MIN(ref g, ref b));
        ref double mid = ref MID(ref r, ref g, ref b);
        ref double max = ref MAX(ref r, ref MAX(ref g, ref b));

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

    private static void SetLum(ref double r, ref double g, ref double b, double l)
    {
        double d = l - Lum(r, g, b);
        r += d;
        g += d;
        b += d;
        ClipColor(ref r, ref g, ref b);
    }

    private static void ClipColor(ref double r, ref double g, ref double b)
    {
        double l = Lum(r, g, b);
        double n = Math.Min(r, Math.Min(g, b));
        double x = Math.Max(r, Math.Max(g, b));

        if (n < 0)
        {
            r = l + (((r - l) * l) / (l - n));
            g = l + (((g - l) * l) / (l - n));
            b = l + (((b - l) * l) / (l - n));
        }

        if (x > 1)
        {
            r = l + (((r - l) * (1 - l)) / (x - l));
            g = l + (((g - l) * (1 - l)) / (x - l));
            b = l + (((b - l) * (1 - l)) / (x - l));
        }
    }

    private static void Normal(ref Pixel backdrop, in Pixel src, int opacity)
    {
        if (src.A == 0)
        {
            return;
        }

        byte alpha = Combine32.MUL_UN8(src.A, opacity);

        if (backdrop.A == 0)
        {
            backdrop = src;
            backdrop.A = alpha;
            return;
        }

        backdrop.A = (byte)(alpha + backdrop.A - Combine32.MUL_UN8(backdrop.A, alpha));
        backdrop.R = (byte)(backdrop.R + (src.R - backdrop.R) * alpha / backdrop.A);
        backdrop.G = (byte)(backdrop.G + (src.G - backdrop.G) * alpha / backdrop.A);
        backdrop.B = (byte)(backdrop.B + (src.B - backdrop.B) * alpha / backdrop.A);
    }

    private static void Multiply(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte r = Combine32.MUL_UN8(backdrop.R, source.R);
        byte g = Combine32.MUL_UN8(backdrop.G, source.G);
        byte b = Combine32.MUL_UN8(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Screen(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte r = (byte)(backdrop.R + source.R - Combine32.MUL_UN8(backdrop.R, source.R));
        byte g = (byte)(backdrop.G + source.G - Combine32.MUL_UN8(backdrop.G, source.G));
        byte b = (byte)(backdrop.B + source.B - Combine32.MUL_UN8(backdrop.B, source.B));
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Overlay(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte overlay(byte b, byte s)
        {
            if (b < 128)
            {
                b <<= 1;
                return Combine32.MUL_UN8(s, b);
            }
            else
            {
                b = (byte)((b << 1) - 255);
                return (byte)(s + b - Combine32.MUL_UN8(s, b));
            }
        }

        byte r = overlay(backdrop.R, source.R);
        byte g = overlay(backdrop.G, source.G);
        byte b = overlay(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Darken(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte blend(byte b, byte s) => Math.Min(b, s);

        byte r = blend(backdrop.R, source.R);
        byte g = blend(backdrop.G, source.G);
        byte b = blend(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Lighten(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte lighten(byte b, byte s) => Math.Max(b, s);

        byte r = lighten(backdrop.R, source.R);
        byte g = lighten(backdrop.G, source.G);
        byte b = lighten(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void ColorDodge(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte dodge(byte b, byte s)
        {
            if (b == 0)
            {
                return 0;
            }

            s = (byte)(255 - s);

            if (b >= s)
            {
                return 255;
            }
            else
            {
                return Combine32.DIV_UN8(b, s);
            }
        }

        byte r = dodge(backdrop.R, source.R);
        byte g = dodge(backdrop.G, source.G);
        byte b = dodge(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void ColorBurn(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte burn(byte b, byte s)
        {
            if (b == 255)
            {
                return 255;
            }

            b = (byte)(255 - b);

            if (b >= s)
            {
                return 0;
            }
            else
            {
                return (byte)(255 - Combine32.DIV_UN8(b, s));
            }
        }

        byte r = burn(backdrop.R, source.R);
        byte g = burn(backdrop.G, source.G);
        byte b = burn(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    //  Not working
    private static void HardLight(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte hardlight(byte b, byte s)
        {
            if (s < 128)
            {
                s <<= 1;
                return Combine32.MUL_UN8(b, s);
            }
            else
            {
                s = (byte)((s << 1) - 255);
                return (byte)(b + s - Combine32.MUL_UN8(b, s));
            }
        }

        byte r = hardlight(backdrop.R, source.R);
        byte g = hardlight(backdrop.G, source.G);
        byte b = hardlight(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void SoftLight(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte softlight(byte _b, byte _s)
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

            return (byte)(r * 255 + 0.5);
        }

        byte r = softlight(backdrop.R, source.R);
        byte g = softlight(backdrop.G, source.G);
        byte b = softlight(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Difference(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte difference(byte b, byte s)
        {
            return (byte)Math.Abs(b - s);
        }

        byte r = difference(backdrop.R, source.R);
        byte g = difference(backdrop.G, source.G);
        byte b = difference(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Exclusion(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte exclusion(byte b, byte s)
        {
            return (byte)(b + s - 2 * Combine32.MUL_UN8(b, s));
        }

        byte r = exclusion(backdrop.R, source.R);
        byte g = exclusion(backdrop.G, source.G);
        byte b = exclusion(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void HslHue(ref Pixel backdrop, in Pixel source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double s = Sat(r, g, b);
        double l = Lum(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        SetSat(ref r, ref g, ref b, s);
        SetLum(ref r, ref g, ref b, l);

        Pixel src = new Pixel((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void HslSaturation(ref Pixel backdrop, in Pixel source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double s = Sat(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;
        double l = Lum(r, g, b);

        SetSat(ref r, ref g, ref b, s);
        SetLum(ref r, ref g, ref b, l);

        Pixel src = new Pixel((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void HslColor(ref Pixel backdrop, in Pixel source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double l = Lum(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        SetLum(ref r, ref g, ref b, l);

        Pixel src = new Pixel((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void HslLuminosity(ref Pixel backdrop, in Pixel source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double l = Lum(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;

        SetLum(ref r, ref g, ref b, l);

        Pixel src = new Pixel((byte)(r * 255), (byte)(g * 255), (byte)(b * 255), source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Addition(ref Pixel backdrop, in Pixel source, int opacity)
    {
        int r = backdrop.R + source.R;
        int g = backdrop.G + source.G;
        int b = backdrop.B + source.B;
        Pixel src = new Pixel((byte)Math.Min(r, 255), (byte)Math.Min(g, 255), (byte)Math.Min(b, 255), source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Subtract(ref Pixel backdrop, in Pixel source, int opacity)
    {
        int r = backdrop.R - source.R;
        int g = backdrop.G - source.G;
        int b = backdrop.B - source.B;
        Pixel src = new Pixel((byte)Math.Max(r, 0), (byte)Math.Max(g, 0), (byte)Math.Max(b, 0), source.A);
        Normal(ref backdrop, in src, opacity);
    }

    private static void Divide(ref Pixel backdrop, in Pixel source, int opacity)
    {
        byte divide(byte b, byte s)
        {
            if (b == 0)
            {
                return 0;
            }
            else if (b >= s)
            {
                return 255;
            }
            else
            {
                return Combine32.DIV_UN8(b, s);
            }
        }
        byte r = divide(backdrop.R, source.R);
        byte g = divide(backdrop.G, source.G);
        byte b = divide(backdrop.B, source.B);
        Pixel src = new Pixel(r, g, b, source.A);
        Normal(ref backdrop, in src, opacity);
    }
}
