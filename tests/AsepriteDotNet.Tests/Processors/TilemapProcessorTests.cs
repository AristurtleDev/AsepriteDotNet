// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class TilemapProcessorTestFixture
{
    public AsepriteFile AsepriteFile { get; }
    public Rgba32 Red { get; } = new Rgba32(255, 0, 0, 255);
    public Rgba32 Green { get; } = new Rgba32(0, 255, 0, 255);
    public Rgba32 Blue { get; } = new Rgba32(0, 0, 255, 255);
    public Rgba32 White { get; } = new Rgba32(255, 255, 255, 255);
    public Rgba32 Gray { get; } = new Rgba32(128, 128, 128, 255);
    public Rgba32 Black { get; } = new Rgba32(0, 0, 0, 255);
    public Rgba32 Transparent { get; } = new Rgba32(0, 0, 0, 0);

    public TilemapProcessorTestFixture()
    {
        string fileName = "tilemap-processor-tests";

        List<AsepriteTileset> tilesets = new List<AsepriteTileset>()
        {
            new AsepriteTileset(new AsepriteTilesetProperties() {Id = 0, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 }, "tileset-0", new Rgba32[] { Transparent, Red, Green, Blue }),
            new AsepriteTileset(new AsepriteTilesetProperties() {Id = 1, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 }, "tileset-0", new Rgba32[] { Transparent, White, Gray, Black })
        };

        List<AsepriteLayer> layers = new List<AsepriteLayer>()
        {
            new AsepriteTilemapLayer(new AsepriteLayerProperties() {Flags = 1, BlendMode = 0, Opacity = 255 }, "visible", tilesets[0]),
            new AsepriteTilemapLayer(new AsepriteLayerProperties() {Flags = 0, BlendMode = 0, Opacity = 255 }, "hidden", tilesets[1])
        };

        List<AsepriteTile> cel0Tiles = new List<AsepriteTile>()
        {
            new AsepriteTile(0, false, false, false),
            new AsepriteTile(1, false, false, false),
            new AsepriteTile(2, false, false, false),
            new AsepriteTile(3, false, false, false)
        };

        List<AsepriteTile> cel1Tiles = new List<AsepriteTile>()
        {
            new AsepriteTile(2, false, false, false),
            new AsepriteTile(3, false, false, false)
        };

        AsepriteCelProperties celProperties = new AsepriteCelProperties() { Opacity = 255 };
        AsepriteTilemapCelProperties tilemapCelProperties = new AsepriteTilemapCelProperties() { Width = 2, Height = 2 };
        List<AsepriteCel> cels = new List<AsepriteCel>()
        {
            new AsepriteTilemapCel(celProperties, layers[0], tilemapCelProperties, cel0Tiles.ToArray()),
            new AsepriteTilemapCel(celProperties, layers[1], tilemapCelProperties, cel1Tiles.ToArray())
        };

        List<AsepriteFrame> frames = new List<AsepriteFrame>()
        {
            new($"{fileName} 0", 2, 2, 100, cels)
        };

        AsepriteFile = new AsepriteFile(fileName, new AsepritePalette(0), 0, 0, AsepriteColorDepth.RGBA, frames, layers, [], [], tilesets, new AsepriteUserData(), []);
    }
}

public sealed class TilemapProcessorTests : IClassFixture<TilemapProcessorTestFixture>
{
    private readonly TilemapProcessorTestFixture _fixture;

