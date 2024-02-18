//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Collections.Frozen;
using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents the palette in an Aseprite file.
/// </summary>
public sealed class AsepritePalette
{
    /// <summary>
    /// An array of <see cref="Color"/> color elements that represent the colors of the palette.  Order of elements is
    /// the same as the order of colors in the palette in Aseprite.
    /// </summary>
    public FrozenSet<Color> Colors { get; }

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
        Colors = colors.ToFrozenSet();
        TransparentIndex  = transparentIndex;
    }
}
