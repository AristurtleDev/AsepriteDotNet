//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core.Document;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines core properties of an Aseprite layer.
/// </summary>
public abstract class AsepriteLayer
{
    /// <summary>
    /// Gets a value that indicates whether this layer is visible.
    /// </summary>
    public bool IsVisible { get; }

    /// <summary>
    /// Gets a value that indicates whether this layer is the background layer.
    /// </summary>
    public bool IsBackgroundLayer { get; }

    /// <summary>
    /// Gets a value that indicates whether this layer is a reference layer.
    /// </summary>
    public bool IsReferenceLayer { get; }

    /// <summary>
    /// Gets the child level of this layer in relation to its parent.
    /// </summary>
    /// <remarks>
    /// See <see href="https://github.com/aseprite/aseprite/blob/main/docs/ase-file-specs.md#note1"/> for more
    /// information.
    /// </remarks>
    public int ChildLevel { get; }

    /// <summary>
    /// Gets the blend mode used by this layer when blending cels on this layer with the layer below it.
    /// </summary>
    public AsepriteBlendMode BlendMode { get; }

    /// <summary>
    /// Gets the opacity level of this layer.
    /// </summary>
    public int Opacity { get; }

    /// <summary>
    /// Gets the name of this layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this layer in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; } = new AsepriteUserData();

    internal AsepriteLayer(AsepriteLayerProperties header, string name)
    {
        Name = name;
        IsVisible = (header.Flags & 1) != 0;
        IsBackgroundLayer = (header.Flags & 8) != 0;
        IsReferenceLayer = (header.Flags & 64) != 0;
        ChildLevel = header.Level;
        BlendMode = (AsepriteBlendMode)header.BlendMode;
        Opacity = header.Opacity;
    }
}
