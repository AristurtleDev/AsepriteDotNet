//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a tileset in an Aseprite file.
/// </summary>
public sealed class Tileset
{
    private readonly AseColor[] _pixels;

    /// <summary>
    /// Gets the ID of this tileset.
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// Gets the total number of tiles in this tileset.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    /// Gets the width, in pixels, of each tile in this tileset.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height, in pixels, of each tile in this tileset.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the name of this tileset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the collection of color value that represents the pixel data of the image of this tileset.  Order of color
    /// elements is from top-left pixel read left-to-right top-to-bottom.
    /// </summary>
    public ReadOnlySpan<AseColor> Pixels => _pixels;

    internal Tileset(TilesetProperties tilesetProperties, string name, AseColor[] pixels)
    {
        ID = (int)tilesetProperties.Id;
        TileCount = (int)tilesetProperties.NumberOfTiles;
        Width = (int)tilesetProperties.TileWidth;
        Height = (int)tilesetProperties.TileHeight;
        Name = name;
        _pixels = pixels;
    }
}
