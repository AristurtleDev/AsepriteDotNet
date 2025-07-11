// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Core;
using AsepriteDotNet.Core.Document;
using AsepriteDotNet.Core.Types;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class AnimatedTilemapProcessorTestFixture
{
    public AsepriteFile AsepriteFile { get; }
    public Rgba32 Red { get; } = new Rgba32(255, 0, 0, 255);
    public Rgba32 Green { get; } = new Rgba32(0, 255, 0, 255);
    public Rgba32 Blue { get; } = new Rgba32(0, 0, 255, 255);
    public Rgba32 White { get; } = new Rgba32(255, 255, 255, 255);
    public Rgba32 Gray { get; } = new Rgba32(128, 128, 128, 255);
    public Rgba32 Black { get; } = new Rgba32(0, 0, 0, 255);
    public Rgba32 Transparent { get; } = new Rgba32(0, 0, 0, 0);


    public AnimatedTilemapProcessorTestFixture()
    {
        string fileName = "animated-tilemap-processor-tests";

        AsepritePalette palette = new AsepritePalette(0);
        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();
        AsepriteUserData userData = new();

        AsepriteTilesetProperties tileset0Properties = new AsepriteTilesetProperties() { Id = 0, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 };
        AsepriteTilesetProperties tileset1Properties = new AsepriteTilesetProperties() { Id = 1, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 };
        AsepriteTileset[] tilesets = new AsepriteTileset[]
        {
            new AsepriteTileset(tileset0Properties, "tileset-0", new Rgba32[] {Transparent, Red, Green, Blue }),
            new AsepriteTileset(tileset1Properties, "tileset-1", new Rgba32[] {Transparent, White, Gray, Black}),
        };

        AsepriteLayerProperties layer1 = new AsepriteLayerProperties() { Flags = 1, BlendMode = 0 };
        AsepriteLayerProperties layer2 = new AsepriteLayerProperties() { Flags = 0, BlendMode = 0 };

        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new AsepriteTilemapLayer(layer1, "visible", tilesets[0]),
            new AsepriteTilemapLayer(layer2, "hidden", tilesets[1]),
        };

        AsepriteTile[] frame0Cel0Tiles = new AsepriteTile[]
        {
            new AsepriteTile(0, false, false, false),
            new AsepriteTile(1, false, false, false),
            new AsepriteTile(2, false, false, false),
            new AsepriteTile(3, false, false, false)
        };

        AsepriteTile[] frame0Cel1Tiles = new AsepriteTile[]
        {
            new AsepriteTile(2, false, false, false),
            new AsepriteTile(3, false, false, false)
        };

        AsepriteCelProperties celProperties1 = new AsepriteCelProperties() { LayerIndex = 0, Opacity = 255, Type = 3, X = 0, Y = 0, ZIndex = 0 };
        AsepriteCelProperties celProperties2 = new AsepriteCelProperties() { LayerIndex = 1, Opacity = 255, Type = 3, X = 0, Y = 1, ZIndex = 0 };
        AsepriteTilemapCelProperties tilemapCelProperties = new AsepriteTilemapCelProperties() { Width = 2, Height = 2 };

        AsepriteCel[] frame0Cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(celProperties1, layers[0], tilemapCelProperties, frame0Cel1Tiles),
            new AsepriteTilemapCel(celProperties2, layers[01], tilemapCelProperties, frame0Cel1Tiles)
        };

        AsepriteTile[] frame1Cel0Tiles = new AsepriteTile[]
        {
            new AsepriteTile(3, false, false, false),
            new AsepriteTile(2, false, false, false),
            new AsepriteTile(1, false, false, false),
            new AsepriteTile(0, false, false, false)
        };

        AsepriteTile[] frame1Cel1Tiles = new AsepriteTile[]
        {
            new AsepriteTile(3, false, false, false),
            new AsepriteTile(2, false, false, false)
        };

        AsepriteCel[] frame1Cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(celProperties1, layers[0], tilemapCelProperties, frame1Cel1Tiles),
            new AsepriteTilemapCel(celProperties2, layers[01], tilemapCelProperties, frame1Cel1Tiles)
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new AsepriteFrame($"{fileName}", 2, 2, 100, new List<AsepriteCel>(frame0Cels)),
            new AsepriteFrame($"{fileName}", 2, 2, 200, new List<AsepriteCel>(frame1Cels))
        };

        AsepriteFile = new AsepriteFile(fileName,
                                        palette,
                                        0,
                                        0,
                                        AsepriteColorDepth.RGBA,
                                        new List<AsepriteFrame>(frames),
                                        new List<AsepriteLayer>(layers),
                                        new List<AsepriteTag>(tags),
                                        new List<AsepriteSlice>(slices),
                                        new List<AsepriteTileset>(tilesets),
                                        userData,
                                        new List<string>());
    }
}

