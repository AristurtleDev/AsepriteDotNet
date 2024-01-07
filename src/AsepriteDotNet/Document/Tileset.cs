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
using AsepriteDotNet.Image;
using AsepriteDotNet.IO.Image;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a tileset in an Aseprite image.
/// </summary>
public class Tileset
{
    /// <summary>
    ///     Gets the ID of this <see cref="Tileset"/>.
    /// </summary>
    public int ID { get; }

    /// <summary>
    ///     Gets the total number of tiles in this ,<see cref="Tileset"/>.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    ///     Gets the width and height, in pixels of each tile in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    public Size TileSize { get; }

    /// <summary>
    ///     Gets the name of this <see cref="Tileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets or Sets an <see cref="Array"/> of <see cref="Rgba32"/> elements
    ///     that represents the raw pixel data for this <see cref="Tileset"/>.
    /// </summary>
    public Rgba32[] Pixels { get; }

    internal Tileset(int id, int count, Size tileSize, string name, Rgba32[] pixels) =>
        (ID, TileCount, TileSize, Name, Pixels) = (id, count, tileSize, name, pixels);

    /// <summary>
    ///     Generates a new <see cref="Tilesheet"/> class instance from this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="options">
    ///     The options to adhere to when generating the tilesheet.
    /// </param>
    /// <returns>
    ///     The <see cref="Tilesheet"/> that is created by this method.
    /// </returns>
    public Tilesheet ToTilesheet(TilesheetOptions options)
    {
        List<TilesheetTile> sheetTiles = new();
        List<Rgba32[]> tileColorLookup = SplitTiles();
        Dictionary<int, int> tileDuplicateMap = new();

        int columns, rows;
        int width, height;

        int totalTiles = tileColorLookup.Count;

        if (options.MergeDuplicates)
        {
            for (int i = 0; i < tileColorLookup.Count; i++)
            {
                for (int d = 0; d < i; d++)
                {
                    if (tileColorLookup[i].SequenceEqual(tileColorLookup[d]))
                    {
                        tileDuplicateMap.Add(i, d);
                        break;
                    }
                }
            }
        }

        //  Since we are merging duplicates, we need to subtract the number of
        //  duplicates from the total tiles
        totalTiles -= tileDuplicateMap.Count;

        if (options.PackingMethod == PackingMethod.HorizontalStrip)
        {
            columns = totalTiles;
            rows = 1;
        }
        else if (options.PackingMethod == PackingMethod.VerticalStrip)
        {
            columns = 1;
            rows = totalTiles;
        }
        else
        {
            double sqrt = Math.Sqrt(totalTiles);
            columns = (int)Math.Floor(sqrt);
            if (Math.Abs(sqrt % 1) >= double.Epsilon)
            {
                columns++;
            }

            rows = totalTiles / columns;
            if (totalTiles % columns != 0)
            {
                rows++;
            }
        }

        width = (columns * TileSize.Width) +
                (options.BorderPadding * 2) +
                (options.Spacing * (columns - 1)) +
                (options.InnerPadding * 2 * columns);

        height = (rows * TileSize.Height) +
                 (options.BorderPadding * 2) +
                 (options.Spacing * (rows - 1)) +
                 (options.InnerPadding * 2 * rows);

        Size sheetSize = new(width, height);

        Rgba32[] sheetPixels = new Rgba32[width * height];

        Dictionary<int, TilesheetTile> originalToDuplicateTileLookup = new();

        int tOffset = 0;

        for (int tileNum = 0; tileNum < TileCount; tileNum++)
        {
            if (!options.MergeDuplicates || !tileDuplicateMap.ContainsKey(tileNum))
            {
                //  Calculate the x and y position of the tile's top-left pixel
                //  relative to the top-left of the file tilesheet
                int tileCol = (tileNum - tOffset) % columns;
                int tileRow = (tileNum - tOffset) / columns;

                //  Inject the pixel color data from the tile into the final
                //  tilesheet color data array
                Rgba32[] tilePixels = tileColorLookup[tileNum];

                for (int pixelNum = 0; pixelNum < tilePixels.Length; pixelNum++)
                {
                    int x = (pixelNum % TileSize.Width) + (tileCol * TileSize.Width);
                    int y = (pixelNum / TileSize.Width) + (tileRow * TileSize.Height);

                    //  Adjust for padding/spacing
                    x += options.BorderPadding;
                    y += options.BorderPadding;

                    if (options.Spacing > 0)
                    {
                        if (tileCol > 0)
                        {
                            x += options.Spacing * tileCol;
                        }

                        if (tileRow > 0)
                        {
                            y += options.Spacing * tileRow;
                        }
                    }

                    if (options.InnerPadding > 0)
                    {
                        x += options.InnerPadding * (tileCol + 1);
                        y += options.InnerPadding * (tileRow + 1);

                        if (tileCol > 0)
                        {
                            x += options.InnerPadding * tileCol;
                        }

                        if (tileRow > 0)
                        {
                            y += options.InnerPadding * tileRow;
                        }
                    }

                    int index = y * width + x;
                    sheetPixels[index] = tilePixels[pixelNum];
                }

                //  Now create the tile source rectangle data
                Rectangle sourceRectangle = new(0, 0, TileSize.Width, TileSize.Height);
                sourceRectangle.X += options.BorderPadding;
                sourceRectangle.Y += options.BorderPadding;

                if (options.Spacing > 0)
                {
                    if (tileCol > 0)
                    {
                        sourceRectangle.X += options.Spacing * tileCol;
                    }

                    if (tileRow > 0)
                    {
                        sourceRectangle.Y += options.Spacing * tileRow;
                    }
                }

                if (options.InnerPadding > 0)
                {
                    sourceRectangle.X += options.InnerPadding * (tileCol + 1);
                    sourceRectangle.Y += options.InnerPadding * (tileRow + 1);

                    if (tileCol > 0)
                    {
                        sourceRectangle.X += options.InnerPadding * tileCol;
                    }

                    if (tileRow > 0)
                    {
                        sourceRectangle.Y += options.InnerPadding * tileRow;
                    }
                }

                TilesheetTile tile = new(sourceRectangle);

                sheetTiles.Add(tile);
                originalToDuplicateTileLookup.Add(tileNum, tile);
            }
            else
            {
                //  We are merging duplicates and it was detected that the
                //  current tile to process is a duplicate.  So we still need to
                //  add the tile, but we need to make sure the tile source
                //  rectangle is the same as the tile it's a duplicate of.
                TilesheetTile original = originalToDuplicateTileLookup[tileDuplicateMap[tileNum]];
                sheetTiles.Add(new TilesheetTile(original.SourceRectangle));
                tOffset++;
            }
        }

        return new Tilesheet(Name, sheetSize, sheetTiles, sheetPixels);
    }


