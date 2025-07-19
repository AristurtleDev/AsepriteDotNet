//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet.Types;

/// <summary>
/// Represents a cel (cell) containing visual content positioned within a layer and frame.
/// </summary>
/// <remarks>
/// Cels are the fundamental building blocks of sprite animation, containing pixel data or tile references
/// that are composited together to form the final rendered frame. Each cel belongs to a specific layer
/// and can be positioned anywhere within the sprite canvas using pixel-precise coordinates.
/// </remarks>
public abstract class AsepriteCel
{
    /// <summary>
    /// Gets the layer that contains this cel.
    /// </summary>
    /// <remarks>
    /// Establishes the hierarchical relationship between the cel and its parent layer,
    /// which determines rendering order, blend modes, and visibility inheritance.
    /// </remarks>
    public AsepriteLayer Layer { get; internal set; }

    /// <summary>
    /// Gets the position of the cel within the sprite canvas.
    /// </summary>
    /// <remarks>
    /// Coordinates can be negative, allowing cels to extend beyond the visible sprite bounds.
    /// The position determines where the cel's content is placed during frame composition.
    /// </remarks>
    public Point Location { get; internal set; }

    /// <summary>
    /// Gets the opacity level applied to this cel during rendering.
    /// </summary>
    /// <remarks>
    /// Cel opacity is combined with layer opacity during final composition.
    /// This value is independent of any alpha channel data in the cel's pixel content.
    /// </remarks>
    public int Opacity { get; internal set; }

    /// <summary>
    /// Gets the Z-index offset that modifies the rendering order of this cel.
    /// </summary>
    /// <remarks>
    /// The final rendering order is calculated as: layerIndex + zIndex. Z-index allows individual cels to break normal layer ordering for special effects or composition requirements without restructuring the entire layer hierarchy.
    /// </remarks>
    public int ZIndex { get; internal set; }

    /// <summary>
    /// Gets the user-defined metadata associated with this cel.
    /// </summary>
    public AsepriteUserData UserData { get; internal set; } = new();

    internal AsepriteCel() { }
}
