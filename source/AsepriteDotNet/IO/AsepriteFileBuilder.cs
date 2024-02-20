using System.Drawing;

namespace AsepriteDotNet.IO;

internal partial class AsepriteFileBuilder
{
    private readonly FileHeader _header;
    private readonly bool _isLayerOpacityValid;
    private readonly AsepritePalette _palette;
    private readonly List<string> _warnings;
    private readonly List<AsepriteLayer> _layers;
    private readonly List<AsepriteTileset> _tilesets;
    private readonly AsepriteFrame[] _frames;

    private AsepriteGroupLayer? _lastGroupLayer;
    private readonly List<AsepriteLayer> _childLayers;
    private readonly List<AsepriteCel> _currentFrameCels;


    internal AsepriteFileBuilder(FileHeader header)
    {
        _header = header;
        _frames = new AsepriteFrame[_header.FrameCount];
        _warnings = new List<string>();
        _layers = new List<AsepriteLayer>();
        _tilesets = new List<AsepriteTileset>();
        _childLayers = new List<AsepriteLayer>();
        _isLayerOpacityValid = (_header.Flags & 1) != 0;
        _palette = new AsepritePalette(new AseColor[_header.NumberOfColors], _header.TransparentIndex);
    }

    internal void BeginFrameRead()
    {
        _currentFrameCels.Clear();
    }

    internal void AddImageLayer(LayerChunkHeader header, string name)
    {
        AsepriteImageLayer layer = new AsepriteImageLayer(header, name);
        AddLayerToChildren(layer);
        _layers.Add(layer);
    }

    internal void AddGroupLayer(LayerChunkHeader header, string name)
    {
        AsepriteGroupLayer groupLayer = new AsepriteGroupLayer(header, name);
        _lastGroupLayer?.SetChildren(_childLayers);
        _childLayers.Clear();
        _lastGroupLayer = groupLayer;
        _layers.Add(groupLayer);
    }

    internal void AddTilemapLayer(LayerChunkHeader header, string name, int tilesetIndex)
    {
        AsepriteTileset tileset = _tilesets[tilesetIndex];
        AsepriteTilemapLayer tilemapLayer = new AsepriteTilemapLayer(header, name, tileset);
        AddLayerToChildren(tilemapLayer);
        _layers.Add(tilemapLayer);
    }

    private void AddLayerToChildren(AsepriteLayer layer)
    {
        if (layer.ChildLevel != 0 && _lastGroupLayer is not null)
        {
            _childLayers.Add(layer);
        }
    }

    internal void AddImageCel(CelProperties celProperties, ImageCelProperties imageCelProperties, byte[] pixelData)
    {
        AsepriteLayer layer = _layers[celProperties.LayerIndex];
        AseColor[] colorData = PixelsToColor(pixelData, (AsepriteColorDepth)_header.Depth, _palette);
        AsepriteImageCel imageCel = new AsepriteImageCel(celProperties, layer, imageCelProperties, colorData);
        _currentFrameCels.Add(imageCel);
    }

    internal void AddLinkedCel(CelProperties celProperties, int linkedFrameIndex)
    {
        AsepriteCel otherCel = _frames[linkedFrameIndex].Cels[_currentFrameCels.Count];
        AsepriteLinkedCel linkedCel = new AsepriteLinkedCel(celProperties, otherCel);
        _currentFrameCels.Add(linkedCel);
    }

    internal void AddTilemapCel(CelProperties celProperties, TilemapCelProperties tilemapCelProperties, byte[] tileData)
    {

    }

    private static AseColor[] PixelsToColor(byte[] pixels, AsepriteColorDepth depth, AsepritePalette palette)
    {
        return depth switch
        {
            AsepriteColorDepth.Indexed => IndexedPixelsToColor(pixels, palette),
            AsepriteColorDepth.Grayscale => GrayscalePixelsToColor(pixels),
            AsepriteColorDepth.RGBA => RgbaPixelsToColor(pixels),
            _ => throw new InvalidOperationException($"Unknown Color Depth: {depth}"),
        };
    }

    private static AseColor[] RgbaPixelsToColor(byte[] pixels)
    {
        int bpp = (int)AsepriteColorDepth.RGBA / 8;
        AseColor[] result = new AseColor[pixels.Length / bpp];

        for (int i = 0, b = 0; i < result.Length; i++, b += bpp)
        {
            byte red = pixels[b];
            byte green = pixels[b + 1];
            byte blue = pixels[b + 2];
            byte alpha = pixels[b + 3];
            result[i] = new AseColor(red, green, blue, alpha);
        }

        return result;
    }

    private static AseColor[] GrayscalePixelsToColor(byte[] pixels)
    {
        int bpp = (int)AsepriteColorDepth.Grayscale / 8;
        AseColor[] result = new AseColor[pixels.Length / bpp];

        for (int i = 0, b = 0; i < result.Length; i++, b += bpp)
        {
            byte red = pixels[b];
            byte green = pixels[b];
            byte blue = pixels[b];
            byte alpha = pixels[b + 1];
            result[i] = new AseColor(red, green, blue, alpha);
        }

        return result;
    }

    private static AseColor[] IndexedPixelsToColor(byte[] pixels, AsepritePalette palette)
    {
        int bpp = (int)AsepriteColorDepth.Indexed / 8;
        AseColor[] result = new AseColor[pixels.Length / bpp];

        for (int i = 0; i < pixels.Length; i++)
        {
            int index = pixels[i];

            if (index == palette.TransparentIndex)
            {
                result[i] = new AseColor(0, 0, 0, 0);
            }
            else
            {
                result[i] = palette.Colors[i];
            }
        }

        return result;
    }



}