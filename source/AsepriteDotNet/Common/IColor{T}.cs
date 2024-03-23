// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Numerics;
using System.Runtime.CompilerServices;

namespace AsepriteDotNet.Common;

/// <summary>
/// Defines a color value with four 8-bit component values, each representing the red, green, blue, and alpha value of
/// the underlying color type.
/// </summary>
/// <typeparam name="TColor">The underlying color type.</typeparam>
public interface IColor<TColor> where TColor : struct
{
    /// <summary>
    /// Gets or Sets the red color component value.
    /// </summary>
    byte R { get; set; }

    /// <summary>
    /// Gets or Sets the green color component value.
    /// </summary>
    byte G { get; set; }

    /// <summary>
    /// Gets or Sets the blue color component value.
    /// </summary>
    byte B { get; set; }

    /// <summary>
    /// Gets or Sets the alpha color component value
    /// </summary>
    byte A { get; set; }

    /// <summary>
    /// Gets or Sets the packed value of this <see cref="IColor{T}"/> represented as a 32-bit unsigned integer in
    /// the least to most significant bit order of red, green, blue, alpha.
    /// </summary>
    uint PackedValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    /// <summary>
    /// Sets the red, green, blue, and alpha color component values based on the component values of the given
    /// <see cref="Vector4"/>.
    /// </summary>
    /// <param name="vector">The <see cref="Vector4"/> value to derive the component values from.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void FromVector4(Vector4 vector);

    /// <summary>
    /// Returns this value as a <see cref="Vector4"/> representation value.
    /// </summary>
    /// <returns>The <see cref="Vector4"/> representation of this value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Vector4 ToVector4();

    /// <summary>
    /// Converts this <see cref="IColor{T}"/> value to another of type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="TOut">The type base type to convert to.</typeparam>
    /// <returns>The converted <see cref="IColor{T}"/> type. s</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    TOut To<TOut>() where TOut : struct, IColor<TOut>;
}
