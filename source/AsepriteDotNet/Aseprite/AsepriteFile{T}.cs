//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite;

/// <summary>
/// Represents the contents loaded from an Aseprite file.
/// </summary>
public class AsepriteFile<TColor> where TColor : struct, IColor<TColor>
{
    private readonly AsepriteFrame<TColor>[] _frames;
    private readonly AsepriteLayer<TColor>[] _layers;
    private readonly AsepriteTag<TColor>[] _tags;
    private readonly AsepriteSlice<TColor>[] _slices;
    private readonly AsepriteTileset<TColor>[] _tilesets;
    private readonly string[] _warnings;

    /// <summary>
    /// Gets the width of the canvas, in pixels.
    /// </summary>
    public int CanvasWidth { get; }

    /// <summary>
    /// Gets the height of the canvas, in pixels.
    /// </summary>
    public int CanvasHeight { get; }

    /// <summary>
    /// Gets the <see cref="Aseprite.AsepriteColorDepth"/> mode used in Aseprite.
    /// </summary>
    public AsepriteColorDepth ColorDepth { get; }

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteFrame{TColor}"/> elements in this
    /// <see cref="AsepriteFile{TColor}"/>.  Order of elements is from first-to-last.
    /// </summary>
    public ReadOnlySpan<AsepriteFrame<TColor>> Frames => _frames;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteLayer{TColor}"/> elements in this
    /// <see cref="AsepriteFile{TColor}"/>.  Order of elements is from bottom-to-top.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer<TColor>> Layers => _layers;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteTag{TColor}"/> elements in this
    /// <see cref="AsepriteFile{TColor}"/>.  ORder of elements is as defined in the Aseprite UI from left-to-right.
    /// </summary>
    public ReadOnlySpan<AsepriteTag<TColor>> Tags => _tags;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteSlice{TColor}"/> elements in this
    /// <see cref="AsepriteFile{TColor}"/>.  Order of elements is in the order they were created in Aseprite.
    /// </summary>
    public ReadOnlySpan<AsepriteSlice<TColor>> Slices => _slices;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteTileset{TColor}"/> element in this
    /// <see cref="AsepriteFile{TColor}"/>.
    /// </summary>
    public ReadOnlySpan<AsepriteTileset<TColor>> Tilesets => _tilesets;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of any warnings issued when the Aseprite file was parsed to create this
    /// <see cref="AsepriteFile{TColor}"/> instance.  You can use this to see if there were any non-fatal errors that
    /// occurred while parsing the file.
    /// </summary>
    public ReadOnlySpan<string> Warnings => _warnings;

    /// <summary>
    /// Gets the <see cref="AsepritePalette{TColor}"/> for this <see cref="AsepriteFile{TColor}"/>.
    /// </summary>
    public AsepritePalette<TColor> Palette { get; }

    /// <summary>
    /// Gets the <see cref="AsepriteUserData{TColor}"/> that was set in the properties for the sprite in Aseprite.
    /// </summary>
    public AsepriteUserData<TColor> UserData { get; }

    /// <summary>
    /// Gets the name of the Aseprite file (without the extension).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the total number of <see cref="AsepriteFrame{TColor}"/> elements in this file.
    /// </summary>
    public int FrameCount => _frames.Length;

