// Copyright[MethodImpl(MethodImplOptions.AggressiveInlining)] (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Common;

/// <summary>
/// Defines four 32-bit signed integer values that represent the location and size of a rectangular boundary in a two
/// dimensional plane.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Rectangle : IEquatable<Rectangle>
{
    /// <summary>
    /// A <see cref="Rectangle"/> who's <see cref="Rectangle.X"/>, <see cref="Rectangle.Y"/>,
    /// <see cref="Rectangle.Width"/> and <see cref="Rectangle.Height"/> component values all zero.
    /// </summary>
    public static readonly Rectangle Empty;

    /// <summary>
    /// The top-left x-coordinate position of this rectangle.
    /// </summary>
    public int X;

    /// <summary>
    /// The top-left y-coordinate position of this rectangle.
    /// </summary>
    public int Y;

    /// <summary>
    /// The width of this rectangle, in pixels.
    /// </summary>
    public int Width;

    /// <summary>
    /// The height of this rectangle, in pixels.
    /// </summary>
    public int Height;

    /// <summary>
    /// Gets a value that indicates if this is an empty point.
    /// </summary>
    public readonly bool IsEmpty => Equals(Empty);

    /// <summary>
    /// Gets or Sets the top-left xy-coordinate position of this rectangle.
    /// </summary>
    public Point Location
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => new Point(X, Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (X, Y) = (value.X, value.Y);
    }

    /// <summary>
    /// Gets or SEts the size of this rectangle.
    /// </summary>
    public Size Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly get => new Size(Width, Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => (Width, Height) = (value.Width, value.Height);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> value.
    /// </summary>
    /// <param name="x">The top-left x-coordinate position of the rectangle.</param>
    /// <param name="y">The top-left y-coordinate position of the rectangle.</param>
    /// <param name="width">The width of the rectangle, in pixels.</param>
    /// <param name="height">The height of the rectangle, in pixels.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle(int x, int y, int width, int height) => (X, Y, Width, Height) = (x, y, width, height);

    /// <summary>
    /// Initializes a new instance of the <see cref="Rectangle"/> value.
    /// </summary>
    /// <param name="location">The top-left xy-coordinate location of the rectangle.</param>
    /// <param name="size">The size of the rectangle, in pixels.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rectangle(Point location, Size size) => (X, Y, Width, Height) = (location.X, location.Y, size.Width, size.Height);

    /// <summary>
    /// Returns this <see cref="Rectangle"/> value as a <see cref="Vector4"/> value.
    /// </summary>
    /// <returns>This <see cref="Rectangle"/> value as a <see cref="Vector4"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector4 ToVector4() => new Vector4(X, Y, Width, Height);

    /// <summary>
    /// Returns  this <see cref="Rectangle"/> value as a <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">The type to convert this <see cref="Rectangle"/> to.</typeparam>
    /// <param name="converter">A function that defines how to perform the conversion.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly T To<T>(Func<Rectangle, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        return converter(this);
    }

    /// <summary>
    /// Returns a value that indicates if two <see cref="Rectangle"/> values are equal.
    /// </summary>
    /// <param name="left">The <see cref="Rectangle"/> value on the left of the equality operator.</param>
    /// <param name="right">The <see cref="Rectangle"/> value on the right of the equality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rectangle left, Rectangle right) => left.Equals(right);

    /// <summary>
    /// Returns a value that indicates if two <see cref="Rectangle"/> values are not equal.
    /// </summary>
    /// <param name="left">The <see cref="Rectangle"/> value on the left of the inequality operator.</param>
    /// <param name="right">The <see cref="Rectangle"/> value on the right of the inequality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rectangle left, Rectangle right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Rectangle other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rectangle other) => X.Equals(other.X)
                                                    && Y.Equals(other.Y)
                                                    && Width.Equals(other.Width)
                                                    && Height.Equals(other.Height);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{nameof(Rectangle)}: ({X}, {Y}, {Width}, {Height})";
}
