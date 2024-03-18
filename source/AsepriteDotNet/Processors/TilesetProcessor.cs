// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;

namespace AsepriteDotNet.Processors;

public static class TilesetProcessor
{
    public static Tileset Process(AsepriteFile file, int tilesetIndex)
    {
        AsepriteTileset aseTileset = file.Tilesets[tilesetIndex];
        return Process(aseTileset);
    }

    public static Tileset Process(AsepriteFile file, string tilesetName)
    {
        AsepriteTileset aseTileset = file.GetTileset(tilesetName);
        return Process(aseTileset);
    }

    public static Tileset Process(AsepriteTileset aseTileset)
    {
        Texture texture = new Texture(aseTileset.Name, aseTileset.Size, aseTileset.Pixels.ToArray());
        return new Tileset(aseTileset.ID, aseTileset.Name, texture, aseTileset.Size);
    }
}
