//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using AsepriteDotNet.Aseprite.Types;

namespace AsepriteDotNet.Aseprite;

/// <summary>
/// Represents the contents loaded from an Aseprite file.
/// </summary>
public sealed class AsepriteFile
{
    private readonly AsepriteFrame[] _frames;
    private readonly AsepriteLayer[] _layers;
    private readonly AsepriteTag[] _tags;
    private readonly AsepriteSlice[] _slices;
    private readonly AsepriteTileset[] _tilesets;
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
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteFrame"/> elements in this
    /// <see cref="AsepriteFile"/>.  Order of elements is from first-to-last.
    /// </summary>
    public ReadOnlySpan<AsepriteFrame> Frames => _frames;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteLayer"/> elements in this
    /// <see cref="AsepriteFile"/>.  Order of elements is from bottom-to-top.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer> Layers => _layers;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteTag"/> elements in this
    /// <see cref="AsepriteFile"/>.  ORder of elements is as defined in the Aseprite UI from left-to-right.
    /// </summary>
    public ReadOnlySpan<AsepriteTag> Tags => _tags;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteSlice"/> elements in this
    /// <see cref="AsepriteFile"/>.  Order of elements is in the order they were created in Aseprite.
    /// </summary>
    public ReadOnlySpan<AsepriteSlice> Slices => _slices;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteTileset"/> element in this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlySpan<AsepriteTileset> Tilesets => _tilesets;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of any warnings issued when the Aseprite file was parsed to create this
    /// <see cref="AsepriteFile"/> instance.  You can use this to see if there were any non-fatal errors that
    /// occurred while parsing the file.
    /// </summary>
    public ReadOnlySpan<string> Warnings => _warnings;

    /// <summary>
    /// Gets the <see cref="AsepritePalette"/> for this <see cref="AsepriteFile"/>.
    /// </summary>
    public AsepritePalette Palette { get; }

    /// <summary>
    /// Gets the <see cref="AsepriteUserData"/> that was set in the properties for the sprite in Aseprite.
    /// </summary>
    public AsepriteUserData UserData { get; }

    /// <summary>
    /// Gets the name of the Aseprite file (without the extension).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the total number of <see cref="AsepriteFrame"/> elements in this file.
    /// </summary>
    public int FrameCount => _frames.Length;

    internal AsepriteFile(string name, AsepritePalette palette, int canvasWidth, int canvasHeight, AsepriteColorDepth colorDepth, List<AsepriteFrame> frames, List<AsepriteLayer> layers, List<AsepriteTag> tags, List<AsepriteSlice> slices, List<AsepriteTileset> tilesets, AsepriteUserData userData, List<string> warnings)
    {
        Name = name;
        CanvasWidth = canvasWidth;
        CanvasHeight = canvasHeight;
        ColorDepth = colorDepth;
        _frames = frames.ToArray();
        _tags = tags.ToArray();
        _slices = slices.ToArray();
        _tilesets = tilesets.ToArray();
        _warnings = warnings.ToArray();
        _layers = layers.ToArray();
        Palette = palette;
        UserData = userData;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteFrame"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteFrame"/> to get.</param>
    /// <returns>The <see cref="AsepriteFrame"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of frames.
    /// </exception>
    public AsepriteFrame GetFrame(int index) => _frames[index];

    /// <summary>
    /// Gets the <see cref="AsepriteFrame"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteFrame"/> to get.</param>
    /// <param name="frame">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteFrame"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteFrame"/> was found; otherwise <see langword="false"/>.  This
    /// method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteFrame"/> elements in this file.
    /// </returns>
    public bool TryGetFrame(int index, [NotNullWhen(true)] out AsepriteFrame? frame)
    {
        frame = default;
        try { frame = _frames[index]; }
        catch (ArgumentOutOfRangeException) { }
        return frame is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteLayer"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteLayer"/> to get.</param>
    /// <returns>The <see cref="AsepriteLayer"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of layers.
    /// </exception>
    public AsepriteLayer GetLayer(int index) => _layers[index];

    /// <summary>
    /// Returns the <see cref="AsepriteLayer"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteLayer"/>.</param>
    /// <returns>The <see cref="AsepriteLayer"/> with the specified name.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteLayer"/> with the name specified does not exist.
    /// </exception>
    public AsepriteLayer GetLayer(string name)
    {
#if NET6_0
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), $"{nameof(name)} cannot be null or an empty string.");
        }
#elif NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNullOrEmpty(name);
#endif

