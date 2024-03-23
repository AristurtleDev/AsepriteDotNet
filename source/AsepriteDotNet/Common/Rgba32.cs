// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Common;


/// <summary>
/// Represents an RGBA color value, where each component (red, green, blue, and alpha) is stored in a separate byte.
/// </summary>
/// <remarks>
/// The <see cref="Rgba32"/> struct provides properties to get and set the individual component values of the color,
/// as well as methods to convert the color to and from other representations, such as packed 32-bit values or
/// <see cref="Vector4"/> instances.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public struct Rgba32 : IEquatable<Rgba32>, IColor<Rgba32>
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
        readonly get => Unsafe.As<Rgba32, uint>(ref Unsafe.AsRef(in this));
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
    public Rgba32(Vector4 vector) : this() => FromVector4(vector);

    /// <inheritdoc/>
    public void FromVector4(Vector4 vector) => this = Calc.UnpackColor<Rgba32>(vector);
    

    /// <inheritdoc/>
    public TIn To<TIn>() where TIn : struct, IColor<TIn>
    {
        TIn result = default(TIn);
        result.FromVector4(ToVector4());
        return result;
    }

    /// <inheritdoc/>
    public Vector4 ToVector4() => Calc.PackColor(R, G, B, A);

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
}
