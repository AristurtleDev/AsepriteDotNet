/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
namespace AsepriteDotNet.Common;

public struct Color
{
    internal const byte RGBA_R_SHIFT = 0;
    internal const byte RGBA_G_SHIFT = 8;
    internal const byte RGBA_B_SHIFT = 16;
    internal const byte RGBA_A_SHIFT = 24;
    internal const uint RGBA_R_MASK = 0x000000ff;
    internal const uint RGBA_G_MASK = 0x0000ff00;
    internal const uint RGBA_B_MASK = 0x00ff0000;
    internal const uint RGBA_RGB_MASK = 0x00ffffff;
    internal const uint RGBA_A_MASK = 0xff000000;
    internal const byte ONE_HALF = 0x80;
    internal const byte MASK = 0xFF;

    private uint _value;

    public byte R => unchecked((byte)((_value >> RGBA_R_SHIFT) & 0xFF));
    public byte G => unchecked((byte)((_value >> RGBA_G_SHIFT) & 0xFF));
    public byte B => unchecked((byte)((_value >> RGBA_B_SHIFT) & 0xFF));
    public byte A => unchecked((byte)((_value >> RGBA_A_SHIFT) & 0xFF));

    private Color(uint value)
    {
        _value = value;
    }

    public static Color FromRGBA(int red, int green, int blue, int alpha)
    {
        CheckByte(red, nameof(red));
        CheckByte(green, nameof(green));
        CheckByte(blue, nameof(blue));
        CheckByte(alpha, nameof(alpha));
        return FromRGBA(RGBA(red, green, blue, alpha));
    }

    private (int, int, int, int) GetRGBAValues() => (R, G, B, A);

    public static Color FromRGBA(uint argb) => new Color(argb);

    public static Color Blend(BlendMode mode, Color backdrop, Color source, int opacity) => mode switch
    {
        #pragma warning disable format
        BlendMode.Normal     => Normal(backdrop, source, opacity),
        BlendMode.Multiply   => Multiply(backdrop, source, opacity),
        BlendMode.Screen     => Screen(backdrop, source, opacity),
        BlendMode.Overlay    => Overlay(backdrop, source, opacity),
        BlendMode.Darken     => Darken(backdrop, source, opacity),
        BlendMode.Lighten    => Lighten(backdrop, source, opacity),
        BlendMode.ColorDodge => ColorDodge(backdrop, source, opacity),
        BlendMode.ColorBurn  => ColorBurn(backdrop, source, opacity),
        BlendMode.HardLight  => HardLight(backdrop, source, opacity),
        BlendMode.SoftLight  => SoftLight(backdrop, source, opacity),
        BlendMode.Difference => Difference(backdrop, source, opacity),
        BlendMode.Exclusion  => Exclusion(backdrop, source, opacity),
        BlendMode.Hue        => HslHue(backdrop, source, opacity),
        BlendMode.Saturation => HslSaturation(backdrop, source, opacity),
        BlendMode.Color      => HslColor(backdrop, source, opacity),
        BlendMode.Luminosity => HslLuminosity(backdrop, source, opacity),
        BlendMode.Addition   => Addition(backdrop, source, opacity),
        BlendMode.Subtract   => Subtract(backdrop, source, opacity),
        BlendMode.Divide     => Divide(backdrop, source, opacity),
        _                    => throw new InvalidOperationException($"Unknown blend mode '{mode}'")
        #pragma warning restore format
    };

    private static void CheckByte(int value, string name)
    {
        if (unchecked((uint)value > byte.MaxValue))
        {
            throw new ArgumentException(name, $"{name} must be a value from 0 to 255");
        }
    }


    private static uint RGBA(int r, int g, int b, int a) => (uint)r << RGBA_R_SHIFT |
                                                            (uint)g << RGBA_G_SHIFT |
                                                            (uint)b << RGBA_B_SHIFT |
                                                            (uint)a << RGBA_A_SHIFT;
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

    private static byte MUL_UN8(int a, int b)
    {
        int t = (a * b) + 0x80;
        return (byte)(((t >> 8) + t) >> 8);
    }

    private static byte DIV_UN8(int a, int b)
    {
        return (byte)(((ushort)a * 0xFF + (b / 2)) / b);
    }

    private static Color Normal(Color backdrop, Color src, int opacity)
    {
        if ((backdrop._value & RGBA_A_MASK) == 0)
        {
            int a = src.A;
            a = MUL_UN8(a, opacity);
            a <<= RGBA_A_SHIFT;
            return new Color((uint)((src._value & RGBA_RGB_MASK) | (uint)a));
        }
        else if ((src._value & RGBA_A_MASK) == 0)
        {
            return backdrop;
        }

        int Br = backdrop.R;
        int Bg = backdrop.G;
        int Bb = backdrop.B;
        int Ba = backdrop.A;

        int Sr = src.R;
        int Sg = src.G;
        int Sb = src.B;
        int Sa = src.A;
        Sa = MUL_UN8(Sa, opacity);


        int Ra = Sa + Ba - MUL_UN8(Ba, Sa);

        int Rr = Br + (Sr - Br) * Sa / Ra;
        int Rg = Bg + (Sg - Bg) * Sa / Ra;
        int Rb = Bb + (Sb - Bb) * Sa / Ra;

        return Color.FromRGBA(Rr, Rg, Rb, Ra);
    }

