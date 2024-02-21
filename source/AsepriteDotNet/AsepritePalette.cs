//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Collections;

namespace AsepriteDotNet;

/// <summary>
/// Represents the palette in an Aseprite file.
/// </summary>
public sealed class AsepritePalette : IEnumerable<AseColor>
{
    private AseColor[] _colors = Array.Empty<AseColor>();

    /// <summary>
    /// Gets the <see cref="AseColor"/> element at the specified index from this <see cref="AsepritePalette"/>.
    /// </summary>
    /// <param name="index">The index of the <see cref="AseColor"/> element.</param>
    /// <returns>The <see cref="AseColor"/> element at the specified <paramref name="index"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the <paramref name="index"/> is less than zero or greater than or equal to <see cref="Count"/>.
    /// </exception>
    public AseColor this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _colors.Length);
            return _colors[index];
        }

        internal set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _colors.Length);
            _colors[index] = value;
        }
    }

    /// <summary>
    /// Gets the index of the <see cref="AseColor"/> element in this <see cref="AsepritePalette"/> that should be
    /// interpreted as a transparent color.
    /// </summary>
    public int TransparentIndex { get; }

    /// <summary>
    /// Gets the total number of <see cref="AseColor"/> elements in this <see cref="AsepritePalette"/>.
    /// </summary>
    public int Count => _colors.Length;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of the <see cref="AseColor"/> elements in this 
    /// <see cref="AsepritePalette"/>.  Order of elements is the same as the order of colors in the palette in Aseprite.
    /// </summary>
    public ReadOnlySpan<AseColor> Colors => _colors;

    internal AsepritePalette(int transparentIndex) => TransparentIndex = transparentIndex;

    internal void Resize(int newSize)
    {
        AseColor[] newColors = new AseColor[newSize];
        Array.Copy(_colors, newColors, _colors.Length);
        _colors = newColors;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="AseColor"/> elements in this 
    /// <see cref="AsepritePalette"/>.
    /// </summary>
    /// <returns>
    /// An enumerator that iterates through the <see cref="AseColor"/> elements in this <see cref="AsepritePalette"/>.
    /// </returns>
    public IEnumerator<AseColor> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
