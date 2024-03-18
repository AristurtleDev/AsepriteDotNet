// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Defines a rectangular region within a larger texture that represents a sub texture.
/// </summary>
/// <param name="Name">The name of the texture region.</param>
/// <param name="Bounds">The bounds of the texture region relative to the bounds of the source texture.</param>
/// <param name="Slices">The slices contained within the region, relative to the bounds of the region.</param>
public record TextureRegion(string Name, Rectangle Bounds, Slice[] Slices);
