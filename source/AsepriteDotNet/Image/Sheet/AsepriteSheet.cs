/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using AsepriteDotNet.Common;
using AsepriteDotNet.Document;

namespace AsepriteDotNet.Image.Sheet;

public sealed class AsepriteSheet
{
    public List<SpritesheetFrame> Frames { get; set; } = new();
    public List<SpritesheetAnimation> Animations { get; set; } = new();
    public Size Size { get; set; } = Size.Empty;
    public Color[] Pixels { get; set; } = Array.Empty<Color>();

    private AsepriteSheet() { }

    public static AsepriteSheet FromFile(AsepriteFile file, SpritesheetOptions options)
    {
        AsepriteSheet sheet = new();

        //  Process frames and the pixels
        {
            Dictionary<int, Color[]> frameColorLookup = new Dictionary<int, Color[]>();
            Dictionary<int, int> frameDuplicateMap = new Dictionary<int, int>();

            for (int frameNum = 0; frameNum < file.Frames.Count; frameNum++)
            {
                Frame frame = file.Frames[frameNum];
                frameColorLookup.Add(frameNum, file.FlattenFrame(frame, options.OnlyVisibleLayers));
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

            if (options.SpritesheetType == SpritesheetType.HorizontalStrip)
            {
                columns = totalFrames;
                rows = 1;
            }
            else if (options.SpritesheetType == SpritesheetType.VerticalStrip)
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

            width = (columns * file.Size.Width) +
                    (options.BorderPadding * 2) +
                    (options.Spacing * (columns - 1)) +
                    (options.InnerPadding * 2 * columns);

            height = (rows * file.Size.Height) +
                     (options.BorderPadding * 2) +
                     (options.Spacing * (rows - 1)) +
                     (options.InnerPadding * 2 * rows);

            sheet.Size = new(width, height);

            sheet.Pixels = new Color[width * height];
            sheet.Frames = new List<SpritesheetFrame>();
            Dictionary<int, SpritesheetFrame> originalToDuplicateFrameLookup = new();

            int fOffset = 0;

            for (int frameNum = 0; frameNum < file.Frames.Count; frameNum++)
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
                        int x = (pixelNum % file.Size.Width) + (frameCol * file.Size.Width);
                        int y = (pixelNum / file.Size.Width) + (frameRow * file.Size.Height);

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
                        sheet.Pixels[index] = pixels[pixelNum];
                    }

                    //  Now create the frame data
                    Rectangle sourceRectangle = new(0, 0, file.Size.Width, file.Size.Height);
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

                    SpritesheetFrame frame = new()
                    {
                        SourceRectangle = sourceRectangle,
                        Duration = file.Frames[frameNum].Duration
                    };

                    sheet.Frames.Add(frame);
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
                    SpritesheetFrame duplicate = new()
                    {
                        SourceRectangle = original.SourceRectangle,
                        Duration = original.Duration
                    };
                    sheet.Frames.Add(duplicate);
                    fOffset++;
                }
            }

        }

        //  Process Animations
        {
            sheet.Animations = new();

            for (int tagNum = 0; tagNum < file.Tags.Count; tagNum++)
            {
                Tag tag = file.Tags[tagNum];
                SpritesheetAnimation animation = new();
                animation.Direction = tag.LoopDirection;
                animation.Name = tag.Name;
                animation.Frames = new(sheet.Frames.GetRange(tag.From, tag.To - tag.From + 1));
                sheet.Animations.Add(animation);
            }
        }

        //  Process Sices
        {

        }

        return sheet;
    }

    public void SaveAsPng(string path)
    {
        Png.SaveTo(path, Size, Pixels);
    }
}