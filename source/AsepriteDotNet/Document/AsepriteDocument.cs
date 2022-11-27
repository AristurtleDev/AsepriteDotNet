/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Collections.ObjectModel;
using System.Drawing;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents the contents of an aserpite file.
/// </summary>
public sealed class AsepriteDocument
{
    private List<Frame> _frames = new();
    private List<Layer> _layers = new();
    private List<Tag> _tags = new();
    private List<Slice> _slices = new();
    private List<Tileset> _tilesets = new();
    private List<string> _warnings = new();

    /// <summary>
    ///     Gets the width and height, in pixels, of the sprite in this
    ///     <see cref="AsepriteDocument"/>.
    /// </summary>
    public Size Size { get; internal set; }

    /// <summary>
    ///     Gets the <see cref="ColorDepth"/> (bits per pixel) used by this
    ///     <see cref="AsepriteDocument"/>.
    /// </summary>
    public ColorDepth ColorDepth { get; internal set; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Frame"/> elements for
    ///     this <see cref="AsepriteDocument"/>.
    /// </summary>
    public ReadOnlyCollection<Frame> Frames { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Layer"/> elements for
    ///     this <see cref="AsepriteDocument"/>.
    /// </summary>
    public ReadOnlyCollection<Layer> Layers { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Tag"/> elements for
    ///     this <see cref="AsepriteDocument"/>.
    /// </summary>
    public ReadOnlyCollection<Tag> Tags { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Slice"/> elements for
    ///     this <see cref="AsepriteDocument"/>.
    /// </summary>
    public ReadOnlyCollection<Slice> Slices { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Tileset"/> elements
    ///     for this <see cref="AsepriteDocument"/>.
    /// </summary>
    public ReadOnlyCollection<Tileset> Tilesets { get; }

    /// <summary>
    ///     Gets a read-only collection of all warnings that were procduced
    ///     while importing this <see cref="AsepriteDocument"/>.
    /// </summary>
    public ReadOnlyCollection<string> Warnings { get; }

    /// <summary>
    ///     Gets the <see cref="Palette"/> for this 
    ///     <see cref="AsepriteDocument"/>
    /// </summary>
    public Palette Palette { get; } = new();

    internal AsepriteDocument()
    {
        Frames = _frames.AsReadOnly();
        Layers = _layers.AsReadOnly();
        Tags = _tags.AsReadOnly();
        Slices = _slices.AsReadOnly();
        Tilesets = _tilesets.AsReadOnly();
        Warnings = _warnings.AsReadOnly();
    }

    internal void Add(Frame frame) => _frames.Add(frame);
    internal void Add(Layer layer) => _layers.Add(layer);
    internal void Add(Tag tag) => _tags.Add(tag);
    internal void Add(Slice slice) => _slices.Add(slice);
    internal void Add(Tileset tileset) => _tilesets.Add(tileset);
    internal void AddWarning(string message) => _warnings.Add(message);
}