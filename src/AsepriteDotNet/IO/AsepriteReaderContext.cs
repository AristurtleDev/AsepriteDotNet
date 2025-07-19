// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Drawing;
using AsepriteDotNet.FileFormat;
using AsepriteDotNet.Types;

namespace AsepriteDotNet.IO;

/// <summary>
/// Maintains parsing state and collected data during Aseprite file processing.
/// </summary>
/// <remarks>
/// Serves as the central state container for the file parsing pipeline, accumulating sprite components
/// and tracking context information needed for proper chunk interpretation and data association.
/// The context ensures correct relationships between layers, cels, user data, and other components
/// during the sequential parsing process.
/// </remarks>
internal sealed class AsepriteReaderContext
{
    /// <summary>
    /// Gets the palette being constructed during file parsing with dynamic resizing support.
    /// </summary>
    public AsepritePalette Palette { get; } = new();

    /// <summary>
    /// Gets the collection of frames being assembled during file parsing.
    /// </summary>
    public List<AsepriteFrame> Frames { get; } = [];

    /// <summary>
    /// Gets the collection of layers being constructed during file parsing.
    /// </summary>
    public List<AsepriteLayer> Layers { get; } = [];

    /// <summary>
    /// Gets the collection of animation tags being assembled during file parsing.
    /// </summary>
    /// <remarks>
    /// User data association for tags follows a special sequential pattern handled by the tag iterator.
    /// </remarks>
    public List<AsepriteTag> Tags { get; } = [];

    /// <summary>
    /// Gets the collection of named sprite regions being constructed during file parsing.
    /// </summary>
    public List<AsepriteSlice> Slices { get; } = [];

    /// <summary>
    /// Gets the collection of tilesets being assembled during file parsing.
    /// </summary>
    public List<AsepriteTileset> Tilesets { get; } = [];

    /// <summary>
    /// Gets the user data container for sprite-level metadata.
    /// </summary>
    /// <remarks>
    /// Sprite user data appears in the first frame after the Palette Chunk as specified in Aseprite v1.3+.
    /// </remarks>
    public AsepriteUserData SpriteUserData { get; } = new();

    /// <summary>
    /// Gets or sets the total number of frames expected in the sprite from the file header.
    /// </summary>
    public int FrameCount { get; set; }

    /// <summary>
    /// Gets or sets the sprite canvas dimensions from the file header.
    /// </summary>
    public Size CanvasSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether layer opacity values are valid based on header flags.
    /// </summary>
    /// <remarks>
    /// Corresponds to bit 1 of the header flags field. When false, layer opacity should be treated as fully opaque (255).
    /// Controls whether the opacity field in Layer Chunks (0x2004) should be processed or ignored.
    /// </remarks>
    public bool LayerOpacityValid { get; set; }

    /// <summary>
    /// Gets or sets the type of the most recently processed chunk for user data association tracking.
    /// </summary>
    /// <remarks>
    /// Reset to None at the start of each frame to ensure proper context isolation.
    /// </remarks>
    public ChunkType LastReadChunkType { get; set; } = ChunkType.None;

    /// <summary>
    /// Gets or sets the current target for user data association during chunk processing.
    /// </summary>
    /// <remarks>
    /// Points to the user data container of the most recently processed chunk (layer, cel, slice, etc.).
    /// </remarks>
    public AsepriteUserData CurrentUserData { get; set; }

    /// <summary>
    /// Gets or sets the color depth and pixel format for the entire sprite from the file header.
    /// </summary>
    /// <remarks>
    /// Used by the pixel conversion functions to properly decode cel and tileset image data.
    /// </remarks>
    public AsepriteColorDepth ColorDepth { get; set; }

    /// <summary>
    /// Gets or sets the frame currently being processed during chunk iteration.
    /// </summary>
    /// <remarks>
    /// Set at the beginning of each frame processing cycle and used as the target for cel additions.
    /// Reset between frames to ensure proper isolation and prevent cross-frame contamination.
    /// </remarks>
    public AsepriteFrame CurrentFrame { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether any palette chunk has been successfully processed.
    /// </summary>
    /// <remarks>
    /// Prevents processing of legacy palette chunks (0x0004, 0x0011) when a modern palette chunk (0x2019)
    /// has already been processed.
    /// </remarks>
    public bool PaletteRead { get; set; }

    /// <summary>
    /// Gets or sets the current position in the tag sequence for user data association.
    /// </summary>
    /// <remarks>
    /// Tags have special user data association rules where user data chunks appear sequentially after
    /// the Tags Chunk in the same order as the tags. This iterator tracks which tag should receive
    /// the next user data chunk during the sequential processing.
    /// </remarks>
    public int TagIterator { get; set; }

    /// <summary>
    /// Gets the mapping of child levels to their corresponding group layers for hierarchy construction.
    /// </summary>
    public Dictionary<int, AsepriteGroupLayer> LastGroupsByChildLevel = [];
}
