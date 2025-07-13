//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;
using AsepriteDotNet.Core.FileFormat.Data;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a slice starting on a specific frame.
/// </summary>
public sealed class AsepriteSliceKey
{
    /// <summary>
    /// Gets the index of the frame that the properties of this key are applied starting on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// Gets the rectangular bounds of the slice during this key.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Gets the bounds of the center of the slice during this key.
    /// </summary>
    public Rectangle CenterBounds { get; }

    /// <summary>
    /// Gets the xy-coordinate pivot point of the slice during this key.
    /// </summary>
    public Point Pivot { get; }

    internal AsepriteSliceKey(SliceKeyData sliceKeyData, NinePatchData? ninePatchData, PivotData? pivotData)
    {
        FrameIndex = (int)sliceKeyData.FrameNumber;

        Bounds = new Rectangle((int)sliceKeyData.X, (int)sliceKeyData.Y, (int)sliceKeyData.Width, (int)sliceKeyData.Height);

        //  If this is not a nine patch, make the center bounds equal to the key bounds.
        //  NOTE: Might want to make this all 0's instead. See what users say and update accordingly.
        CenterBounds = new Rectangle((int)(ninePatchData?.X ?? 0), (int)(ninePatchData?.Y ?? 0), (int)(ninePatchData?.Width ?? 0), (int)(ninePatchData?.Height ?? 0));

        //  If this did not have pivot data, make pivot (0, 0)
        Pivot = new Point((int)(pivotData?.X ?? 0), (int)(pivotData?.Y ?? 0));
    }
}
