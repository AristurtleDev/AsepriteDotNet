// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class SpriteProcessorTestsFixture
{
    public string Name { get; } = "sprite-processor-test";
    public AsepriteFile AsepriteFile { get; }
    public Rgba32 Black { get; } =  new Rgba32(0, 0, 0, 255);
    public Rgba32 White { get; } = new Rgba32(255, 255, 255, 255);

    public SpriteProcessorTestsFixture()
    {
        AsepritePalette palette = new AsepritePalette(0);
        palette.Resize(2);
        palette[0] = Black;
        palette[1] = White;
        

        AsepriteTileset[] tilesets = Array.Empty<AsepriteTileset>();

        AsepriteLayerProperties layerProperties = new AsepriteLayerProperties() { Flags = 1, BlendMode = 0, Opacity = 255 };
        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new AsepriteImageLayer(layerProperties, "layer")
        };

        AsepriteCelProperties celProperties1 = new AsepriteCelProperties() { LayerIndex = 0, Opacity = 255, Type = 3, X = 0, Y = 0, ZIndex = 0 };
        AsepriteCelProperties celProperties2 = new AsepriteCelProperties() { LayerIndex = 1, Opacity = 255, Type = 3, X = 0, Y = 1, ZIndex = 0 };
        AsepriteImageCelProperties imageCelProperties = new AsepriteImageCelProperties() { Width = 2, Height = 2 };
        AsepriteCel[] frame0Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(celProperties1, layers[0], imageCelProperties, new Rgba32[] {Black, Black, Black, Black })
        };

        AsepriteCel[] frame1Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(celProperties1, layers[0], imageCelProperties, new Rgba32[] {White, White, White, White })
        };


        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{Name} 0", 2, 2, 100, new List<AsepriteCel>(frame0Cels)),
            new($"{Name} 1", 2, 2, 100, new List<AsepriteCel>(frame1Cels))
        };

        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();


        AsepriteUserData userData = new();
        AsepriteFile = new AsepriteFile(Name,
                                        palette,
                                        2,
                                        3,
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

public sealed class SpriteProcessorTests : IClassFixture<SpriteProcessorTestsFixture>
{
    private readonly SpriteProcessorTestsFixture _fixture;


    public SpriteProcessorTests(SpriteProcessorTestsFixture fixture) => _fixture = fixture;

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Process_Sprite_And_Texture_Name_Include_Correct_Index(int frame)
    {
        Sprite sprite = SpriteProcessor.Process(_fixture.AsepriteFile, frame);

        string name = $"{_fixture.Name} {frame}";

        Assert.Equal(name, sprite.Name);
        Assert.Equal(name, sprite.Texture.Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void Process_Throws_Exception_When_Frame_Index_Out_Of_Range(int frame)
    {
        Assert.Throws<IndexOutOfRangeException>(() => SpriteProcessor.Process(_fixture.AsepriteFile, frame));
    }

    [Fact]
    public void Process_Processes_Given_Frame()
    {
        Sprite frame0Sprite = SpriteProcessor.Process(_fixture.AsepriteFile, 0);
        Sprite frame1Sprite = SpriteProcessor.Process(_fixture.AsepriteFile, 1);

        Rgba32[] frame0Expected = new Rgba32[] { _fixture.Black, _fixture.Black, _fixture.Black, _fixture.Black };
        Rgba32[] frame1Expected = new Rgba32[] { _fixture.White, _fixture.White, _fixture.White, _fixture.White };

        Rgba32[] frame0Actual = frame0Sprite.Texture.Pixels.ToArray();
        Rgba32[] frame1Actual = frame1Sprite.Texture.Pixels.ToArray();

        Assert.Equal(frame0Expected, frame0Actual);
        Assert.Equal(frame1Expected, frame1Actual);
    }
}
