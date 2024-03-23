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
    public AsepriteFile<Rgba32> AsepriteFile { get; }
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

        List<AsepriteTileset<Rgba32>> tilesets = new List<AsepriteTileset<Rgba32>>()
        {
            new AsepriteTileset<Rgba32>(new AsepriteTilesetProperties() {Id = 0, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 }, "tileset-0", new Rgba32[] { Transparent, Red, Green, Blue }),
            new AsepriteTileset<Rgba32>(new AsepriteTilesetProperties() {Id = 1, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 }, "tileset-0", new Rgba32[] { Transparent, White, Gray, Black })
        };

        List<AsepriteLayer<Rgba32>> layers = new List<AsepriteLayer<Rgba32>>()
        {
            new AsepriteTilemapLayer<Rgba32>(new AsepriteLayerProperties() {Flags = 1, BlendMode = 0, Opacity = 255 }, "visible", tilesets[0]),
            new AsepriteTilemapLayer<Rgba32>(new AsepriteLayerProperties() {Flags = 0, BlendMode = 0, Opacity = 255 }, "hidden", tilesets[1])
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
        List<AsepriteCel<Rgba32>> cels = new List<AsepriteCel<Rgba32>>()
        {
            new AsepriteTilemapCel<Rgba32>(celProperties, layers[0], tilemapCelProperties, cel0Tiles.ToArray()),
            new AsepriteTilemapCel<Rgba32>(celProperties, layers[1], tilemapCelProperties, cel1Tiles.ToArray())
        };

        List<AsepriteFrame<Rgba32>> frames = new List<AsepriteFrame<Rgba32>>()
        {
            new($"{fileName} 0", 2, 2, 100, cels)
        };

        AsepriteFile = new AsepriteFile<Rgba32>(fileName, new AsepritePalette<Rgba32>(0), 0, 0, AsepriteColorDepth.RGBA, frames, layers, [], [], tilesets, new AsepriteUserData<Rgba32>(), []);
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
        Tilemap<Rgba32> tilemap = TilemapProcessor.Process(_fixture.AsepriteFile, 0, options);

        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        Assert.Equal(1, tilemap.Tilesets.Length);
        Tileset<Rgba32> tileset = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Assert.Equal(tileset, tilemap.Tilesets[0]);

        Assert.Equal(1, tilemap.Layers.Length);
        AsepriteTilemapLayer<Rgba32> aseLayer = (AsepriteTilemapLayer<Rgba32>)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel<Rgba32> aseCel = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[0].Cels[0];
        AssertLayer(tilemap.Layers[0], aseLayer.Name, aseLayer.Tileset.ID, aseCel.Size.Width, aseCel.Size.Height, aseCel.Location, aseCel.Tiles);
    }

    [Fact]
    public void Process_OnlyVisibleLayers_False_Processes_All_Layers()
    {
        ProcessorOptions options = ProcessorOptions.Default with { OnlyVisibleLayers = false };
        Tilemap<Rgba32> tilemap = TilemapProcessor.Process(_fixture.AsepriteFile, 0, options);

        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        Assert.Equal(2, tilemap.Tilesets.Length);
        Tileset<Rgba32> tileset0 = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Tileset<Rgba32> tileset1 = TilesetProcessor.Process(_fixture.AsepriteFile, 1);
        Assert.Equal(tileset0, tilemap.Tilesets[0]);
        Assert.Equal(tileset1, tilemap.Tilesets[1]);

        Assert.Equal(2, tilemap.Layers.Length);

        AsepriteTilemapLayer<Rgba32> aseLayer0 = (AsepriteTilemapLayer<Rgba32>)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel<Rgba32> aseCel0 = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[0].Cels[0];
        AssertLayer(tilemap.Layers[0], aseLayer0.Name, aseLayer0.Tileset.ID, aseCel0.Size.Width, aseCel0.Size.Height, aseCel0.Location, aseCel0.Tiles);

        AsepriteTilemapLayer<Rgba32> aseLayer1 = (AsepriteTilemapLayer<Rgba32>)_fixture.AsepriteFile.Layers[1];
        AsepriteTilemapCel<Rgba32> aseCel1 = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[0].Cels[1];
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
        List<AsepriteLayer<Rgba32>> layers = new List<AsepriteLayer<Rgba32>>()
        {
            new AsepriteTilemapLayer<Rgba32>(layerProperties, "layer-0", _fixture.AsepriteFile.Tilesets[0]),
            new AsepriteTilemapLayer<Rgba32>(layerProperties, "layer-1", _fixture.AsepriteFile.Tilesets[0]),
            new AsepriteTilemapLayer<Rgba32>(layerProperties, "layer-0", _fixture.AsepriteFile.Tilesets[0])
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
        List<AsepriteCel<Rgba32>> cels = new List<AsepriteCel<Rgba32>>()
        {
            new AsepriteTilemapCel<Rgba32>(celProperties, layers[0], tilemapCelProperties, tiles),
            new AsepriteTilemapCel<Rgba32>(celProperties, layers[1], tilemapCelProperties, tiles),
            new AsepriteTilemapCel<Rgba32>(celProperties, layers[2], tilemapCelProperties, tiles)
        };

        List<AsepriteFrame<Rgba32>> frames = new List<AsepriteFrame<Rgba32>>()
        {
            new($"{_fixture.AsepriteFile.Name} 0", 2, 2, 100, cels)
        };

        //  Reuse the fixture, but use the layers array from above with duplicate layer names
        AsepriteFile<Rgba32> aseFile = new AsepriteFile<Rgba32>(_fixture.AsepriteFile.Name,
                                                                _fixture.AsepriteFile.Palette,
                                                                _fixture.AsepriteFile.CanvasWidth,
                                                                _fixture.AsepriteFile.CanvasHeight,
                                                                AsepriteColorDepth.RGBA,
                                                                frames,
                                                                layers,
                                                                new List<AsepriteTag<Rgba32>>(_fixture.AsepriteFile.Tags.ToArray()),
                                                                new List<AsepriteSlice<Rgba32>>(_fixture.AsepriteFile.Slices.ToArray()),
                                                                new List<AsepriteTileset<Rgba32>>(_fixture.AsepriteFile.Tilesets.ToArray()),
                                                                _fixture.AsepriteFile.UserData,
                                                                []);

        Assert.Throws<InvalidOperationException>(() => TilemapProcessor.Process(aseFile, 0));
    }

    private static void AssertLayer(TilemapLayer layer, string name, int tilesetID, int columns, int rows, Point offset, ReadOnlySpan<AsepriteTile> tiles)
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
