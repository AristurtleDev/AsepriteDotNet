// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.FileFormat;

/// <summary>
/// Defines flags for slice properties in the Slice Chunk (0x2022) of the Aseprite file format.
/// </summary>
[Flags]
internal enum SliceFlags : uint
{
    /// <summary>
    /// Slice includes 9-patch scaling information with center bounds.
    /// </summary>
    IsNinePatch = 1,

    /// <summary>
    /// Slice includes pivot point information for transformation operations.
    /// </summary>
    HasPivot = 2
}
