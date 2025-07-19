//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Core;

/// <summary>
/// Defines blend modes for layer composition that control how pixels are mathematically combined during rendering.
/// </summary>
/// <remarks>
/// Blend modes determine the mathematical operations applied when combining layer pixels with underlying layers.
/// </remarks>
public enum AsepriteBlendMode
{
    /// <summary>
    /// Standard alpha blending where the source pixel replaces the destination based on alpha values.
    /// </summary>
    /// <remarks>
    /// Formula: Result = Source × SourceAlpha + Destination × (1 - SourceAlpha)
    /// </remarks>
    Normal = 0,

    /// <summary>
    /// Darkens the image by multiplying source and destination color values.
    /// </summary>
    /// <remarks>
    /// Formula: Result = Source × Destination
    /// </remarks>
    Multiply = 1,

    /// <summary>
    /// Lightens the image by inverting, multiplying, and inverting again.
    /// </summary>
    /// <remarks>
    /// Formula: Result = 1 - (1 - Source) × (1 - Destination)
    /// </remarks>
    Screen = 2,

    /// <summary>
    /// Combines multiply and screen blend modes based on destination brightness.
    /// </summary>
    /// <remarks>
    /// Uses multiply for destination values below 0.5, screen for values above 0.5.
    /// </remarks>
    Overlay = 3,

    /// <summary>
    /// Selects the darker color value between source and destination for each channel.
    /// </summary>
    /// <remarks>
    /// Formula: Result = min(Source, Destination)
    /// </remarks>
    Darken = 4,

    /// <summary>
    /// Selects the lighter color value between source and destination for each channel.
    /// </summary>
    /// <remarks>
    /// Formula: Result = max(Source, Destination)
    /// </remarks>
    Lighten = 5,

    /// <summary>
    /// Brightens the destination based on the source color through division-based calculations.
    /// </summary>
    /// <remarks>
    /// Formula: Result = Destination ÷ (1 - Source)
    /// </remarks>
    ColorDodge = 6,

    /// <summary>
    /// Darkens the destination based on the source color through inverse division.
    /// </summary>
    /// <remarks>
    /// Formula: Result = 1 - (1 - Destination) ÷ Source
    /// </remarks>
    ColorBurn = 7,

    /// <summary>
    /// Combines multiply and screen based on source brightness for dramatic contrast.
    /// </summary>
    /// <remarks>
    /// Uses multiply for source values below 0.5, screen for values above 0.5.
    /// </remarks>
    HardLight = 8,

    /// <summary>
    /// Applies subtle dodge and burn effects based on source brightness.
    /// </summary>
    /// <remarks>
    /// Uses color burn for source values below 0.5, color dodge for values above 0.5.
    /// </remarks>
    SoftLight = 9,

    /// <summary>
    /// Subtracts the smaller value from the larger value for each color channel.
    /// </summary>
    /// <remarks>
    /// Formula: Result = |Source - Destination|
    /// </remarks>
    Difference = 10,

    /// <summary>
    /// Similar to difference but with lower contrast and smoother transitions.
    /// </summary>
    /// <remarks>
    /// Formula: Result = Source + Destination - 2 × Source × Destination
    /// </remarks>
    Exclusion = 11,

    /// <summary>
    /// Preserves the hue of the source while using saturation and luminosity from destination.
    /// </summary>
    /// <remarks>
    /// Converts colors to HSL space, replaces hue component, and converts back to RGB.
    /// </remarks>
    Hue = 12,

    /// <summary>
    /// Preserves the saturation of the source while using hue and luminosity from destination.
    /// </summary>
    /// <remarks>
    /// Converts colors to HSL space, replaces saturation component, and converts back to RGB.
    /// </remarks>
    Saturation = 13,

    /// <summary>
    /// Preserves the hue and saturation of the source while using luminosity from destination.
    /// </summary>
    /// <remarks>
    /// Converts colors to HSL space, replaces hue and saturation components, and converts back to RGB.
    /// </remarks>
    Color = 14,

    /// <summary>
    /// Preserves the luminosity of the source while using hue and saturation from destination.
    /// </summary>
    /// <remarks>
    /// Converts colors to HSL space, replaces luminosity component, and converts back to RGB.
    /// </remarks>
    Luminosity = 15,

    /// <summary>
    /// Adds source and destination color values together with clamping to prevent overflow.
    /// </summary>
    /// <remarks>
    /// Formula: Result = min(Source + Destination, 1.0)
    /// </remarks>
    Addition = 16,

    /// <summary>
    /// Subtracts source color values from destination with clamping to prevent underflow.
    /// </summary>
    /// <remarks>
    /// Formula: Result = max(Destination - Source, 0.0)
    /// </remarks>
    Subtract = 17,

    /// <summary>
    /// Divides destination color values by source color values.
    /// </summary>
    /// <remarks>
    /// Formula: Result = Destination ÷ Source
    /// </remarks>
    Divide = 18
}
