//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Types;

/// <summary>
/// Represents a linked cel that references content from another cel instead of storing its own pixel data.
/// </summary>
/// <remarks>
/// Linked cels provide file size optimization by referencing content from other frames rather than
/// duplicating identical pixel data. Corresponds to cel type 1 in the Cel Chunk (0x2005) specification.
/// Position, opacity, and Z-index can differ from the referenced cel while sharing the same visual content.
/// </remarks>
public sealed class AsepriteLinkedCel : AsepriteCel
{
    /// <summary>
    /// Gets the target cel that contains the actual content referenced by this linked cel.
    /// </summary>
    /// <remarks>
    /// The referenced cel is typically an <see cref="AsepriteImageCel"/> or <see cref="AsepriteTilemapCel"/>
    /// from another frame within the same layer. The link is established through the frame position
    /// specified in the Cel Chunk data during file parsing.
    /// </remarks>
    public AsepriteCel Cel { get; internal set; }

    internal AsepriteLinkedCel() { }
}

