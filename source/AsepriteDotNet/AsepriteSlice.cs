//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a slice element in an Aseprite file.
/// </summary>
public sealed class AsepriteSlice
{
    private readonly AsepriteSliceKey[] _keys;

    /// <summary>
    /// The collection of all <see cref="AsepriteSliceKey"/> elements for this <see cref="AsepriteSlice"/>.  Each key is
    /// similar to an animation key that defines the properties of the <see cref="AsepriteSlice"/> starting on a
    /// specific frame.
    /// </summary>
    public ReadOnlySpan<AsepriteSliceKey> Keys => _keys;

    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteSlice"/> has nine patch data in its keys; otherwise,
    /// <see langword="false"/>.
    /// </summary>
    public bool IsNinePatch { get; }

    /// <summary>
    /// <see langword="true"/> if this <see cref="AsepriteSlice"/> has pivot data in its keys; otherwise,
    /// <see langword="false"/>.
    /// Indicates whether this slice was marked to have a pivot point in Aseprite.
    /// </summary>
    public bool HasPivot { get; }

    /// <summary>
    /// The name of this <see cref="AsepriteSlice"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The <see cref="AsepriteUserData"/> that was set in the properties for this <see cref="AsepriteSlice"/> in
    /// Aseprite.
    /// </summary>
    public AsepriteUserData? UserData { get; } = new AsepriteUserData();

    internal AsepriteSlice(SliceProperties sliceProperties, string name, AsepriteSliceKey[] keys)
    {
        Name = name;
        IsNinePatch = sliceProperties.Flags.HasFlag(1);
        HasPivot = sliceProperties.Flags.HasFlag(2);
        _keys = keys;
    }
}
