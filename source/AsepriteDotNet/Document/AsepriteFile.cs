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

using AsepriteDotNet.Image;

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
    public Size Size { get; }

    /// <summary>
    ///     Gets the <see cref="ColorDepth"/> (bits per pixel) used by this
    ///     <see cref="AsepriteFile"/>.
    /// </summary>
    public ColorDepth ColorDepth { get; }

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
    public Palette Palette { get; }

    internal AsepriteFile(Palette palette, Size size, ColorDepth colorDepth)
    {
        Size = size;
        ColorDepth = colorDepth;
        Palette = palette;
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

    /// <summary>
    ///     Creates a new <see cref="AsepriteSheet"/> class instance from the
    ///     data within this <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="options">
    ///     The options to adhere to when creating the
    ///     <see cref="AsepriteSheet"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="AsepriteSheet"/> that is created by this method.
    /// </returns>
    public AsepriteSheet ToAsepritesheet(SpritesheetOptions options)
    {
        List<SpritesheetFrame> sheetFrames = new();
        List<SpritesheetAnimation> sheetAnimations = new();

        //  Process frames and the pixels

        Dictionary<int, Color[]> frameColorLookup = new Dictionary<int, Color[]>();
        Dictionary<int, int> frameDuplicateMap = new Dictionary<int, int>();

        for (int frameNum = 0; frameNum < Frames.Count; frameNum++)
        {
            Frame frame = Frames[frameNum];
            frameColorLookup.Add(frameNum, FlattenFrame(frame, options.OnlyVisibleLayers));
        }

        int columns, rows;
        int width, height;

        int totalFrames = frameColorLookup.Count;

        if (options.MergeDuplicates)
        {
            for (int i = 0; i < frameColorLookup.Count; i++)
            {
                for (int d = 0; d < i; d++)
                {
                    if (frameColorLookup[i].SequenceEqual(frameColorLookup[d]))
                    {
                        frameDuplicateMap.Add(i, d);
                        break;
                    }
                }
            }

            //  Since we are merging duplicates, we need to subtract the
            //  number of duplicates from the total frame count
            totalFrames -= frameDuplicateMap.Count;
        }

        if (options.PackingMethod == SpritesheetPackingMethod.HorizontalStrip)
        {
            columns = totalFrames;
            rows = 1;
        }
        else if (options.PackingMethod == SpritesheetPackingMethod.VerticalStrip)
        {
            columns = 1;
            rows = totalFrames;
        }
        else
        {
            // https://en.wikipedia.org/wiki/Square_packing_in_a_square
            double sqrt = Math.Sqrt(totalFrames);
            columns = (int)Math.Floor(sqrt);
            if (Math.Abs(sqrt % 1) >= double.Epsilon)
            {
                columns++;
            }

            rows = totalFrames / columns;
            if (totalFrames % columns != 0)
            {
                rows++;
            }
        }

        width = (columns * Size.Width) +
                (options.BorderPadding * 2) +
                (options.Spacing * (columns - 1)) +
                (options.InnerPadding * 2 * columns);

        height = (rows * Size.Height) +
                 (options.BorderPadding * 2) +
                 (options.Spacing * (rows - 1)) +
                 (options.InnerPadding * 2 * rows);

        Size sheetSize = new(width, height);

        Color[] sheetPixels = new Color[width * height];

        Dictionary<int, SpritesheetFrame> originalToDuplicateFrameLookup = new();

        int fOffset = 0;

        for (int frameNum = 0; frameNum < Frames.Count; frameNum++)
        {
            if (!options.MergeDuplicates || !frameDuplicateMap.ContainsKey(frameNum))
            {
                //  Calculate the x and y position of the frame's top-left
                //  ixel relative to the top-left of the final spritesheet
                int frameCol = (frameNum - fOffset) % columns;
                int frameRow = (frameNum - fOffset) / columns;

                //  Inject the pixel color data from the frame into the
                //  final spritesheet color data array
                Color[] pixels = frameColorLookup[frameNum];

                for (int pixelNum = 0; pixelNum < pixels.Length; pixelNum++)
                {
                    int x = (pixelNum % Size.Width) + (frameCol * Size.Width);
                    int y = (pixelNum / Size.Width) + (frameRow * Size.Height);

                    //  Adjust for padding/spacing
                    x += options.BorderPadding;
                    y += options.BorderPadding;

                    if (options.Spacing > 0)
                    {
                        if (frameCol > 0)
                        {
                            x += options.Spacing * frameCol;
                        }

                        if (frameRow > 0)
                        {
                            y += options.Spacing * frameRow;
                        }
                    }

                    if (options.InnerPadding > 0)
                    {
                        x += options.InnerPadding * (frameCol + 1);
                        y += options.InnerPadding * (frameRow + 1);

                        if (frameCol > 0)
                        {
                            x += options.InnerPadding * frameCol;
                        }

                        if (frameRow > 0)
                        {
                            y += options.InnerPadding * frameRow;
                        }
                    }

                    int index = y * width + x;
                    sheetPixels[index] = pixels[pixelNum];
                }

                //  Now create the frame data
                Rectangle sourceRectangle = new(0, 0, Size.Width, Size.Height);
                sourceRectangle.X += options.BorderPadding;
                sourceRectangle.Y += options.BorderPadding;

                if (options.Spacing > 0)
                {
                    if (frameCol > 0)
                    {
                        sourceRectangle.X += options.Spacing * frameCol;
                    }

                    if (frameRow > 0)
                    {
                        sourceRectangle.Y += options.Spacing * frameRow;
                    }
                }

                if (options.InnerPadding > 0)
                {
                    sourceRectangle.X += options.InnerPadding * (frameCol + 1);
                    sourceRectangle.Y += options.InnerPadding * (frameRow + 1);

                    if (frameCol > 0)
                    {
                        sourceRectangle.X += options.InnerPadding * frameCol;
                    }
                    if (frameRow > 0)
                    {
                        sourceRectangle.Y += options.InnerPadding * frameRow;
                    }
                }

                SpritesheetFrame frame = new(sourceRectangle, Frames[frameNum].Duration);

                sheetFrames.Add(frame);
                originalToDuplicateFrameLookup.Add(frameNum, frame);
            }
            else
            {
                //  We are merging dupicates and it was detected that the
                //  current frame to process is a duplicate.  So we still
                //  need to add the spritesheet frame, but we need to make
                //  user the data is the same as the frame it's a duplicate
                //  of
                SpritesheetFrame original = originalToDuplicateFrameLookup[frameDuplicateMap[frameNum]];
                sheetFrames.Add(original.CreateCopy());
                fOffset++;
            }
        }

        //  Process Animations
        for (int tagNum = 0; tagNum < Tags.Count; tagNum++)
        {
            Tag tag = Tags[tagNum];
            string name = tag.Name;
            LoopDirection direction = tag.LoopDirection;
            List<SpritesheetFrame> aFrames = new(sheetFrames.GetRange(tag.From, tag.To - tag.From + 1));
            SpritesheetAnimation animation = new(aFrames, name, direction);
            sheetAnimations.Add(animation);
        }


        //  Process Sices

        //  Slice keys in Aseprite are defined with a frame value that
        //  indicates what frame the key starts on, but doesn't give
        //  what frame it ends on or is transformed on.  So we'll have to
        //  interpolate the slices per frame
        for (int sliceNum = 0; sliceNum < Slices.Count; sliceNum++)
        {
            Slice slice = Slices[sliceNum];

            SliceKey? lastKey = default;

            for (int keyNum = 0; keyNum < slice.Count; keyNum++)
            {
                SliceKey key = slice[keyNum];

                string name = slice.Name;
                Color color = slice.UserData.Color ?? Color.FromArgb(255, 0, 0, 255);
                Rectangle bounds = key.Bounds;
                Rectangle? center = key.CenterBounds;
                Point? pivot = key.Pivot;
                SpritesheetSlice sheetSlice = new(bounds, center, pivot, name, color);

                if (lastKey is not null && lastKey.Frame < key.Frame)
                {
                    for (int offset = 1; offset < key.Frame - lastKey.Frame; offset++)
                    {
                        string interpolatedName = slice.Name;
                        Color interpolatedColor = slice.UserData.Color ?? Color.FromArgb(255, 0, 0, 255);
                        Rectangle interpolatedBounds = lastKey.Bounds;
                        Rectangle? interpolatedCenter = lastKey.CenterBounds;
                        Point? interpolatedPivot = lastKey.Pivot;

                        SpritesheetSlice interpolated = new(interpolatedBounds, interpolatedCenter, interpolatedPivot, interpolatedName, interpolatedColor);

                        sheetFrames[lastKey.Frame + offset].AddSlice(interpolated);
                    }
                }

                sheetFrames[key.Frame].AddSlice(sheetSlice);
                lastKey = key;
            }

            if (lastKey?.Frame < sheetFrames.Count - 1)
            {
                for (int offset = 1; offset < sheetFrames.Count - lastKey.Frame; offset++)
                {
                    string interpolatedName = slice.Name;
                    Color interpolatedColor = slice.UserData.Color ?? Color.FromArgb(255, 0, 0, 255);
                    Rectangle interpolatedBounds = lastKey.Bounds;
                    Rectangle? interpolatedCenter = lastKey.CenterBounds;
                    Point? interpolatedPivot = lastKey.Pivot;

                    SpritesheetSlice interpolated = new(interpolatedBounds, interpolatedCenter, interpolatedPivot, interpolatedName, interpolatedColor);

                    sheetFrames[lastKey.Frame + offset].AddSlice(interpolated);
                }
            }
        }

        return new AsepriteSheet(sheetSize, sheetFrames, sheetAnimations, sheetPixels);
    }

    /// <summary>
    ///     Flattens the <see cref="Frame"/> at the specified 
    ///     <paramref name="frameIndex"/> by blending each <see cref="Cel"/>
    ///     in the <see cref="Frame"/>, starting with the top most cel and
    ///     blending down.  The result is an <see cref="Array"/> of
    ///     <see cref="Color"/> elements representing the final flattened image
    ///     of the <see cref="Frame"/>.
    /// </summary>
    /// <param name="frameIndex">
    ///     The index of the <see cref="Frame"/> to flatten.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Whether only the <see cref="Cel"/> elements that are on a 
    ///     <see cref="Layer"/> that is visible should be included.
    /// </param>
    /// <returns>
    ///     A new <see cref="Array"/> of <see cref="Color"/> elements that
    ///     represent the image of the <see cref="Frame"/> once it has been
    ///     flattened.
    /// </returns>
    public Color[] FlattenFrame(int frameIndex, bool onlyVisibleLayers)
    {
        Frame frame = Frames[frameIndex];
        return FlattenFrame(frame, onlyVisibleLayers);

    }

    /// <summary>
    ///     Flattens the specified <see cref="Frame"/> by blending each
    ///     <see cref="Cel"/> in the <see cref="Frame"/>, starting with the top
    ///     most cel and blending down.  The result is an array of
    ///     <see cref="Color"/> elements representing the final flattened image
    ///     of the frame.
    /// </summary>
    /// <remarks>
    ///     Any instance of a <see cref="TilemapCel"/> within the 
    ///     <see cref="Frame"/> is ignored.
    /// </remarks>
    /// <param name="frame">
    ///     The <see cref="Frame"/> to flatten
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Whether only the <see cref="Cel"/> elements that are on a 
    ///     <see cref="Layer"/> that is visible should be included.
    /// </param>
    /// <returns>
    ///     A new <see cref="Array"/> of <see cref="Color"/> elements that
    ///     represent the image of the <see cref="Frame"/> once it has been
    ///     flattened.
    /// </returns>
    public Color[] FlattenFrame(Frame frame, bool onlyVisibleLayers)
    {
        Color[] flattened = new Color[Size.Width * Size.Height];

        for (int celNum = 0; celNum < frame.Cels.Count; celNum++)
        {
            Color[] pixels = Array.Empty<Color>();
            Size celSize = Size.Empty;
            Point celPos = Point.Empty;
            Cel cel = frame[celNum];

            //  If only visible and layer cel is on is not visible,
            //  skip processing
            if (!onlyVisibleLayers || (onlyVisibleLayers && cel.Layer.IsVisible))
            {
                if (cel is ImageCel imageCel)
                {
                    pixels = imageCel.Pixels;
                    celSize = imageCel.Size;
                    celPos = imageCel.Position;
                }
                else if (cel is LinkedCel linkedCel)
                {
                    if (linkedCel.Cel is ImageCel otherImageCel)
                    {
                        pixels = otherImageCel.Pixels;
                        celSize = otherImageCel.Size;
                        celPos = otherImageCel.Position;
                    }
                    else if (linkedCel.Cel is TilemapCel otherTileMapCel)
                    {
                        continue;   //  Tilemap cels not supported
                    }
                }
                else if (cel is TilemapCel tilemapCel)
                {
                    continue;   //  Tilemap cels not supported
                }

                if (pixels.Length > 0)
                {
                    byte opacity = BlendFunctions.MUL_UN8(cel.Opacity, cel.Layer.Opacity);

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
                        flattened[index] = BlendFunctions.Blend(cel.Layer.BlendMode, backdrop, source, opacity);
                    }
                }
            }
        }

        return flattened;
    }
}