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
    public AsepriteFile<SystemColor> AsepriteFile { get; }
    public SystemColor Black { get; } = new SystemColor(0, 0, 0, 255);
    public SystemColor White { get; } = new SystemColor(255, 255, 255, 255);

    public SpriteProcessorTestsFixture()
    {
        AsepritePalette<SystemColor> palette = new AsepritePalette<SystemColor>(0);
        palette.Resize(2);
        palette[0] = Black;
        palette[1] = White;


        AsepriteTileset<SystemColor>[] tilesets = Array.Empty<AsepriteTileset<SystemColor>>();

        AsepriteLayerProperties layerProperties = new AsepriteLayerProperties() { Flags = 1, BlendMode = 0, Opacity = 255 };
        AsepriteLayer<SystemColor>[] layers = new AsepriteLayer<SystemColor>[]
        {
            new AsepriteImageLayer<SystemColor>(layerProperties, "layer")
        };

        AsepriteCelProperties celProperties1 = new AsepriteCelProperties() { LayerIndex = 0, Opacity = 255, Type = 3, X = 0, Y = 0, ZIndex = 0 };
        AsepriteCelProperties celProperties2 = new AsepriteCelProperties() { LayerIndex = 1, Opacity = 255, Type = 3, X = 0, Y = 1, ZIndex = 0 };
        AsepriteImageCelProperties imageCelProperties = new AsepriteImageCelProperties() { Width = 2, Height = 2 };
        AsepriteCel<SystemColor>[] frame0Cels = new AsepriteCel<SystemColor>[]
        {
            new AsepriteImageCel<SystemColor>(celProperties1, layers[0], imageCelProperties, new SystemColor[] {Black, Black, Black, Black })
        };

        AsepriteCel<SystemColor>[] frame1Cels = new AsepriteCel<SystemColor>[]
        {
            new AsepriteImageCel<SystemColor>(celProperties1, layers[0], imageCelProperties, new SystemColor[] {White, White, White, White })
        };


        AsepriteFrame<SystemColor>[] frames = new AsepriteFrame<SystemColor>[]
        {
            new($"{Name} 0", 2, 2, 100, new List<AsepriteCel<SystemColor>>(frame0Cels)),
            new($"{Name} 1", 2, 2, 100, new List<AsepriteCel<SystemColor>>(frame1Cels))
        };

        AsepriteTag<SystemColor>[] tags = Array.Empty<AsepriteTag<SystemColor>>();
        AsepriteSlice<SystemColor>[] slices = Array.Empty<AsepriteSlice<SystemColor>>();


        AsepriteUserData<SystemColor> userData = new();
        AsepriteFile = new AsepriteFile<SystemColor>(Name,
                                                palette,
                                                2,
                                                3,
                                                AsepriteColorDepth.RGBA,
                                                new List<AsepriteFrame<SystemColor>>(frames),
                                                new List<AsepriteLayer<SystemColor>>(layers),
                                                new List<AsepriteTag<SystemColor>>(tags),
                                                new List<AsepriteSlice<SystemColor>>(slices),
                                                new List<AsepriteTileset<SystemColor>>(tilesets),
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
        Sprite<SystemColor> sprite = SpriteProcessor.Process(_fixture.AsepriteFile, frame);

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
        Sprite<SystemColor> frame0Sprite = SpriteProcessor.Process(_fixture.AsepriteFile, 0);
        Sprite<SystemColor> frame1Sprite = SpriteProcessor.Process(_fixture.AsepriteFile, 1);

        SystemColor[] frame0Expected = new SystemColor[] { _fixture.Black, _fixture.Black, _fixture.Black, _fixture.Black };
        SystemColor[] frame1Expected = new SystemColor[] { _fixture.White, _fixture.White, _fixture.White, _fixture.White };

        SystemColor[] frame0Actual = frame0Sprite.Texture.Pixels.ToArray();
        SystemColor[] frame1Actual = frame1Sprite.Texture.Pixels.ToArray();

        Assert.Equal(frame0Expected, frame0Actual);
        Assert.Equal(frame1Expected, frame1Actual);
    }
}
