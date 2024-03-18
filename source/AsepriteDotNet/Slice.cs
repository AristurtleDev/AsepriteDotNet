// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Common;

namespace AsepriteDotNet;

/// <summary>
/// Represents a slice defined by it's xy-coordinate position, width, height, and origin.
/// </summary>
/// <param name="Name">The name of this slice.</param>
/// <param name="Bounds">Gets the bounds of this slice.</param>
/// <param name="Origin">Gets the xy-coordinate origin point of this slice.</param>
/// <param name="Color">The color of this slice.</param>
public record Slice(string Name, Rectangle Bounds, Point Origin, Rgba32 Color) { }
