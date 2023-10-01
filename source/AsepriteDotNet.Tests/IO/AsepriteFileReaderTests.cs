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
using AsepriteDotNet.IO;

namespace AsepriteDotNet.Tests;

public sealed class AsepriteFileReaderTest
{
    private string GetPath(string name)
    {
        return Path.Combine(Environment.CurrentDirectory, "Files", name);
    }

    [Fact]
    public void AsepriteFileReader_ReadFileTest()
    {
        string path = GetPath("read-test.aseprite");
        AsepriteFile doc = AsepriteFileReader.ReadFile(path);

        //  Expected pallette colors
        Color pal0 = Color.FromRGBA(223, 7, 114, 255);
        Color pal1 = Color.FromRGBA(254, 84, 111, 255);
        Color pal2 = Color.FromRGBA(255, 158, 125, 255);
        Color pal3 = Color.FromRGBA(255, 208, 128, 255);
        Color pal4 = Color.FromRGBA(255, 253, 255, 255);
        Color pal5 = Color.FromRGBA(11, 255, 230, 255);
        Color pal6 = Color.FromRGBA(1, 203, 207, 255);
        Color pal7 = Color.FromRGBA(1, 136, 165, 255);
        Color pal8 = Color.FromRGBA(62, 50, 100, 255);
        Color pal9 = Color.FromRGBA(53, 42, 85, 255);

        //  Validate palette
        Assert.True(doc.Palette.Count == 10);
        Assert.Equal(pal0, doc.Palette[0]);
        Assert.Equal(pal1, doc.Palette[1]);
        Assert.Equal(pal2, doc.Palette[2]);
        Assert.Equal(pal3, doc.Palette[3]);
        Assert.Equal(pal4, doc.Palette[4]);
        Assert.Equal(pal5, doc.Palette[5]);
        Assert.Equal(pal6, doc.Palette[6]);
        Assert.Equal(pal7, doc.Palette[7]);
        Assert.Equal(pal8, doc.Palette[8]);
        Assert.Equal(pal9, doc.Palette[9]);

        //  Validate Layers
        Assert.Equal(11, doc.Layers.Count);
        Assert.IsType<ImageLayer>(doc.Layers[0]);
        Assert.True(doc.Layers[0].IsBackgroundLayer);
        Assert.Equal("background", doc.Layers[0].Name);
        Assert.True(doc.Layers[0].IsVisible);
        Assert.Equal("hidden", doc.Layers[1].Name);
        Assert.False(doc.Layers[1].IsVisible);
        Assert.Equal("user-data", doc.Layers[2].Name);
        Assert.Equal("user-data text", doc.Layers[2].UserData.Text);
        Assert.Equal(Color.FromRGBA(223, 7, 114, 255), doc.Layers[2].UserData.Color);
        Assert.Equal("reference", doc.Layers[3].Name);
        Assert.True(doc.Layers[3].IsReferenceLayer);
        Assert.Equal("75-opacity", doc.Layers[4].Name);
        Assert.Equal(75, doc.Layers[4].Opacity);
        Assert.Equal("blendmode-difference", doc.Layers[5].Name);
        Assert.Equal(BlendMode.Difference, doc.Layers[5].BlendMode);
        Assert.Equal("tilemap", doc.Layers[6].Name);
        Assert.Equal(0, Assert.IsType<TilemapLayer>(doc.Layers[6]).Tileset.ID);
        Assert.Equal(2, Assert.IsType<GroupLayer>(doc.Layers[7]).Children.Count);
        Assert.Equal(1, doc.Layers[8].ChildLevel);
        Assert.Equal(1, doc.Layers[9].ChildLevel);

        //  Validate Tags
        Assert.Equal(3, doc.Tags.Count);
        Assert.Equal("tag0to2forward", doc.Tags[0].Name);
        Assert.Equal(0, doc.Tags[0].From);
        Assert.Equal(2, doc.Tags[0].To);
        Assert.Equal(Color.FromRGBA(0, 0, 0, 255), doc.Tags[0].Color);
        Assert.Equal(LoopDirection.Forward, doc.Tags[0].LoopDirection);
        Assert.Equal("tag3pingpong", doc.Tags[1].Name);
        Assert.Equal(LoopDirection.PingPong, doc.Tags[1].LoopDirection);
        Assert.Equal("tag4userdata", doc.Tags[2].Name);
        Assert.Equal(Color.FromRGBA(11, 255, 230, 255), doc.Tags[2].Color);
        Assert.Equal(Color.FromRGBA(11, 255, 230, 255), doc.Tags[2].UserData.Color);
        Assert.Equal("tag-4-user-data", doc.Tags[2].UserData.Text);

        //  Validate Frames
        Assert.Equal(7, doc.Frames.Count);
        Assert.Equal(100, doc.Frames[0].Duration);
        Assert.Equal(200, doc.Frames[1].Duration);
        Assert.Equal(123, doc.Frames[2].Duration);
        Assert.Equal(2, doc.Frames[0].Cels.Count);  //  Background and Reference Layer cels

        //  Validate Cels
        ImageCel fgCel = Assert.IsType<ImageCel>(doc.Frames[2].Cels[1]);
        Assert.Equal("foreground", fgCel.Layer.Name);
        Assert.Equal(new Size(16, 16), fgCel.Size);
        Assert.Equal(new Point(8, 8), fgCel.Position);
        Assert.Equal(fgCel.Size.Width * fgCel.Size.Height, fgCel.Pixels.Length);
    }

