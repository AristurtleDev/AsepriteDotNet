// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a sprite texture.
/// </summary>
/// <param name="Name">The name of the sprite.</param>
/// <param name="Texture">The source texture of the sprite.</param>
/// <param name="Slices">The slices contained within this sprite.</param>
public record Sprite(string Name, Texture Texture, Slice[] Slices) { }
