// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="AnimatedTilemap{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
/// </summary>
public static class AnimatedTilemapProcessor
{
    /// <summary>
    /// Processes an <see cref="AnimatedTilemap{TColor}"/> from an <see cref="AsepriteFile{TColor}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process.</param>
    /// <returns>The <see cref="AnimatedTilemap{TColor}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate layer names are found.</exception>
    public static AnimatedTilemap<TColor> Process<TColor>(AsepriteFile<TColor> file)
        where TColor : struct, IColor<TColor> => Process(file, ProcessorOptions.Default);

    /// <summary>
    /// Processes an <see cref="AnimatedTilemap{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>
    /// <returns>The <see cref="AnimatedTilemap{TColor}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when duplicate layer names are found.</exception>
    public static AnimatedTilemap<TColor> Process<TColor>(AsepriteFile<TColor> file, ProcessorOptions options)
        where TColor : struct, IColor<TColor>
    {
        ArgumentNullException.ThrowIfNull(file);        

        List<Tileset<TColor>> tilesets = new List<Tileset<TColor>>();
        TilemapFrame[] frames = new TilemapFrame[file.Frames.Length];
        HashSet<int> tilesetIDCheck = new HashSet<int>();

        for (int f = 0; f < file.Frames.Length; f++)
        {
            AsepriteFrame<TColor> aseFrame = file.Frames[f];
            List<TilemapLayer> tilemapLayers = new List<TilemapLayer>();
            HashSet<string> layerNameCheck = new HashSet<string>();

            for (int c = 0; c < aseFrame.Cels.Length; c++)
            {
                //  Only care about tilemap cels
                if (aseFrame.Cels[c] is not AsepriteTilemapCel<TColor> aseTilemapCel) { continue; }

                //  Only continue if layer is visible or if explicitly told to include non-visible layers
                if (!aseTilemapCel.Layer.IsVisible && options.OnlyVisibleLayers) { continue; }

                Debug.Assert(aseTilemapCel.Layer is AsepriteTilemapLayer<TColor>);
                AsepriteTilemapLayer<TColor> aseTilemapLayer = (AsepriteTilemapLayer<TColor>)aseTilemapCel.Layer;

                //  Need to perform a check that we don't have duplicate layer names.  This is because Aseprite allows
                //  duplicate layer names, be we require unique names from this point on.
                if (!layerNameCheck.Add(aseTilemapLayer.Name))
                {
                    throw new InvalidOperationException($"Duplicate layer name '{aseTilemapLayer.Name}' found.  Layer names must be unique for tile maps");
                }

                if (tilesetIDCheck.Add(aseTilemapLayer.Tileset.ID))
                {
                    Tileset<TColor> tileset = TilesetProcessor.Process<TColor>(aseTilemapLayer.Tileset);
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

            frames[f] = new TilemapFrame(aseFrame.Duration, tilemapLayers.ToArray());
        }

        return new AnimatedTilemap<TColor>(file.Name, tilesets.ToArray(), frames);
    }
}
