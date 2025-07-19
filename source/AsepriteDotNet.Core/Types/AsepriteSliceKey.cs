//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a slice starting on a specific frame.
/// </summary>
public sealed class AsepriteSliceKey
{
    /// <summary>
    /// Gets the index of the frame that the properties of this key are applied starting on.
    /// </summary>
    public int FrameIndex { get; internal set; }

    /// <summary>
    /// Gets the rectangular bounds of the slice during this key.
    /// </summary>
    public Rectangle Bounds { get; internal set; }

    /// <summary>
    /// Gets the bounds of the center of the slice during this key.
    /// </summary>
    public Rectangle CenterBounds { get; internal set; }

    /// <summary>
    /// Gets the xy-coordinate pivot point of the slice during this key.
    /// </summary>
    public Point Pivot { get; internal set; }

    internal AsepriteSliceKey() { }
}