    private static Color Multiply(Color backdrop, Color source, int opacity)
    {
        int r = MUL_UN8(backdrop.R, source.R);
        int g = MUL_UN8(backdrop.G, source.G);
        int b = MUL_UN8(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Screen(Color backdrop, Color source, int opacity)
    {
        int r = backdrop.R + source.R - MUL_UN8(backdrop.R, source.R);
        int g = backdrop.G + source.G - MUL_UN8(backdrop.G, source.G);
        int b = backdrop.B + source.B - MUL_UN8(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Overlay(Color backdrop, Color source, int opacity)
    {
        int overlay(int b, int s)
        {
            if (b < 128)
            {
                return MUL_UN8(s, b << 1);
            }
            else
            {
                return s + b - MUL_UN8(s, b << 1);
            }
        }

        int r = overlay(backdrop.R, source.R);
        int g = overlay(backdrop.G, source.G);
        int b = overlay(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Darken(Color backdrop, Color source, int opacity)
    {
        int blend(int b, int s) => Math.Min(b, s);

        int r = blend(backdrop.R, source.R);
        int g = blend(backdrop.G, source.G);
        int b = blend(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Lighten(Color backdrop, Color source, int opacity)
    {
        int lighten(int b, int s) => Math.Max(b, s);

        int r = lighten(backdrop.R, source.R);
        int g = lighten(backdrop.G, source.G);
        int b = lighten(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color ColorDodge(Color backdrop, Color source, int opacity)
    {
        int dodge(int b, int s)
        {
            if (b == 0)
            {
                return 0;
            }

            s = 255 - s;

            if (b >= s)
            {
                return 255;
            }
            else
            {
                return DIV_UN8(b, s);
            }
        }

        int r = dodge(backdrop.R, source.R);
        int g = dodge(backdrop.G, source.G);
        int b = dodge(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color ColorBurn(Color backdrop, Color source, int opacity)
    {
        int burn(int b, int s)
        {
            if (b == 255)
            {
                return 255;
            }

            b = (255 - b);

            if (b >= s)
            {
                return 0;
            }
            else
            {
                return 255 - DIV_UN8(b, s);
            }
        }

        int r = burn(backdrop.R, source.R);
        int g = burn(backdrop.G, source.G);
        int b = burn(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color HardLight(Color backdrop, Color source, int opacity)
    {
        int hardlight(int b, int s)
        {
            if (s < 128)
            {
                return MUL_UN8(b, s << 1);
            }
            else
            {
                return b + (s << 1) - MUL_UN8(b, s << 1);
            }
        }

        int r = hardlight(backdrop.R, source.R);
        int g = hardlight(backdrop.G, source.G);
        int b = hardlight(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color SoftLight(Color backdrop, Color source, int opacity)
    {
        int softlight(int _b, int _s)
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

        int r = softlight(backdrop.R, source.R);
        int g = softlight(backdrop.G, source.G);
        int b = softlight(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Difference(Color backdrop, Color source, int opacity)
    {
        int difference(int b, int s)
        {
            return Math.Abs(b - s);
        }

        int r = difference(backdrop.R, source.R);
        int g = difference(backdrop.G, source.G);
        int b = difference(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Exclusion(Color backdrop, Color source, int opacity)
    {
        int exclusion(int b, int s)
        {
            return b + s - 2 * MUL_UN8(b, s);
        }

        int r = exclusion(backdrop.R, source.R);
        int g = exclusion(backdrop.G, source.G);
        int b = exclusion(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color HslHue(Color backdrop, Color source, int opacity)
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

        uint rgba = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color HslSaturation(Color backdrop, Color source, int opacity)
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

        uint rgba = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color HslColor(Color backdrop, Color source, int opacity)
    {
        double r = backdrop.R / 255.0;
        double g = backdrop.G / 255.0;
        double b = backdrop.B / 255.0;
        double l = Lum(r, g, b);

        r = source.R / 255.0;
        g = source.G / 255.0;
        b = source.B / 255.0;

        SetLum(ref r, ref g, ref b, l);

        uint rgba = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color HslLuminosity(Color backdrop, Color source, int opacity)
    {
        double r = source.R / 255.0;
        double g = source.G / 255.0;
        double b = source.B / 255.0;
        double l = Lum(r, g, b);

        r = backdrop.R / 255.0;
        g = backdrop.G / 255.0;
        b = backdrop.B / 255.0;

        SetLum(ref r, ref g, ref b, l);

        uint rgba = RGBA((int)(255.0 * r), (int)(255.0 * g), (int)(255.0 * b), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Addition(Color backdrop, Color source, int opacity)
    {
        int r = backdrop.R + source.R;
        int g = backdrop.G + source.G;
        int b = backdrop.B + source.B;
        uint rgba = RGBA(Math.Min(r, 255),
                         Math.Min(g, 255),
                         Math.Min(b, 255), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Subtract(Color backdrop, Color source, int opacity)
    {
        int r = backdrop.R - source.R;
        int g = backdrop.G - source.G;
        int b = backdrop.B - source.B;
        uint rgba = RGBA(Math.Max(r, 0), Math.Max(g, 0), Math.Max(b, 0), 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }

    private static Color Divide(Color backdrop, Color source, int opacity)
    {
        int divide(int b, int s)
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
                return DIV_UN8(b, s);
            }
        }
        int r = divide(backdrop.R, source.R);
        int g = divide(backdrop.G, source.G);
        int b = divide(backdrop.B, source.B);
        uint rgba = RGBA(r, g, b, 0) | (source._value & RGBA_A_MASK);
        return Normal(backdrop, new Color(rgba), opacity);
    }
}