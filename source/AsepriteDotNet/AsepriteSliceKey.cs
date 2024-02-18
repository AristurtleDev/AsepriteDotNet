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
    /// The <see cref="AsepriteSlice"/> this <see cref="AsepriteSliceKey"/> is for.
    /// </summary>
    public AsepriteSlice Slice { get; }

    /// <summary>
    /// The index of the <see cref="AsepriteFrame"/> that the properties of this <see cref="AsepriteSliceKey"/> are
    /// applied on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// The x-coordinate position of the top-left corner of the bounds of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int X { get; }

    /// <summary>
    /// The y-coordinate position of the top-left corner of the bounds of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// The width, in pixels, of the <see cref="AsepriteSlice"/> during the <see cref="AsepriteFrame"/> of this
    /// <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// The height, in pixels, of the <see cref="AsepriteSlice"/> during the <see cref="AsepriteFrame"/> of this
    /// <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// The x-coordinate position of the top-left corner of the center bounds of the <see cref="AsepriteSlice"/> during
    /// the <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>.
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignored.
    /// </remarks>
    public int CenterX { get; }

    /// <summary>
    /// The y-coordinate position of the top-left corner of the center bounds of the <see cref="AsepriteSlice"/> during
    /// the <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>.
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignore.
    /// </remarks>
    public int CenterY { get; }

    /// <summary>
    /// The width, in pixels, of the center bounds of the see <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignored.
    /// </remarks>
    public int CenterWidth { get; }

    /// <summary>
    /// The height, in pixels, of the center bounds of the see <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignored.
    /// </remarks>
    public int CenterHeight { get; }

    /// <summary>
    /// The x-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/> in which the slice should pivot from.
    /// </summary>
    public int PivotX { get; }

    /// <summary>
    /// The y-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/> in which the slice should pivot from.
    /// </summary>
    public int PivotY { get; }

    internal AsepriteSliceKey(AsepriteSlice slice, int frame, int x, int y, int width, int height, int centerX, int centerY, int centerWidth, int centerHeight, int pivotX, int pivotY)
    {
        Slice = slice;
        FrameIndex = frame;
        X = x;
        Y = y;
        Width = width;
        Height = height;
        CenterX = centerX;
        CenterY = centerY;
        CenterWidth = centerWidth;
        CenterHeight = centerHeight;
        PivotX = pivotX;
        PivotY = pivotY;
    }

}
