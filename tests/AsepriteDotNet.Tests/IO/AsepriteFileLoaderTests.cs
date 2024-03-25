// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;

namespace AsepriteDotNet.Tests.IO
{
    public class AsepriteFileLoaderTests
    {
        private static string GetPath(string name)
        {
            return Path.Combine(Environment.CurrentDirectory, "Files", name);
        }

        [Fact]
        public void AsepriteFileReader_ReadFileTest()
        {
            string path = GetPath("read-test.aseprite");
            AsepriteFile<SystemColor> doc = AsepriteFileLoader.FromFile<SystemColor>(path);

            //  Expected palette colors
            SystemColor pal0 = new SystemColor(223, 7, 114, 255);
            SystemColor pal1 = new SystemColor(254, 84, 111, 255);
            SystemColor pal2 = new SystemColor(255, 158, 125, 255);
            SystemColor pal3 = new SystemColor(255, 208, 128, 255);
            SystemColor pal4 = new SystemColor(255, 253, 255, 255);
            SystemColor pal5 = new SystemColor(11, 255, 230, 255);
            SystemColor pal6 = new SystemColor(1, 203, 207, 255);
            SystemColor pal7 = new SystemColor(1, 136, 165, 255);
            SystemColor pal8 = new SystemColor(62, 50, 100, 255);
            SystemColor pal9 = new SystemColor(53, 42, 85, 255);

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
            Assert.IsType<AsepriteImageLayer<SystemColor>>(doc.Layers[0]);
            Assert.True(doc.Layers[0].IsBackgroundLayer);
            Assert.Equal("background", doc.Layers[0].Name);
            Assert.True(doc.Layers[0].IsVisible);
            Assert.Equal("hidden", doc.Layers[1].Name);
            Assert.False(doc.Layers[1].IsVisible);
            Assert.Equal("user-data", doc.Layers[2].Name);
            Assert.Equal("user-data text", doc.Layers[2].UserData.Text);
            Assert.Equal(new SystemColor(223, 7, 114, 255), doc.Layers[2].UserData.Color);
            Assert.Equal("reference", doc.Layers[3].Name);
            Assert.True(doc.Layers[3].IsReferenceLayer);
            Assert.Equal("75-opacity", doc.Layers[4].Name);
            Assert.Equal(75, doc.Layers[4].Opacity);
            Assert.Equal("blendmode-difference", doc.Layers[5].Name);
            Assert.Equal(AsepriteBlendMode.Difference, doc.Layers[5].BlendMode);
            Assert.Equal("tilemap", doc.Layers[6].Name);
            Assert.Equal(0, Assert.IsType<AsepriteTilemapLayer<SystemColor>>(doc.Layers[6]).Tileset.ID);
            Assert.Equal(2, Assert.IsType<AsepriteGroupLayer<SystemColor>>(doc.Layers[7]).Children.Length);
            Assert.Equal(1, doc.Layers[8].ChildLevel);
            Assert.Equal(1, doc.Layers[9].ChildLevel);

            //  Validate Tags
            Assert.Equal(3, doc.Tags.Length);
            Assert.Equal("tag0to2forward", doc.Tags[0].Name);
            Assert.Equal(0, doc.Tags[0].From);
            Assert.Equal(2, doc.Tags[0].To);
            Assert.Equal(new SystemColor(0, 0, 0, 255), doc.Tags[0].Color);
            Assert.Equal(AsepriteLoopDirection.Forward, doc.Tags[0].LoopDirection);
            Assert.Equal("tag3pingpong", doc.Tags[1].Name);
            Assert.Equal(AsepriteLoopDirection.PingPong, doc.Tags[1].LoopDirection);
            Assert.Equal("tag4userdata", doc.Tags[2].Name);
            Assert.Equal(new SystemColor(11, 255, 230, 255), doc.Tags[2].Color);
            Assert.Equal(new SystemColor(11, 255, 230, 255), doc.Tags[2].UserData.Color);
            Assert.Equal("tag-4-user-data", doc.Tags[2].UserData.Text);

            //  Validate Frames
            Assert.Equal(7, doc.Frames.Length);
            Assert.Equal(TimeSpan.FromMilliseconds(100), doc.Frames[0].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(200), doc.Frames[1].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(123), doc.Frames[2].Duration);
            Assert.Equal(2, doc.Frames[0].Cels.Length);  //  Background and Reference Layer cels

            //  Validate Cels
            AsepriteImageCel<SystemColor> fgCel = Assert.IsType<AsepriteImageCel<SystemColor>>(doc.Frames[2].Cels[1]);
            Assert.Equal("foreground", fgCel.Layer.Name);
            Assert.Equal(16, fgCel.Size.Width);
            Assert.Equal(16, fgCel.Size.Height);
            Assert.Equal(8, fgCel.Location.X);
            Assert.Equal(8, fgCel.Location.Y);
            Assert.Equal(fgCel.Size.Width * fgCel.Size.Height, fgCel.Pixels.Length);
        }

