// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class TilesetProcessorTestFixture
{
    public AsepriteFile AsepriteFile { get; }
    public Rgba32 Red { get; } = new Rgba32(255, 0, 0, 255);
    public Rgba32 Green { get; } = new Rgba32(0, 255, 0, 255);
    public Rgba32 Blue { get; } = new Rgba32(0, 0, 255, 255);
    public Rgba32 White { get; } = new Rgba32(255, 255, 255, 255);
    public Rgba32 Gray { get; } = new Rgba32(128, 128, 128, 255);
    public Rgba32 Black { get; } = new Rgba32(0, 0, 0, 255);
    public Rgba32 Transparent { get; } = new Rgba32(0, 0, 0, 0);

    public TilesetProcessorTestFixture()
    {
        AsepritePalette palette = new AsepritePalette(0);
        List<AsepriteTileset> tilesets = new List<AsepriteTileset>()
        {
            new AsepriteTileset(new AsepriteTilesetProperties(){Id = 0, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 }, "tileset-0", new Rgba32[] {Transparent, Red, Green, Blue}),
            new AsepriteTileset(new AsepriteTilesetProperties(){Id = 1, NumberOfTiles = 4, TileWidth = 1, TileHeight = 1 }, "tileset-1", new Rgba32[] {Transparent, White, Gray, Black}),
        };

        AsepriteFile = new("file", palette, 0, 0, AsepriteColorDepth.RGBA, [], [], [], [], tilesets, new AsepriteUserData(), []);
    }
}

public sealed class TilesetProcessorTests : IClassFixture<TilesetProcessorTestFixture>
{
    private readonly TilesetProcessorTestFixture _fixture;

    public TilesetProcessorTests(TilesetProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Process_By_Index()
    {
        int index = 0;
        AsepriteTileset aseTileset = _fixture.AsepriteFile.Tilesets[index];

        Tileset tileset = TilesetProcessor.Process(_fixture.AsepriteFile, index);

        Assert.Equal(aseTileset.ID, tileset.ID);
        Assert.Equal(aseTileset.Name, tileset.Name);
        Assert.Equal(aseTileset.TileSize.Width, tileset.TileSize.Width);
        Assert.Equal(aseTileset.TileSize.Height, tileset.TileSize.Height);
        Assert.Equal(aseTileset.Name, tileset.Texture.Name);
        Assert.Equal(aseTileset.TileSize.Width, tileset.Texture.Size.Width);
        Assert.Equal(aseTileset.TileSize.Height * aseTileset.TileCount, tileset.Texture.Size.Height);
        Assert.Equal(aseTileset.Pixels.ToArray(), tileset.Texture.Pixels.ToArray());
    }

    [Fact]
    public void Process_By_Name()
    {
        AsepriteTileset aseTileset = _fixture.AsepriteFile.Tilesets[0];

        Tileset tileset = TilesetProcessor.Process(_fixture.AsepriteFile, aseTileset.Name);

        Assert.Equal(aseTileset.ID, tileset.ID);
        Assert.Equal(aseTileset.Name, tileset.Name);
        Assert.Equal(aseTileset.TileSize.Width, tileset.TileSize.Width);
        Assert.Equal(aseTileset.TileSize.Height, tileset.TileSize.Height);
        Assert.Equal(aseTileset.Name, tileset.Texture.Name);
        Assert.Equal(aseTileset.TileSize.Width, tileset.Texture.Size.Width);
        Assert.Equal(aseTileset.TileSize.Height * aseTileset.TileCount, tileset.Texture.Size.Height);
        Assert.Equal(aseTileset.Pixels.ToArray(), tileset.Texture.Pixels.ToArray());
    }

    [Fact]
    public void Process_Index_Out_Of_Range_Throws_Exception()
    {
        Assert.Throws<IndexOutOfRangeException>(() => TilesetProcessor.Process(_fixture.AsepriteFile, -1));
        Assert.Throws<IndexOutOfRangeException>(() => TilesetProcessor.Process(_fixture.AsepriteFile, _fixture.AsepriteFile.Tilesets.Length));
        Assert.Throws<IndexOutOfRangeException>(() => TilesetProcessor.Process(_fixture.AsepriteFile, _fixture.AsepriteFile.Tilesets.Length + 1));
    }

    [Fact]
    public void Process_Bad_Name_Throws_Exception()
    {
        Assert.Throws<ArgumentException>(() => TilesetProcessor.Process(_fixture.AsepriteFile, string.Empty));
    }
}
