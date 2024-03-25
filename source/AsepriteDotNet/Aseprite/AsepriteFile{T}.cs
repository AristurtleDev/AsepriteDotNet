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
public class AsepriteFile<T> where T: IColor, new()
{
    private readonly AsepriteFrame<T>[] _frames;
    private readonly AsepriteLayer<T>[] _layers;
    private readonly AsepriteTag<T>[] _tags;
    private readonly AsepriteSlice<T>[] _slices;
    private readonly AsepriteTileset<T>[] _tilesets;
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
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteFrame{T}"/> elements in this
    /// <see cref="AsepriteFile{T}"/>.  Order of elements is from first-to-last.
    /// </summary>
    public ReadOnlySpan<AsepriteFrame<T>> Frames => _frames;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteLayer{T}"/> elements in this
    /// <see cref="AsepriteFile{T}"/>.  Order of elements is from bottom-to-top.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer<T>> Layers => _layers;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteTag{T}"/> elements in this
    /// <see cref="AsepriteFile{T}"/>.  ORder of elements is as defined in the Aseprite UI from left-to-right.
    /// </summary>
    public ReadOnlySpan<AsepriteTag<T>> Tags => _tags;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteSlice{T}"/> elements in this
    /// <see cref="AsepriteFile{T}"/>.  Order of elements is in the order they were created in Aseprite.
    /// </summary>
    public ReadOnlySpan<AsepriteSlice<T>> Slices => _slices;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteTileset{T}"/> element in this
    /// <see cref="AsepriteFile{T}"/>.
    /// </summary>
    public ReadOnlySpan<AsepriteTileset<T>> Tilesets => _tilesets;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of any warnings issued when the Aseprite file was parsed to create this
    /// <see cref="AsepriteFile{T}"/> instance.  You can use this to see if there were any non-fatal errors that
    /// occurred while parsing the file.
    /// </summary>
    public ReadOnlySpan<string> Warnings => _warnings;

    /// <summary>
    /// Gets the <see cref="AsepritePalette{T}"/> for this <see cref="AsepriteFile{T}"/>.
    /// </summary>
    public AsepritePalette<T> Palette { get; }

    /// <summary>
    /// Gets the <see cref="AsepriteUserData{T}"/> that was set in the properties for the sprite in Aseprite.
    /// </summary>
    public AsepriteUserData<T> UserData { get; }

    /// <summary>
    /// Gets the name of the Aseprite file (without the extension).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the total number of <see cref="AsepriteFrame{T}"/> elements in this file.
    /// </summary>
    public int FrameCount => _frames.Length;

    internal AsepriteFile(string name, AsepritePalette<T> palette, int canvasWidth, int canvasHeight, AsepriteColorDepth colorDepth, List<AsepriteFrame<T>> frames, List<AsepriteLayer<T>> layers, List<AsepriteTag<T>> tags, List<AsepriteSlice<T>> slices, List<AsepriteTileset<T>> tilesets, AsepriteUserData<T> userData, List<string> warnings)
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
    /// Returns the <see cref="AsepriteFrame{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteFrame{T}"/> to get.</param>
    /// <returns>The <see cref="AsepriteFrame{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of frames.
    /// </exception>
    public AsepriteFrame<T> GetFrame(int index) => _frames[index];