    internal AsepriteFile(string name, AsepritePalette<TColor> palette, int canvasWidth, int canvasHeight, AsepriteColorDepth colorDepth, List<AsepriteFrame<TColor>> frames, List<AsepriteLayer<TColor>> layers, List<AsepriteTag<TColor>> tags, List<AsepriteSlice<TColor>> slices, List<AsepriteTileset<TColor>> tilesets, AsepriteUserData<TColor> userData, List<string> warnings)
    {
        Name = name;
        CanvasWidth = canvasWidth;
        CanvasHeight = canvasHeight;
        ColorDepth = colorDepth;
        _frames = [.. frames];
        _tags = [.. tags];
        _slices = [.. slices];
        _tilesets = [.. tilesets];
        _warnings = [.. warnings];
        _layers = [.. layers];
        Palette = palette;
        UserData = userData;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteFrame{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteFrame{TColor}"/> to get.</param>
    /// <returns>The <see cref="AsepriteFrame{TColor}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of frames.
    /// </exception>
    public AsepriteFrame<TColor> GetFrame(int index) => _frames[index];

    /// <summary>
    /// Gets the <see cref="AsepriteFrame{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteFrame{TColor}"/> to get.</param>
    /// <param name="frame">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteFrame{TColor}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteFrame{TColor}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteFrame{TColor}"/> elements in this file.
    /// </returns>
    public bool TryGetFrame(int index, [NotNullWhen(true)] out AsepriteFrame<TColor>? frame)
    {
        frame = default;
        try { frame = _frames[index]; }
        catch (ArgumentOutOfRangeException) { }
        return frame is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteLayer{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteLayer{TColor}"/> to get.</param>
    /// <returns>The <see cref="AsepriteLayer{TColor}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of layers.
    /// </exception>
    public AsepriteLayer<TColor> GetLayer(int index) => _layers[index];

    /// <summary>
    /// Returns the <see cref="AsepriteLayer{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteLayer{TColor}"/>.</param>
    /// <returns>The <see cref="AsepriteLayer{TColor}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteLayer{TColor}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteLayer<TColor> GetLayer(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _layers.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(layer => layer.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteLayer{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteLayer{TColor}"/> to get.</param>
    /// <param name="layer">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteLayer{TColor}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteLayer{TColor}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteLayer{TColor}"/> elements in this file.
    /// </returns>
    public bool TryGetLayer(int index, [NotNullWhen(true)] out AsepriteLayer<TColor>? layer)
    {
        layer = default;
        try { layer = _layers[index]; }
        catch (ArgumentOutOfRangeException) { }
        return layer is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteLayer{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteLayer{TColor}"/></param>
    /// <param name="layer">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteLayer{TColor}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteLayer{TColor}"/> with the specified name was found in this
    /// file; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out AsepriteLayer<TColor>? layer)
    {
        layer = _layers.AsParallel()
                       .WithDegreeOfParallelism(Environment.ProcessorCount)
                       .FirstOrDefault(layer => layer.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return layer is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteTag{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTag{TColor}"/> to get.</param>
    /// <returns>The <see cref="AsepriteTag{TColor}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of tags.
    /// </exception>
    public AsepriteTag<TColor> GetTag(int index) => _tags[index];

    /// <summary>
    /// Returns the <see cref="AsepriteTag{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteTag{TColor}"/>.</param>
    /// <returns>The <see cref="AsepriteTag{TColor}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteTag{TColor}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteTag<TColor> GetTag(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _tags.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(tag => tag.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTag{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTag{TColor}"/> to get.</param>
    /// <param name="tag">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTag{TColor}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteTag{TColor}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteTag{TColor}"/> elements in this file.
    /// </returns>
    public bool TryGetTag(int index, [NotNullWhen(true)] out AsepriteTag<TColor>? tag)
    {
        tag = default;
        try { tag = _tags[index]; }
        catch (ArgumentOutOfRangeException) { }
        return tag is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTag{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteTag{TColor}"/></param>
    /// <param name="tag">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTag{TColor}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteTag{TColor}"/> with the specified name was found in this file;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTag(string name, [NotNullWhen(true)] out AsepriteTag<TColor>? tag)
    {
        tag = _tags.AsParallel()
                   .WithDegreeOfParallelism(Environment.ProcessorCount)
                   .FirstOrDefault(tag => tag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return tag is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteSlice{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteSlice{TColor}"/> to get.</param>
    /// <returns>The <see cref="AsepriteSlice{TColor}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of slices.
    /// </exception>
    public AsepriteSlice<TColor> GetSlice(int index) => _slices[index];

    /// <summary>
    /// Returns the <see cref="AsepriteSlice{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteSlice{TColor}"/>.</param>
    /// <returns>The <see cref="AsepriteSlice{TColor}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteSlice{TColor}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteSlice<TColor> GetSlice(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _slices.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(slice => slice.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteSlice{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteSlice{TColor}"/> to get.</param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteSlice{TColor}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteSlice{TColor}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteSlice{TColor}"/> elements in this file.
    /// </returns>
    public bool TryGetSlice(int index, [NotNullWhen(true)] out AsepriteSlice<TColor>? slice)
    {
        slice = default;
        try { slice = _slices[index]; }
        catch (ArgumentOutOfRangeException) { }
        return slice is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteSlice{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteSlice{TColor}"/></param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteSlice{TColor}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteSlice{TColor}"/> with the specified name was found in this
    /// file; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetSlice(string name, [NotNullWhen(true)] out AsepriteSlice<TColor>? slice)
    {
        slice = _slices.AsParallel()
                       .WithDegreeOfParallelism(Environment.ProcessorCount)
                       .FirstOrDefault(slice => slice.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return slice is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteTileset{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTileset{TColor}"/> to get.</param>
    /// <returns>The <see cref="AsepriteTileset{TColor}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of tilesets.
    /// </exception>
    public AsepriteTileset<TColor> GetTileset(int index) => _tilesets[index];

    /// <summary>
    /// Returns the <see cref="AsepriteTileset{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteTileset{TColor}"/>.</param>
    /// <returns>The <see cref="AsepriteTileset{TColor}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteTileset{TColor}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteTileset<TColor> GetTileset(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _tilesets.AsParallel()
                        .WithDegreeOfParallelism(Environment.ProcessorCount)
                        .First(tileset => tileset.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTileset{TColor}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTileset{TColor}"/> to get.</param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTileset{TColor}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteTileset{TColor}"/> was found; otherwise
    /// <see langword="false"/>.  This method returns <see langword="false"/> if <paramref name="index"/> is less than
    /// zero or is greater than or equal to the total number of <see cref="AsepriteTileset{TColor}"/> elements in this
    /// file.
    /// </returns>
    public bool TryGetTileset(int index, [NotNullWhen(true)] out AsepriteTileset<TColor>? slice)
    {
        slice = default;
        try { slice = _tilesets[index]; }
        catch (ArgumentOutOfRangeException) { }
        return slice is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTileset{TColor}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteTileset{TColor}"/></param>
    /// <param name="tileset">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTileset{TColor}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteTileset{TColor}"/> with the specified name was found in this
    /// file; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTileset(string name, [NotNullWhen(true)] out AsepriteTileset<TColor>? tileset)
    {
        tileset = _tilesets.AsParallel()
                           .WithDegreeOfParallelism(Environment.ProcessorCount)
                           .FirstOrDefault(tileset => tileset.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return tileset is not null;
    }
}
