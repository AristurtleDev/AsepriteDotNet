// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines a texture atlas that is composed of a texture with defined texture regions.
/// </summary>
/// <param name="Name">The name of the texture atlas.</param>
/// <param name="Texture">The source texture of the texture atlas.</param>
/// <param name="TextureRegions">The texture regions within the texture.</param>
public record TextureAtlas(string Name, Texture Texture, TextureRegion[] TextureRegions);
