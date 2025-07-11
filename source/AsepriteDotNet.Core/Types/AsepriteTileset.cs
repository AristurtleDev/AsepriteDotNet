//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;
using AsepriteDotNet.Core.Document;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a tileset in an Aseprite file.
/// </summary>
public sealed class AsepriteTileset
{
    private readonly Rgba32[] _pixels;

    /// <summary>
    /// Gets the ID of this tileset.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets the total number of tiles in this tileset.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    /// Gets the size of this tileset.
    /// </summary>
    [Obsolete("Use TileSize instead as it more accurately describes this property.  This will be removed in a future release")]
    public Size Size { get; }

    /// <summary>
    /// Gets the size of each tile in this tileset.
    /// </summary>
    public Size TileSize { get; }


    /// <summary>
    /// Gets the name of this tileset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the collection of color value that represents the pixel data of the image of this tileset.  Order of color
    /// elements is from top-left pixel read left-to-right top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Rgba32> Pixels => _pixels;

    internal AsepriteTileset(AsepriteTilesetProperties tilesetProperties, string name, Rgba32[] pixels)
    {
        ID = (int)tilesetProperties.Id;
        TileCount = (int)tilesetProperties.NumberOfTiles;
#pragma warning disable CS0618 // Type or member is obsolete
        Size = new Size(tilesetProperties.TileWidth, tilesetProperties.TileHeight);
#pragma warning restore CS0618 // Type or member is obsolete
        TileSize = new Size(tilesetProperties.TileWidth, tilesetProperties.TileHeight);
        Name = name;
        _pixels = pixels;
    }
}