public sealed class AnimatedTilemapProcessorTests : IClassFixture<AnimatedTilemapProcessorTestFixture>
{
    private readonly AnimatedTilemapProcessorTestFixture _fixture;

    public AnimatedTilemapProcessorTests(AnimatedTilemapProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Process_OnlyVisibleLayers_True_Processes_Only_Visible_Layers()
    {
        AnimatedTilemap tilemap = AnimatedTilemapProcessor.Process(_fixture.AsepriteFile, true);

        //  Expect the name of the aseprite file to be the name of the tilemap.
        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        //  File has 2 layers, but one is hidden, so expect only 1 layer.
        Assert.Equal(1, tilemap.Tilesets.Length);

        //  Each layer uses a different tileset, but again, one is hidden, so expect only one tileset.
        Assert.Equal(1, tilemap.Tilesets.Length);

        //  Expect that the tileset was processed correctly
        Tileset tileset = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Assert.Equal(tileset, tilemap.Tilesets[0]);

        //  The file has 2 frames, so expect 2 frames
        Assert.Equal(2, tilemap.Frames.Length);

        //  Expect that the duration of the tilemap frames was taken from the frames in the file
        Assert.Equal(_fixture.AsepriteFile.Frames[0].Duration, tilemap.Frames[0].Duration);
        Assert.Equal(_fixture.AsepriteFile.Frames[1].Duration, tilemap.Frames[1].Duration);

        //  Again, one of the layers is hidden, so expect only one layer processed for each frame.
        Assert.Equal(1, tilemap.Frames[0].Layers.Length);
        Assert.Equal(1, tilemap.Frames[1].Layers.Length);

        // Getting reference to the aseprite layers and cels used to construct the  tilemap layers
        AsepriteTilemapLayer aseLayer0 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel aseFrame0Cel = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[0];
        AsepriteTilemapCel aseFrame1Cel = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[1].Cels[0];

        //  Expect that the single layer of each frame was constructed properly from the aseprite layer and cel data.
        AssertLayer(tilemap.Frames[0].Layers[0], aseLayer0, aseFrame0Cel);
        AssertLayer(tilemap.Frames[1].Layers[0], aseLayer0, aseFrame1Cel);
    }

    [Fact]
    public void Process_OnlyVisibleLayers_False_Processes_All_Layers()
    {
        AnimatedTilemap tilemap = AnimatedTilemapProcessor.Process(_fixture.AsepriteFile, false);

        //  Expect the name of the aseprite file to be the name of the tilemap.
        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        //  Since all layers were processed, and there are 2 layers, each with a different tileset, expect 2 tilesets
        Assert.Equal(2, tilemap.Tilesets.Length);

        //  Each layer uses a different tileset, and since both were processed, expect two tilesets.
        Assert.Equal(2, tilemap.Tilesets.Length);

        //  Expect that both of the tilesets were processed correctly
        Tileset tileset0 = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Tileset tileset1 = TilesetProcessor.Process(_fixture.AsepriteFile, 1);
        Assert.Equal(tileset0, tilemap.Tilesets[0]);
        Assert.Equal(tileset1, tilemap.Tilesets[1]);

        //  The file has 2 frames, so expect 2 frames
        Assert.Equal(2, tilemap.Frames.Length);

        //  Expect that the duration of the tilemap frames was taken from the frames in the file
        Assert.Equal(_fixture.AsepriteFile.Frames[0].Duration, tilemap.Frames[0].Duration);
        Assert.Equal(_fixture.AsepriteFile.Frames[1].Duration, tilemap.Frames[1].Duration);

        //  Since all layers were processed, and there are 2 layers, expect that each frame has 2 layers.
        Assert.Equal(2, tilemap.Frames[0].Layers.Length);
        Assert.Equal(2, tilemap.Frames[1].Layers.Length);

        // Getting reference to the aseprite layers and cels used to construct the  tilemap layers
        AsepriteTilemapLayer aseLayer0 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapLayer aseLayer1 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[1];
        AsepriteTilemapCel aseFrame0Cel0 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[0];
        AsepriteTilemapCel aseFrame0Cel1 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[1];
        AsepriteTilemapCel aseFrame1Cel0 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[1].Cels[0];
        AsepriteTilemapCel aseFrame1Cel1 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[1].Cels[1];

        //  Expect that the layers on each frame were constructed properly from the aseprite layer and cel data
        AssertLayer(tilemap.Frames[0].Layers[0], aseLayer0, aseFrame0Cel0);
        AssertLayer(tilemap.Frames[0].Layers[1], aseLayer1, aseFrame0Cel1);
        AssertLayer(tilemap.Frames[1].Layers[0], aseLayer0, aseFrame1Cel0);
        AssertLayer(tilemap.Frames[1].Layers[1], aseLayer1, aseFrame1Cel1);
    }

    [Fact]
    public void Process_Duplicate_AsepriteLayer_Names_Throws_Exception()
    {
        AsepriteLayerProperties layerProperties = new AsepriteLayerProperties() { BlendMode = 0, Opacity = 255, Flags = 1 };
        AsepriteLayer[] layers = new AsepriteLayer[]
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


        AsepriteCelProperties celProperties = new AsepriteCelProperties() { LayerIndex = 0, Opacity = 255, Type = 3, X = 0, Y = 0, ZIndex = 0 };
        AsepriteTilemapCelProperties tilemapCelProperties = new AsepriteTilemapCelProperties() { Width = 2, Height = 2 };
        AsepriteCel[] cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(celProperties, layers[0], tilemapCelProperties, tiles),
            new AsepriteTilemapCel(celProperties with {LayerIndex = 1 }, layers[1], tilemapCelProperties, tiles),
            new AsepriteTilemapCel(celProperties with {LayerIndex = 2 }, layers[0], tilemapCelProperties, tiles),
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{_fixture.AsepriteFile.Name} 0", 2, 2, 100, new List<AsepriteCel>(cels))
        };

        //  Reuse the fixture, but use the layers array from above with duplicate layer names

        AsepriteFile aseFile = new AsepriteFile(_fixture.AsepriteFile.Name,
                                                _fixture.AsepriteFile.Palette,
                                                _fixture.AsepriteFile.CanvasWidth,
                                                _fixture.AsepriteFile.CanvasHeight,
                                                AsepriteColorDepth.RGBA,
                                                new List<AsepriteFrame>(frames),
                                                new List<AsepriteLayer>(layers),
                                                new List<AsepriteTag>(_fixture.AsepriteFile.Tags.ToArray()),
                                                new List<AsepriteSlice>(_fixture.AsepriteFile.Slices.ToArray()),
                                                new List<AsepriteTileset>(_fixture.AsepriteFile.Tilesets.ToArray()),
                                                _fixture.AsepriteFile.UserData,
                                                new List<string>());

        Assert.Throws<InvalidOperationException>(() => AnimatedTilemapProcessor.Process(aseFile, true));
    }

    private static void AssertLayer(TilemapLayer tilemapLayer, AsepriteTilemapLayer aseLayer, AsepriteTilemapCel aseCel)
    {
        Assert.Equal(aseLayer.Name, tilemapLayer.Name);
        Assert.Equal(aseLayer.Tileset.ID, tilemapLayer.TilesetID);
        Assert.Equal(aseCel.Size.Width, tilemapLayer.Columns);
        Assert.Equal(aseCel.Size.Height, tilemapLayer.Rows);
        Assert.Equal(aseCel.Location, tilemapLayer.Offset);

        for (int i = 0; i < aseCel.Tiles.Length; i++)
        {
            AsepriteTile aseTile = aseCel.Tiles[i];
            TilemapTile tilemapTile = tilemapLayer.Tiles[i];

            Assert.Equal(aseTile.ID, tilemapTile.TilesetTileID);
            Assert.Equal(aseTile.FlipHorizontally, tilemapTile.FlipHorizontally);
            Assert.Equal(aseTile.FlipVertically, tilemapTile.FlipVertically);
            Assert.Equal(aseTile.FlipDiagonally, tilemapTile.FlipDiagonally);
        }
    }
}
