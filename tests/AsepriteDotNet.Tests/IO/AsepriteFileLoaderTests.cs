// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
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
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            //  Expected palette colors
            Rgba32 pal0 = new Rgba32(223, 7, 114, 255);
            Rgba32 pal1 = new Rgba32(254, 84, 111, 255);
            Rgba32 pal2 = new Rgba32(255, 158, 125, 255);
            Rgba32 pal3 = new Rgba32(255, 208, 128, 255);
            Rgba32 pal4 = new Rgba32(255, 253, 255, 255);
            Rgba32 pal5 = new Rgba32(11, 255, 230, 255);
            Rgba32 pal6 = new Rgba32(1, 203, 207, 255);
            Rgba32 pal7 = new Rgba32(1, 136, 165, 255);
            Rgba32 pal8 = new Rgba32(62, 50, 100, 255);
            Rgba32 pal9 = new Rgba32(53, 42, 85, 255);

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
            Assert.IsType<AsepriteImageLayer>(doc.Layers[0]);
            Assert.True(doc.Layers[0].IsBackgroundLayer);
            Assert.Equal("background", doc.Layers[0].Name);
            Assert.True(doc.Layers[0].IsVisible);
            Assert.Equal("hidden", doc.Layers[1].Name);
            Assert.False(doc.Layers[1].IsVisible);
            Assert.Equal("user-data", doc.Layers[2].Name);
            Assert.Equal("user-data text", doc.Layers[2].UserData.Text);
            Assert.Equal(new Rgba32(223, 7, 114, 255), doc.Layers[2].UserData.Color);
            Assert.Equal("reference", doc.Layers[3].Name);
            Assert.True(doc.Layers[3].IsReferenceLayer);
            Assert.Equal("75-opacity", doc.Layers[4].Name);
            Assert.Equal(75, doc.Layers[4].Opacity);
            Assert.Equal("blendmode-difference", doc.Layers[5].Name);
            Assert.Equal(AsepriteBlendMode.Difference, doc.Layers[5].BlendMode);
            Assert.Equal("tilemap", doc.Layers[6].Name);
            Assert.Equal(0, Assert.IsType<AsepriteTilemapLayer>(doc.Layers[6]).Tileset.ID);
            Assert.Equal(2, Assert.IsType<AsepriteGroupLayer>(doc.Layers[7]).Children.Length);
            Assert.Equal(1, doc.Layers[8].ChildLevel);
            Assert.Equal(1, doc.Layers[9].ChildLevel);

            //  Validate Tags
            Assert.Equal(4, doc.Tags.Length);
            Assert.Equal("tag0to2forward", doc.Tags[0].Name);
            Assert.Equal(0, doc.Tags[0].From);
            Assert.Equal(2, doc.Tags[0].To);
            Assert.Equal("tag-1-user-data", doc.Tags[0].UserData.Text);
            Assert.Equal(new Rgba32(0, 0, 0, 255), doc.Tags[0].Color);
            Assert.Equal(AsepriteLoopDirection.Forward, doc.Tags[0].LoopDirection);
            Assert.Equal("tag3pingpong", doc.Tags[1].Name);
            Assert.Equal(AsepriteLoopDirection.PingPong, doc.Tags[1].LoopDirection);
            Assert.Equal("tag4userdata", doc.Tags[2].Name);
            Assert.Equal(new Rgba32(11, 255, 230, 255), doc.Tags[2].Color);
            Assert.Equal(new Rgba32(11, 255, 230, 255), doc.Tags[2].UserData.Color);
            Assert.Equal("tag-4-user-data", doc.Tags[2].UserData.Text);
            Assert.False(doc.Tags[3].UserData.HasText);

            //  Validate Frames
            Assert.Equal(7, doc.Frames.Length);
            Assert.Equal(TimeSpan.FromMilliseconds(100), doc.Frames[0].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(200), doc.Frames[1].Duration);
            Assert.Equal(TimeSpan.FromMilliseconds(123), doc.Frames[2].Duration);
            Assert.Equal(2, doc.Frames[0].Cels.Length);  //  Background and Reference Layer cels

            //  Validate Cels
            AsepriteImageCel fgCel = Assert.IsType<AsepriteImageCel>(doc.Frames[2].Cels[1]);
            Assert.Equal("foreground", fgCel.Layer.Name);
            Assert.Equal(16, fgCel.Size.Width);
            Assert.Equal(16, fgCel.Size.Height);
            Assert.Equal(8, fgCel.Location.X);
            Assert.Equal(8, fgCel.Location.Y);
            Assert.Equal(fgCel.Size.Width * fgCel.Size.Height, fgCel.Pixels.Length);

            //  Validate Slices
            Assert.Equal(1, doc.Slices.Length);
            AsepriteSlice slice = doc.Slices[0];
            Assert.Equal("test-slice", slice.Name);
            Assert.Equal(1, slice.Keys.Length);
            Assert.True(slice.IsNinePatch);
            Assert.True(slice.HasPivot);
            AsepriteSliceKey sliceKey = slice.Keys[0];
            Assert.Equal(new Rectangle(0, 0, 32, 32), sliceKey.Bounds);
            Assert.Equal(new Rectangle(1, 2, 3, 4), sliceKey.CenterBounds);
            Assert.Equal(new Point(5, 6), sliceKey.Pivot);

        }

        [Fact]
        public void AsepriteFileReader_ColorModeRGBA_PixelsTest()
        {
            string path = GetPath("rgba-pixel-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Assert.Equal(10, doc.Palette.Count);

            Rgba32 tran = new Rgba32(0, 0, 0, 0);
            Rgba32 pal0 = doc.Palette[0];
            Rgba32 pal1 = doc.Palette[1];
            Rgba32 pal2 = doc.Palette[2];
            Rgba32 pal3 = doc.Palette[3];
            Rgba32 pal4 = doc.Palette[4];
            Rgba32 pal5 = doc.Palette[5];
            Rgba32 pal6 = doc.Palette[6];
            Rgba32 pal7 = doc.Palette[7];
            Rgba32 pal8 = doc.Palette[8];
            Rgba32 pal9 = doc.Palette[9];

            Rgba32[] expected = new Rgba32[]
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

            AsepriteImageCel cel = Assert.IsType<AsepriteImageCel>(doc.Frames[0].Cels[0]);
            Assert.Equal(expected, cel.Pixels);
        }

        [Fact]
        public void AsepriteFileReader_ColorModeIndexed_PixelsTest()
        {
            string path = GetPath("indexed-pixel-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Assert.Equal(11, doc.Palette.Count);

            Rgba32 pal0 = new Rgba32(0, 0, 0, 0);
            Rgba32 pal1 = doc.Palette[1];
            Rgba32 pal2 = doc.Palette[2];
            Rgba32 pal3 = doc.Palette[3];
            Rgba32 pal4 = doc.Palette[4];
            Rgba32 pal5 = doc.Palette[5];
            Rgba32 pal6 = doc.Palette[6];
            Rgba32 pal7 = doc.Palette[7];
            Rgba32 pal8 = doc.Palette[8];
            Rgba32 pal9 = doc.Palette[9];
            Rgba32 pal10 = doc.Palette[10];

            Rgba32[] expected = new Rgba32[]
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

            AsepriteImageCel cel = Assert.IsType<AsepriteImageCel>(doc.Frames[0].Cels[0]);
            Assert.Equal(expected, cel.Pixels);
        }

        [Fact]
        public void AsepriteFileReader_GrayscaleModeRGBA_PixelsTest()
        {
            string path = GetPath("grayscale-pixel-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Assert.Equal(8, doc.Palette.Count);

            Rgba32 tran = new Rgba32(0, 0, 0, 0);
            Rgba32 pal0 = doc.Palette[0];
            Rgba32 pal1 = doc.Palette[1];
            Rgba32 pal2 = doc.Palette[2];
            Rgba32 pal3 = doc.Palette[3];
            Rgba32 pal4 = doc.Palette[4];
            Rgba32 pal5 = doc.Palette[5];
            Rgba32 pal6 = doc.Palette[6];
            Rgba32 pal7 = doc.Palette[7];

            Rgba32[] expected = new Rgba32[]
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

            AsepriteImageCel cel = Assert.IsType<AsepriteImageCel>(doc.Frames[0].Cels[0]);
            Assert.Equal(expected, cel.Pixels);
        }

        [Fact]
        public void AsepriteFileReader_TilemapTest()
        {
            string path = GetPath("tilemap-test.aseprite");
            AsepriteFile doc = AsepriteFileLoader.FromFile(path);

            Rgba32 tran = new Rgba32(0, 0, 0, 0);
            Rgba32 pal0 = doc.Palette[0];
            Rgba32 pal1 = doc.Palette[1];
            Rgba32 pal2 = doc.Palette[2];
            Rgba32 pal3 = doc.Palette[3];
            Rgba32 pal4 = doc.Palette[4];
            Rgba32 pal5 = doc.Palette[5];
            Rgba32 pal6 = doc.Palette[6];
            Rgba32 pal7 = doc.Palette[7];
            Rgba32 pal8 = doc.Palette[8];
            Rgba32 pal9 = doc.Palette[9];

            Assert.Equal(1, doc.Tilesets.Length);
            AsepriteTileset tileset = doc.Tilesets[0];
            Assert.Equal("test-tileset", tileset.Name);

            Assert.Equal(0, tileset.ID);
            Assert.Equal(11, tileset.TileCount);
            Assert.Equal(8, tileset.TileSize.Width);
            Assert.Equal(8, tileset.TileSize.Height);

            Rgba32[] expectedTilesetPixels = new Rgba32[]
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


            AsepriteTilemapLayer tilesLayer = Assert.IsType<AsepriteTilemapLayer>(doc.Layers[1]);
            Assert.Equal(tileset, tilesLayer.Tileset);

            AsepriteTilemapCel tilesCel = Assert.IsType<AsepriteTilemapCel>(doc.Frames[0].Cels[1]);
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
            Assert.Equal(new Rgba32(1, 2, 3, 4), doc.UserData.Color);
        }

        [Fact]
        public void AsepriteFileReader_ReadTagsTest()
        {
            string path = GetPath("read-test.aseprite");
            AsepriteTag[] tags = AsepriteFileLoader.ReadTags(path);

            Assert.Equal(4, tags.Length);
            Assert.Equal("tag0to2forward", tags[0].Name);
            Assert.Equal(0, tags[0].From);
            Assert.Equal(2, tags[0].To);
            Assert.Equal("tag-1-user-data", tags[0].UserData.Text);
            Assert.Equal(new Rgba32(0, 0, 0, 255), tags[0].Color);
            Assert.Equal(AsepriteLoopDirection.Forward, tags[0].LoopDirection);
            Assert.Equal("tag3pingpong", tags[1].Name);
            Assert.Equal(AsepriteLoopDirection.PingPong, tags[1].LoopDirection);
            Assert.Equal("tag4userdata", tags[2].Name);
            Assert.Equal(new Rgba32(11, 255, 230, 255), tags[2].Color);
            Assert.Equal(new Rgba32(11, 255, 230, 255), tags[2].UserData.Color);
            Assert.Equal("tag-4-user-data", tags[2].UserData.Text);
            Assert.False(tags[3].UserData.HasText);
        }

        //  There was an issue where slice data was read incorrectly.  This test was put in place to ensure that
        //  doesn't happen again....
        [Fact]
        public void AsepriteFileReader_SliceTest()
        {
            AsepriteSliceKeyProperties sliceKeyProperties = new AsepriteSliceKeyProperties()
            {
                FrameNumber = 0,
                X = 2,
                Y = 2,
                Width = 28,
                Height = 27
            };

            AsepriteSliceKey expected = new AsepriteSliceKey(sliceKeyProperties, null, null);

            string path = GetPath("slice-test.aseprite");
            AsepriteFile aseFile = AsepriteFileLoader.FromFile(path);

            AsepriteSliceKey actual = aseFile.Slices[0].Keys[0];

            Assert.Equal(expected.Bounds.X, actual.Bounds.X);
            Assert.Equal(expected.Bounds.Y, actual.Bounds.Y);
            Assert.Equal(expected.Bounds.Width, actual.Bounds.Width);
            Assert.Equal(expected.Bounds.Height, actual.Bounds.Height);
        }
    }
}
