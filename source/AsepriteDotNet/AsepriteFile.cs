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

using AsepriteDotNet.Document;
using AsepriteDotNet.Image;
using AsepriteDotNet.IO;

namespace AsepriteDotNet;

/// <summary>
///     Represents the contents of an Aseprite file.
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
    ///     Gets a read-only collection of all warnings that were produced
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
    ///     Loads the Aseprite file at the specified
    ///     <paramref name="filePath"/>.  The result is a new 
    ///     <see cref="AsepriteFile"/> class instance initialized with the 
    ///     data read from the Aseprite file.
    /// </summary>
    /// <param name="filePath">
    ///     The absolute file path to the Aseprite file.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="AsepriteFile"/> class initialized
    ///     with the data read from the Aseprite file.
    /// </returns>
    /// <exception cref="AsepriteFileLoadException">
    ///     Thrown if an exception occurs during the loading of the Aseprite
    ///     file. Refer to the inner exception for the exact error.
    /// </exception>
    public static AsepriteFile Load(string filePath)
    {
        try
        {
            return AsepriteFileReader.ReadFile(filePath);
        }
        catch (Exception ex)
        {
            throw new AsepriteFileLoadException($"An error occurred while loading the Aseprite file. Please see inner exception for exact error.", ex);
        }
    }

    /// <summary>
    ///     Generates a new <see cref="AsepriteSheet"/> class instance from the
    ///     data within this <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="spritesheetOptions">
    ///     The options to adhere to when generating the spritesheet from this
    ///     <see cref="AsepriteFile"/>.
    /// </param>
    /// <param name="tilesheetOptions">
    ///     The options to adhere to when generating the tilesheets from this
    ///     <see cref="AsepriteFile"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="AsepriteSheet"/> that is created by this method.
    /// </returns>
    public AsepriteSheet ToAsepriteSheet(SpritesheetOptions spritesheetOptions, TilesheetOptions tilesheetOptions)
    {
        Spritesheet spritesheet = ToSpritesheet(spritesheetOptions);
        List<Tilesheet> tilesheets = new();

        for (int i = 0; i < Tilesets.Count; i++)
        {
            Tilesheet tilesheet = Tilesets[i].ToTilesheet(tilesheetOptions);
            tilesheets.Add(tilesheet);
        }

        return new AsepriteSheet(spritesheet, tilesheets);
    }

    /// <summary>
    ///     Generates a new <see cref="Spritesheet"/> class instance from the
    ///     frame, slice, and tag data within this <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="options">
    ///     The options to adhere to when generating the spritesheet for the
    ///     <see cref="AsepriteSheet"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="Spritesheet"/> that is created by this method.
    /// </returns>
    public Spritesheet ToSpritesheet(SpritesheetOptions options)
    {
        List<SpritesheetFrame> sheetFrames = new();
        List<SpritesheetAnimation> sheetAnimations = new();

        //  Process frames and the pixels

        Dictionary<int, Color[]> frameColorLookup = new Dictionary<int, Color[]>();
        Dictionary<int, int> frameDuplicateMap = new Dictionary<int, int>();

        for (int frameNum = 0; frameNum < Frames.Count; frameNum++)
        {
            frameColorLookup.Add(frameNum, Frames[frameNum].FlattenFrame(options.OnlyVisibleLayers));
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

        if (options.PackingMethod == PackingMethod.HorizontalStrip)
        {
            columns = totalFrames;
            rows = 1;
        }
        else if (options.PackingMethod == PackingMethod.VerticalStrip)
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
                //  pixel relative to the top-left of the final spritesheet
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
                //  We are merging duplicates and it was detected that the
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


        //  Process Slices

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
                Color color = slice.UserData.Color ?? Color.FromRGBA(0, 0, 255, 255);
                Rectangle bounds = key.Bounds;
                Rectangle? center = key.CenterBounds;
                Point? pivot = key.Pivot;
                SpritesheetSlice sheetSlice = new(bounds, center, pivot, name, color);

                if (lastKey is not null && lastKey.Frame < key.Frame)
                {
                    for (int offset = 1; offset < key.Frame - lastKey.Frame; offset++)
                    {
                        string interpolatedName = slice.Name;
                        Color interpolatedColor = slice.UserData.Color ?? Color.FromRGBA(0, 0, 255, 255);
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
                    Color interpolatedColor = slice.UserData.Color ?? Color.FromRGBA(0, 0, 255, 255);
                    Rectangle interpolatedBounds = lastKey.Bounds;
                    Rectangle? interpolatedCenter = lastKey.CenterBounds;
                    Point? interpolatedPivot = lastKey.Pivot;

                    SpritesheetSlice interpolated = new(interpolatedBounds, interpolatedCenter, interpolatedPivot, interpolatedName, interpolatedColor);

                    sheetFrames[lastKey.Frame + offset].AddSlice(interpolated);
                }
            }
        }

        return new Spritesheet(sheetSize, sheetFrames, sheetAnimations, sheetPixels);
    }
}