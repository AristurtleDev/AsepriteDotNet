// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Runtime.Versioning;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="Sprite"/> from an <see cref="AsepriteFile"/>.
/// </summary>
public static class SpriteProcessor
{
    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile"/> as a <see cref="Sprite"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <param name="options">
    /// Optional options to use when processing.  If <see langword="null"/>, then
    /// <see cref="ProcessorOptions.Default"/> will be used.
    /// </param>
    /// <returns>The <see cref="Sprite"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    [Obsolete("This method will be removed in a future release.  Users should switch to one of the other appropriate Process methods", false)]
    public static Sprite Process(AsepriteFile file, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        options ??= ProcessorOptions.Default;

        return Process(file, frameIndex, options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);

    }

    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile"/> as a <see cref="Sprite"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be processed.</param>>
    /// <returns>The <see cref="Sprite"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite Process(AsepriteFile file, int frameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = false)
    {
        ArgumentNullException.ThrowIfNull(file);

        List<string> layers = new List<string>();
        for (int i = 0; i < file.Layers.Length; i++)
        {
            AsepriteLayer layer = file.Layers[i];
            if (onlyVisibleLayers && !layer.IsVisible) { continue; }
            if (!includeBackgroundLayer && layer.IsBackgroundLayer) { continue; }
            if (!includeTilemapLayers && layer is AsepriteTilemapLayer) { continue; }
            layers.Add(layer.Name);
        }

        return Process(file, frameIndex, layers);
    }

    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile"/> as a <see cref="Sprite"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <param name="layers">
    /// A collection containing the name of the layers to process.  Only cels on a layer who's name matches a name in
    /// this collection will be processed.
    /// </param>
    /// <returns>
    /// The <see cref="Sprite"/> created by this method.  If <paramref name="layers"/> is empty or contains zero
    /// elements, then <see cref="Sprite.Empty"/> is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite Process(AsepriteFile file, int frameIndex, ICollection<string> layers)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (layers is null || layers.Count == 0)
        {
            return Sprite.Empty;
        }

        AsepriteFrame aseFrame = file.Frames[frameIndex];
        Rgba32[] pixels = aseFrame.FlattenFrame(layers);
        Texture texture = new Texture(aseFrame.Name, aseFrame.Size, pixels);
        Slice[] slices = ProcessorUtilities.GetSlicesForFrame(frameIndex, file.Slices);
        return new Sprite(aseFrame.Name, texture, slices);
    }
}
