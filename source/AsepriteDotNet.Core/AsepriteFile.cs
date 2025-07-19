//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core;

/// <summary>
/// Represents a complete Aseprite sprite file (.ase/.aseprite) containing all frames, layers, and metadata.
/// </summary>
/// <remarks>
/// Serves as the root container for all sprite data parsed from an Aseprite file, providing access to
/// frames, layers, animation tags, slices, tilesets, and associated metadata. All collections maintain
/// the original order as specified in the file format for consistent rendering and animation playback.
/// </remarks>
public sealed class AsepriteFile
{
    internal AsepriteFrame[] InternalFrames { get; set; }
    internal AsepriteLayer[] InternalLayers { get; set; }
    internal AsepriteTag[] InternalTags { get; set; }
    internal AsepriteSlice[] InternalSlices { get; set; }
    internal AsepriteTileset[] InternalTilesets { get; set; }

    /// <summary>
    /// Gets the dimensions of the sprite canvas in pixels.
    /// </summary>
    /// <remarks>
    /// Defines the rendering area for the sprite. Individual cels can extend beyond these bounds
    /// using negative positioning, but this represents the primary composition area.
    /// </remarks>
    public Size CanvasSize { get; internal set; }

    /// <summary>
    /// Gets the color depth and pixel format used throughout the sprite.
    /// </summary>
    /// <remarks>
    /// Determines how pixel data is stored and interpreted: 8-bit indexed, 16-bit grayscale, or 32-bit RGBA.
    /// All frames and cels within the sprite use this same color format.
    /// </remarks>
    public AsepriteColorDepth ColorDepth { get; internal set; }

    /// <summary>
    /// Gets the collection of animation frames in chronological order.
    /// </summary>
    /// <remarks>
    /// Frames are ordered sequentially and contain the cels and timing information needed for animation playback.
    /// Each frame represents a single moment in the animation with its own duration and cel composition.
    /// </remarks>
    public ReadOnlySpan<AsepriteFrame> Frames => InternalFrames;

    /// <summary>
    /// Gets the complete layer hierarchy including normal, group, and tilemap layers.
    /// </summary>
    /// <remarks>
    /// Layers are ordered as they appear in the Layer Chunks, with child levels indicating hierarchical
    /// relationships.
    /// </remarks>
    public ReadOnlySpan<AsepriteLayer> Layers => InternalLayers;

    /// <summary>
    /// Gets the collection of animation tags that organize frames into named sequences.
    /// </summary>
    /// <remarks>
    /// Tags define animation sequences with custom loop directions and repeat counts.
    /// </remarks>
    public ReadOnlySpan<AsepriteTag> Tags => InternalTags;

    /// <summary>
    /// Gets the collection of named sprite regions for UI scaling and sprite positioning.
    /// </summary>
    /// <remarks>
    /// Slices define rectangular regions with optional 9-patch scaling and pivot points for game engine integration,
    /// supporting both static bounds and keyframe animation for dynamic sprite regions.
    /// </remarks>
    public ReadOnlySpan<AsepriteSlice> Slices => InternalSlices;

    /// <summary>
    /// Gets the collection of tilesets used by tilemap layers.
    /// </summary>
    /// <remarks>
    /// Tilesets provide the visual content referenced by tilemap layers,
    /// </remarks>
    public ReadOnlySpan<AsepriteTileset> Tilesets => InternalTilesets;

    /// <summary>
    /// Gets the color palette used for indexed color sprites.
    /// </summary>
    public AsepritePalette Palette { get; internal set; }

    /// <summary>
    /// Gets the user-defined metadata associated with the entire sprite.
    /// </summary>
    public AsepriteUserData UserData { get; internal set; }

    /// <summary>
    /// Gets the descriptive name of the sprite file.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Gets the total number of frames in the animation sequence.
    /// </summary>
    /// <remarks>
    /// Equivalent to Frames.Length but provides direct access to the count without span allocation.
    /// </remarks>
    public int FrameCount => InternalFrames.Length;

    internal AsepriteFile() { }

    /// <summary>
    /// Gets the first layer with the specified name using case-sensitive comparison.
    /// </summary>
    /// <param name="name">The exact name of the layer to find.</param>
    /// <returns>The <see cref="AsepriteLayer"/> with the matching name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no layer with the specified name is found.</exception>
    /// <remarks>
    /// Uses ordinal string comparison for exact name matching. Returns the first matching layer
    /// if multiple layers share the same name.
    /// </remarks>
    public AsepriteLayer GetLayer(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        for (int i = 0; i < InternalLayers.Length; i++)
        {
            AsepriteLayer layer = InternalLayers[i];
            if (layer.Name.Equals(name, StringComparison.Ordinal))
            {
                return layer;
            }
        }

        throw new InvalidOperationException($"Unable to find a layer with the name '{name}'");
    }

    /// <summary>
    /// Attempts to get the first layer with the specified name without throwing exceptions.
    /// </summary>
    /// <param name="name">The exact name of the layer to find.</param>
    /// <param name="layer">When this method returns, contains the layer if found; otherwise, null.</param>
    /// <returns><c>true</c> if a layer with the specified name was found; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Provides safe layer lookup for scenarios where the layer existence is uncertain.
    /// Uses case-sensitive ordinal string comparison.
    /// </remarks>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out AsepriteLayer layer)
    {
        layer = null;

        for (int i = 0; i < InternalLayers.Length; i++)
        {
            AsepriteLayer possibleLayer = InternalLayers[i];
            if (possibleLayer.Name.Equals(name, StringComparison.Ordinal))
            {
                layer = possibleLayer;
                break;
            }
        }

        return layer is not null;
    }