    /// <summary>
    /// Gets the <see cref="AsepriteFrame{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteFrame{T}"/> to get.</param>
    /// <param name="frame">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteFrame{T}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteFrame{T}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteFrame{T}"/> elements in this file.
    /// </returns>
    public bool TryGetFrame(int index, [NotNullWhen(true)] out AsepriteFrame<T>? frame)
    {
        frame = default;
        try { frame = _frames[index]; }
        catch (ArgumentOutOfRangeException) { }
        return frame is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteLayer{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteLayer{T}"/> to get.</param>
    /// <returns>The <see cref="AsepriteLayer{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of layers.
    /// </exception>
    public AsepriteLayer<T> GetLayer(int index) => _layers[index];

    /// <summary>
    /// Returns the <see cref="AsepriteLayer{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteLayer{T}"/>.</param>
    /// <returns>The <see cref="AsepriteLayer{T}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteLayer{T}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteLayer<T> GetLayer(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _layers.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(layer => layer.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteLayer{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteLayer{T}"/> to get.</param>
    /// <param name="layer">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteLayer{T}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteLayer{T}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteLayer{T}"/> elements in this file.
    /// </returns>
    public bool TryGetLayer(int index, [NotNullWhen(true)] out AsepriteLayer<T>? layer)
    {
        layer = default;
        try { layer = _layers[index]; }
        catch (ArgumentOutOfRangeException) { }
        return layer is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteLayer{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteLayer{T}"/></param>
    /// <param name="layer">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteLayer{T}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteLayer{T}"/> with the specified name was found in this
    /// file; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out AsepriteLayer<T>? layer)
    {
        layer = _layers.AsParallel()
                       .WithDegreeOfParallelism(Environment.ProcessorCount)
                       .FirstOrDefault(layer => layer.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return layer is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteTag{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTag{T}"/> to get.</param>
    /// <returns>The <see cref="AsepriteTag{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of tags.
    /// </exception>
    public AsepriteTag<T> GetTag(int index) => _tags[index];

    /// <summary>
    /// Returns the <see cref="AsepriteTag{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteTag{T}"/>.</param>
    /// <returns>The <see cref="AsepriteTag{T}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteTag{T}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteTag<T> GetTag(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _tags.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(tag => tag.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTag{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTag{T}"/> to get.</param>
    /// <param name="tag">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTag{T}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteTag{T}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteTag{T}"/> elements in this file.
    /// </returns>
    public bool TryGetTag(int index, [NotNullWhen(true)] out AsepriteTag<T>? tag)
    {
        tag = default;
        try { tag = _tags[index]; }
        catch (ArgumentOutOfRangeException) { }
        return tag is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTag{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteTag{T}"/></param>
    /// <param name="tag">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTag{T}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteTag{T}"/> with the specified name was found in this file;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTag(string name, [NotNullWhen(true)] out AsepriteTag<T>? tag)
    {
        tag = _tags.AsParallel()
                   .WithDegreeOfParallelism(Environment.ProcessorCount)
                   .FirstOrDefault(tag => tag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return tag is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteSlice{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteSlice{T}"/> to get.</param>
    /// <returns>The <see cref="AsepriteSlice{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of slices.
    /// </exception>
    public AsepriteSlice<T> GetSlice(int index) => _slices[index];

    /// <summary>
    /// Returns the <see cref="AsepriteSlice{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteSlice{T}"/>.</param>
    /// <returns>The <see cref="AsepriteSlice{T}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteSlice{T}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteSlice<T> GetSlice(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _slices.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(slice => slice.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteSlice{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteSlice{T}"/> to get.</param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteSlice{T}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteSlice{T}"/> was found; otherwise <see langword="false"/>.
    /// This method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteSlice{T}"/> elements in this file.
    /// </returns>
    public bool TryGetSlice(int index, [NotNullWhen(true)] out AsepriteSlice<T>? slice)
    {
        slice = default;
        try { slice = _slices[index]; }
        catch (ArgumentOutOfRangeException) { }
        return slice is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteSlice{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteSlice{T}"/></param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteSlice{T}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteSlice{T}"/> with the specified name was found in this
    /// file; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetSlice(string name, [NotNullWhen(true)] out AsepriteSlice<T>? slice)
    {
        slice = _slices.AsParallel()
                       .WithDegreeOfParallelism(Environment.ProcessorCount)
                       .FirstOrDefault(slice => slice.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return slice is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteTileset{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTileset{T}"/> to get.</param>
    /// <returns>The <see cref="AsepriteTileset{T}"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of tilesets.
    /// </exception>
    public AsepriteTileset<T> GetTileset(int index) => _tilesets[index];

    /// <summary>
    /// Returns the <see cref="AsepriteTileset{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteTileset{T}"/>.</param>
    /// <returns>The <see cref="AsepriteTileset{T}"/> with the specified name.</returns>
    /// <exception cref="ArgumentException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteTileset{T}"/> with the name specified does not exist.
    /// </exception>
    public AsepriteTileset<T> GetTileset(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        return _tilesets.AsParallel()
                        .WithDegreeOfParallelism(Environment.ProcessorCount)
                        .First(tileset => tileset.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTileset{T}"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTileset{T}"/> to get.</param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTileset{T}"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteTileset{T}"/> was found; otherwise
    /// <see langword="false"/>.  This method returns <see langword="false"/> if <paramref name="index"/> is less than
    /// zero or is greater than or equal to the total number of <see cref="AsepriteTileset{T}"/> elements in this
    /// file.
    /// </returns>
    public bool TryGetTileset(int index, [NotNullWhen(true)] out AsepriteTileset<T>? slice)
    {
        slice = default;
        try { slice = _tilesets[index]; }
        catch (ArgumentOutOfRangeException) { }
        return slice is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTileset{T}"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteTileset{T}"/></param>
    /// <param name="tileset">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTileset{T}"/> with the
    /// specified name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteTileset{T}"/> with the specified name was found in this
    /// file; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTileset(string name, [NotNullWhen(true)] out AsepriteTileset<T>? tileset)
    {
        tileset = _tilesets.AsParallel()
                           .WithDegreeOfParallelism(Environment.ProcessorCount)
                           .FirstOrDefault(tileset => tileset.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return tileset is not null;
    }
}
