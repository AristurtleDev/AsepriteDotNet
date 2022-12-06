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

using AsepriteDotNet.Common;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents the contents of an aserpite file.
/// </summary>
public sealed class AsepriteFile
{
    private List<Frame> _frames = new();
    private List<Layer> _layers = new();
    private List<Tag> _tags = new();
    private List<Slice> _slices = new();
    private List<Tileset> _tilesets = new();
    private List<string> _warnings = new();

    /// <summary>
    ///     Gets the width and height, in pixels, of the sprite in this
    ///     <see cref="AsepriteFile"/>.
    /// </summary>
    public Size Size { get; internal set; }

    /// <summary>
    ///     Gets the <see cref="ColorDepth"/> (bits per pixel) used by this
    ///     <see cref="AsepriteFile"/>.
    /// </summary>
    public ColorDepth ColorDepth { get; internal set; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Frame"/> elements for
    ///     this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlyCollection<Frame> Frames { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Layer"/> elements for
    ///     this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlyCollection<Layer> Layers { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Tag"/> elements for
    ///     this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlyCollection<Tag> Tags { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Slice"/> elements for
    ///     this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlyCollection<Slice> Slices { get; }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Tileset"/> elements
    ///     for this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlyCollection<Tileset> Tilesets { get; }

    /// <summary>
    ///     Gets a read-only collection of all warnings that were procduced
    ///     while importing this <see cref="AsepriteFile"/>.
    /// </summary>
    public ReadOnlyCollection<string> Warnings { get; }

    /// <summary>
    ///     Gets the <see cref="Palette"/> for this 
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    public Palette Palette { get; } = new();

    internal AsepriteFile()
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

    public Color[] FlattenFrame(Frame frame, bool onlyVisible)
    {
        Color[] flattened = new Color[Size.Width * Size.Height];

        for (int celNum = 0; celNum < frame.Cels.Count; celNum++)
        {
            Color[] pixels = Array.Empty<Color>();
            Size celSize = Size.Empty;
            Point celPos = Point.Empty;
            Cel cel = frame[celNum];
            Layer layer = Layers[cel.LayerIndex];

            //  If only visible and layer cel is on is not visible,
            //  skip processing
            if (!onlyVisible || (onlyVisible && layer.IsVisible))
            {
                if (cel is ImageCel imageCel)
                {
                    pixels = imageCel.Pixels;
                    celSize = imageCel.Size;
                    celPos = imageCel.Position;
                }
                else if (cel is LinkedCel linkedCel)
                {
                    Frame linkedFrame = Frames[linkedCel.Frame];
                    Cel otherCel = linkedFrame[celNum];

                    if (otherCel is ImageCel otherImageCel)
                    {
                        pixels = otherImageCel.Pixels;
                        celSize = otherImageCel.Size;
                        celPos = otherImageCel.Position;
                    }
                    else if (otherCel is TilemapCel otherTileMapCel)
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (cel is TilemapCel tilemapCel)
                {
                    throw new NotImplementedException();
                }

                if (pixels.Length > 0)
                {
                    byte opacity = Color.MUL_UN8(cel.Opacity, layer.Opacity);

                    for (int p = 0; p < pixels.Length; p++)
                    {
                        int x = (p % celSize.Width) + celPos.X;
                        int y = (p / celSize.Width) + celPos.Y;
                        int index = y * Size.Width + x;

                        //  Sometimes a cell can have a negative x and/or y 
                        //  value. This is caused by selecting an area within 
                        //  aseprite and then moving a portion of the selected
                        //  pixels outside the canvas.  We don't care about 
                        //  these pixels so if the index is outside the range of
                        //  the array to store them in then we'll just ignore 
                        //  them.
                        if (index < 0 || index >= flattened.Length) { continue; }

                        Color backdrop = flattened[index];
                        Color source = pixels[p];
                        flattened[index] = Color.Blend(layer.BlendMode, backdrop, source, opacity);
                    }
                }
            }
        }

        return flattened;
    }
}