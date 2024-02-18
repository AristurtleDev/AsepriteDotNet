//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents a single cel in a frame that contains tilemap data.
/// </summary>
public sealed class AsepriteTilemapCel : AsepriteCel
{
    private readonly AsepriteTile[] _tiles;

    /// <summary>
    /// The size of this <see cref="AsepriteTilemapCel"/>, in tiles.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// The total number of bits per tile for each tile in this cel.
    /// </summary>
    public int BitsPerTile { get; }

    /// <summary>
    /// The bitmask used to determine the ID of tiles for this cel.
    /// </summary>
    public uint TileIDBitmask { get; }

    /// <summary>
    /// The bitmask used to determine the horizontal flip property of the tiles in this cel.
    /// </summary>
    public uint FlipHorizontallyBitmask { get; }

    /// <summary>
    /// The bitmask used to determine the vertical flip property of the tiles in this cel.
    /// </summary>
    public uint FlipVerticallyBitmask { get; }

    /// <summary>
    /// The bitmask used to determine the diagonal flip property of the tiles in this cel.
    /// </summary>
    public uint FlipDiagonallyBitmask { get; }

    /// <summary>
    /// The collection of tiles that make up this cel.  Tile elements are in order of left-to-right, read
    /// top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AsepriteTile> Tiles => _tiles;

    internal AsepriteTilemapCel(Size size, int bitsPerTile, uint tileIDBitmask, uint flipHorizontallyBitmask, uint flipVerticallyBitmask, uint flipDiagonallyBitmask, AsepriteTile[] tiles, AsepriteLayer layer, Point position, int opacity, AsepriteUserData? userData)
        : base(layer, position, opacity, userData)
    {
        Size = size;
        BitsPerTile = bitsPerTile;
        TileIDBitmask = tileIDBitmask;
        FlipHorizontallyBitmask = flipHorizontallyBitmask;
        FlipVerticallyBitmask = flipVerticallyBitmask;
        FlipDiagonallyBitmask = flipDiagonallyBitmask;
        _tiles = [.. tiles];
    }

}
