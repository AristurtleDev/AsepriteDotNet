// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.FileFormat;

/// <summary>
/// Defines flags for the Cel Extra Chunk (0x2006) in the Aseprite file format.
/// </summary>
[Flags]
internal enum CelExtraFlags : uint
{
    /// <summary>
    /// Precise bounds are set for the cel.
    /// </summary>
    PreciseBoundsSet = 1
}
