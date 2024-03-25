// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace AsepriteDotNet.Common;

/// <summary>
/// Represents a color value defined by four 8-bit component values, each representing the red, green, blue, and alpha
/// component values of the color.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Rgba32 : IEquatable<Rgba32>
{
    private static readonly Vector4 s_maxBytes = Vector128.Create(255.0f).AsVector4();
    private static readonly Vector4 s_half = Vector128.Create(0.5f).AsVector4();
    internal const int StructSize = sizeof(uint);

    /// <summary>
    /// The red color component.
    /// </summary>
    public byte R;

    /// <summary>
    /// The green color component.
    /// </summary>
    public byte G;

    /// <summary>
    /// The blue color component.
    /// </summary>
    public byte B;

    /// <summary>
    /// The alpha color component.
    /// </summary>
    public byte A;

    /// <summary>
    /// Gets or Sets the packed value of this <see cref="Rgba32"/> represented as a 32-bit unsigned integer in the
    /// least to most significant bit order of red, green, blue, alpha.
    /// </summary>
    public uint PackedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => Unsafe.As<Rgba32, uint>(ref Unsafe.AsRef(in this));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Unsafe.As<Rgba32, uint>(ref this) = value;
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="Rgba32"/> value.
    /// </summary>
    /// <param name="r">The red color component.</param>
    /// <param name="g">The green color component.</param>
    /// <param name="b">The blue color component.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgba32(byte r, byte g, byte b) => (R, G, B, A) = (r, g, b, byte.MaxValue);

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgba32"/> value.
    /// </summary>
    /// <param name="r">The red color component.</param>
    /// <param name="g">The green color component.</param>
    /// <param name="b">The blue color component.</param>
    /// <param name="a">The alpha color component.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgba32(byte r, byte g, byte b, byte a) => (R, G, B, A) = (r, g, b, a);

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgba32"/> value from a <see cref="Vector4"/>
    /// </summary>
    /// <param name="vector">The vector containing the red, green, blue, and alpha components.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgba32(Vector4 vector) : this() => this = Pack(vector);

    /// <summary>
    /// Returns this <see cref="Rgba32"/> value as a <see cref="Vector4"/> value.
    /// </summary>
    /// <returns>This <see cref="Rgba32"/> value as a <see cref="Vector4"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector4 ToVector4() => new Vector4(R, G, B, A) / s_maxBytes;

    /// <summary>
    /// Returns  this <see cref="Rgba32"/> value as a <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">The type to convert this <see cref="Rgba32"/> to.</typeparam>
    /// <param name="converter">A function that defines how to perform the conversion.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly T To<T>(Func<Rgba32, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        return converter(this);
    }

    /// <summary>
    /// Returns a value that indicates if two <see cref="Rgba32"/> values are equal.
    /// </summary>
    /// <param name="left">The <see cref="Rgba32"/> value on the left of the equality operator.</param>
    /// <param name="right">The <see cref="Rgba32"/> value on the right of the equality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rgba32 left, Rgba32 right) => left.Equals(right);

    /// <summary>
    /// Returns a value that indicates if two <see cref="Rgba32"/> values are not equal.
    /// </summary>
    /// <param name="left">The <see cref="Rgba32"/> value on the left of the inequality operator.</param>
    /// <param name="right">The <see cref="Rgba32"/> value on the right of the inequality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rgba32 left, Rgba32 right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Rgba32 other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rgba32 other) => PackedValue.Equals(other.PackedValue);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{nameof(Rgba32)}: ({R}, {G}, {B}, {A})";

    /// <inheritdoc/>
    public override readonly int GetHashCode() => PackedValue.GetHashCode();

    /// <summary>
    /// Packs a <see cref="Vector4"/> value into a new <see cref="Rgba32"/> value.
    /// </summary>
    /// <param name="vector">The vector to pack.</param>
    /// <returns>The <see cref="Rgba32"/> value created by this method.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rgba32 Pack(Vector4 vector)
    {
        vector *= s_maxBytes;
        vector += s_half;
        vector = Vector4.Min(Vector4.Max(vector, Vector4.Zero), s_maxBytes);
        Vector128<byte> result = Vector128.ConvertToInt32(vector.AsVector128()).AsByte();
        return new Rgba32(result.GetElement(0), result.GetElement(4), result.GetElement(8), result.GetElement(12));
    }

    /// <summary>
    /// Deconstructs this <see cref="Rgba32"/> value into its individual R, G, B, A component values.
    /// </summary>
    /// <param name="r">When this method returns, contains the <see cref="R"/> color component value.</param>
    /// <param name="g">When this method returns, contains the <see cref="G"/> color component value.</param>
    /// <param name="b">When this method returns, contains the <see cref="B"/> color component value.</param>
    /// <param name="a">When this method returns, contains the <see cref="A"/> color component value.</param>
    public readonly void Deconstruct(out byte r, out byte g, out byte b, out byte a)
    {
        r = R;
        g = G;
        b = B;
        a = A;
    }
}
