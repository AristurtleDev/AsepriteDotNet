// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

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
    /// <returns>The <see cref="Sprite"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite Process(AsepriteFile file, int frameIndex, ProcessorOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        options ??= ProcessorOptions.Default;

        AsepriteFrame aseFrame = file.Frames[frameIndex];
        Rgba32[] pixels = aseFrame.FlattenFrame(options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
        Texture texture = new Texture(aseFrame.Name, aseFrame.Size, pixels);
        Slice[] slices = ProcessorUtilities.GetSlicesForFrame(frameIndex, file.Slices);
        return new Sprite(aseFrame.Name, texture, slices);
    }

    
}
