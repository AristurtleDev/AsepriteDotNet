//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that tilemap cels are placed on.  This class cannot be
/// inherited.
/// </summary>
public sealed class TilemapLayer : Layer
{
    /// <summary>
    /// Gets the tileset that is used by all tilemap cels on this tilemap layer.
    /// </summary>
    public Tileset Tileset { get; }

    internal TilemapLayer(LayerProperties header, string name, Tileset tileset)
        : base(header, name)
    {
        Tileset = tileset;
    }
}
