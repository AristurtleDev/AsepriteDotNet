//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines a slice element in an Aseprite file.
/// </summary>
public sealed class AsepriteSlice
{
    internal AsepriteSliceKey[] InternalKeys { get;  set; }

    /// <summary>
    /// Gets the slice key elements for this slice.
    /// </summary>
    /// <remarks>
    /// Each key is similar to an animation key that defines the properties of this slice starting on a specified frame.
    /// </remarks>
    public ReadOnlySpan<AsepriteSliceKey> Keys => InternalKeys;

    /// <summary>
    /// Gets a value that indicates whether this slice has nine patch data in its keys.
    /// </summary>
    public bool IsNinePatch { get; internal set; }

    /// <summary>
    /// Gets a value that indicates whether this slice has pivot data in its keys.
    /// </summary>
    public bool HasPivot { get; internal set; }

    /// <summary>
    /// Gets the name of this slice.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this slice in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; internal set; } = new();

    internal AsepriteSlice() { }
}
