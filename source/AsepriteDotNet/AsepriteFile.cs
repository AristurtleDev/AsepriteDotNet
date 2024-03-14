//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Drawing;
using AsepriteDotNet.Document;

namespace AsepriteDotNet;

/// <summary>
/// Represents the contents loaded from an Aseprite file.
/// </summary>
public sealed class AsepriteFile
{
    private readonly Frame[] _frames;
    private readonly Layer[] _layers;
    private readonly Tag[] _tags;
    private readonly Slice[] _slices;
    private readonly Tileset[] _tilesets;
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
    /// Gets the <see cref="AsepriteColorDepth"/> mode used in Aseprite.
    /// </summary>
    public AsepriteColorDepth ColorDepth { get; }

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="AsepriteFrame"/> elements in this
    /// <see cref="AsepriteFile"/>.  Order of elements is from first-to-last.
    /// </summary>
    public ReadOnlySpan<Frame> Frames => _frames;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="Layer"/> elements in this
    /// <see cref="AsepriteFile"/>.  Order of elements is from bottom-to-top.
    /// </summary>
    public ReadOnlySpan<Layer> Layers => _layers;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="Tag"/> elements in this
    /// <see cref="AsepriteFile"/>.  ORder of elements is as defined in the Aseprite UI from left-to-right.
    /// </summary>
    public ReadOnlySpan<Tag> Tags => _tags;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="Slice"/> elements in this
    /// <see cref="AsepriteFile"/>.  Order of elements is in the order they were created in Aseprite.
    /// </summary>
    public ReadOnlySpan<Slice> Slices => _slices;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of all <see cref="Tileset"/> element in this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlySpan<Tileset> Tilesets => _tilesets;

    /// <summary>
    /// Gets a <see cref="ReadOnlySpan{T}"/> of any warnings issued when the Aseprite file was parsed to create this
    /// <see cref="AsepriteFile"/> instance.  You can use this to see if there were any non-fatal errors that
    /// occurred while parsing the file.
    /// </summary>
    public ReadOnlySpan<string> Warnings => _warnings;

    /// <summary>
    /// Gets the <see cref="AsepriteDotNet.Document.Palette"/> for this <see cref="AsepriteFile"/>.
    /// </summary>
    public Palette Palette { get; }

    /// <summary>
    /// Gets the <see cref="AsepriteDotNet.Document.UserData"/> that was set in the properties for the sprite in Aseprite.
    /// </summary>
    public UserData UserData { get; }

    /// <summary>
    /// Gets the name of the Aseprite file (without the extension).
    /// </summary>
    public string Name { get; }

    internal AsepriteFile(string name, Palette palette, int canvasWidth, int canvasHeight, AsepriteColorDepth colorDepth, List<Frame> frames, List<Layer> layers, List<Tag> tags, List<Slice> slices, List<Tileset> tilesets, UserData userData, List<string> warnings)
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
}