    public TilemapProcessorTests(TilemapProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Process_OnlyVisibleLayers_True_Processes_Only_Visible_Layers()
    {
        ProcessorOptions options = ProcessorOptions.Default with { OnlyVisibleLayers = true };
        Tilemap tilemap = TilemapProcessor.Process(_fixture.AsepriteFile, 0, options);

        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        Assert.Equal(1, tilemap.Tilesets.Length);
        Tileset tileset = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Assert.Equal(tileset, tilemap.Tilesets[0]);

        Assert.Equal(1, tilemap.Layers.Length);
        AsepriteTilemapLayer aseLayer = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel aseCel = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[0];
        AssertLayer(tilemap.Layers[0], aseLayer.Name, aseLayer.Tileset.ID, aseCel.Size.Width, aseCel.Size.Height, aseCel.Location, aseCel.Tiles);
    }

    [Fact]
    public void Process_OnlyVisibleLayers_False_Processes_All_Layers()
    {
        ProcessorOptions options = ProcessorOptions.Default with { OnlyVisibleLayers = false };
        Tilemap tilemap = TilemapProcessor.Process(_fixture.AsepriteFile, 0, options);

        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        Assert.Equal(2, tilemap.Tilesets.Length);
        Tileset tileset0 = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Tileset tileset1 = TilesetProcessor.Process(_fixture.AsepriteFile, 1);
        Assert.Equal(tileset0, tilemap.Tilesets[0]);
        Assert.Equal(tileset1, tilemap.Tilesets[1]);

        Assert.Equal(2, tilemap.Layers.Length);

        AsepriteTilemapLayer aseLayer0 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel aseCel0 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[0];
        AssertLayer(tilemap.Layers[0], aseLayer0.Name, aseLayer0.Tileset.ID, aseCel0.Size.Width, aseCel0.Size.Height, aseCel0.Location, aseCel0.Tiles);

        AsepriteTilemapLayer aseLayer1 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[1];
        AsepriteTilemapCel aseCel1 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[1];
        AssertLayer(tilemap.Layers[1], aseLayer1.Name, aseLayer1.Tileset.ID, aseCel1.Size.Width, aseCel1.Size.Height, aseCel1.Location, aseCel1.Tiles);
    }

    [Fact]
    public void Process_Index_Out_Of_Range_Throws_Exception()
    {
        Assert.Throws<IndexOutOfRangeException>(() => TilemapProcessor.Process(_fixture.AsepriteFile, -1));
        Assert.Throws<IndexOutOfRangeException>(() => TilemapProcessor.Process(_fixture.AsepriteFile, _fixture.AsepriteFile.Frames.Length));
        Assert.Throws<IndexOutOfRangeException>(() => TilemapProcessor.Process(_fixture.AsepriteFile, _fixture.AsepriteFile.Frames.Length + 1));
    }

    [Fact]
    public void Process_Duplicate_AsepriteLayer_Names_Throws_Exception()
    {

        AsepriteLayerProperties layerProperties = new AsepriteLayerProperties() { Flags = 1, BlendMode = 0, Opacity = 255 };
        List<AsepriteLayer> layers = new List<AsepriteLayer>()
        {
            new AsepriteTilemapLayer(layerProperties, "layer-0", _fixture.AsepriteFile.Tilesets[0]),
            new AsepriteTilemapLayer(layerProperties, "layer-1", _fixture.AsepriteFile.Tilesets[0]),
            new AsepriteTilemapLayer(layerProperties, "layer-0", _fixture.AsepriteFile.Tilesets[0])
        };

        AsepriteTile[] tiles = new AsepriteTile[]
        {
            new AsepriteTile(0, false, false, false),
            new AsepriteTile(1, false, false, false),
            new AsepriteTile(2, false, false, false),
            new AsepriteTile(3, false, false, false)
        };

        AsepriteCelProperties celProperties = new AsepriteCelProperties() { Opacity = 255 };
        AsepriteTilemapCelProperties tilemapCelProperties = new AsepriteTilemapCelProperties() { Width = 2, Height = 2 };
        List<AsepriteCel> cels = new List<AsepriteCel>()
        {
            new AsepriteTilemapCel(celProperties, layers[0], tilemapCelProperties, tiles),
            new AsepriteTilemapCel(celProperties, layers[1], tilemapCelProperties, tiles),
            new AsepriteTilemapCel(celProperties, layers[2], tilemapCelProperties, tiles)
        };

        List<AsepriteFrame> frames = new List<AsepriteFrame>()
        {
            new($"{_fixture.AsepriteFile.Name} 0", 2, 2, 100, cels)
        };

        //  Reuse the fixture, but use the layers array from above with duplicate layer names
        AsepriteFile aseFile = new AsepriteFile(_fixture.AsepriteFile.Name,
                                                _fixture.AsepriteFile.Palette,
                                                _fixture.AsepriteFile.CanvasWidth,
                                                _fixture.AsepriteFile.CanvasHeight,
                                                AsepriteColorDepth.RGBA,
                                                frames,
                                                layers,
                                                new List<AsepriteTag>(_fixture.AsepriteFile.Tags.ToArray()),
                                                new List<AsepriteSlice>(_fixture.AsepriteFile.Slices.ToArray()),
                                                new List<AsepriteTileset>(_fixture.AsepriteFile.Tilesets.ToArray()),
                                                _fixture.AsepriteFile.UserData,
                                                []);

        Assert.Throws<InvalidOperationException>(() => TilemapProcessor.Process(aseFile, 0));
    }

    private void AssertLayer(TilemapLayer layer, string name, int tilesetID, int columns, int rows, Point offset, ReadOnlySpan<AsepriteTile> tiles)
    {
        Assert.Equal(name, layer.Name);
        Assert.Equal(tilesetID, layer.TilesetID);
        Assert.Equal(columns, layer.Columns);
        Assert.Equal(rows, layer.Rows);
        Assert.Equal(offset, layer.Offset);

        for (int i = 0; i < tiles.Length; i++)
        {
            Assert.Equal(tiles[i].ID, layer.Tiles[i].TilesetTileID);
            Assert.Equal(tiles[i].FlipHorizontally, layer.Tiles[i].FlipHorizontally);
            Assert.Equal(tiles[i].FlipVertically, layer.Tiles[i].FlipVertically);
            Assert.Equal(tiles[i].FlipDiagonally, layer.Tiles[i].FlipDiagonally);
        }
    }
}
