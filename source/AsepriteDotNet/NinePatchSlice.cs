// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Represents a slice with nine-patch data.  This record cannot be inherited.
/// </summary>
/// <param name="Name">The name of this slice.</param>
/// <param name="Bounds">Gets the bounds of this slice.</param>
/// <param name="Origin">Gets the xy-coordinate origin point of this slice.</param>
/// <param name="Color">The color of this slice.</param>
/// <param name="CenterBounds">Gets the bounds of the center of this slice.</param>
public sealed record NinePatchSlice(string Name, Rectangle Bounds, Point Origin, Rgba32 Color, Rectangle CenterBounds)
    : Slice(Name, Bounds, Origin, Color)
{ }