    [Fact]
    public void AsepriteFileReader_ColorModeRGBA_PixelsTest()
    {
        string path = GetPath("rgba-pixel-test.aseprite");
        AsepriteFile doc = AsepriteFileReader.ReadFile(path);

        Assert.Equal(10, doc.Palette.Count);

        Color tran = Color.Transparent;
        Color pal0 = doc.Palette[0];
        Color pal1 = doc.Palette[1];
        Color pal2 = doc.Palette[2];
        Color pal3 = doc.Palette[3];
        Color pal4 = doc.Palette[4];
        Color pal5 = doc.Palette[5];
        Color pal6 = doc.Palette[6];
        Color pal7 = doc.Palette[7];
        Color pal8 = doc.Palette[8];
        Color pal9 = doc.Palette[9];

        Color[] expected = new Color[]
        {
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5,
            pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1,
            pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3,
            pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5,
            pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1,
            pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3,
            pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5,
            pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1,
            pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3,
            pal4, pal5, pal6, pal7, pal8, pal9, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9,
            pal0, pal9, pal1, pal8, pal2, pal7, pal3, pal6, tran, tran, tran, tran, tran, tran, tran, tran
        };

        ImageCel cel = Assert.IsType<ImageCel>(doc.Frames[0].Cels[0]);
        Assert.Equal(expected, cel.Pixels);
    }

    [Fact]
    public void AsepriteFileReader_ColorModeIndexed_PixelsTest()
    {
        string path = GetPath("indexed-pixel-test.aseprite");
        AsepriteFile doc = AsepriteFileReader.ReadFile(path);

        Assert.Equal(11, doc.Palette.Count);

        Color pal0 = Color.Transparent;
        Color pal1 = doc.Palette[1];
        Color pal2 = doc.Palette[2];
        Color pal3 = doc.Palette[3];
        Color pal4 = doc.Palette[4];
        Color pal5 = doc.Palette[5];
        Color pal6 = doc.Palette[6];
        Color pal7 = doc.Palette[7];
        Color pal8 = doc.Palette[8];
        Color pal9 = doc.Palette[9];
        Color pal10 = doc.Palette[10];

        Color[] expected = new Color[]
        {
            pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6,
            pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2,
            pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8,
            pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4,
            pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10,
            pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6,
            pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2,
            pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8,
            pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4,
            pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10,
            pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6,
            pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2,
            pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8,
            pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4,
            pal5, pal6, pal7, pal8, pal9, pal10, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal8, pal9, pal10,
            pal1, pal10, pal2, pal9, pal3, pal8, pal4, pal7, pal0, pal0, pal0, pal0, pal0, pal0, pal0, pal0
        };

        ImageCel cel = Assert.IsType<ImageCel>(doc.Frames[0].Cels[0]);
        Assert.Equal(expected, cel.Pixels);
    }

    [Fact]
    public void AsepriteFileReader_GrayscaleModeRGBA_PixelsTest()
    {
        string path = GetPath("grayscale-pixel-test.aseprite");
        AsepriteFile doc = AsepriteFileReader.ReadFile(path);

        Assert.Equal(8, doc.Palette.Count);

        Color tran = Color.Transparent;
        Color pal0 = doc.Palette[0];
        Color pal1 = doc.Palette[1];
        Color pal2 = doc.Palette[2];
        Color pal3 = doc.Palette[3];
        Color pal4 = doc.Palette[4];
        Color pal5 = doc.Palette[5];
        Color pal6 = doc.Palette[6];
        Color pal7 = doc.Palette[7];

        Color[] expected = new Color[]
        {
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0,
            pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7, pal0, pal1, pal2, pal3, pal4, pal5, pal6, pal7,
            pal7, pal6, pal5, pal4, pal3, pal2, pal1, pal0, tran, tran, tran, tran, tran, tran, tran, tran
        };

        ImageCel cel = Assert.IsType<ImageCel>(doc.Frames[0].Cels[0]);
        Assert.Equal(expected, cel.Pixels);
    }

