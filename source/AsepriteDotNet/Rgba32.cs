//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

using System.Runtime.InteropServices;

/// <summary>
/// Represents a color value defined by four 8-bit component values, each representing the red, green, blue, and alpha
/// component values of the color.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Rgba32 : IEquatable<Rgba32>
{
    /// <summary>
    /// The red color component of this <see cref="Rgba32"/>.
    /// </summary>
    [FieldOffset(0)]
    public byte R = 0x00;

    /// <summary>
    /// The green color component of this <see cref="Rgba32"/>.
    /// </summary>
    [FieldOffset(1)]
    public byte G = 0x00;

    /// <summary>
    /// The blue color component of this <see cref="Rgba32"/>.
    /// </summary>
    [FieldOffset(2)]
    public byte B = 0x00;

    /// <summary>
    /// The alpha component of this <see cref="Rgba32"/>.
    /// </summary>
    [FieldOffset(3)]
    public byte A = byte.MaxValue;

    /// <summary>
    /// The packed value of this <see cref="Rgba32"/> represented as a 32-bit unsigned integer in least to most
    /// significant bit order of red, green, blue, alpha.
    /// blue, and alpha
    /// </summary>
    [FieldOffset(0)]
    public uint PackedValue = 0xFF000000;

    /// <summary>
    /// Creates a new <see cref="Rgba32"/> value.
    /// </summary>
    /// <param name="value">
    /// A 32-bit unsigned integer that represents the packed R, G, B, A component values of the <see cref="Rgba32"/> in
    /// least to most significant bit order of red, green, blue, alpha.
    /// </param>
    public Rgba32(uint value) => PackedValue = value;

    /// <summary>
    /// Creates a new <see cref="Rgba32"/> value.
    /// </summary>
    /// <param name="r">The red color component of the <see cref="Rgba32"/>.</param>
    /// <param name="g">The green color component of the <see cref="Rgba32"/>.</param>
    /// <param name="b">The blue color component of the <see cref="Rgba32"/>.</param>
    public Rgba32(byte r, byte g, byte b) => (R, G, B, A) = (r, g, b, byte.MaxValue);

    /// <summary>
    /// Creates a new <see cref="Rgba32"/> value.
    /// </summary>
    /// <param name="r">The red color component of the <see cref="Rgba32"/>.</param>
    /// <param name="g">The green color component of the <see cref="Rgba32"/>.</param>
    /// <param name="b">The blue color component of the <see cref="Rgba32"/>.</param>
    /// <param name="a">The alpha component of the <see cref="Rgba32"/>.</param>
    public Rgba32(byte r, byte g, byte b, byte a) => (R, G, B, A) = (r, g, b, a);

    /// <inheritdoc />
    public override readonly bool Equals(object obj) => obj is Rgba32 other && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(Rgba32 other) => PackedValue == other.PackedValue;

    /// <inheritdoc/>
    public override readonly int GetHashCode() => PackedValue.GetHashCode();

    /// <summary>
    /// Returns a value that indicates whether two <see cref="Rgba32"/> value are equal.
    /// </summary>
    /// <param name="left">The <see cref="Rgba32"/> value on the left side of the equality operator.</param>
    /// <param name="right">The <see cref="Rgba32"/> value on the right side of the equality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Rgba32 left, Rgba32 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Returns a value that indicates whether two <see cref="Rgba32"/> values are not equal.
    /// </summary>
    /// <param name="left">The <see cref="Rgba32"/> value on the left side of the inequality operator.</param>
    /// <param name="right">The <see cref="Rgba32"/> value on the right side of the inequality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Rgba32 left, Rgba32 right)
    {
        return !(left == right);
    }
}