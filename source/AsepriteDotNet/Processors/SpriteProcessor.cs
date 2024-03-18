// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// 
/// </summary>
public static class SpriteProcessor
{
    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile"/> as a <see cref="Sprite"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile"/> to process the frame from.</param>
    /// <param name="frameIndex">The index of the frame to process.</param>
    /// <param name="onlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
    /// <param name="includeBackgroundLayer">
    /// Indicates whether the layer marked as the background layer should be processed.
    /// </param>
    /// <param name="includeTilemapLayers">Indicates whether tilemap layers should be included.</param>
    /// <returns>The <see cref="Sprite"/> created by this method.</returns>
    public static Sprite Process(AsepriteFile file, int frameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true)
    {
        AsepriteFrame aseFrame = file.Frames[frameIndex];
        Rgba32[] pixels = aseFrame.FlattenFrame(onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
        Texture texture = new Texture(aseFrame.Name, aseFrame.Size, pixels);
        Slice[] slices = ProcessorUtilities.GetSlicesForFrame(frameIndex, file.Slices);
        return new Sprite(aseFrame.Name, texture, slices);
    }

    
}
