//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteLayer"/> that <see cref="AsepriteImageCel"/> elements are placed on.
/// </summary>
public sealed class AsepriteImageLayer : AsepriteLayer
{
    internal AsepriteImageLayer(string name, bool isVisible, bool isBackground, bool isReference, int childLevel, AsepriteBlendMode blendMode, int opacity)
        : base(name, isVisible, isBackground, isReference, childLevel, blendMode, opacity) { }
}
