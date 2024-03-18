//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using AsepriteDotNet.Aseprite.Document;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that contains child layers.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteGroupLayer : AsepriteLayer
{
    private readonly List<AsepriteLayer> _children = new List<AsepriteLayer>();

    /// <summary>
    /// Gets the child layers that were grouped inside this group layer.
    /// The order of layer elements is from bottom most to top most layer in the group.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer> Children => CollectionsMarshal.AsSpan(_children);

    internal AsepriteGroupLayer(AsepriteLayerProperties header, string name) : base(header, name) { }

    internal void AddChild(AsepriteLayer layer) => _children.Add(layer);
}
