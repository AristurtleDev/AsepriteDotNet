//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a normal image layer that contains pixel-based artwork and supports full RGBA color blending.
/// </summary>
/// <remarks>
/// Image layers are the standard layer type for traditional sprite artwork and animation frames.
/// They support compressed or raw image data that can be painted, transformed, and composited using
/// various blend modes. Each cel within this layer contains RGBA pixel data organized in row-major format.
/// </remarks>
public sealed class AsepriteImageLayer : AsepriteLayer
{
    internal AsepriteImageLayer() { }
}
