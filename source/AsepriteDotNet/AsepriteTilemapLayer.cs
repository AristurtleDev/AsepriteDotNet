//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents a layer in an Aseprite file that tilemap cels are placed on.
/// </summary>
public sealed class AsepriteTilemapLayer : AsepriteLayer
{
    /// <summary>
    /// The <see cref="AsepriteTileset"/> that is used by all <see cref="AsepriteTilemapCel"/> elements on this
    /// <see cref="AsepriteTilemapLayer"/>.
    /// </summary>
    public AsepriteTileset Tileset { get; }

    internal AsepriteTilemapLayer(LayerProperties header, string name, AsepriteTileset tileset)
        : base(header, name)
    {
        Tileset = tileset;
    }
}
