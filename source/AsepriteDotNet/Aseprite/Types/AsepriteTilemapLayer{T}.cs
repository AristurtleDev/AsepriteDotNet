//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that tilemap cels are placed on.  This class cannot be
/// inherited.
/// </summary>
public sealed class AsepriteTilemapLayer<TColor> : AsepriteLayer<TColor> where TColor : struct, IColor<TColor>
{
    /// <summary>
    /// Gets the tileset that is used by all tilemap cels on this tilemap layer.
    /// </summary>
    public AsepriteTileset<TColor> Tileset { get; }

    internal AsepriteTilemapLayer(AsepriteLayerProperties header, string name, AsepriteTileset<TColor> tileset)
        : base(header, name)
    {
        Tileset = tileset;
    }
}