        [Fact]
        public void AsepriteFileReader_ColorModeRGBA_PixelsTest()
        {
            string path = GetPath("rgba-pixel-test.aseprite");
            AsepriteFile<SystemColor> doc = AsepriteFileLoader.FromFile<SystemColor>(path);

            Assert.Equal(10, doc.Palette.Count);

            SystemColor tran = new SystemColor(0, 0, 0, 0);
            SystemColor pal0 = doc.Palette[0];
            SystemColor pal1 = doc.Palette[1];
            SystemColor pal2 = doc.Palette[2];
            SystemColor pal3 = doc.Palette[3];
            SystemColor pal4 = doc.Palette[4];
            SystemColor pal5 = doc.Palette[5];
            SystemColor pal6 = doc.Palette[6];
            SystemColor pal7 = doc.Palette[7];
            SystemColor pal8 = doc.Palette[8];
            SystemColor pal9 = doc.Palette[9];

            SystemColor[] expected = new SystemColor[]
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

            AsepriteImageCel<SystemColor> cel = Assert.IsType<AsepriteImageCel<SystemColor>>(doc.Frames[0].Cels[0]);
            Assert.Equal(expected, cel.Pixels);
        }

        [Fact]
        public void AsepriteFileReader_ColorModeIndexed_PixelsTest()
        {
            string path = GetPath("indexed-pixel-test.aseprite");
            AsepriteFile<SystemColor> doc = AsepriteFileLoader.FromFile<SystemColor>(path);

            Assert.Equal(11, doc.Palette.Count);

            SystemColor pal0 = new SystemColor(0, 0, 0, 0);
            SystemColor pal1 = doc.Palette[1];
            SystemColor pal2 = doc.Palette[2];
            SystemColor pal3 = doc.Palette[3];
            SystemColor pal4 = doc.Palette[4];
            SystemColor pal5 = doc.Palette[5];
            SystemColor pal6 = doc.Palette[6];
            SystemColor pal7 = doc.Palette[7];
            SystemColor pal8 = doc.Palette[8];
            SystemColor pal9 = doc.Palette[9];
            SystemColor pal10 = doc.Palette[10];

            SystemColor[] expected = new SystemColor[]
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

            AsepriteImageCel<SystemColor> cel = Assert.IsType<AsepriteImageCel<SystemColor>>(doc.Frames[0].Cels[0]);
            Assert.Equal(expected, cel.Pixels);
        }

        [Fact]
        public void AsepriteFileReader_GrayscaleModeRGBA_PixelsTest()
        {
            string path = GetPath("grayscale-pixel-test.aseprite");
            AsepriteFile<SystemColor> doc = AsepriteFileLoader.FromFile<SystemColor>(path);

            Assert.Equal(8, doc.Palette.Count);

            SystemColor tran = new SystemColor(0, 0, 0, 0);
            SystemColor pal0 = doc.Palette[0];
            SystemColor pal1 = doc.Palette[1];
            SystemColor pal2 = doc.Palette[2];
            SystemColor pal3 = doc.Palette[3];
            SystemColor pal4 = doc.Palette[4];
            SystemColor pal5 = doc.Palette[5];
            SystemColor pal6 = doc.Palette[6];
            SystemColor pal7 = doc.Palette[7];

            SystemColor[] expected = new SystemColor[]
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

            AsepriteImageCel<SystemColor> cel = Assert.IsType<AsepriteImageCel<SystemColor>>(doc.Frames[0].Cels[0]);
            Assert.Equal(expected, cel.Pixels);
        }

        [Fact]
        public void AsepriteFileReader_TilemapTest()
        {
            string path = GetPath("tilemap-test.aseprite");
            AsepriteFile<SystemColor> doc = AsepriteFileLoader.FromFile<SystemColor>(path);

            SystemColor tran = new SystemColor(0, 0, 0, 0);
            SystemColor pal0 = doc.Palette[0];
            SystemColor pal1 = doc.Palette[1];
            SystemColor pal2 = doc.Palette[2];
            SystemColor pal3 = doc.Palette[3];
            SystemColor pal4 = doc.Palette[4];
            SystemColor pal5 = doc.Palette[5];
            SystemColor pal6 = doc.Palette[6];
            SystemColor pal7 = doc.Palette[7];
            SystemColor pal8 = doc.Palette[8];
            SystemColor pal9 = doc.Palette[9];

            Assert.Equal(1, doc.Tilesets.Length);
            AsepriteTileset<SystemColor> tileset = doc.Tilesets[0];
            Assert.Equal("test-tileset", tileset.Name);

            Assert.Equal(0, tileset.ID);
            Assert.Equal(11, tileset.TileCount);
            Assert.Equal(8, tileset.Size.Width);
            Assert.Equal(8, tileset.Size.Height);

            SystemColor[] expectedTilesetPixels = new SystemColor[]
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


            AsepriteTilemapLayer<SystemColor> tilesLayer = Assert.IsType<AsepriteTilemapLayer<SystemColor>>(doc.Layers[1]);
            Assert.Equal(tileset, tilesLayer.Tileset);

            AsepriteTilemapCel<SystemColor> tilesCel = Assert.IsType<AsepriteTilemapCel<SystemColor>>(doc.Frames[0].Cels[1]);
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
            AsepriteFile<SystemColor> doc = AsepriteFileLoader.FromFile<SystemColor>(path);
            Assert.Equal("Test Sprite UserData", doc.UserData.Text);
            Assert.Equal(new SystemColor(1, 2, 3, 4), doc.UserData.Color);
        }
    }
}
