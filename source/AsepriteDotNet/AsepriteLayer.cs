//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a layer in an Aseprite file.
/// </summary>
public abstract class AsepriteLayer
{
    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteLayer"/> is visible; otherwise, <see langword="false"/>.
    /// </summary>
    public bool IsVisible;

    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteLayer"/> was marked as the background layer in Aseprite;
    /// otherwise, <see langword="false"/>.
    /// </summary>
    public bool IsBackgroundLayer;

    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteLayer"/> was marked as a reference layer containing a
    /// reference image in Aseprite; otherwise, <see langword="false"/>.
    /// </summary>
    public bool IsReferenceLayer;

    /// <summary>
    /// The level of this <see cref="AsepriteLayer"/> in relation to its parent.
    /// </summary>
    /// <remarks>
    /// See <see href="https://github.com/aseprite/aseprite/blob/main/docs/ase-file-specs.md#note1"/> for more
    /// information.
    /// </remarks>
    public int ChildLevel;

    /// <summary>
    /// Indicates the <see cref="AsepriteBlendMode"/> used by the <see cref="AsepriteCel"/> elements that are on
    /// this <see cref="AsepriteLayer"/> when blending with <see cref="AsepriteCel"/> elements on the
    /// <see cref="AsepriteLayer"/> below.
    /// </summary>
    public AsepriteBlendMode BlendMode;

    /// <summary>
    /// The opacity level for this <see cref="AsepriteLayer"/>.
    /// </summary>
    public int Opacity;

    /// <summary>
    /// The name of this <see cref="AsepriteLayer"/>.
    /// </summary>
    public string Name;

    /// <summary>
    /// The <see cref="AsepriteUserData"/> that was set in the properties for this <see cref="AsepriteLayer"/> in
    /// Aseprite.
    /// </summary>
    public AsepriteUserData UserData;
}
