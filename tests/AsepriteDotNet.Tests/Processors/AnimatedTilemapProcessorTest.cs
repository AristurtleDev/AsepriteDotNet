// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class AnimatedTilemapProcessorTestFixture
{
    public AsepriteFile<Rgba32> AsepriteFile { get; }
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

        AsepritePalette<Rgba32> palette = new AsepritePalette<Rgba32>(0);
        AsepriteTag<Rgba32>[] tags = Array.Empty<AsepriteTag<Rgba32>>();
        AsepriteSlice<Rgba32>[] slices = Array.Empty<AsepriteSlice<Rgba32>>();
        AsepriteUserData<Rgba32> userData = new();

        AsepriteTilesetProperties tileset0Properties = new AsepriteTilesetProperties() { Id = 0, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 };
        AsepriteTilesetProperties tileset1Properties = new AsepriteTilesetProperties() { Id = 1, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 };
        AsepriteTileset<Rgba32>[] tilesets = new AsepriteTileset<Rgba32>[]
        {
            new AsepriteTileset<Rgba32>(tileset0Properties, "tileset-0", new Rgba32[] {Transparent, Red, Green, Blue }),
            new AsepriteTileset<Rgba32>(tileset1Properties, "tileset-1", new Rgba32[] {Transparent, White, Gray, Black}),
        };

        AsepriteLayerProperties layer1 = new AsepriteLayerProperties() { Flags = 1, BlendMode = 0 };
        AsepriteLayerProperties layer2 = new AsepriteLayerProperties() { Flags = 0, BlendMode = 0 };

        AsepriteLayer<Rgba32>[] layers = new AsepriteLayer<Rgba32>[]
        {
            new AsepriteTilemapLayer<Rgba32>(layer1, "visible", tilesets[0]),
            new AsepriteTilemapLayer<Rgba32>(layer2, "hidden", tilesets[1]),
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

        AsepriteCel<Rgba32>[] frame0Cels = new AsepriteCel<Rgba32>[]
        {
            new AsepriteTilemapCel<Rgba32>(celProperties1, layers[0], tilemapCelProperties, frame0Cel1Tiles),
            new AsepriteTilemapCel<Rgba32>(celProperties2, layers[01], tilemapCelProperties, frame0Cel1Tiles)
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

        AsepriteCel<Rgba32>[] frame1Cels = new AsepriteCel<Rgba32>[]
        {
            new AsepriteTilemapCel<Rgba32>(celProperties1, layers[0], tilemapCelProperties, frame1Cel1Tiles),
            new AsepriteTilemapCel<Rgba32>(celProperties2, layers[01], tilemapCelProperties, frame1Cel1Tiles)
        };

        AsepriteFrame<Rgba32>[] frames = new AsepriteFrame<Rgba32>[]
        {
            new AsepriteFrame<Rgba32>($"{fileName}", 2, 2, 100, new List<AsepriteCel<Rgba32>>(frame0Cels)),
            new AsepriteFrame<Rgba32>($"{fileName}", 2, 2, 200, new List<AsepriteCel<Rgba32>>(frame1Cels))
        };

        AsepriteFile = new AsepriteFile<Rgba32>(fileName,
                                                palette,
                                                0,
                                                0,
                                                AsepriteColorDepth.RGBA,
                                                new List<AsepriteFrame<Rgba32>>(frames),
                                                new List<AsepriteLayer<Rgba32>>(layers),
                                                new List<AsepriteTag<Rgba32>>(tags),
                                                new List<AsepriteSlice<Rgba32>>(slices),
                                                new List<AsepriteTileset<Rgba32>>(tilesets),
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
        AnimatedTilemap<Rgba32> tilemap = AnimatedTilemapProcessor.Process(_fixture.AsepriteFile, ProcessorOptions.Default);

        //  Expect the name of the aseprite file to be the name of the tilemap.
        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        //  File has 2 layers, but one is hidden, so expect only 1 layer.
        Assert.Equal(1, tilemap.Tilesets.Length);

        //  Each layer uses a different tileset, but again, one is hidden, so expect only one tileset.
        Assert.Equal(1, tilemap.Tilesets.Length);

        //  Expect that the tileset was processed correctly
        Tileset<Rgba32> tileset = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
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
        AsepriteTilemapLayer<Rgba32> aseLayer0 = (AsepriteTilemapLayer<Rgba32>)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel<Rgba32> aseFrame0Cel = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[0].Cels[0];
        AsepriteTilemapCel<Rgba32> aseFrame1Cel = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[1].Cels[0];

        //  Expect that the single layer of each frame was constructed properly from the aseprite layer and cel data.
        AssertLayer(tilemap.Frames[0].Layers[0], aseLayer0, aseFrame0Cel);
        AssertLayer(tilemap.Frames[1].Layers[0], aseLayer0, aseFrame1Cel);
    }

    [Fact]
    public void Process_OnlyVisibleLayers_False_Processes_All_Layers()
    {
        ProcessorOptions options = ProcessorOptions.Default with { OnlyVisibleLayers = false };
        AnimatedTilemap<Rgba32> tilemap = AnimatedTilemapProcessor.Process<Rgba32>(_fixture.AsepriteFile, options);

        //  Expect the name of the aseprite file to be the name of the tilemap.
        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        //  Since all layers were processed, and there are 2 layers, each with a different tileset, expect 2 tilesets
        Assert.Equal(2, tilemap.Tilesets.Length);

        //  Each layer uses a different tileset, and since both were processed, expect two tilesets.
        Assert.Equal(2, tilemap.Tilesets.Length);

        //  Expect that both of the tilesets were processed correctly
        Tileset<Rgba32> tileset0 = TilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Tileset<Rgba32> tileset1 = TilesetProcessor.Process(_fixture.AsepriteFile, 1);
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
        AsepriteTilemapLayer<Rgba32> aseLayer0 = (AsepriteTilemapLayer<Rgba32>)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapLayer<Rgba32> aseLayer1 = (AsepriteTilemapLayer<Rgba32>)_fixture.AsepriteFile.Layers[1];
        AsepriteTilemapCel<Rgba32> aseFrame0Cel0 = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[0].Cels[0];
        AsepriteTilemapCel<Rgba32> aseFrame0Cel1 = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[0].Cels[1];
        AsepriteTilemapCel<Rgba32> aseFrame1Cel0 = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[1].Cels[0];
        AsepriteTilemapCel<Rgba32> aseFrame1Cel1 = (AsepriteTilemapCel<Rgba32>)_fixture.AsepriteFile.Frames[1].Cels[1];

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
        AsepriteLayer<Rgba32>[] layers = new AsepriteLayer<Rgba32>[]
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


        AsepriteCelProperties celProperties = new AsepriteCelProperties() { LayerIndex = 0, Opacity = 255, Type = 3, X = 0, Y = 0, ZIndex = 0 };
        AsepriteTilemapCelProperties tilemapCelProperties = new AsepriteTilemapCelProperties() { Width = 2, Height = 2 };
        AsepriteCel<Rgba32>[] cels = new AsepriteCel<Rgba32>[]
        {
            new AsepriteTilemapCel<Rgba32>(celProperties, layers[0], tilemapCelProperties, tiles),
            new AsepriteTilemapCel<Rgba32>(celProperties with {LayerIndex = 1 }, layers[1], tilemapCelProperties, tiles),
            new AsepriteTilemapCel<Rgba32>(celProperties with {LayerIndex = 2 }, layers[0], tilemapCelProperties, tiles),
        };

        AsepriteFrame<Rgba32>[] frames = new AsepriteFrame<Rgba32>[]
        {
            new($"{_fixture.AsepriteFile.Name} 0", 2, 2, 100, new List<AsepriteCel<Rgba32>>(cels))
        };

        //  Reuse the fixture, but use the layers array from above with duplicate layer names

        AsepriteFile<Rgba32> aseFile = new AsepriteFile<Rgba32>(_fixture.AsepriteFile.Name,
                                                                _fixture.AsepriteFile.Palette,
                                                                _fixture.AsepriteFile.CanvasWidth,
                                                                _fixture.AsepriteFile.CanvasHeight,
                                                                AsepriteColorDepth.RGBA,
                                                                new List<AsepriteFrame<Rgba32>>(frames),
                                                                new List<AsepriteLayer<Rgba32>>(layers),
                                                                new List<AsepriteTag<Rgba32>>(_fixture.AsepriteFile.Tags.ToArray()),
                                                                new List<AsepriteSlice<Rgba32>>(_fixture.AsepriteFile.Slices.ToArray()),
                                                                new List<AsepriteTileset<Rgba32>>(_fixture.AsepriteFile.Tilesets.ToArray()),
                                                                _fixture.AsepriteFile.UserData,
                                                                new List<string>());

        Assert.Throws<InvalidOperationException>(() => AnimatedTilemapProcessor.Process(aseFile));
    }

    private static void AssertLayer(TilemapLayer tilemapLayer, AsepriteTilemapLayer<Rgba32> aseLayer, AsepriteTilemapCel<Rgba32> aseCel)
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
