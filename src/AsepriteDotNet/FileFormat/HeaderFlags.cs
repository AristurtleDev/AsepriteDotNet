// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.FileFormat;

/// <summary>
/// Defines flags for the main header in the Aseprite file format that control layer property validation.
/// </summary>
[Flags]
internal enum HeaderFlags : uint
{
    /// <summary>
    /// Indicates that layer opacity values are valid and should be used during rendering.
    /// </summary>
    /// <remarks>
    /// When this flag is set, the opacity field in layer chunks contains valid data for
    /// image and tilemap layers. When not set, layer opacity should be treated as fully opaque (255).
    /// Group layer opacity is controlled by additional header flags beyond this basic validation.
    /// </remarks>
    LayerOpacityValid = 1
}
