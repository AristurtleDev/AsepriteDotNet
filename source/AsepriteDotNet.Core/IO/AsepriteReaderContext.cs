using System.Drawing;
using AsepriteDotNet.Core.FileFormat;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core.IO;

internal sealed class AsepriteReaderContext
{
    public AsepritePalette Palette { get; } = new();
    public List<AsepriteFrame> Frames { get; } = [];
    public List<AsepriteLayer> Layers { get; } = [];
    public List<AsepriteTag> Tags { get; } = [];
    public List<AsepriteSlice> Slices { get; } = [];
    public List<AsepriteTileset> Tilesets { get; } = [];
    public AsepriteUserData SpriteUserData { get; } = new();

    public int FrameCount { get; set; }
    public Size CanvasSize { get; set; }
    public bool LayerOpacityValid { get; set; }
    public ChunkType LastReadChunkType { get; set; } = ChunkType.None;
    public AsepriteUserData CurrentUserData { get; set; }
    public AsepriteColorDepth ColorDepth { get; set; }
    public AsepriteFrame CurrentFrame { get; set; }
    public bool PaletteRead { get; set; }
    public int TagIterator { get; set; }
    public Dictionary<int, AsepriteGroupLayer> LastGroupsByChildLevel = [];
}
