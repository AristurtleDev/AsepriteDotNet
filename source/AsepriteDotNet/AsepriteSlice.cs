//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a slice element in an Aseprite file.
/// </summary>
public sealed class AsepriteSlice
{
    /// <summary>
    /// The collection of all <see cref="AsepriteSliceKey"/> elements for this <see cref="AsepriteSlice"/>.  Each key is
    /// similar to an animation key that defines the properties of the <see cref="AsepriteSlice"/> starting on a
    /// specific frame.
    /// </summary>
    public List<AsepriteSliceKey> Keys;

    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteSlice"/> has nine patch data in its keys; otherwise,
    /// <see langword="false"/>.
    /// </summary>
    public bool IsNinePatch;

    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteSlice"/> has pivot data in its keys; otherwise,
    /// <see langword="false"/>.
    /// Indicates whether this slice was marked to have a pivot point in Aseprite.
    /// </summary>
    public bool HasPivot;

    /// <summary>
    /// The name of this <see cref="AsepriteSlice"/>.
    /// </summary>
    public string Name;

    /// <summary>
    /// The <see cref="AsepriteUserData"/> that was set in the properties for this <see cref="AsepriteSlice"/> in
    /// Aseprite.
    /// </summary>
    public AsepriteUserData UserData;

    /// <summary>
    /// Creates a new instance of the <see cref="AsepriteSlice"/> class.
    /// </summary>
    public AsepriteSlice()
    {
        Keys = new List<AsepriteSliceKey>();
    }
}
