//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a slice starting on a specific frame.
/// </summary>
public sealed class SliceKey
{
    /// <summary>
    /// Gets the index of the frame that the properties of this key are applied starting on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// Gets the top-left x-coordinate position of the bounds of the slice during this key.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the top-left y-coordinate position of the bounds of the slice during this key.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the width, in pixels, of the bounds of the slice during this key.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height, in pixels, of the bounds of the slice during this key.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the top-left x-coordinate position of the center bounds of the slice during this key.
    /// </summary>
    public int CenterX { get; }

    /// <summary>
    /// Gets the top-left y-coordinate position of the center bounds of the slice during this key.
    /// </summary>
    public int CenterY { get; }

    /// <summary>
    /// Gets the width, in pixels, of the center bounds of the slice during this key.
    /// </summary>
    public int CenterWidth { get; }

    /// <summary>
    /// Gets the height, in pixels, of the center bounds of the slice during this key.
    /// </summary>
    public int CenterHeight { get; }

    /// <summary>
    /// Gets the x-coordinate position relative to the top-left corner of the slice in which the slice should pivot
    /// from during this key.
    /// </summary>
    public int PivotX { get; }

    /// <summary>
    /// Gets the y-coordinate position relative to the top-left corner of the slice in which the slice should pivot
    /// from during this key.
    /// </summary>
    public int PivotY { get; }

    internal SliceKey(SliceKeyProperties keyProperties, NinePatchProperties? ninePatchProperties, PivotProperties? pivotProperties)
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
