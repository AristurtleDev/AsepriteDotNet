// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.Processors;

namespace AsepriteDotNet.Tests.Processors;

public sealed class TextureAtlasProcessorTestFixture
{
    public string Name { get; } = "sprite-processor-test";
    public AsepriteFile<Rgba32> AsepriteFile { get; }
    public Rgba32 Red { get; } = new Rgba32(255, 0, 0, 255);
    public Rgba32 Green { get; } = new Rgba32(0, 255, 0, 255);
    public Rgba32 Blue { get; } = new Rgba32(0, 0, 255, 255);
    public Rgba32 Yellow { get; } = new Rgba32(255, 255, 0, 255);

    public TextureAtlasProcessorTestFixture()
    {
        int width = 2;
        int height = 2;

        AsepritePalette<Rgba32> palette = new AsepritePalette<Rgba32>(0);
        palette.Resize(4);
        palette[0] = Red;
        palette[0] = Green;
        palette[0] = Blue;
        palette[0] = Yellow;

        AsepriteTileset<Rgba32>[] tilesets = Array.Empty<AsepriteTileset<Rgba32>>();

        List<AsepriteLayer<Rgba32>> layers = new List<AsepriteLayer<Rgba32>>()
        {
            new AsepriteImageLayer<Rgba32>(new AsepriteLayerProperties() {Flags = 1, BlendMode = 0, Opacity = 255 }, "layer")
        };

        AsepriteCelProperties celProperties = new AsepriteCelProperties() { Opacity = 255, LayerIndex = 0 };
        AsepriteImageCelProperties imageCelProperties = new AsepriteImageCelProperties() { Width = 2, Height = 2 };
        List<AsepriteCel<Rgba32>> frame0Cels = new List<AsepriteCel<Rgba32>>()
        {
            new AsepriteImageCel<Rgba32>(celProperties, layers[0], imageCelProperties, new Rgba32[] {Red, Red, Red, Red })
        };

        List<AsepriteCel<Rgba32>> frame1Cels = new List<AsepriteCel<Rgba32>>()
        {
            new AsepriteImageCel<Rgba32>(celProperties, layers[0], imageCelProperties, new Rgba32[] { Green, Green, Green, Green })
        };

        List<AsepriteCel<Rgba32>> frame2Cels = new List<AsepriteCel<Rgba32>>()
        {
            new AsepriteImageCel<Rgba32>(celProperties, layers[0], imageCelProperties, new Rgba32[] { Blue, Blue, Blue, Blue })
        };

        List<AsepriteCel<Rgba32>> frame3Cels = new List<AsepriteCel<Rgba32>>()
        {
            new AsepriteImageCel<Rgba32>(celProperties, layers[0], imageCelProperties, new Rgba32[] { Red, Red, Red, Red })
        };

        List<AsepriteFrame<Rgba32>> frames = new List<AsepriteFrame<Rgba32>>()
        {
            new($"{Name} 0", width, height, 100, frame0Cels),
            new($"{Name} 1", width, height, 100, frame1Cels),
            new($"{Name} 2", width, height, 100, frame2Cels),
            new($"{Name} 3", width, height, 100, frame3Cels),
        };

        AsepriteFile = new AsepriteFile<Rgba32>(Name, palette, width, height, AsepriteColorDepth.RGBA, frames, layers, [], [], [], new AsepriteUserData<Rgba32>(), []);
    }
}

public sealed class TextureAtlasProcessorTests : IClassFixture<TextureAtlasProcessorTestFixture>
{
    private readonly TextureAtlasProcessorTestFixture _fixture;

    //  These are the colors that will be expected in the  texture created during each process.  They are created
    //  here an named this way so that it's easier to visualize what the pixel array should be in the test below.
    private readonly Rgba32 _ = new Rgba32(0, 0, 0, 0);     //  Represents a transparent pixel from padding/spacing, not source
    private readonly Rgba32 r = new Rgba32(255, 0, 0, 255); //  Represents a red pixel
    private readonly Rgba32 g = new Rgba32(0, 255, 0, 255); //  Represents a green pixel
    private readonly Rgba32 b = new Rgba32(0, 0, 255, 255); //  Represents a blue pixel
    private readonly Rgba32 t = new Rgba32(0, 0, 0, 0);     //  Represents a transparent pixel from source, not padding/spacing.

    public TextureAtlasProcessorTests(TextureAtlasProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void Process_Atlas_And_Texture_Name_Same_As_File_Name()
    {
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile);
        Assert.Equal(_fixture.Name, atlas.Name);
        Assert.Equal(_fixture.Name, atlas.Texture.Name);
    }

