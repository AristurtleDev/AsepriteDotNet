//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information


//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using AsepriteDotNet.Core.FileFormat.Data;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that tilemap cels are placed on.  This class cannot be
/// inherited.
/// </summary>
public sealed class AsepriteTilemapLayer : AsepriteLayer
{
    /// <summary>
    /// Gets the tileset that is used by all tilemap cels on this tilemap layer.
    /// </summary>
    public AsepriteTileset Tileset { get; }

    internal AsepriteTilemapLayer(LayerData layerData, string name, AsepriteTileset tileset)
        : base(layerData, name)
    {
        Tileset = tileset;
    }
}
