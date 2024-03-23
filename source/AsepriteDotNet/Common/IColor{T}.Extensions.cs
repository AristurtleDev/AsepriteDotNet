// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Common;

/// <summary>
/// Defines extension methods for the <see cref="IColor{T}"/> interface.
/// </summary>
public static class IPackedColorTExtensions
{
    /// <summary>
    /// Returns a <typeparamref name="TColor"/> value created from the specified byte array.
    /// </summary>
    /// <typeparam name="TColor">The type representing a packed color.</typeparam>
    /// <param name="buffer">
    /// A byte array containing four elements representing the R, G, B, and A color components in that order.
    /// </param>
    /// <returns>The <typeparamref name="TColor"/> created from this method.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="buffer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="buffer"/> does not contain exactly four elements.
    /// </exception>
    public static TColor IPackedColorFromBytes<TColor>(this byte[] buffer) where TColor : struct, IColor<TColor>
    {
        ArgumentNullException.ThrowIfNull(buffer);
        if(buffer.Length != 4)
        {
            throw new ArgumentException("Invalid buffer length.  Buffer must contain exactly 4 values representing the R, G, B, and A color components in that order.");
        }

        TColor value = default(TColor);
        value.R = buffer[0];
        value.G = buffer[1];
        value.B = buffer[2];
        value.A = buffer[3];
        return value;
    }

    /// <summary>
    /// Converts an array of input <see cref="IColor{T}"/>  values to an array of output
    /// <see cref="IColor{T}"/> values.
    /// </summary>
    /// <typeparam name="TIn">The type of the input <see cref="IColor{T}"/> structure.</typeparam>
    /// <typeparam name="TOut">The type of the output <see cref="IColor{T}"/> structure.</typeparam>
    /// <param name="input">The array of input <see cref="IColor{T}"/> structures to be converted.</param>
    /// <returns>An array of output <see cref="IColor{T}"/> structures converted from the input.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="input"/> parameter is null.</exception>

    public static TOut[] As<TIn, TOut>(this TIn[] input)
    where TIn : struct, IColor<TIn>
    where TOut : struct, IColor<TOut>
    {
        ArgumentNullException.ThrowIfNull(input);

        TOut[] converted = new TOut[input.Length];

        Parallel.For(0, input.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i) =>
        {
            converted[i] = input[i].To<TOut>();
        });

        return converted;
    }
}