    [Fact]
    public void Process_One_Region_Per_Frame()
    {
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile);
        Assert.Equal(_fixture.AsepriteFile.Frames.Length, atlas.Regions.Length);
    }

    [Fact]
    public void ProcessRegion_Names_Are_Frame_Names()
    {
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile);

        Assert.Equal(_fixture.AsepriteFile.Frames[0].Name, atlas.Regions[0].Name);
        Assert.Equal(_fixture.AsepriteFile.Frames[1].Name, atlas.Regions[1].Name);
        Assert.Equal(_fixture.AsepriteFile.Frames[2].Name, atlas.Regions[2].Name);
        Assert.Equal(_fixture.AsepriteFile.Frames[3].Name, atlas.Regions[3].Name);
    }

    [Fact]
    public void Process_Duplicate_Frame_Is_Merged()
    {
        ProcessorOptions options = ProcessorOptions.Default with { MergeDuplicateFrames = true };
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Rgba32[] expected = new Rgba32[]
        {
            r, r, g, g,
            r, r, g, g,
            b, b, t, t,
            b, b, t, t
        };

        Rgba32[] actual = atlas.Texture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.Regions[0].Bounds);
        Assert.Equal(new Rectangle(2, 0, 2, 2), atlas.Regions[1].Bounds);
        Assert.Equal(new Rectangle(0, 2, 2, 2), atlas.Regions[2].Bounds);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.Regions[3].Bounds);
    }

    [Fact]
    public void Process_Duplicate_Frame_Not_Merged()
    {
        ProcessorOptions options = ProcessorOptions.Default with { MergeDuplicateFrames = false };
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Rgba32[] expected = new Rgba32[]
        {
            r, r, g, g,
            r, r, g, g,
            b, b, r, r,
            b, b, r, r,
        };

        Rgba32[] actual = atlas.Texture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.Regions[0].Bounds);
        Assert.Equal(new Rectangle(2, 0, 2, 2), atlas.Regions[1].Bounds);
        Assert.Equal(new Rectangle(0, 2, 2, 2), atlas.Regions[2].Bounds);
        Assert.Equal(new Rectangle(2, 2, 2, 2), atlas.Regions[3].Bounds);
    }

    [Fact]
    public void Process_Border_Padding_Added_Correctly()
    {
        ProcessorOptions options = ProcessorOptions.Default with { BorderPadding = 1 };
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Rgba32[] expected = new Rgba32[]
        {
            _, _, _, _, _, _,
            _, r, r, g, g, _,
            _, r, r, g, g, _,
            _, b, b, t, t, _,
            _, b, b, t, t, _,
            _, _, _, _, _, _
        };

        Rgba32[] actual = atlas.Texture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.Regions[0].Bounds);
        Assert.Equal(new Rectangle(3, 1, 2, 2), atlas.Regions[1].Bounds);
        Assert.Equal(new Rectangle(1, 3, 2, 2), atlas.Regions[2].Bounds);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.Regions[3].Bounds);
    }

    [Fact]
    public void Process_Spacing_Added_Correctly()
    {
        ProcessorOptions options = ProcessorOptions.Default with { Spacing = 1 };
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Rgba32[] expected = new Rgba32[]
        {
            r, r, _, g, g,
            r, r, _, g, g,
            _, _, _, _, _,
            b, b, _, t, t,
            b, b, _, t, t
        };

        Rgba32[] actual = atlas.Texture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.Regions[0].Bounds);
        Assert.Equal(new Rectangle(3, 0, 2, 2), atlas.Regions[1].Bounds);
        Assert.Equal(new Rectangle(0, 3, 2, 2), atlas.Regions[2].Bounds);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.Regions[3].Bounds);
    }

    [Fact]
    public void Process_InnerPadding_Added_Correctly()
    {
        ProcessorOptions options = ProcessorOptions.Default with { InnerPadding = 1 };
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Rgba32[] expected = new Rgba32[]
        {
            _, _, _, _, _, _, _, _,
            _, r, r, _, _, g, g, _,
            _, r, r, _, _, g, g, _,
            _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,
            _, b, b, _, _, t, t, _,
            _, b, b, _, _, t, t, _,
            _, _, _, _, _, _, _, _,
        };

        Rgba32[] actual = atlas.Texture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.Regions[0].Bounds);
        Assert.Equal(new Rectangle(5, 1, 2, 2), atlas.Regions[1].Bounds);
        Assert.Equal(new Rectangle(1, 5, 2, 2), atlas.Regions[2].Bounds);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.Regions[3].Bounds);
    }

    [Fact]
    public void Process_Combined_Border_Padding_Spacing_Inner_Padding_Added_Correctly()
    {
        ProcessorOptions options = ProcessorOptions.Default with { BorderPadding = 1, Spacing = 1, InnerPadding = 1 };
        TextureAtlas<Rgba32> atlas = TextureAtlasProcessor.Process(_fixture.AsepriteFile, options);

        Rgba32[] expected = new Rgba32[]
        {
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, r, r, _, _, _, g, g, _, _,
            _, _, r, r, _, _, _, g, g, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, b, b, _, _, _, t, t, _, _,
            _, _, b, b, _, _, _, t, t, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _
        };

        Rgba32[] actual = atlas.Texture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(2, 2, 2, 2), atlas.Regions[0].Bounds);
        Assert.Equal(new Rectangle(7, 2, 2, 2), atlas.Regions[1].Bounds);
        Assert.Equal(new Rectangle(2, 7, 2, 2), atlas.Regions[2].Bounds);
        Assert.Equal(new Rectangle(2, 2, 2, 2), atlas.Regions[3].Bounds);
    }
}
