// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines a processor for processing a <see cref="Sprite{T}"/> from an <see cref="AsepriteFile{T}"/>.
/// </summary>
public static class SpriteProcessor
{
    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile{T}"/> as a <see cref="Sprite{T}"/> using the
    /// <see cref="ProcessorOptions.Default"/> options.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <returns>The <see cref="Sprite{T}"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite<T> Process<T>(AsepriteFile<T> file, int frameIndex)
        where T: IColor, new() => Process(file, frameIndex, ProcessorOptions.Default);

    /// <summary>
    /// Processes a frame from an <see cref="AsepriteFile{T}"/> as a <see cref="Sprite{T}"/>.
    /// </summary>
    /// <param name="file">The <see cref="AsepriteFile{T}"/> to process the frame from.</param>
    /// <param name="frameIndex">The zero-based index of the frame to process.</param>
    /// <param name="options">The <see cref="ProcessorOptions"/> to use when processing</param>
    /// <returns>The <see cref="Sprite{T}"/> created by this method.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="file"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frameIndex"/> is less than zero or is greater than or equal to the total number of
    /// frames in the file.
    /// </exception>
    public static Sprite<T> Process<T>(AsepriteFile<T> file, int frameIndex, ProcessorOptions options)
        where T: IColor, new()
    {
        ArgumentNullException.ThrowIfNull(file);

        AsepriteFrame<T> aseFrame = file.Frames[frameIndex];
        T[] pixels = aseFrame.FlattenFrame<T>(options.OnlyVisibleLayers, options.IncludeBackgroundLayer, options.IncludeTilemapLayers);
        Texture<T> texture = new Texture<T>(aseFrame.Name, aseFrame.Size, pixels);
        Slice<T>[] slices = ProcessorUtilities.GetSlicesForFrame<T>(frameIndex, file.Slices);
        return new Sprite<T>(aseFrame.Name, texture, slices);
    }


}
