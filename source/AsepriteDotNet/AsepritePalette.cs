//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents the palette in an Aseprite file.
/// </summary>
public sealed class AsepritePalette
{
    private readonly Color[] _colors;

    /// <summary>
    /// An array of <see cref="Color"/> color elements that represent the colors of the palette.  Order of elements is
    /// the same as the order of colors in the palette in Aseprite.
    /// </summary>
    public ReadOnlySpan<Color> Colors => _colors;

    /// <summary>
    /// The index of the element in <see cref="Colors"/> that represents a color that should be interpreted as a
    /// transparent color.
    /// </summary>
    /// <remarks>
    /// This value is only valid when the <see cref="AsepriteColorDepth"/> mode used is
    /// <see cref="AsepriteColorDepth.Indexed"/>.
    /// </remarks>
    public int TransparentIndex { get; }

    internal AsepritePalette(Color[] colors, int transparentIndex)
    {
        _colors = colors;
        TransparentIndex = transparentIndex;
    }
}
