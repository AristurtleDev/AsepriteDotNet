// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a texture that is composed of color values that represent an image.
/// </summary>
/// <param name="Name">The name of the texture.</param>
/// <param name="Size">The size of the texture.</param>
/// <param name="Pixels">
/// The color values that represent the image of the texture.  Color values are red from left-to-right top-to-bottom
/// </param>
public record Texture(string Name, Size Size, Rgba32[] Pixels);
