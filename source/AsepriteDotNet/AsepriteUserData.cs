//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Drawing;

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
    public Color? Color { get; }

    internal AsepriteUserData(string? text, Color? color) => (Text, Color) = (text, color);
}
