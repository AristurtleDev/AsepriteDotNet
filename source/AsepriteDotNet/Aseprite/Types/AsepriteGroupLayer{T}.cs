//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that contains child layers.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteGroupLayer<T> : AsepriteLayer<T> where T: IColor, new()
{
    private readonly List<AsepriteLayer<T>> _children = new List<AsepriteLayer<T>>();

    /// <summary>
    /// Gets the child layers that were grouped inside this group layer.
    /// The order of layer elements is from bottom most to top most layer in the group.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer<T>> Children => CollectionsMarshal.AsSpan(_children);

    internal AsepriteGroupLayer(AsepriteLayerProperties header, string name) : base(header, name) { }

    internal void AddChild(AsepriteLayer<T> layer) => _children.Add(layer);
}
