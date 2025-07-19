//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Types;

/// <summary>
/// Represents a layer in the sprite hierarchy that organizes and renders content with specific properties and transformations.
/// </summary>
/// <remarks>
/// Layers define the organizational structure of sprite composition and control how content is rendered
/// through properties like visibility, blend modes, and opacity. The layer hierarchy is established
/// through child levels and determines the order of composition during frame rendering.
/// </remarks>
public abstract class AsepriteLayer
{
    /// <summary>
    /// Gets a value indicating whether this layer is visible and should be included in rendering.
    /// </summary>
    /// <remarks>
    /// Corresponds to the "Visible" flag (bit 1) in the Layer Chunk (0x2004) specification.
    /// Hidden layers are excluded from both editor display and export functionality.
    /// </remarks>
    public bool IsVisible { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether this layer serves as the background layer with special rendering properties.
    /// </summary>
    /// <remarks>
    /// Corresponds to the "Background" flag (bit 8) in the Layer Chunk specification.
    /// Background layers typically appear at the bottom of the layer stack and do not support alpha transparency.
    /// </remarks>
    public bool IsBackgroundLayer { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether this layer serves as a reference layer for drawing guidance.
    /// </summary>
    /// <remarks>
    /// Corresponds to the "Reference layer" flag (bit 64) in the Layer Chunk specification.
    /// Reference layers are typically used for sketches, guidelines, or imported reference images.
    /// </remarks>
    public bool IsReferenceLayer { get; internal set; }

    /// <summary>
    /// Gets the hierarchical depth level of this layer within the layer tree structure.
    /// </summary>
    /// <remarks>
    /// Used to establish parent-child relationships between layers and group layers.
    /// Each increment represents one level deeper in the hierarchy from the previous layer read from the file.
    /// </remarks>
    public int ChildLevel { get; internal set; }

    /// <summary>
    /// Gets the blend mode used when compositing this layer with underlying layers.
    /// </summary>
    /// <remarks>
    /// Controls mathematical operations applied when combining this layer's pixels with the layers below.
    /// The blend mode is always valid for image and tilemap layers, and valid for group layers when
    /// header flags indicate group blending is supported.
    /// </remarks>
    public AsepriteBlendMode BlendMode { get; internal set; }

    /// <summary>
    /// Gets the opacity level applied to this layer during composition.
    /// </summary>
    /// <remarks>
    /// Layer opacity is valid when the sprite header flags indicate layer opacity validity (bit 1).
    /// For group layers, opacity is valid when header flags indicate group blending support (bit 2).
    /// This value is combined with individual cel opacity during final rendering.
    /// </remarks>
    public int Opacity { get; internal set; }

    /// <summary>
    /// Gets the descriptive name assigned to this layer.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the user-defined metadata associated with this layer.
    /// </summary>
    public AsepriteUserData UserData { get; internal set; } = new();

    internal AsepriteLayer() { }
}
