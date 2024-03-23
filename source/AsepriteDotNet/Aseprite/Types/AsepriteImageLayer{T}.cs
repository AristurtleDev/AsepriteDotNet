//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that image cels are placed on.  This class cannot be
/// inherited.
/// </summary>
public sealed class AsepriteImageLayer<TColor> : AsepriteLayer<TColor> where TColor : struct, IColor<TColor>
{
    internal AsepriteImageLayer(AsepriteLayerProperties header, string name) : base(header, name) { }
}
