// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Defines a sprite sheet that contains a texture atlas and animation tag definitions.
/// </summary>
/// <param name="Name">The name of the sprite sheet.</param>
/// <param name="TextureAtlas">The source texture atlas.</param>
/// <param name="AniamtionTags">The animation tag definitions.</param>
public record SpriteSheet(string Name, TextureAtlas TextureAtlas, AnimationTag[] AniamtionTags);

