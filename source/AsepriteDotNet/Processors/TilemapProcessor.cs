// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="Tilemap"/> from an <see cref="AsepriteFile"/>.
/// </summary>
public static class TilemapProcessor
{
    /// <summary>
    /// Processes a <see cref="Tilemap"/> from an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process.</param>
    /// <param name="frameIndex">The index of the frame containing the tilemap to process.</param>
    /// <param name="options">
    /// Optional options to use when processing.  If <see langword="null"/>, then
    /// <see cref="ProcessorOptions.Default"/> will be used.
    /// </param>
    /// <returns>The <see cref="Tilemap"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate layer names are found.</exception>
    public static Tilemap Process(AsepriteFile file, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        options ??= ProcessorOptions.Default;

        AsepriteFrame aseFrame = file.Frames[frameIndex];

        List<Tileset> tilesets = new List<Tileset>();
        List<TilemapLayer> tilemapLayers = new List<TilemapLayer>();
        HashSet<string> layerNameCheck = new HashSet<string>();
        HashSet<int> tilesetIDCheck = new HashSet<int>();

        for (int c = 0; c < aseFrame.Cels.Length; c++)
        {
            //  Only care about tilemap cels
            if (aseFrame.Cels[c] is not AsepriteTilemapCel aseTilemapCel) { continue; }

            //  Only continue if layer is visible or if explicitly told to include non-visible layers
            if (!aseTilemapCel.Layer.IsVisible && options.OnlyVisibleLayers) { continue; }

            Debug.Assert(aseTilemapCel.Layer is AsepriteTilemapLayer);
            AsepriteTilemapLayer aseTilemapLayer = (AsepriteTilemapLayer)aseTilemapCel.Layer;

            //  Need to perform a check that we don't have duplicate layer names.  This is because Aseprite allows
            //  duplicate layer names, be we require unique names from this point on.
            if (!layerNameCheck.Add(aseTilemapLayer.Name))
            {
                throw new InvalidOperationException($"Duplicate layer name '{aseTilemapLayer.Name}' found.  Layer names must be unique for tile maps");
            }

            if (tilesetIDCheck.Add(aseTilemapLayer.Tileset.ID))
            {
                Tileset tileset = TilesetProcessor.Process(aseTilemapLayer.Tileset);
                tilesets.Add(tileset);
            }

            TilemapTile[] tiles = new TilemapTile[aseTilemapCel.Tiles.Length];

            for (int t = 0; t < aseTilemapCel.Tiles.Length; t++)
            {
                AsepriteTile aseTile = aseTilemapCel.Tiles[t];
                tiles[t] = new TilemapTile(aseTile.ID, aseTile.FlipHorizontally, aseTile.FlipVertically, aseTile.FlipDiagonally);
            }

            TilemapLayer tilemapLayer = new TilemapLayer(aseTilemapLayer.Name, aseTilemapLayer.Tileset.ID, aseTilemapCel.Size.Width, aseTilemapCel.Size.Height, aseTilemapCel.Location, tiles);
            tilemapLayers.Add(tilemapLayer);
        }

        return new Tilemap(file.Name, [.. tilesets], [.. tilemapLayers]);
    }
}
