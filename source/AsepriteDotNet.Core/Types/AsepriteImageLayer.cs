//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core.FileFormat.Data;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that
/// Defines the properties of a layer in an Aseprite file that image cels are placed on.  This class cannot be
/// inherited.
/// </summary>
public sealed class AsepriteImageLayer : AsepriteLayer
{
    internal AsepriteImageLayer(LayerData layerData, string name) : base(layerData, name) { }
}
