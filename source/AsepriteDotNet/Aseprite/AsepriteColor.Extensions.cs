// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite;

/// <summary>
/// Defines extension method for the <see cref="AsepriteColor"/> type.
/// </summary>
public static class AsepriteColorExtensions
{
    /// <summary>
    /// Converts an <see cref="Rgba32"/> value to the specified target type using the provided converter.
    /// </summary>
    /// <typeparam name="T">The target type to convert to.</typeparam>
    /// <param name="color">The <see cref="Rgba32"/> to be converted.</param>
    /// <param name="converter">
    /// A function that performs the conversion from an <see cref="Rgba32"/> value to a <typeparamref name="T"/>
    /// value.
    /// </param>
    /// <returns>The converted value as type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="color"/> is <see langword="null"/>
    ///
    /// -or
    ///
    /// <paramref name="converter"/> is null.
    /// </exception>
    public static T As<T>(this Rgba32 color, Func<Rgba32, T> converter) where T : struct
    {
        Rgba32[] colors = new Rgba32[12];

        ArgumentNullException.ThrowIfNull(converter);
        return converter(color);
    }

    /// <summary>
    /// Converts an array of <see cref="Rgba32"/> values to an array of <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="colors">THe array of <see cref="Rgba32"/> value to convert.</param>
    /// <param name="converter">
    /// A function that performs the conversion from an <see cref="Rgba32"/> value to a <typeparamref name="T"/>
    /// value.
    /// </param>
    /// <returns></returns>
    public static T[] As<T>(this Rgba32[] colors, Func<Rgba32, T> converter) where T : struct
    {
        ArgumentNullException.ThrowIfNull(colors);
        ArgumentNullException.ThrowIfNull(converter);

        T[] converted = new T[colors.Length];
        colors.AsParallel()
              .AsOrdered()
              .WithDegreeOfParallelism(Environment.ProcessorCount * 4)
              .Select((color, index) =>
              {
                  converted[index] = converter(color);
                  return true;
              }).ToArray();

        return converted;
    }
}
