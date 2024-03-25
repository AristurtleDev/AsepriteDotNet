// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Common;

/// <summary>
/// Extension methods for the <see cref="IColor{T}"/> interface.
/// </summary>
public static class IColorTExtensions
{
    /// <summary>
    /// Converts an array of <see cref="IColor{T}"/> instances to an array of <typeparamref name="T"/> instances.
    /// </summary>
    /// <typeparam name="T">The type implementing <see cref="IColor{T}"/>.</typeparam>
    /// <param name="colors">The array of <see cref="IColor{T}"/> instances to convert.</param>
    /// <returns>
    /// An array of <typeparamref name="T"/> instances representing the values of the input <see cref="IColor{T}"/>
    /// instances.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="colors"/> array is <see langword="null"/>.
    /// </exception>
    public static T[] As<T>(this IColor<T>[] colors) where T : IColor<T>
    {
        ArgumentNullException.ThrowIfNull(colors);

        T[] result = new T[colors.Length];
        Parallel.For(0, colors.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i) =>
        {
            result[i] = colors[i].Value;
        });
        return result;
    }
}
