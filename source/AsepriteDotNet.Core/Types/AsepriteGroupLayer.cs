//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents a group layer that organizes child layers into a hierarchical structure without containing pixel data.
/// </summary>
/// <remarks>
/// Group layers provide organizational structure for complex sprite compositions and can apply
/// combined transformations or blend modes to all contained layers. They support collapsed display
/// in the editor interface and enable hierarchical opacity when header flags indicate group blending is valid.
/// </remarks>
public sealed class AsepriteGroupLayer : AsepriteLayer
{
    internal List<AsepriteLayer> InternalChildren = [];

    /// <summary>
    /// Gets the collection of child layers contained within this group.
    /// </summary>
    /// <remarks>
    /// Child layers are ordered as they appear in the file and maintain their relative positions
    /// within the group hierarchy. Child layers can include normal image layers, tilemap layers,
    /// or nested group layers for complex organizational structures.
    /// </remarks>
    public ReadOnlySpan<AsepriteLayer> Children => CollectionsMarshal.AsSpan(InternalChildren);

    internal AsepriteGroupLayer() { }
}

