// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Compression;

namespace AsepriteDotNet;

internal static class AsepriteColorUtilitiesVectors
{
    /// <summary>
    /// Converts an array of <see cref="byte"/> data to an array of <see cref="Rgba32"/> values based on the
    /// specified <see cref="AsepriteColorDepth"/>.
    /// </summary>
    /// <param name="pixels">The array of <see cref="byte"/> data that represents the color data.</param>
    /// <param name="depth">The color depth.</param>
    /// <param name="preMultiplyAlpha">
    /// Indicates whether color values should be translated to a premultiplied alpha value.
    /// </param>
    /// <param name="palette">
    /// The palette used for <see cref="AsepriteColorDepth.Indexed">ColorDepth.Index</see>.  Optional, only required when
    /// <paramref name="depth"/> is equal to <see cref="AsepriteColorDepth.Indexed">ColorDepth.Indexed</see>.
    /// </param>
    /// <returns>An array of <see cref="Rgba32"/> values converted from the data.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="pixels"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="depth"/> is an unknown <see cref="AsepriteColorDepth"/> value.
    /// </exception>
    internal static Rgba32[] PixelsToColor(byte[] pixels, AsepriteColorDepth depth, bool preMultiplyAlpha, AsepritePalette? palette = null)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        int bpp = (int)depth / 8;
        Rgba32[] result = new Rgba32[pixels.Length / bpp];

        for (int i = 0, b = 0; i < result.Length; i++, b += bpp)
        {
            byte red, green, blue, alpha;
            red = green = blue = alpha = 0;

            switch (depth)
            {
                case AsepriteColorDepth.RGBA:
                    red = pixels[b];
                    green = pixels[b + 1];
                    blue = pixels[b + 2];
                    alpha = pixels[b + 3];
                    break;

                case AsepriteColorDepth.Grayscale:
                    red = green = blue = pixels[b];
                    alpha = pixels[b + 1];
                    break;

                case AsepriteColorDepth.Indexed:
                    int index = pixels[i];
                    if (index != palette?.TransparentIndex)
                    {
                        if(palette is not null)
                        {
                            palette.Colors[index].Deconstruct(out red, out green, out blue, out alpha);
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException($"Unknown Color Depth: {depth}");
            }
            result[i] = preMultiplyAlpha ? Rgba32.FromNonPreMultiplied(red, green, blue, alpha) : new Rgba32(red, green, blue, alpha);
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
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
    internal static Vector4[] Blend(Vector4[] backdrop, Vector4[] source, int opacity, AsepriteBlendMode blendMode)
    {
        //  The original method performs alpha check on backdrop and source and does an early exit
        //  based on that.  Not sure how you would implement that here?
        //  See line 200 in AsepriteColorUtilities.cs

        Vector4[] blended = blendMode switch
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

    private static Vector4[] Normal(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Multiply(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Screen(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Overlay(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Darken(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Lighten(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] ColorDodge(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] ColorBurn(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] HardLight(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] SoftLight(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Difference(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Exclusion(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] HslHue(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] HslSaturation(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] HslColor(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] HslLuminosity(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Addition(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Subtract(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }

    private static Vector4[] Divide(Vector4[] backdrop, Vector4[] source, int opacity)
    {
        throw new NotImplementedException();
    }
}
