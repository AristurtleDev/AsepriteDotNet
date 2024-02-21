//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteLayer"/> that <see cref="AsepriteImageCel"/> elements are placed on.
/// </summary>
public sealed class AsepriteImageLayer : AsepriteLayer
{
    internal AsepriteImageLayer(LayerProperties header, string name) : base(header, name) { }
}
