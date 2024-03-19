// Copyright[MethodImpl(MethodImplOptions.AggressiveInlining)] (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Common;

/// <summary>
/// Defines an ordered pair of 32-bit signed integer values that represents the width and height of a two dimensional
/// object.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Size : IEquatable<Size>
{
    /// <summary>
    /// A <see cref="Size"/> who's <see cref="Size.Width"/> and <see cref="Size.Height"/> component values are both zero.
    /// </summary>
    public static readonly Size Empty;

    /// <summary>
    /// The width, in pixels.
    /// </summary>
    public int Width;

    /// <summary>
    /// The height, in pixels.
    /// </summary>
    public int Height;

    /// <summary>
    /// Gets a value that indicates if this is an empty size.
    /// </summary>
    public readonly bool IsEmpty => Equals(Empty);

    /// <summary>
    /// Initializes a new instance of the <see cref="Size"/> value.
    /// </summary>
    /// <param name="width">The width, in pixels.</param>
    /// <param name="height">The height, in pixels.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Size(int width, int height) => (Width, Height) = (width, height);

    /// <summary>
    /// Returns this <see cref="Size"/> value as a <see cref="Vector2"/> value.
    /// </summary>
    /// <returns>This <see cref="Size"/> value as a <see cref="Vector2"/> value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Vector2 ToVector2() => new Vector2(Width, Height);

    /// <summary>
    /// Returns  this <see cref="Size"/> value as a <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">The type to convert this <see cref="Size"/> to.</typeparam>
    /// <param name="converter">A function that defines how to perform the conversion.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly T To<T>(Func<Size, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        return converter(this);
    }

    /// <summary>
    /// Returns a value that indicates if two <see cref="Size"/> values are equal.
    /// </summary>
    /// <param name="left">The <see cref="Size"/> value on the left of the equality operator.</param>
    /// <param name="right">The <see cref="Size"/> value on the right of the equality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Size left, Size right) => left.Equals(right);

    /// <summary>
    /// Returns a value that indicates if two <see cref="Size"/> values are not equal.
    /// </summary>
    /// <param name="left">The <see cref="Size"/> value on the left of the inequality operator.</param>
    /// <param name="right">The <see cref="Size"/> value on the right of the inequality operator.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Size left, Size right) => !left.Equals(right);

    /// <inheritdoc/>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is Size other && Equals(other);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Size other) => Width.Equals(other.Width) && Height.Equals(other.Height);

    /// <inheritdoc/>
    public override readonly int GetHashCode() => HashCode.Combine(Width, Height);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{nameof(Size)}: ({Width}, {Height})";
}
