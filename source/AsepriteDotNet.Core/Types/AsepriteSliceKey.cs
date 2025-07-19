//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a keyframe that defines slice bounds and properties for a specific frame in the animation timeline.
/// </summary>
/// <remarks>
/// Slice keys establish the slice bounds starting from a specific frame and remaining valid until the next key
/// or end of animation. This enables animated slice bounds for dynamic sprite regions during animation playback,
/// supporting both 9-patch scaling and pivot point transformations.
/// </remarks>
public sealed class AsepriteSliceKey
{
    /// <summary>
    /// Gets the zero-based frame number where this slice key becomes active.
    /// </summary>
    /// <remarks>
    /// The slice key remains valid from this frame until the next key or end of animation.
    /// Frame indices correspond to the animation timeline and enable time-based slice property changes.
    /// </remarks>
    public int FrameIndex { get; internal set; }

    /// <summary>
    /// Gets the rectangular bounds of the slice within the sprite canvas.
    /// </summary>
    /// <remarks>
    /// Coordinates are relative to the sprite's origin (top-left corner). The slice can have zero
    /// width or height to indicate it is hidden during this portion of the animation timeline.
    /// </remarks>
    public Rectangle Bounds { get; internal set; }

    /// <summary>
    /// Gets the center region bounds for 9-patch scaling when the slice is configured as a 9-patch.
    /// </summary>
    /// <remarks>
    /// Only valid when the parent slice has IsNinePatch set to true. Coordinates are relative to the
    /// slice origin, defining the region that scales while preserving corner and edge areas during
    /// 9-patch UI scaling operations.
    /// </remarks>
    public Rectangle CenterBounds { get; internal set; }

    /// <summary>
    /// Gets the pivot point coordinates for transformation operations when the slice has pivot information.
    /// </summary>
    /// <remarks>
    /// Only valid when the parent slice has HasPivot set to true. Coordinates are relative to the
    /// slice bounds origin and define the center point for rotation, scaling, and positioning
    /// operations in game engines and animation systems.
    /// </remarks>
    public Point Pivot { get; internal set; }

    internal AsepriteSliceKey() { }
}
