//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents a key for an <see cref="AsepriteSlice"/>
/// </summary>
/// <remarks>
/// A slice key can be thought of like frame keys in animations.  They contain the properties of the slice on a
/// specific frame.
/// </remarks>
public sealed class AsepriteSliceKey
{
    /// <summary>
    /// The <see cref="AsepriteSlice"/> this <see cref="AsepriteSliceKey"/> is for.
    /// </summary>
    public AsepriteSlice Slice { get; }

    /// <summary>
    /// The index of the <see cref="AsepriteFrame"/> that the properties of this <see cref="AsepriteSliceKey"/> are
    /// applied on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// The bounds of the <see cref="AsepriteSlice"/> during the <see cref="AsepriteFrame"/> of this
    /// <see cref="AsepriteSliceKey"/>, relative to the bounds of the frame.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// The center bounds of the <see cref="AsepriteSlice"/> during the <see cref="AsepriteFrame"/> of this
    /// <see cref="AsepriteSliceKey"/>, relative ot the bounds of the frame.
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for 
    /// <see cref="Slice"/>; otherwise, <see langword="null"/>.
    /// </remarks>
    public Rectangle? CenterBounds { get; }

    /// <summary>
    /// The xy-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/> in which the slice should pivot from.
    /// </summary>
    public Point? Pivot { get; }

    internal AsepriteSliceKey(AsepriteSlice slice, int frame, Rectangle bounds,  Rectangle? centerBounds, Point? pivot)
    {
        Slice = slice;
        FrameIndex = frame;
        Bounds = bounds;
        CenterBounds = centerBounds;
        Pivot = pivot;
    }

}
