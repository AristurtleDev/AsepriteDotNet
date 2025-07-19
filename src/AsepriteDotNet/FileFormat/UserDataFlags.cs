// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.FileFormat;

/// <summary>
/// Defines flags for user data content in the User Data Chunk (0x2020) of the Aseprite file format.
/// </summary>
[Flags]
internal enum UserDataFlags : uint
{
    /// <summary>
    /// User data includes a text description string.
    /// </summary>
    HasText = 1,

    /// <summary>
    /// User data includes RGBA color information.
    /// </summary>
    HasColor = 2,

    /// <summary>
    /// User data includes typed property maps.
    /// </summary>
    HasProperties = 4
}
