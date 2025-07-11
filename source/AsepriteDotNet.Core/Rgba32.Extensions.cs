// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core;

/// <summary>
/// Defines extension methods for the <see cref="Rgba32"/> struct.
/// </summary>
public static class Rgba32Extensions
{
    /// <summary>
    /// Converts an <see cref="Rgba32"/> value to the specified <typeparamref name="T"/> type using the provided
    /// conversion functions.
    /// </summary>
    /// <typeparam name="T">The target type to convert to.</typeparam>
    /// <param name="rgba">The <see cref="Rgba32"/> value to convert.</param>
    /// <param name="converter">
    /// A function that performs the conversion from an <see cref="Rgba32"/> value to a <typeparamref name="T"/> type.
    /// </param>
    /// <returns>The converted value as type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="rgba"/> parameter is <see langword="null"/>.
    ///
    /// -or-
    ///
    /// Thrown if <paramref name="converter"/> parameter is <see langword="null"/>
    /// </exception>
    public static T As<T>(this Rgba32 rgba, Func<Rgba32, T> converter)
    {
        ArgumentNullException.ThrowIfNull(rgba);
        ArgumentNullException.ThrowIfNull(converter);
        return converter(rgba);
    }

    /// <summary>
    /// Converts an array of <see cref="Rgba32"/> values to an array of the specified <typeparamref name="T"/> type
    /// using the provided conversion function.
    /// </summary>
    /// <typeparam name="T">The target type to convert to.</typeparam>
    /// <param name="colors">The array of <see cref="Rgba32"/> elements to convert</param>
    /// <param name="converter">
    /// A function that performs the conversion from an <see cref="Rgba32"/> value to a <typeparamref name="T"/> type.
    /// </param>
    /// <returns>
    /// An array of <typeparamref name="T"/> values converted from the array of <see cref="Rgba32"/> values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Throw if <paramref name="colors"/> parameter is <see langword="null"/>.
    ///
    /// -or-
    ///
    /// Thrown if <paramref name="converter"/> parameter is <see langword="null"/>
    /// </exception>
    public static T[] As<T>(this Rgba32[] colors, Func<Rgba32, T> converter)
    {
        ArgumentNullException.ThrowIfNull(colors);
        ArgumentNullException.ThrowIfNull(converter);
        T[] converted = new T[colors.Length];
        Parallel.For(0, colors.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i) =>
        {
            converted[i] = converter(colors[i]);
        });
        return converted;
    }

    /// <summary>
    /// Converts read-only span of <see cref="Rgba32"/> values to an array of the specified <typeparamref name="T"/>
    /// type using the provided conversion function.
    /// </summary>
    /// <typeparam name="T">The target type to convert to.</typeparam>
    /// <param name="colors">The read-only span of <see cref="Rgba32"/> elements to convert</param>
    /// <param name="converter">
    /// A function that performs the conversion from an <see cref="Rgba32"/> value to a <typeparamref name="T"/> type.
    /// </param>
    /// <returns>
    /// An array of <typeparamref name="T"/> values converted from the array of <see cref="Rgba32"/> values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="converter"/> parameter is <see langword="null"/>
    /// </exception>
    public static unsafe T[] As<T>(this ReadOnlySpan<Rgba32> colors, Func<Rgba32, T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter);
        T[] converted = new T[colors.Length];
        fixed (Rgba32* ptrColors = colors)
        {
            //  Have to use temporary variable to hold pointer since fixed pointers cannot be used inside the
            //  lambda function below.
            Rgba32* ptr = ptrColors;
            Parallel.For(0, colors.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i) =>
            {
                Rgba32* color = ptr + i;
                converted[i] = converter(*color);
            });
        }
        return converted;
    }
}