        return _layers.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(layer => layer.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteLayer"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteLayer"/> to get.</param>
    /// <param name="layer">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteLayer"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteLayer"/> was found; otherwise <see langword="false"/>.  This
    /// method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteLayer"/> elements in this file.
    /// </returns>
    public bool TryGetLayer(int index, [NotNullWhen(true)] out AsepriteLayer? layer)
    {
        layer = default;
        try { layer = _layers[index]; }
        catch (ArgumentOutOfRangeException) { }
        return layer is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteLayer"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteLayer"/></param>
    /// <param name="layer">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteLayer"/> with the specified
    /// name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteLayer"/> with the specified name was found in this file;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out AsepriteLayer? layer)
    {
        layer = _layers.AsParallel()
                       .WithDegreeOfParallelism(Environment.ProcessorCount)
                       .FirstOrDefault(layer => layer.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return layer is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteTag"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTag"/> to get.</param>
    /// <returns>The <see cref="AsepriteTag"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of tags.
    /// </exception>
    public AsepriteTag GetTag(int index) => _tags[index];

    /// <summary>
    /// Returns the <see cref="AsepriteTag"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteTag"/>.</param>
    /// <returns>The <see cref="AsepriteTag"/> with the specified name.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteTag"/> with the name specified does not exist.
    /// </exception>
    public AsepriteTag GetTag(string name)
    {
#if NET6_0
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), $"{nameof(name)} cannot be null or an empty string");
        }
#elif NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNullOrEmpty(name);
#endif

        return _tags.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(tag => tag.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTag"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTag"/> to get.</param>
    /// <param name="tag">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTag"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteTag"/> was found; otherwise <see langword="false"/>.  This
    /// method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteTag"/> elements in this file.
    /// </returns>
    public bool TryGetTag(int index, [NotNullWhen(true)] out AsepriteTag? tag)
    {
        tag = default;
        try { tag = _tags[index]; }
        catch (ArgumentOutOfRangeException) { }
        return tag is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTag"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteTag"/></param>
    /// <param name="tag">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTag"/> with the specified
    /// name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteTag"/> with the specified name was found in this file;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTag(string name, [NotNullWhen(true)] out AsepriteTag? tag)
    {
        tag = _tags.AsParallel()
                   .WithDegreeOfParallelism(Environment.ProcessorCount)
                   .FirstOrDefault(tag => tag.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return tag is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteSlice"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteSlice"/> to get.</param>
    /// <returns>The <see cref="AsepriteSlice"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of slices.
    /// </exception>
    public AsepriteSlice GetSlice(int index) => _slices[index];

    /// <summary>
    /// Returns the <see cref="AsepriteSlice"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteSlice"/>.</param>
    /// <returns>The <see cref="AsepriteSlice"/> with the specified name.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteSlice"/> with the name specified does not exist.
    /// </exception>
    public AsepriteSlice GetSlice(string name)
    {
#if NET6_0
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), $"{nameof(name)} cannot be null or an empty string");
        }
#elif NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNullOrEmpty(name);
#endif

        return _slices.AsParallel()
                      .WithDegreeOfParallelism(Environment.ProcessorCount)
                      .First(slice => slice.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteSlice"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteSlice"/> to get.</param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteSlice"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteSlice"/> was found; otherwise <see langword="false"/>.  This
    /// method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteSlice"/> elements in this file.
    /// </returns>
    public bool TryGetSlice(int index, [NotNullWhen(true)] out AsepriteSlice? slice)
    {
        slice = default;
        try { slice = _slices[index]; }
        catch (ArgumentOutOfRangeException) { }
        return slice is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteSlice"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteSlice"/></param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteSlice"/> with the specified
    /// name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteSlice"/> with the specified name was found in this file;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetSlice(string name, [NotNullWhen(true)] out AsepriteSlice? slice)
    {
        slice = _slices.AsParallel()
                       .WithDegreeOfParallelism(Environment.ProcessorCount)
                       .FirstOrDefault(slice => slice.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return slice is not null;
    }

    /// <summary>
    /// Returns the <see cref="AsepriteTileset"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTileset"/> to get.</param>
    /// <returns>The <see cref="AsepriteTileset"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// if <paramref name="index"/> is less than zero or greater than or equal to the total number of tilesets.
    /// </exception>
    public AsepriteTileset GetTileset(int index) => _tilesets[index];

    /// <summary>
    /// Returns the <see cref="AsepriteTileset"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of <see cref="AsepriteTileset"/>.</param>
    /// <returns>The <see cref="AsepriteTileset"/> with the specified name.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="name"/> is <see langword="null"/> or an empty string.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An <see cref="AsepriteTileset"/> with the name specified does not exist.
    /// </exception>
    public AsepriteTileset GetTileset(string name)
    {
#if NET6_0
        if(string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name), $"{nameof(name)} cannot be null or an empty string");
        }
#elif NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNullOrEmpty(name);
#endif

        return _tilesets.AsParallel()
                        .WithDegreeOfParallelism(Environment.ProcessorCount)
                        .First(tileset => tileset.Name.Equals(name, StringComparison.Ordinal));
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTileset"/> at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="AsepriteTileset"/> to get.</param>
    /// <param name="slice">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTileset"/>; otherwise,
    /// <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="AsepriteTileset"/> was found; otherwise <see langword="false"/>.  This
    /// method returns <see langword="false"/> if <paramref name="index"/> is less than zero or is greater than or
    /// equal to the total number of <see cref="AsepriteTileset"/> elements in this file.
    /// </returns>
    public bool TryGetTileset(int index, [NotNullWhen(true)] out AsepriteTileset? slice)
    {
        slice = default;
        try { slice = _tilesets[index]; }
        catch (ArgumentOutOfRangeException) { }
        return slice is not null;
    }

    /// <summary>
    /// Gets the <see cref="AsepriteTileset"/> with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="AsepriteTileset"/></param>
    /// <param name="tileset">
    /// When this method returns <see langword="true"/>, contains the <see cref="AsepriteTileset"/> with the specified
    /// name; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an <see cref="AsepriteTileset"/> with the specified name was found in this file;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTileset(string name, [NotNullWhen(true)] out AsepriteTileset? tileset)
    {
        tileset = _tilesets.AsParallel()
                           .WithDegreeOfParallelism(Environment.ProcessorCount)
                           .FirstOrDefault(tileset => tileset.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        return tileset is not null;
    }
}
