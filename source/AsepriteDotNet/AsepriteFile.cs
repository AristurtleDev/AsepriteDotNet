//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents the contents loaded from an Aseprite file.
/// </summary>
public sealed class AsepriteFile
{
    private readonly AsepriteFrame[] _frames;
    private readonly AsepriteLayer[] _layers;
    private readonly AsepriteTag[] _tags;
    private readonly AsepriteSlice[] _slices;
    private readonly string[] _warnings;

    /// <summary>
    /// Gets the size defined for the canvas in Aseprite.
    /// </summary>
    public Size CanvasSize { get; }

    /// <summary>
    /// Gets the <see cref="AsepriteColorDepth"/> mode used in Aseprite.
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
    public AsepriteUserData? UserData { get; }

    /// <summary>
    /// Gets the name of the Aseprite file (without the extension).
    /// </summary>
    public string Name { get; }

    internal AsepriteFile(string name, AsepritePalette palette, Size canvasSize, AsepriteColorDepth colorDepth, List<AsepriteFrame> frames, List<AsepriteLayer> layers, List<AsepriteTag> tags, List<AsepriteSlice> slices, AsepriteUserData? userData, List<string> warnings)
    {
        Name = name;
        CanvasSize = canvasSize;
        ColorDepth = colorDepth;
        _frames = [.. frames];
        _tags = [.. tags];
        _slices = [.. slices];
        _warnings = [.. warnings];
        _layers = [.. layers];
        Palette = palette;
        UserData = userData;
    }
}
