//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that
/// Defines the properties of a layer in an Aseprite file that image cels are placed on.  This class cannot be
/// inherited.
/// </summary>
public sealed class ImageLayer : Layer
{
    internal ImageLayer(LayerProperties header, string name) : base(header, name) { }
}
