//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core;

public sealed class AsepriteFile
{
    internal AsepriteFrame[] InternalFrames { get; set; }
    internal AsepriteLayer[] InternalLayers { get; set; }
    internal AsepriteTag[] InternalTags { get; set; }
    internal AsepriteSlice[] InternalSlices { get; set; }
    internal AsepriteTileset[] InternalTilesets { get; set; }

    public Size CanvasSize { get; internal set; }

    public AsepriteColorDepth ColorDepth { get; internal set; }

    public ReadOnlySpan<AsepriteFrame> Frames => InternalFrames;

    public ReadOnlySpan<AsepriteLayer> Layers => InternalLayers;

    public ReadOnlySpan<AsepriteTag> Tags => InternalTags;

    public ReadOnlySpan<AsepriteSlice> Slices => InternalSlices;

    public ReadOnlySpan<AsepriteTileset> Tilesets => InternalTilesets;

    public AsepritePalette Palette { get; internal set; }

    public AsepriteUserData UserData { get; internal set; }

    public string Name { get; internal set; }

    public int FrameCount => InternalFrames.Length;

    internal AsepriteFile() { }

    public AsepriteFrame GetFrame(int index) => InternalFrames[index];

    public bool TryGetFrame(int index, [NotNullWhen(true)] out AsepriteFrame frame)
    {
        frame = default;
        try { frame = InternalFrames[index]; }
        catch (ArgumentOutOfRangeException) { }
        return frame is not null;
    }

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
