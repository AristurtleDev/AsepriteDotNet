//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents an <see cref="AsepriteLayer"/> in an Aseprite file that contains child layers.
/// </summary>
public sealed class AsepriteGroupLayer : AsepriteLayer
{
    private AsepriteLayer[] _children;

    /// <summary>
    /// The collection of all child <see cref="AsepriteLayer"/> elements grouped into this
    /// <see cref="AsepriteGroupLayer"/>.  Order of <see cref="AsepriteLayer"/> elements is from bottom most layer to
    /// top most layer in the group.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer> Children => _children;

    internal AsepriteGroupLayer(LayerProperties header, string name) : base(header, name)
    {
        _children = Array.Empty<AsepriteLayer>();
    }

    internal void SetChildren(List<AsepriteLayer> children)
    {
        _children = [.. children];
    }
}
