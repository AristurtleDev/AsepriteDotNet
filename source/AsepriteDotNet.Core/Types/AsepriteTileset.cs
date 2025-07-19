//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a tileset in an Aseprite file.
/// </summary>
public sealed class AsepriteTileset
{
    internal Rgba32[] InternalPixels { get; set; }

    /// <summary>
    /// Gets the ID of this tileset.
    /// </summary>
    public int ID { get; internal set; }

    /// <summary>
    /// Gets the total number of tiles in this tileset.
    /// </summary>
    public int TileCount { get; internal set; }

    /// <summary>
    /// Gets the size of each tile in this tileset.
    /// </summary>
    public Size TileSize { get; internal set; }

    /// <summary>
    /// Gets the name of this tileset.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the collection of color value that represents the pixel data of the image of this tileset.  Order of color
    /// elements is from top-left pixel read left-to-right top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Rgba32> Pixels => InternalPixels;

    internal AsepriteTileset() { }
}