    /// <summary>
    /// Gets the first animation tag with the specified name using case-sensitive comparison.
    /// </summary>
    /// <param name="name">The exact name of the tag to find.</param>
    /// <returns>The <see cref="AsepriteTag"/> with the matching name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no tag with the specified name is found.</exception>
    /// <remarks>
    /// Returns the first matching tag if multiple tags share the same name.
    /// </remarks>
    public AsepriteTag GetTag(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        for (int i = 0; i < InternalTags.Length; i++)
        {
            AsepriteTag tag = InternalTags[i];
            if (tag.Name.Equals(name, StringComparison.Ordinal))
            {
                return tag;
            }
        }

        throw new InvalidOperationException($"Unable to find a tag with the name '{name}'");
    }

    /// <summary>
    /// Attempts to get the first animation tag with the specified name without throwing exceptions.
    /// </summary>
    /// <param name="name">The exact name of the tag to find.</param>
    /// <param name="tag">When this method returns, contains the tag if found; otherwise, null.</param>
    /// <returns><c>true</c> if a tag with the specified name was found; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Provides safe tag lookup for animation sequence management scenarios.
    /// Uses case-sensitive ordinal string comparison.
    /// </remarks>
    public bool TryGetTag(string name, [NotNullWhen(true)] out AsepriteTag tag)
    {
        tag = default;

        for (int i = 0; i < InternalTags.Length; i++)
        {
            AsepriteTag possibleTag = InternalTags[i];
            if (possibleTag.Name.Equals(name, StringComparison.Ordinal))
            {
                tag = possibleTag;
                break;
            }
        }

        return tag is not null;
    }

    /// <summary>
    /// Gets the first slice with the specified name using case-sensitive comparison.
    /// </summary>
    /// <param name="name">The exact name of the slice to find.</param>
    /// <returns>The <see cref="AsepriteSlice"/> with the matching name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no slice with the specified name is found.</exception>
    /// <remarks>
    /// Returns the first matching slice if multiple slices share the same name.
    /// </remarks>
    public AsepriteSlice GetSlice(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        for (int i = 0; i < InternalSlices.Length; i++)
        {
            AsepriteSlice slice = InternalSlices[i];
            if (slice.Name.Equals(name, StringComparison.Ordinal))
            {
                return slice;
            }
        }

        throw new InvalidOperationException($"Unable to find a slice with the name '{name}'");
    }

    /// <summary>
    /// Attempts to get the first slice with the specified name without throwing exceptions.
    /// </summary>
    /// <param name="name">The exact name of the slice to find.</param>
    /// <param name="slice">When this method returns, contains the slice if found; otherwise, null.</param>
    /// <returns><c>true</c> if a slice with the specified name was found; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Provides safe slice lookup for sprite region management workflows.
    /// Uses case-sensitive ordinal string comparison.
    /// </remarks>
    public bool TryGetSlice(string name, [NotNullWhen(true)] out AsepriteSlice slice)
    {
        slice = default;

        for (int i = 0; i < InternalSlices.Length; i++)
        {
            AsepriteSlice possibleSlice = InternalSlices[i];
            if (possibleSlice.Name.Equals(name, StringComparison.Ordinal))
            {
                slice = possibleSlice;
                break;
            }
        }

        return slice is not null;
    }

    /// <summary>
    /// Gets the first tileset with the specified name using case-sensitive comparison.
    /// </summary>
    /// <param name="name">The exact name of the tileset to find.</param>
    /// <returns>The <see cref="AsepriteTileset"/> with the matching name.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no tileset with the specified name is found.</exception>
    /// <remarks>
    /// Returns the first matching tileset if multiple tilesets share the same name.
    /// </remarks>
    public AsepriteTileset GetTileset(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        for (int i = 0; i < InternalTilesets.Length; i++)
        {
            AsepriteTileset tileSet = InternalTilesets[i];
            if (tileSet.Name.Equals(name, StringComparison.Ordinal))
            {
                return tileSet;
            }
        }

        throw new InvalidOperationException($"Unable to find a tileset with the name '{name}'");
    }

    /// <summary>
    /// Attempts to get the first tileset with the specified name without throwing exceptions.
    /// </summary>
    /// <param name="name">The exact name of the tileset to find.</param>
    /// <param name="tileset">When this method returns, contains the tileset if found; otherwise, null.</param>
    /// <returns><c>true</c> if a tileset with the specified name was found; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Provides safe tileset lookup for tile-based rendering and management workflows.
    /// Uses case-sensitive ordinal string comparison.
    /// </remarks>
    public bool TryGetTileset(string name, [NotNullWhen(true)] out AsepriteTileset tileset)
    {
        tileset = default;

        for (int i = 0; i < InternalTilesets.Length; i++)
        {
            AsepriteTileset possibleTileset = InternalTilesets[i];
            if (possibleTileset.Name.Equals(name, StringComparison.Ordinal))
            {
                tileset = possibleTileset;
                break;
            }
        }

        return tileset is not null;
    }
}
