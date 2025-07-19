//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a color palette containing indexed RGBA colors for sprite rendering and display.
/// </summary>
public sealed class AsepritePalette
{
    private Rgba32[] _colors = Array.Empty<Rgba32>();

    /// <summary>
    /// Gets or sets the color at the specified palette index.
    /// </summary>
    /// <param name="index">The zero-based index of the color entry.</param>
    /// <returns>The <see cref="Rgba32"/> color value at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="index"/> is outside the valid range [0, Count).</exception>
    public Rgba32 this[int index]
    {
        get => _colors[index];
        internal set => _colors[index] = value;
    }

    /// <summary>
    /// Gets the palette index that represents the transparent color in non-background layers.
    /// </summary>
    /// <remarks>
    /// Corresponds to the "Palette entry (index) which represent transparent color" field
    /// in the header specification. Only applies to indexed color sprites and affects
    /// how transparency is handled during layer composition for non-background layers.
    /// </remarks>
    public int TransparentIndex { get; internal set; }

    /// <summary>
    /// Gets the total number of colors currently stored in the palette.
    /// </summary>
    /// <remarks>
    /// A value of 0 indicates an empty palette, while 256 represents the maximum size for
    /// traditional 8-bit indexed color modes.
    /// </remarks>
    public int Count => _colors.Length;

    /// <summary>
    /// Gets all colors in the palette as a read-only span.
    /// </summary>
    /// <remarks>
    /// Colors are ordered by their palette index from 0 to Count-1.
    /// </remarks>
    public ReadOnlySpan<Rgba32> Colors => _colors;

    internal AsepritePalette() { }

    internal void Resize(int newSize)
    {
        Rgba32[] newColors = new Rgba32[newSize];
        Array.Copy(_colors, newColors, _colors.Length);
        _colors = newColors;
    }
}
