//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet.Core.Types;

/// <summary>
/// Represents user-defined metadata that can be associated with sprite components including text descriptions and color annotations.
/// </summary>
/// <remarks>
/// User data provides extensible metadata storage for layers, cels, tags, tiles, and the sprite itself.
/// </remarks>
public sealed class AsepriteUserData
{
    /// <summary>
    /// Gets a value indicating whether this user data contains text information.
    /// </summary>
    /// <remarks>
    /// When true, the Text property is guaranteed to be non-null and can be safely accessed.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(Text))]
    public bool HasText => Text is not null;

    /// <summary>
    /// Gets a value indicating whether this user data contains color information.
    /// </summary>
    /// <remarks>
    /// When true, the Color property is guaranteed to be non-null and can be safely accessed.
    /// </remarks>
    [MemberNotNullWhen(true, nameof(Color))]
    public bool HasColor => Color is not null;

    /// <summary>
    /// Gets the UTF-8 encoded text description associated with the sprite component.
    /// </summary>
    /// <remarks>
    /// Check HasText before accessing to ensure the value is not null, or use the null-conditional operator.
    /// </remarks>
    public string Text { get; internal set; }

    /// <summary>
    /// Gets the RGBA color annotation associated with the sprite component.
    /// </summary>
    /// <remarks>
    /// Commonly used for color-coding layers, tags, or other sprite elements in the Aseprite editor interface.
    /// Check HasColor before accessing to ensure the value is not null, or use the null-conditional operator.
    /// </remarks>
    public Rgba32? Color { get; internal set; }

    internal AsepriteUserData()
    {
        Text = null;
        Color = null;
    }
}
