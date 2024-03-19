//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of the palette in an Aseprite file.
/// </summary>
public sealed class AsepritePalette
{
    private Rgba32[] _colors = Array.Empty<Rgba32>();

    /// <summary>
    /// Gets the color value at the specified index from this palette.
    /// </summary>
    /// <param name="index">The index of the color element to retrieve.</param>
    /// <returns>The color element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the <paramref name="index"/> is less than zero or greater than or equal to <see cref="Count"/>.
    /// </exception>
    public Rgba32 this[int index]
    {
        get => _colors[index];
        internal set => _colors[index] = value;
    }

    /// <summary>
    /// Gets the index of the color element in this palette that should be interpreted as a transparent color.
    /// </summary>
    public int TransparentIndex { get; }

    /// <summary>
    /// Gets the total number of color elements in this palette.
    /// </summary>
    public int Count => _colors.Length;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="Rgba32"/> elements in this
    /// <see cref="AsepritePalette"/>.  Order of elements is the same as the order of colors in the palette in Aseprite.
    /// </summary>
    public ReadOnlySpan<Rgba32> Colors => _colors;

    internal AsepritePalette(int transparentIndex) => TransparentIndex = transparentIndex;

    internal void Resize(int newSize)
    {
        Rgba32[] newColors = new Rgba32[newSize];
        Array.Copy(_colors, newColors, _colors.Length);
        _colors = newColors;
    }
}