    /// <summary>
    ///     Writes the pixel data for this <see cref="ImageCel"/> to disk as a
    ///     .png file.
    /// </summary>
    /// <param name="path">
    ///     The absolute file path to save the generated .png file to.
    /// </param>
    /// <param name="method">
    ///     The packing method to use when creating the tileset image.
    /// </param>
    public void ToPng(string path, PackingMethod method)
    {
        //  Aseprite stores the pixel data for a tileset such that the image
        //  is a vertical packed sprite.  If the packing method being requested
        //  is vertical packing, then we can just output, otherwise, we need to
        //  repack the pixel data for the packing method requested
        if (method == PackingMethod.VerticalStrip)
        {
            PngWriter.SaveTo(path, new Size(TileSize.Width, TileSize.Height * TileCount), Pixels);
        }
        else
        {
            //  Separate each tile into a separate array of pixels
            // Color[][] tiles = new Color[TileCount][];
            // int tileLen = TileSize.Width * TileSize.Height;

            // for (int i = 0; i < TileCount; i++)
            // {
            //     tiles[i] = Pixels[(i * tileLen)..((i * tileLen) + tileLen)];
            // }

            List<Rgba32[]> tiles = SplitTiles();

            int columns, rows;
            int width, height;

            if (method == PackingMethod.HorizontalStrip)
            {
                columns = TileCount;
                rows = 1;
            }
            else
            {
                double sqrt = Math.Sqrt(TileCount);
                columns = (int)Math.Floor(sqrt);
                if (Math.Abs(sqrt % 1) >= double.Epsilon)
                {
                    columns++;
                }

                rows = TileCount / columns;
                if (TileCount % columns != 0)
                {
                    rows++;
                }
            }

            width = columns * TileSize.Width;
            height = rows * TileSize.Height;

            Rgba32[] pixels = new Rgba32[width * height];

            for (int tileNum = 0; tileNum < TileCount; tileNum++)
            {
                int tileCol = tileNum % columns;
                int tileRow = tileNum / columns;
                Rgba32[] tilePixels = tiles[tileNum];

                for (int pixelNum = 0; pixelNum < tilePixels.Length; pixelNum++)
                {
                    int x = (pixelNum % TileSize.Width) + (tileCol * TileSize.Width);
                    int y = (pixelNum / TileSize.Width) + (tileRow * TileSize.Height);
                    int index = y * width + x;
                    pixels[index] = tilePixels[pixelNum];
                }
            }

            PngWriter.SaveTo(path, new Size(width, height), pixels);
        }
    }

    internal List<Rgba32[]> SplitTiles()
    {
        List<Rgba32[]> tiles = new();

        int tileLen = TileSize.Width * TileSize.Height;

        for (int i = 0; i < TileCount; i++)
        {
            tiles.Add(Pixels[(i * tileLen)..((i * tileLen) + tileLen)]);
        }

        return tiles;
    }
}
