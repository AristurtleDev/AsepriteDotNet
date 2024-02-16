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
    public AsepriteSlice Slice;

    /// <summary>
    /// The index of the <see cref="AsepriteFrame"/> that the properties of this <see cref="AsepriteSliceKey"/> are
    /// applied on.
    /// </summary>
    public readonly int FrameIndex;

    /// <summary>
    /// The x-coordinate position of the top-left corner of the bounds of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int X;

    /// <summary>
    /// The y-coordinate position of the top-left corner of the bounds of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int Y;

    /// <summary>
    /// The width, in pixels, of the <see cref="AsepriteSlice"/> during the <see cref="AsepriteFrame"/> of this
    /// <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int Width;

    /// <summary>
    /// The height, in pixels, of the <see cref="AsepriteSlice"/> during the <see cref="AsepriteFrame"/> of this
    /// <see cref="AsepriteSliceKey"/>
    /// </summary>
    public int Height;

    /// <summary>
    /// The x-coordinate position of the top-left corner of the center bounds of the <see cref="AsepriteSlice"/> during
    /// the <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>.
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignored.
    /// </remarks>
    public int CenterX;

    /// <summary>
    /// The y-coordinate position of the top-left corner of the center bounds of the <see cref="AsepriteSlice"/> during
    /// the <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>.
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignore.
    /// </remarks>
    public int CenterY;

    /// <summary>
    /// The width, in pixels, of the center bounds of the see <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignored.
    /// </remarks>
    public int CenterWidth;

    /// <summary>
    /// The height, in pixels, of the center bounds of the see <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/>
    /// </summary>
    /// <remarks>
    /// This value is only valid when <see cref="AsepriteSlice.IsNinePatch"/> is <see langword="true"/> for
    /// <see cref="Slice"/>, otherwise it should be ignored.
    /// </remarks>
    public int CenterHeight;

    /// <summary>
    /// The x-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/> in which the slice should pivot from.
    /// </summary>
    public int PivotX;

    /// <summary>
    /// The y-coordinate position relative to the top-left corner of the <see cref="AsepriteSlice"/> during the
    /// <see cref="AsepriteFrame"/> of this <see cref="AsepriteSliceKey"/> in which the slice should pivot from.
    /// </summary>
    public int PivotY;

    /// <summary>
    /// Creates a new instance of the <see cref="AsepriteSliceKey"/> class.
    /// </summary>
    public AsepriteSliceKey()
    {

    }

}
