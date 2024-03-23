// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="Sprite{TColor}"/> from an <see cref="AsepriteFile{TColor}"/>.
/// </summary>
public static class SpriteProcessor
{
    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile{TColor}"/> as a <see cref="Sprite{TColor}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <returns>The <see cref="Sprite{TColor}"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite<TColor> Process<TColor>(AsepriteFile<TColor> file, int frameIndex)
        where TColor : struct, IColor<TColor> => Process(file, frameIndex, ProcessorOptions.Default);

    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile{TColor}"/> as a <see cref="Sprite{TColor}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{TColor}"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>
    /// <returns>The <see cref="Sprite{TColor}"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite<TColor> Process<TColor>(AsepriteFile<TColor> file, int frameIndex, ProcessorOptions options)
        where TColor : struct, IColor<TColor>
    {
        ArgumentNullException.ThrowIfNull(file);

        AsepriteFrame<TColor> aseFrame = file.Frames[frameIndex];
        TColor[] pixels = aseFrame.FlattenFrame<TColor>(options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
        Texture<TColor> texture = new Texture<TColor>(aseFrame.Name, aseFrame.Size, pixels);
        Slice<TColor>[] slices = ProcessorUtilities.GetSlicesForFrame<TColor>(frameIndex, file.Slices);
        return new Sprite<TColor>(aseFrame.Name, texture, slices);
    }


}
