//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that contains child layers.  This class cannot be inherited.
/// </summary>
public sealed class GroupLayer : Layer
{
    private readonly List<Layer> _children = new List<Layer>();

    /// <summary>
    /// Gets the child layers that were grouped inside this group layer.
    /// The order of layer elements is from bottom most to top most layer in the group.
    /// </summary>
    public ReadOnlySpan<Layer> Children => CollectionsMarshal.AsSpan(_children);

    internal GroupLayer(LayerProperties header, string name) : base(header, name) { }

    internal void AddChild(Layer layer) => _children.Add(layer);
}
