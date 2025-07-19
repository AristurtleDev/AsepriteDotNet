//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Defines the properties of a layer in an Aseprite file that contains child layers.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteGroupLayer : AsepriteLayer
{
    internal List<AsepriteLayer> InternalChildren = [];

    /// <summary>
    /// Gets the child layers that were grouped inside this group layer.
    /// The order of layer elements is from bottom most to top most layer in the group.
    /// </summary>
    public ReadOnlySpan<AsepriteLayer> Children => CollectionsMarshal.AsSpan(InternalChildren);

    internal AsepriteGroupLayer() { }
}
