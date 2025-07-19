//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines core properties of an Aseprite layer.
/// </summary>
public abstract class AsepriteLayer
{
    /// <summary>
    /// Gets a value that indicates whether this layer is visible.
    /// </summary>
    public bool IsVisible { get; internal set; }

    /// <summary>
    /// Gets a value that indicates whether this layer is the background layer.
    /// </summary>
    public bool IsBackgroundLayer { get; internal set; }

    /// <summary>
    /// Gets a value that indicates whether this layer is a reference layer.
    /// </summary>
    public bool IsReferenceLayer { get; internal set; }

    /// <summary>
    /// Gets the child level of this layer in relation to its parent.
    /// </summary>
    /// <remarks>
    /// See <see href="https://github.com/aseprite/aseprite/blob/main/docs/ase-file-specs.md#note1"/> for more
    /// information.
    /// </remarks>
    public int ChildLevel { get; internal set; }

    /// <summary>
    /// Gets the blend mode used by this layer when blending cels on this layer with the layer below it.
    /// </summary>
    public AsepriteBlendMode BlendMode { get; internal set; }

    /// <summary>
    /// Gets the opacity level of this layer.
    /// </summary>
    public int Opacity { get; internal set; }

    /// <summary>
    /// Gets the name of this layer.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this layer in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; internal set; } = new();

    internal AsepriteLayer() { }
}
