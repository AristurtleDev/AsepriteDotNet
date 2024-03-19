// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Net.Http.Headers;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class SpriteSheetProcessorTestFixture
{
    public string Name { get; } = "sprite-processor-test";
    public AsepriteFile AsepriteFile { get; }
    public Rgba32 Red { get; } = new Rgba32(255, 0, 0, 255);
    public Rgba32 Green { get; } = new Rgba32(0, 255, 0, 255);
    public Rgba32 Blue { get; } = new Rgba32(0, 0, 255, 255);
    public Rgba32 Yellow { get; } = new Rgba32(255, 255, 0, 255);


    public SpriteSheetProcessorTestFixture()
    {
        int width = 2;
        int height = 2;

        AsepritePalette palette = new AsepritePalette(0);
        palette.Resize(4);
        palette[0] = Red;
        palette[1] = Green;
        palette[2] = Blue;
        palette[3] = Yellow;

        AsepriteLayerProperties layerProperties = new AsepriteLayerProperties() { Flags = 1, Opacity = 255, BlendMode = 0 };
        List<AsepriteLayer> layers = new List<AsepriteLayer>()
        {
            new AsepriteImageLayer(layerProperties, "layer")
        };

        AsepriteCelProperties celProperties = new AsepriteCelProperties() { Opacity = 255, };
        AsepriteImageCelProperties imageCelPropertes = new AsepriteImageCelProperties() { Width = 2, Height = 2 };
        List<AsepriteCel> frame0Cels = new List<AsepriteCel>()
        {
            new AsepriteImageCel(celProperties, layers[0], imageCelPropertes, new Rgba32[] {Red, Red, Red, Red })
        };

        List<AsepriteCel> frame1Cels = new List<AsepriteCel>()
        {
            new AsepriteImageCel(celProperties, layers[0], imageCelPropertes, new Rgba32[] {Green, Green, Green, Green})
        };

        List<AsepriteCel> frame2Cels = new List<AsepriteCel>()
        {
            new AsepriteImageCel(celProperties, layers[0], imageCelPropertes, new Rgba32[] { Blue, Blue, Blue, Blue})
        };

        List<AsepriteCel> frame3Cels = new List<AsepriteCel>()
        {
            new AsepriteImageCel(celProperties, layers[0], imageCelPropertes, new Rgba32[] { Red, Red, Red, Red})
        };

        List<AsepriteFrame> frames = new List<AsepriteFrame>()
        {
            new($"{Name} 0", width, height, 100, frame0Cels),
            new($"{Name} 1", width, height, 100, frame1Cels),
            new($"{Name} 2", width, height, 100, frame2Cels),
            new($"{Name} 3", width, height, 100, frame3Cels),
        };

        List<AsepriteTag> tags = new List<AsepriteTag>()
        {
            new AsepriteTag(new AsepriteTagProperties() {Direction = 0, From = 0, To = 0}, "tag-0"),
            new AsepriteTag(new AsepriteTagProperties() {Direction = 0, From = 0, To = 1}, "tag-1"),
            new AsepriteTag(new AsepriteTagProperties() {Direction = 2, From = 1, To = 2}, "tag-2"),
        };

        AsepriteFile = new AsepriteFile(Name, palette, width, height, AsepriteColorDepth.RGBA, frames, layers, tags, [], [], new AsepriteUserData(), []);
    }
}

public sealed class SpriteSheetProcessorTests : IClassFixture<SpriteSheetProcessorTestFixture>
{
    private readonly SpriteSheetProcessorTestFixture _fixture;

    public SpriteSheetProcessorTests(SpriteSheetProcessorTestFixture fixture) => _fixture = fixture;

    [Theory]
    [InlineData(true, true, true, true, 1, 1, 1)]
    [InlineData(true, false, true, true, 1, 0, 1)]
    [InlineData(true, true, false, true, 1, 1, 0)]
    [InlineData(true, true, true, false, 0, 1, 1)]
    [InlineData(false, true, true, true, 0, 1, 0)]
    [InlineData(false, false, true, true, 0, 0, 1)]
    [InlineData(false, true, false, true, 0, 0, 0)]
    [InlineData(false, true, true, false, 0, 0, 0)]
    [InlineData(false, false, false, true, 0, 0, 0)]
    [InlineData(false, false, true, false, 0, 0, 0)]
    [InlineData(false, false, false, false, 0, 0, 0)]
    public void Process_Parameters_To_TextureAtlasProcess_Correctly(bool onlyVisible, bool includeBackground, bool includeTilemap, bool mergeDuplicates, int borderPadding, int spacing, int innerPadding)
    {
        ProcessorOptions options = new ProcessorOptions(onlyVisible, includeBackground, includeTilemap, mergeDuplicates, borderPadding, spacing, innerPadding);
        SpriteSheet sheet = SpriteSheetProcessor.Process(_fixture.AsepriteFile, options);
        TextureAtlas expected = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Assert.Equal(expected, sheet.TextureAtlas);
    }

    [Fact]
    public void Process_SpriteSheet_Atlas_and_Texture_Names_Same_As_File_Name()
    {
        SpriteSheet sheet = SpriteSheetProcessor.Process(_fixture.AsepriteFile);
        Assert.Equal(_fixture.Name, sheet.Name);
        Assert.Equal(_fixture.Name, sheet.TextureAtlas.Name);
        Assert.Equal(_fixture.Name, sheet.TextureAtlas.Texture.Name);
    }

    [Fact]
    public void Processes_All_Tags()
    {
        SpriteSheet sheet = SpriteSheetProcessor.Process(_fixture.AsepriteFile);
        Assert.Equal(_fixture.AsepriteFile.Tags.Length, sheet.Tags.Length);
    }

    [Fact]
    public void Process_Duplicate_AsepriteTag_Names_Throws_Exception()
    {
        List<AsepriteTag> tags = new List<AsepriteTag>()
        {
            new AsepriteTag(new AsepriteTagProperties(), "tag-0"),
            new AsepriteTag(new AsepriteTagProperties(), "tag-1"),
            new AsepriteTag(new AsepriteTagProperties(), "tag-0"),
        };

        //  Reuse the fixture, but use the tags array from above with duplicate tag names
        AsepriteFile aseFile = new(_fixture.Name,
                                   _fixture.AsepriteFile.Palette,
                                   _fixture.AsepriteFile.CanvasWidth,
                                   _fixture.AsepriteFile.CanvasHeight,
                                   _fixture.AsepriteFile.ColorDepth,
                                   new List<AsepriteFrame>(_fixture.AsepriteFile.Frames.ToArray()),
                                   new List<AsepriteLayer>(_fixture.AsepriteFile.Layers.ToArray()),
                                   tags,
                                   new List<AsepriteSlice>(_fixture.AsepriteFile.Slices.ToArray()),
                                   new List<AsepriteTileset>(_fixture.AsepriteFile.Tilesets.ToArray()),
                                   _fixture.AsepriteFile.UserData,
                                   new List<string>());

        Assert.Throws<InvalidOperationException>(() => SpriteSheetProcessor.Process(aseFile));
    }
}
