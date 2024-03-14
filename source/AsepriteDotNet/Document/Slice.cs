//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines a slice element in an Aseprite file.
/// </summary>
public sealed class Slice
{
    private readonly SliceKey[] _keys;

    /// <summary>
    /// Gets the slice key elements for this slice.
    /// </summary>
    /// <remarks>
    /// Each key is similar to an animation key that defines the properties of this slice starting on a specified frame.
    /// </remarks>
    public ReadOnlySpan<SliceKey> Keys => _keys;

    /// <summary>
    /// Gets a value that indicates whether this slice has nine patch data in its keys.
    /// </summary>
    public bool IsNinePatch { get; }

    /// <summary>
    /// Gets a value that indicates whether this slice has pivot data in its keys.
    /// </summary>
    public bool HasPivot { get; }

    /// <summary>
    /// Gets the name of this slice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the custom user data that was set in the properties for this slice in Aseprite.
    /// </summary>
    public UserData? UserData { get; } = new UserData();

    internal Slice(string name, bool isNinePatch, bool hasPivot, SliceKey[] keys)
    {
        
        Name = name;
        IsNinePatch = isNinePatch;
        HasPivot = hasPivot;
        _keys = keys;
    }
}
