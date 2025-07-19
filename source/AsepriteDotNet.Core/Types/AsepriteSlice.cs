//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a named region within a sprite with optional 9-patch scaling and pivot point support.
/// </summary>
/// <remarks>
/// Slices define rectangular regions within sprites that can be labeled and referenced by name.
/// Corresponds to the Slice Chunk (0x2022) specification and supports keyframe animation for
/// dynamic bounds changes throughout the animation timeline. Optional 9-patch and pivot features
/// enable UI scaling and transformation workflows.
/// </remarks>
public sealed class AsepriteSlice
{
    internal AsepriteSliceKey[] InternalKeys { get; set; }

    /// <summary>
    /// Gets the collection of keyframes that define slice bounds and properties over time.
    /// </summary>
    /// <remarks>
    /// Each key specifies the slice bounds starting from a specific frame and remaining valid
    /// until the next key or end of animation. Keys enable animated slice bounds for dynamic
    /// sprite regions during animation playback.
    /// </remarks>
    public ReadOnlySpan<AsepriteSliceKey> Keys => InternalKeys;

    /// <summary>
    /// Gets a value indicating whether this slice includes 9-patch scaling information.
    /// </summary>
    /// <remarks>
    /// When enabled, each slice key contains center region data (X, Y, width, height) that defines
    /// the stretchable area for 9-patch scaling. Corner and edge regions remain fixed while the
    /// center area scales to accommodate different UI panel sizes.
    /// </remarks>
    public bool IsNinePatch { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether this slice includes pivot point information for transformations.
    /// </summary>
    /// <remarks>
    /// When enabled, each slice key contains pivot coordinates relative to the slice origin.
    /// Pivot points define the transformation center for rotation, scaling, and positioning
    /// operations commonly used in game engines and animation systems.
    /// </remarks>
    public bool HasPivot { get; internal set; }

    /// <summary>
    /// Gets the descriptive name assigned to this slice.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the user-defined metadata associated with this slice.
    /// </summary>
    public AsepriteUserData UserData { get; internal set; } = new();

    internal AsepriteSlice() { }
}
