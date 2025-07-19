//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;
using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a single frame in an Aseprite animation containing cels, timing, and composition data.
/// </summary>
/// <remarks>
/// Frames are the fundamental units of sprite animation, each containing a collection of cels
/// that are composited together to form the final rendered image. The frame duration determines
/// how long this frame is displayed during animation playback.
/// </remarks>
public sealed class AsepriteFrame
{
    internal List<AsepriteCel> InternalCels { get; } = [];

    /// <summary>
    /// Gets the collection of cels that compose this frame.
    /// </summary>
    /// <remarks>
    /// Cels are composited from back to front using their layer hierarchy and Z-index values
    /// to determine the final rendering order. Empty frames contain no cels.
    /// </remarks>
    public ReadOnlySpan<AsepriteCel> Cels => CollectionsMarshal.AsSpan(InternalCels);

    /// <summary>
    /// Gets the name assigned to this frame.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the dimensions of the frame canvas.
    /// </summary>
    /// <remarks>
    /// Frame size typically matches the sprite's canvas size but may differ in specific
    /// composition scenarios. Cels can extend beyond these bounds using negative coordinates.
    /// </remarks>
    public Size Size { get; internal set; }

    /// <summary>
    /// Gets the display duration for this frame during animation playback.
    /// </summary>
    /// <remarks>
    /// Duration is specified in the frame header and overrides any global speed setting
    /// from the sprite header. Minimum duration is typically 1 millisecond.
    /// </remarks>
    public TimeSpan Duration { get; internal set; }

    /// <summary>
    /// Gets the zero-based index of this frame in the original sprite sequence.
    /// </summary>
    /// <remarks>
    /// This index corresponds to the frame's position as it appears in the Aseprite file
    /// and is used for frame references in linked cels and animation tags.
    /// </remarks>
    public int OriginalIndex { get; internal set; }

    internal AsepriteFrame() { }
}
