//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet;

/// <summary>
/// Represents the custom user data set for an element in Aseprite.
/// </summary>
public sealed class AsepriteUserData
{
    /// <summary>
    /// Gets a value that indicates whether text was set for this <see cref="AsepriteUserData"/> in Aseprite.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Text))]
    public bool HasText => Text is not null;

    /// <summary>
    /// Gets a value that indicates whether a color value was set for this <see cref="AsepriteUserData"/> in Aseprite.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Color))]
    public bool HasColor => Color is not null;

    /// <summary>
    /// Gets the text that was set for this user data in Aseprite; if text was set; otherwise, <see langword="null"/>.
    /// </summary>
    public string? Text { get; internal set; }

    /// <summary>
    /// Gets the color that was set for this user data in Aseprite, if a color was set; otherwise,
    /// <see langword="null"/>.
    /// </summary>
    public AseColor? Color { get; internal set; }

    internal AsepriteUserData()
    {
        Text = null;
        Color = null;
    }
}
