//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Collections.Frozen;

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteCel"/> that contains image data.
/// </summary>
public sealed class AsepriteImageCel : AsepriteCel
{
    /// <summary>
    /// The width of this <see cref="AsepriteImageCel"/>, in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height of this <see cref="AsepriteImageCel"/>, in pixels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// A collection of the <see cref="Rgba32"/> color elements that represents the pixel data that makes up the image
    /// for this <see cref="AsepriteCel"/>.  Order of the color elements starts with the top-left most pixel and is read
    /// left-to-right from top-to-bottom.
    /// </summary>
    public FrozenSet<Rgba32> Pixels { get; }

    internal AsepriteImageCel(AsepriteLayer layer, int x, int y, int opacity, int width, int height, Rgba32[] pixels)
        : base(layer, x, y, opacity)
    {
        Width = width;
        Height = height;
        Pixels = pixels.ToFrozenSet();
    }
}