    [Fact]
    public void AsepriteFileReader_TilemapTest()
    {
        string path = GetPath("tilemap-test.aseprite");
        AsepriteFile doc = AsepriteFileReader.ReadFile(path);

        Color tran = Color.Transparent;
        Color pal0 = doc.Palette[0];
        Color pal1 = doc.Palette[1];
        Color pal2 = doc.Palette[2];
        Color pal3 = doc.Palette[3];
        Color pal4 = doc.Palette[4];
        Color pal5 = doc.Palette[5];
        Color pal6 = doc.Palette[6];
        Color pal7 = doc.Palette[7];
        Color pal8 = doc.Palette[8];
        Color pal9 = doc.Palette[9];

        Assert.Single(doc.Tilesets);
        Tileset tileset = doc.Tilesets[0];
        Assert.Equal("test-tileset", tileset.Name);

        Assert.Equal(0, tileset.ID);
        Assert.Equal(11, tileset.TileCount);
        Assert.Equal(new Size(8, 8), tileset.TileSize);

        Color[] expectedTilesetPixels = new Color[]
        {
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,
            tran, tran, tran, tran, tran, tran, tran, tran,

            pal0, pal0, pal0, pal0, pal0, pal0, pal0, pal0,
            pal0, tran, tran, tran, tran, tran, tran, pal0,
            pal0, tran, tran, tran, tran, tran, tran, pal0,
            pal0, tran, tran, tran, tran, tran, tran, pal0,
            pal0, tran, tran, tran, tran, tran, tran, pal0,
            pal0, tran, tran, tran, tran, tran, tran, pal0,
            pal0, tran, tran, tran, tran, tran, tran, pal0,
            pal0, pal0, pal0, pal0, pal0, pal0, pal0, pal0,

            pal1, pal1, pal1, pal1, pal1, pal1, pal1, pal1,
            pal1, tran, tran, tran, tran, tran, tran, pal1,
            pal1, tran, tran, tran, tran, tran, tran, pal1,
            pal1, tran, tran, tran, tran, tran, tran, pal1,
            pal1, tran, tran, tran, tran, tran, tran, pal1,
            pal1, tran, tran, tran, tran, tran, tran, pal1,
            pal1, tran, tran, tran, tran, tran, tran, pal1,
            pal1, pal1, pal1, pal1, pal1, pal1, pal1, pal1,

            pal2, pal2, pal2, pal2, pal2, pal2, pal2, pal2,
            pal2, tran, tran, tran, tran, tran, tran, pal2,
            pal2, tran, tran, tran, tran, tran, tran, pal2,
            pal2, tran, tran, tran, tran, tran, tran, pal2,
            pal2, tran, tran, tran, tran, tran, tran, pal2,
            pal2, tran, tran, tran, tran, tran, tran, pal2,
            pal2, tran, tran, tran, tran, tran, tran, pal2,
            pal2, pal2, pal2, pal2, pal2, pal2, pal2, pal2,

            pal3, pal3, pal3, pal3, pal3, pal3, pal3, pal3,
            pal3, tran, tran, tran, tran, tran, tran, pal3,
            pal3, tran, tran, tran, tran, tran, tran, pal3,
            pal3, tran, tran, tran, tran, tran, tran, pal3,
            pal3, tran, tran, tran, tran, tran, tran, pal3,
            pal3, tran, tran, tran, tran, tran, tran, pal3,
            pal3, tran, tran, tran, tran, tran, tran, pal3,
            pal3, pal3, pal3, pal3, pal3, pal3, pal3, pal3,

            pal4, pal4, pal4, pal4, pal4, pal4, pal4, pal4,
            pal4, tran, tran, tran, tran, tran, tran, pal4,
            pal4, tran, tran, tran, tran, tran, tran, pal4,
            pal4, tran, tran, tran, tran, tran, tran, pal4,
            pal4, tran, tran, tran, tran, tran, tran, pal4,
            pal4, tran, tran, tran, tran, tran, tran, pal4,
            pal4, tran, tran, tran, tran, tran, tran, pal4,
            pal4, pal4, pal4, pal4, pal4, pal4, pal4, pal4,

            pal5, pal5, pal5, pal5, pal5, pal5, pal5, pal5,
            pal5, tran, tran, tran, tran, tran, tran, pal5,
            pal5, tran, tran, tran, tran, tran, tran, pal5,
            pal5, tran, tran, tran, tran, tran, tran, pal5,
            pal5, tran, tran, tran, tran, tran, tran, pal5,
            pal5, tran, tran, tran, tran, tran, tran, pal5,
            pal5, tran, tran, tran, tran, tran, tran, pal5,
            pal5, pal5, pal5, pal5, pal5, pal5, pal5, pal5,

            pal6, pal6, pal6, pal6, pal6, pal6, pal6, pal6,
            pal6, tran, tran, tran, tran, tran, tran, pal6,
            pal6, tran, tran, tran, tran, tran, tran, pal6,
            pal6, tran, tran, tran, tran, tran, tran, pal6,
            pal6, tran, tran, tran, tran, tran, tran, pal6,
            pal6, tran, tran, tran, tran, tran, tran, pal6,
            pal6, tran, tran, tran, tran, tran, tran, pal6,
            pal6, pal6, pal6, pal6, pal6, pal6, pal6, pal6,

            pal7, pal7, pal7, pal7, pal7, pal7, pal7, pal7,
            pal7, tran, tran, tran, tran, tran, tran, pal7,
            pal7, tran, tran, tran, tran, tran, tran, pal7,
            pal7, tran, tran, tran, tran, tran, tran, pal7,
            pal7, tran, tran, tran, tran, tran, tran, pal7,
            pal7, tran, tran, tran, tran, tran, tran, pal7,
            pal7, tran, tran, tran, tran, tran, tran, pal7,
            pal7, pal7, pal7, pal7, pal7, pal7, pal7, pal7,

            pal8, pal8, pal8, pal8, pal8, pal8, pal8, pal8,
            pal8, tran, tran, tran, tran, tran, tran, pal8,
            pal8, tran, tran, tran, tran, tran, tran, pal8,
            pal8, tran, tran, tran, tran, tran, tran, pal8,
            pal8, tran, tran, tran, tran, tran, tran, pal8,
            pal8, tran, tran, tran, tran, tran, tran, pal8,
            pal8, tran, tran, tran, tran, tran, tran, pal8,
            pal8, pal8, pal8, pal8, pal8, pal8, pal8, pal8,

            pal9, pal9, pal9, pal9, pal9, pal9, pal9, pal9,
            pal9, tran, tran, tran, tran, tran, tran, pal9,
            pal9, tran, tran, tran, tran, tran, tran, pal9,
            pal9, tran, tran, tran, tran, tran, tran, pal9,
            pal9, tran, tran, tran, tran, tran, tran, pal9,
            pal9, tran, tran, tran, tran, tran, tran, pal9,
            pal9, tran, tran, tran, tran, tran, tran, pal9,
            pal9, pal9, pal9, pal9, pal9, pal9, pal9, pal9,
        };

        Assert.Equal(expectedTilesetPixels, tileset.Pixels);


        TilemapLayer tilesLayer = Assert.IsType<TilemapLayer>(doc.Layers[1]);
        Assert.Equal(tileset, tilesLayer.Tileset);
        // Assert.Equal(tileset.ID, tilesLayer.TilesetIndex);

        TilemapCel tilesCel = Assert.IsType<TilemapCel>(doc.Frames[0].Cels[1]);

        Assert.Equal(32, tilesCel.BitsPerTile);
        Assert.Equal((uint)0x1fffffff, tilesCel.TileIdBitmask);
        Assert.Equal((uint)0x20000000, tilesCel.XFlipBitmask);
        Assert.Equal((uint)0x40000000, tilesCel.YFlipBitmask);
        Assert.Equal(0x80000000, tilesCel.RotationBitmask);
        Assert.Equal(16, tilesCel.Tiles.Length);

        uint[] ids = new uint[]
        {
            1, 2, 3, 1,
            3, 4, 5, 2,
            2, 6, 7, 3,
            1, 3, 2, 1
        };

        Assert.Equal(ids, tilesCel.Tiles.Select(tile => tile.TileID).ToArray());

        //  Can't test Tile.XFlip, Tile.YFlip, and Tile.Rotate90 because these
        //  aren't actually implemented in Aseprite yet.
    }

    [Fact]
    public void AsepriteFileReader_SpriteUserDataTest()
    {
        string path = GetPath("sprite-userdata-test.aseprite");
        AsepriteFile doc = AsepriteFileReader.ReadFile(path);
        Assert.Equal("Test Sprite UserData", doc.UserData.Text);
        Assert.Equal(Color.FromRGBA(1, 2, 3, 4), doc.UserData.Color);
    }
}