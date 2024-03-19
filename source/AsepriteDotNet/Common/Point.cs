// Copyright[MethodImpl(MethodImplOptions.AggressiveInlining)] (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Common;

/// <summary>
/// Defines an order pair of 32-bit signed integer values that represent the x- and y-coordinates in a two dimensional
/// plane.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Point : IEquatable<Point>
{
    /// <summary>
    /// A <see cref="Point"/> who's <see cref="Point.X"/> and <see cref="Point.Y"/> component values are both zero.
    /// </summary>
    public static readonly Point Empty;

    /// <summary>
    /// The x-coordinate component of this point.
    /// </summary>
    public int X;

    /// <summary>
    /// The y-coordinate component of this point.
    /// </summary>
    public int Y;

    /// <summary>
    /// Gets a value that indicates if this is an empty point.
    /// </summary>
    public readonly bool IsEmpty => Equals(Empty);

    /// <summary>
    /// Initializes a new instance of the <see cref="Point"/> value.
    /// </summary>
    /// <param name="x">The x-coordinate component of the point.</param>
    /// <param name="y">The y-coordinate component of the point.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Point(int x, int y) => (X, Y) = (x, y);

    /// <summary>
    /// Returns this <see cref="Point"/> value as a <see cref="Vector2"/> value.
    /// </summary>
    /// <returns>This <see cref="Point"/> value as a <see cref="Vector2"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector2 ToVector2() => new Vector2(X, Y);


    /// <summary>
    /// Returns  this <see cref="Point"/> value as a <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">The type to convert this <see cref="Point"/> to.</typeparam>
    /// <param name="converter">A function that defines how to perform the conversion.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly T To<T>(Func<Point, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        return converter(this);
    }

    /// <summary>
    /// Returns a value that indicates if two <see cref="Point"/> values are equal.
    /// </summary>
    /// <param name="left">The <see cref="Point"/> value on the left of the equality operator.</param>
    /// <param name="right">The <see cref="Point"/> value on the right of the equality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Point left, Point right) => left.Equals(right);

    /// <summary>
    /// Returns a value that indicates if two <see cref="Point"/> values are not equal.
    /// </summary>
    /// <param name="left">The <see cref="Point"/> value on the left of the inequality operator.</param>
    /// <param name="right">The <see cref="Point"/> value on the right of the inequality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Point left, Point right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Point other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Point other) => X.Equals(other.X) && Y.Equals(other.Y);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{nameof(Point)}: ({X}, {Y})";
}
