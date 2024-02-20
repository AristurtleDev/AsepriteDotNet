//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents a single cel in a frame that contains tilemap data.
/// </summary>
public sealed class AsepriteTilemapCel : AsepriteCel
{
    private readonly AsepriteTile[] _tiles;

    /// <summary>
    /// Gets the width of this <see cref="AsepriteTilemapCel"/>, in the number of tiles that fit in the cel on the
    /// horizontal (x) axis.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of this <see cref="AsepriteTilemapCel"/>, in the number of tiles that fit in the cel on the
    /// vertical (y) axis.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// The collection of tiles that make up this cel.  Tile elements are in order of left-to-right, read
    /// top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AsepriteTile> Tiles => _tiles;

    internal AsepriteTilemapCel(CelProperties celProperties, AsepriteLayer layer, TilemapCelProperties tilemapCelProperties, AsepriteTile[] tiles)
        : base(celProperties, layer)
    {
        Width = tilemapCelProperties.Width;
        Height = tilemapCelProperties.Height;
        _tiles = tiles;
    }

}
