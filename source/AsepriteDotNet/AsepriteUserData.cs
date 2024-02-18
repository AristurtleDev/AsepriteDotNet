//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

namespace AsepriteDotNet;

/// <summary>
/// Represents the custom user data set for an element in Aseprite.
/// </summary>
public sealed class AsepriteUserData
{
    /// <summary>
    /// Gets the text that was set for this user data in Aseprite; if text was set; otherwise, <see langword="null"/>.
    /// </summary>
    public string? Text { get; }

    /// <summary>
    /// Gets the color that was set for this user data in Aseprite, if a color was set; otherwise,
    /// <see langword="null"/>.
    /// </summary>
    public Rgba32? Color { get; }

    internal AsepriteUserData(string? text, Rgba32? color) => (Text, Color) = (text, color);
}
