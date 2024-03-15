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
            //AsepriteBlendMode.ColorBurn     => ColorBurn(backdrop, source, opacity),
            //AsepriteBlendMode.HardLight     => HardLight(backdrop, source, opacity),
            //AsepriteBlendMode.SoftLight     => SoftLight(backdrop, source, opacity),
            //AsepriteBlendMode.Difference    => Difference(backdrop, source, opacity),
            //AsepriteBlendMode.Exclusion     => Exclusion(backdrop, source, opacity),
            //AsepriteBlendMode.Hue           => HslHue(backdrop, source, opacity),
            //AsepriteBlendMode.Saturation    => HslSaturation(backdrop, source, opacity),
            //AsepriteBlendMode.Color         => HslColor(backdrop, source, opacity),
            //AsepriteBlendMode.Luminosity    => HslLuminosity(backdrop, source, opacity),
            //AsepriteBlendMode.Addition      => Addition(backdrop, source, opacity),
            //AsepriteBlendMode.Subtract      => Subtract(backdrop, source, opacity),
            //AsepriteBlendMode.Divide        => Divide(backdrop, source, opacity),
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
            if(b < 128)
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
            if(b == 0) { return 0; }

            s = 255 - s;

            if (b >= s) { return 255; }

            return Unsigned8Bit.Divide(b, s);
        }

        source.R = (byte)dodge(backdrop.R, source.R);
        source.G = (byte)dodge(backdrop.G, source.G);
        source.B = (byte)dodge(backdrop.B, source.B);
        return Normal(backdrop, source, opacity);
    }






}
