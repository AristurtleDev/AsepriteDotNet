// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Common;

/// <summary>
/// Represents a color that uses <see cref="System.Drawing.Color"/> as the base color type.
/// </summary>

[StructLayout(LayoutKind.Sequential)]
public struct SystemColor : IColor<Color>, IEquatable<SystemColor>
{
    /// <inheritdoc/>
    public byte R { readonly get; set; }

    /// <inheritdoc/>
    public byte G { readonly get; set; }

    /// <inheritdoc/>
    public byte B { readonly get; set; }

    /// <inheritdoc/>
    public byte A { readonly get; set; }

    /// <inheritdoc/>
    public uint PackedValue
    {
        readonly get => Unsafe.As<SystemColor, uint>(ref Unsafe.AsRef(in this));
        set => Unsafe.As<SystemColor, uint>(ref this) = value;
    }

    /// <inheritdoc/>
    public readonly Color Value => Color.FromArgb(A, R, G, B);

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemColor"/> struct with the specified red, green, and blue
    /// component values, and an alpha value of 255 (fully opaque).
    /// </summary>
    /// <param name="r">The red component value.</param>
    /// <param name="g">The green component value.</param>
    /// <param name="b">The blue component value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SystemColor(byte r, byte g, byte b) => (R, G, B, A) = (r, g, b, byte.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemColor"/> struct with the specified red, green, blue, and
    /// alpha component values.
    /// </summary>
    /// <param name="r">The red component value.</param>
    /// <param name="g">The green component value.</param>
    /// <param name="b">The blue component value.</param>
    /// <param name="a">The alpha component value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SystemColor(byte r, byte g, byte b, byte a) => (R, G, B, A) = (r, g, b, a);

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemColor"/> struct with the color components from the specified
    /// <see cref="Vector4"/> instance.
    /// </summary>
    /// <param name="vector">The <see cref="Vector4"/> instance containing the color components.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SystemColor(Vector4 vector) : this() => FromVector4(vector);

    /// <inheritdoc/>
    public void FromVector4(Vector4 vector)
    {
        byte[] rgba = Calc.UnpackColor(vector);
        R = rgba[0];
        G = rgba[1];
        B = rgba[2];
        A = rgba[3];
    }


    /// <inheritdoc/>
    public readonly TIn To<TIn>() where TIn : IColor, new()
    {
        TIn result = new();
        result.FromVector4(ToVector4());
        return result;
    }

    /// <inheritdoc/>
    public readonly Vector4 ToVector4() => Calc.PackColor(R, G, B, A);

    /// <summary>
    /// Compares two <see cref="SystemColor"/> instances for equality.
    /// </summary>
    /// <param name="left">The first <see cref="SystemColor"/> instance to compare.</param>
    /// <param name="right">The second <see cref="SystemColor"/> instance to compare.</param>
    /// <returns><see langword="true"/> if the two instances are equal; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(SystemColor left, SystemColor right) => left.Equals(right);

    /// <summary>
    /// Compares two <see cref="SystemColor"/> instances for inequality.
    /// </summary>
    /// <param name="left">The first <see cref="SystemColor"/> instance to compare.</param>
    /// <param name="right">The second <see cref="SystemColor"/> instance to compare.</param>
    /// <returns>
    /// <see langword="true"/> if the two instances are not equal; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(SystemColor left, SystemColor right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is SystemColor other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(SystemColor other) => PackedValue.Equals(other.PackedValue);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{nameof(SystemColor)}: ({R}, {G}, {B}, {A})";

    /// <inheritdoc/>
    public override readonly int GetHashCode() => PackedValue.GetHashCode();

    /// <summary>
    /// Compares the current <see cref="SystemColor"/> instance with a specified <see cref="Color"/> instance for
    /// equality.
    /// </summary>
    /// <param name="other">The <see cref="Color"/> instance to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true"/> if the current instance is equal to the specified <see cref="Color"/> instance;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public readonly bool Equals(Color other)
    {
        return other.Equals(Value);
    }

    public static Color[] Convert(SystemColor[] values)
    {
        ArgumentNullException.ThrowIfNull(values);

        Color[] result = new Color[values.Length];
        Parallel.For(0, values.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i) =>
        {
            result[i] = values[i].Value;
        });
        return result;
    }
}
