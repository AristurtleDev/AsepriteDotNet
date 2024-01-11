using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AsepriteDotNet;

/// <summary>
///     Represents a pixel defined by a red, green, blue, and alpha component of 1-byte each ranging from 0 to 255.
///     The color components are stored in red, green, blue, alpha order (least to most significant bit).
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct Pixel : IEquatable<Pixel>
{
    /// <summary>
    ///     The red color component of this <see cref="Pixel"/>
    /// </summary>
    [FieldOffset(0)]
    public byte R = 0x00;

    /// <summary>
    ///     The green color component of this <see cref="Pixel"/>
    /// </summary>
    [FieldOffset(1)]
    public byte G = 0x00;

    /// <summary>
    ///     The blue color component of this <see cref="Pixel"/>
    /// </summary>
    [FieldOffset(2)]
    public byte B = 0x00;

    /// <summary>
    ///     The alpha color component of this <see cref="Pixel"/>
    /// </summary>
    [FieldOffset(3)]
    public byte A = byte.MaxValue;

    /// <summary>
    ///     The packed 32-bit unsigned integer value that represents the R, G, B, A color components of this
    ///     <see cref="Pixel"/>.
    /// </summary>
    [FieldOffset(0)]
    public uint Rgba = 0xFF000000;

    /// <summary>
    ///     Creates a new <see cref="Pixel"/> value.
    /// </summary>
    /// <param name="rgba">
    ///     A 32-bit unsigned integer value that represents the packed R, G, B, A component values.
    /// </param>
    public Pixel(uint rgba) => Rgba = rgba;

    /// <summary>
    ///     Creates a new <see cref="Pixel"/> value.
    /// </summary>
    /// <param name="r">
    ///     The red color component.
    /// </param>
    /// <param name="g">
    ///     The green color component.
    /// </param>
    /// <param name="b">
    ///     The blue color component.
    /// </param>
    public Pixel(byte r,  byte g, byte b) => (R, G, B, A) = (r, g, b, byte.MaxValue);

    /// <summary>
    ///     Creates a new <see cref="Pixel"/> value.
    /// </summary>
    /// <param name="r">
    ///     The red color component.
    /// </param>
    /// <param name="g">
    ///     The green color component.
    /// </param>
    /// <param name="b">
    ///     The blue color component.
    /// </param>
    /// <param name="a">
    ///     The alpha color component.
    /// </param>
    public Pixel(byte r, byte g, byte b, byte a) => (R, G, B, A) = (r, g, b, a);

    /// <inheritdoc />
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Pixel other && Equals(other);

    /// <inheritdoc />
    public readonly bool Equals(Pixel other) => Rgba == other.Rgba;

    /// <inheritdoc/>
    public override readonly int GetHashCode() => Rgba.GetHashCode();

    /// <summary>
    ///     Returns <see langword="true"/> if the two <see cref="Pixel"/> values are equal; otherwise,
    ///     <see langword="false"/>.
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Pixel"/> value on the left side of the equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Pixel"/> value on the right side of the equality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Pixel"/> value are not equal; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Pixel left, Pixel right)
    {
        return left.Equals(right);
    }

    /// <summary>
    ///     Returns <see langword="true"/> if the two <see cref="Pixel"/> values are not equal; otherwise,
    ///     <see langword="false"/>
    /// </summary>
    /// <param name="left">
    ///     The <see cref="Pixel"/> value on the left side of the inequality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="Pixel"/> value on the right side of the inequality operator.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the two <see cref="Pixel"/> value are not equal; otherwise,
    ///     <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Pixel left, Pixel right)
    {
        return !(left == right);
    }
}
