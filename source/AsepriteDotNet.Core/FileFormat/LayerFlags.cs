// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.FileFormat;

/// <summary>
/// Defines flags for layer properties in the Layer Chunk (0x2004) of the Aseprite file format.
/// </summary>
[Flags]
internal enum LayerFlags : ushort
{
    /// <summary>
    /// No flags set; layer uses default properties.
    /// </summary>
    None = 0,

    /// <summary>
    /// Layer is visible and should be rendered in final output.
    /// </summary>
    Visible = 1,

    /// <summary>
    /// Layer can be modified through editor operations.
    /// </summary>
    Editable = 2,

    /// <summary>
    /// Layer position and transformations are locked against modification.
    /// </summary>
    Locked = 4,

    /// <summary>
    /// Layer serves as background layer with special rendering properties.
    /// </summary>
    Background = 8,

    /// <summary>
    /// Layer prefers using linked cels to optimize storage for repeated content.
    /// </summary>
    PrefersLinked = 16,

    /// <summary>
    /// Group layer should be displayed in collapsed state in layer hierarchy.
    /// </summary>
    Collapsed = 32,

    /// <summary>
    /// Layer serves as reference layer for drawing guidance and should not be included in final output.
    /// </summary>
    Reference = 64
}
