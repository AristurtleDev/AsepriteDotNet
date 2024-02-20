//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

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
    /// The index of the <see cref="AsepriteFrame"/> that the properties of this <see cref="AsepriteSliceKey"/> are
    /// applied on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// Gets the top-left x-coordinate position of the bounds of the <see cref="AsepriteSlice"/> beginning on the
    /// <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/>.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the top-left y-coordinate position of the bounds of the <see cref="AsepriteSlice"/> beginning on the
    /// <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/>.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the width of the bounds of the <see cref="AsepriteSlice"/> beginning on the <see cref="AsepriteFrame"/> at
    /// <see cref="FrameIndex"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the bounds of the  <see cref="AsepriteSlice"/> beginning on the <see cref="AsepriteFrame"/> at
    /// <see cref="FrameIndex"/>.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the top-left x-coordinate position of the center bounds of the <see cref="AsepriteSlice"/> beginning on the
    /// <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/>.
    /// </summary>
    public int CenterX { get; }

    /// <summary>
    /// Gets the top-left y-coordinate position of the center bounds of the <see cref="AsepriteSlice"/> beginning on the
    /// <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/>.
    /// </summary>
    public int CenterY { get; }

    /// <summary>
    /// Gets the width of the center bounds of the <see cref="AsepriteSlice"/> beginning on the
    /// <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/>.
    /// </summary>
    public int CenterWidth { get; }

    /// <summary>
    /// Gets the height of the center bounds of the <see cref="AsepriteSlice"/> beginning on the
    /// <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/>.
    /// </summary>
    public int CenterHeight { get; }

    /// <summary>
    /// Gets the x-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> beginning on
    /// the <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/> in which the slice should pivot from.
    /// </summary>
    public int PivotX { get; }

    /// <summary>
    /// Gets the y-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> beginning on
    /// the <see cref="AsepriteFrame"/> at <see cref="FrameIndex"/> in which the slice should pivot from.
    /// </summary>
    public int PivotY { get; }

    internal AsepriteSliceKey(SliceKeyProperties keyProperties, NinePatchProperties? ninePatchProperties, PivotProperties? pivotProperties)
    {
        X = (int)keyProperties.X;
        Y = (int)keyProperties.Y;
        Width = (int)keyProperties.Width;
        Height = (int)keyProperties.Height;

        //  If this is not a nine patch, make the center bounds equal to the key bounds.
        //  NOTE: Might want to make this all 0's instead. See what users say and update accordingly.
        CenterX = (int)(ninePatchProperties?.X ?? 0);
        CenterY = (int)(ninePatchProperties?.Y ?? 0);
        CenterWidth = (int)(ninePatchProperties?.Width ?? keyProperties.Width);
        CenterHeight = (int)(ninePatchProperties?.Height ?? keyProperties.Height);

        //  If this did not have pivot data, make pivot (0, 0)
        PivotX = (int)(pivotProperties?.X ?? 0);
        PivotY = (int)(pivotProperties?.Y ?? 0);
    }
}
