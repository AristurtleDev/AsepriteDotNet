// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Document;
using AsepriteDotNet.IO;

namespace AsepriteDotNet.Tests.IO
{
    public class AsepriteFileLoaderTests
    {
        private string GetPath(string name)
        {
            return Path.Combine(Environment.CurrentDirectory, "Files", name);
        }

        [Fact]
        public void AsepriteFileReader_ReadFileTest()
        {
            string path = GetPath("read-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            //  Expected palette colors
            AseColor pal0 = new AseColor(223, 7, 114, 255);
            AseColor pal1 = new AseColor(254, 84, 111, 255);
            AseColor pal2 = new AseColor(255, 158, 125, 255);
            AseColor pal3 = new AseColor(255, 208, 128, 255);
            AseColor pal4 = new AseColor(255, 253, 255, 255);
            AseColor pal5 = new AseColor(11, 255, 230, 255);
            AseColor pal6 = new AseColor(1, 203, 207, 255);
            AseColor pal7 = new AseColor(1, 136, 165, 255);
            AseColor pal8 = new AseColor(62, 50, 100, 255);
            AseColor pal9 = new AseColor(53, 42, 85, 255);

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
            Assert.Equal(11, doc.Layers.Length);
            Assert.IsType<ImageLayer>(doc.Layers[0]);
            Assert.True(doc.Layers[0].IsBackgroundLayer);
            Assert.Equal("background", doc.Layers[0].Name);
            Assert.True(doc.Layers[0].IsVisible);
            Assert.Equal("hidden", doc.Layers[1].Name);
            Assert.False(doc.Layers[1].IsVisible);
            Assert.Equal("user-data", doc.Layers[2].Name);
            Assert.Equal("user-data text", doc.Layers[2].UserData.Text);
            Assert.Equal(new AseColor(223, 7, 114, 255), doc.Layers[2].UserData.Color);
            Assert.Equal("reference", doc.Layers[3].Name);
            Assert.True(doc.Layers[3].IsReferenceLayer);
            Assert.Equal("75-opacity", doc.Layers[4].Name);
            Assert.Equal(75, doc.Layers[4].Opacity);
            Assert.Equal("blendmode-difference", doc.Layers[5].Name);
            Assert.Equal(AsepriteBlendMode.Difference, doc.Layers[5].BlendMode);
            Assert.Equal("tilemap", doc.Layers[6].Name);
            Assert.Equal(0, Assert.IsType<TilemapLayer>(doc.Layers[6]).Tileset.ID);
            Assert.Equal(2, Assert.IsType<GroupLayer>(doc.Layers[7]).Children.Length);
            Assert.Equal(1, doc.Layers[8].ChildLevel);
            Assert.Equal(1, doc.Layers[9].ChildLevel);

            //  Validate Tags
            Assert.Equal(3, doc.Tags.Length);
            Assert.Equal("tag0to2forward", doc.Tags[0].Name);
            Assert.Equal(0, doc.Tags[0].From);
            Assert.Equal(2, doc.Tags[0].To);
            Assert.Equal(new AseColor(0, 0, 0, 255), doc.Tags[0].Color);
            Assert.Equal(AsepriteLoopDirection.Forward, doc.Tags[0].LoopDirection);
            Assert.Equal("tag3pingpong", doc.Tags[1].Name);
            Assert.Equal(AsepriteLoopDirection.PingPong, doc.Tags[1].LoopDirection);
            Assert.Equal("tag4userdata", doc.Tags[2].Name);
            Assert.Equal(new AseColor(11, 255, 230, 255), doc.Tags[2].Color);
            Assert.Equal(new AseColor(11, 255, 230, 255), doc.Tags[2].UserData.Color);
            Assert.Equal("tag-4-user-data", doc.Tags[2].UserData.Text);

            //  Validate Frames
            Assert.Equal(7, doc.Frames.Length);
            Assert.Equal(TimeSpan.FromMilliseconds(100), doc.Frames[0].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(200), doc.Frames[1].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(123), doc.Frames[2].Duration);
            Assert.Equal(2, doc.Frames[0].Cels.Length);  //  Background and Reference Layer cels

            //  Validate Cels
            ImageCel fgCel = Assert.IsType<ImageCel>(doc.Frames[2].Cels[1]);
            Assert.Equal("foreground", fgCel.Layer.Name);
            Assert.Equal(16, fgCel.Width);
            Assert.Equal(16, fgCel.Height);
            Assert.Equal(8, fgCel.X);
            Assert.Equal(8, fgCel.Y);
            Assert.Equal(fgCel.Width * fgCel.Height, fgCel.Pixels.Length);
        }

        [Fact]
        public void AsepriteFileReader_ColorModeRGBA_PixelsTest()
        {
            string path = GetPath("rgba-pixel-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Assert.Equal(10, doc.Palette.Count);

            AseColor tran = new AseColor(0, 0, 0, 0);
            AseColor pal0 = doc.Palette[0];
            AseColor pal1 = doc.Palette[1];
            AseColor pal2 = doc.Palette[2];
            AseColor pal3 = doc.Palette[3];
            AseColor pal4 = doc.Palette[4];
            AseColor pal5 = doc.Palette[5];
            AseColor pal6 = doc.Palette[6];
            AseColor pal7 = doc.Palette[7];
            AseColor pal8 = doc.Palette[8];
            AseColor pal9 = doc.Palette[9];

            AseColor[] expected = new AseColor[]
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
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Assert.Equal(11, doc.Palette.Count);

            AseColor pal0 = new AseColor(0, 0, 0, 0);
            AseColor pal1 = doc.Palette[1];
            AseColor pal2 = doc.Palette[2];
            AseColor pal3 = doc.Palette[3];
            AseColor pal4 = doc.Palette[4];
            AseColor pal5 = doc.Palette[5];
            AseColor pal6 = doc.Palette[6];
            AseColor pal7 = doc.Palette[7];
            AseColor pal8 = doc.Palette[8];
            AseColor pal9 = doc.Palette[9];
            AseColor pal10 = doc.Palette[10];

            AseColor[] expected = new AseColor[]
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
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Assert.Equal(8, doc.Palette.Count);

            AseColor tran = new AseColor(0, 0, 0, 0);
            AseColor pal0 = doc.Palette[0];
            AseColor pal1 = doc.Palette[1];
            AseColor pal2 = doc.Palette[2];
            AseColor pal3 = doc.Palette[3];
            AseColor pal4 = doc.Palette[4];
            AseColor pal5 = doc.Palette[5];
            AseColor pal6 = doc.Palette[6];
            AseColor pal7 = doc.Palette[7];

            AseColor[] expected = new AseColor[]
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
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            AseColor tran = new AseColor(0, 0, 0, 0);
            AseColor pal0 = doc.Palette[0];
            AseColor pal1 = doc.Palette[1];
            AseColor pal2 = doc.Palette[2];
            AseColor pal3 = doc.Palette[3];
            AseColor pal4 = doc.Palette[4];
            AseColor pal5 = doc.Palette[5];
            AseColor pal6 = doc.Palette[6];
            AseColor pal7 = doc.Palette[7];
            AseColor pal8 = doc.Palette[8];
            AseColor pal9 = doc.Palette[9];

            Assert.Equal(1, doc.Tilesets.Length);
            Tileset tileset = doc.Tilesets[0];
            Assert.Equal("test-tileset", tileset.Name);

            Assert.Equal(0, tileset.ID);
            Assert.Equal(11, tileset.TileCount);
            Assert.Equal(8, tileset.Width);
            Assert.Equal(8, tileset.Height);

            AseColor[] expectedTilesetPixels = new AseColor[]
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

            TilemapCel tilesCel = Assert.IsType<TilemapCel>(doc.Frames[0].Cels[1]);
            Assert.Equal(16, tilesCel.Tiles.Length);

            int[] ids = new int[]
            {
            1, 2, 3, 1,
            3, 4, 5, 2,
            2, 6, 7, 3,
            1, 3, 2, 1
            };

            for (int i = 0; i < ids.Length; i++)
            {
                Assert.Equal(ids[i], tilesCel.Tiles[i].ID);
            }
            //Assert.Equal(ids, tilesCel.Tiles.Select(tile => tile.TileID).ToArray());

            //  Can't test Tile.XFlip, Tile.YFlip, and Tile.Rotate90 because these
            //  aren't actually implemented in Aseprite yet.
        }

        [Fact]
        public void AsepriteFileReader_SpriteUserDataTest()
        {
            string path = GetPath("sprite-userdata-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);
            Assert.Equal("Test Sprite UserData", doc.UserData.Text);
            Assert.Equal(new AseColor(1, 2, 3, 4), doc.UserData.Color);
        }
    }
}
