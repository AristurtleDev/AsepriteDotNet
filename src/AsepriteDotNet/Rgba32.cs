// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace AsepriteDotNet;

/// <summary>
/// Represents a 32-bit RGBA color with 8 bits per channel in RGBA order.
/// </summary>
/// <remarks>
/// The color channels are stored in memory as R, G, B, A bytes sequentially.
/// Values are stored in non-premultiplied format unless explicitly created otherwise.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public struct Rgba32 : IEquatable<Rgba32>
{
    private static readonly Vector4 s_maxBytes = Vector128.Create(255.0f).AsVector4();
    private static readonly Vector4 s_half = Vector128.Create(0.5f).AsVector4();
    internal const int StructSize = sizeof(uint);

    /// <summary>
    /// The red color channel component.
    /// </summary>
    /// <value>A value from 0 to 255 representing the red intensity.</value>
    public byte R;

    /// <summary>
    /// The green color channel component.
    /// </summary>
    /// <value>A value from 0 to 255 representing the green intensity.</value>
    public byte G;

    /// <summary>
    /// The blue color channel component.
    /// </summary>
    /// <value>A value from 0 to 255 representing the blue intensity.</value>
    public byte B;

    /// <summary>
    /// The alpha transparency channel component.
    /// </summary>
    /// <value>A value from 0 (fully transparent) to 255 (fully opaque).</value>
    public byte A;

    /// <summary>
    /// Gets or sets the packed representation of the RGBA color as a 32-bit unsigned integer.
    /// </summary>
    /// <value>The RGBA components packed into a single 32-bit value in RGBA byte order.</value>
    public uint PackedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => Unsafe.As<Rgba32, uint>(ref Unsafe.AsRef(in this));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Unsafe.As<Rgba32, uint>(ref this) = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgba32"/> struct with full opacity.
    /// </summary>
    /// <param name="r">The red component value (0-255).</param>
    /// <param name="g">The green component value (0-255).</param>
    /// <param name="b">The blue component value (0-255).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgba32(byte r, byte g, byte b) => (R, G, B, A) = (r, g, b, byte.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgba32"/> struct.
    /// </summary>
    /// <param name="r">The red component value (0-255).</param>
    /// <param name="g">The green component value (0-255).</param>
    /// <param name="b">The blue component value (0-255).</param>
    /// <param name="a">The alpha component value (0-255).</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgba32(byte r, byte g, byte b, byte a) => (R, G, B, A) = (r, g, b, a);

#if NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of the <see cref="Rgba32"/> struct from a <see cref="Vector4"/>.
    /// </summary>
    /// <param name="vector">The vector containing normalized color values (0.0-1.0) in RGBA order.</param>
    /// <remarks>
    /// Vector components are clamped to [0.0, 1.0] range and converted to [0, 255] byte range.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgba32(Vector4 vector) : this() => this = Pack(vector);
#endif

    /// <summary>
    /// Converts the color to a <see cref="Vector4"/> with normalized component values.
    /// </summary>
    /// <returns>A <see cref="Vector4"/> with components in the range [0.0, 1.0] representing RGBA values.</returns>
    /// <remarks>
    /// Each byte component is divided by 255 to produce normalized floating-point values.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector4 ToVector4() => new Vector4(R, G, B, A) / s_maxBytes;

    /// <summary>
    /// Converts this color using the specified converter function.
    /// </summary>
    /// <typeparam name="T">The target type for conversion.</typeparam>
    /// <param name="converter">The function that converts from <see cref="Rgba32"/> to type <typeparamref name="T"/>.</param>
    /// <returns>The converted value of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="converter"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly T To<T>(Func<Rgba32, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        return converter(this);
    }

    /// <summary>
    /// Compares two <see cref="Rgba32"/> instances for equality.
    /// </summary>
    /// <param name="left">The first color to compare.</param>
    /// <param name="right">The second color to compare.</param>
    /// <returns><c>true</c> if the colors have identical RGBA component values; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rgba32 left, Rgba32 right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rgba32 left, Rgba32 right) => !left.Equals(right);

    /// <summary>
    /// Determines whether this color is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with this color.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is an <see cref="Rgba32"/> with identical component values; otherwise, <c>false</c>.</returns>
    public override readonly bool Equals([NotNullWhen(true)] object obj) => obj is Rgba32 other && Equals(other);

    /// <summary>
    /// Determines whether this color is equal to another <see cref="Rgba32"/>.
    /// </summary>
    /// <param name="other">The color to compare with this instance.</param>
    /// <returns><c>true</c> if the colors have identical RGBA component values; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rgba32 other) => PackedValue.Equals(other.PackedValue);

    /// <summary>
    /// Returns a string representation of this color.
    /// </summary>
    /// <returns>A string containing the type name and RGBA component values.</returns>
    public override readonly string ToString() => $"{nameof(Rgba32)}: ({R}, {G}, {B}, {A})";

    /// <summary>
    /// Returns a hash code for this color.
    /// </summary>
    /// <returns>A hash code derived from the packed RGBA value.</returns>
    public override readonly int GetHashCode() => PackedValue.GetHashCode();

#if NET8_0_OR_GREATER
    /// <summary>
    /// Creates an <see cref="Rgba32"/> color from a <see cref="Vector4"/> with normalized components.
    /// </summary>
    /// <param name="vector">The vector containing normalized color values (0.0-1.0) in RGBA order.</param>
    /// <returns>An <see cref="Rgba32"/> color with components converted to the [0, 255] byte range.</returns>
    /// <remarks>
    /// Vector components are multiplied by 255, rounded using 0.5 offset, and clamped to [0, 255] range.
    /// Uses SIMD operations for efficient conversion when available.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rgba32 Pack(Vector4 vector)
    {
        vector *= s_maxBytes;
        vector += s_half;
        vector = Vector4.Min(Vector4.Max(vector, Vector4.Zero), s_maxBytes);
        Vector128<byte> result = Vector128.ConvertToInt32(vector.AsVector128()).AsByte();
        return new Rgba32(result.GetElement(0), result.GetElement(4), result.GetElement(8), result.GetElement(12));
    }
#endif

    /// <summary>
    /// Creates a premultiplied RGBA color from non-premultiplied component values.
    /// </summary>
    /// <param name="r">The red component value (0-255).</param>
    /// <param name="g">The green component value (0-255).</param>
    /// <param name="b">The blue component value (0-255).</param>
    /// <param name="a">The alpha component value (0-255).</param>
    /// <returns>An <see cref="Rgba32"/> color with RGB components multiplied by the alpha ratio.</returns>
    /// <remarks>
    /// Each RGB component is multiplied by (alpha / 255) to produce premultiplied alpha values.
    /// This is commonly used in alpha blending operations for improved performance.
    /// </remarks>
    public static Rgba32 FromNonPreMultiplied(byte r, byte g, byte b, byte a)
    {
        return new Rgba32((byte)(r * a / 255), (byte)(g * a / 255), (byte)(b * a / 255), a);
    }

    /// <summary>
    /// Deconstructs this color into its individual RGBA component values.
    /// </summary>
    /// <param name="r">When this method returns, contains the red component value.</param>
    /// <param name="g">When this method returns, contains the green component value.</param>
    /// <param name="b">When this method returns, contains the blue component value.</param>
    /// <param name="a">When this method returns, contains the alpha component value.</param>
    public readonly void Deconstruct(out byte r, out byte g, out byte b, out byte a)
    {
        r = R;
        g = G;
        b = B;
        a = A;
    }
}
