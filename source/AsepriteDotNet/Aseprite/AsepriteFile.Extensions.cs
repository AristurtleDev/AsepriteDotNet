// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Types;

namespace AsepriteDotNet.Aseprite;

/// <summary>
/// Provides extension methods for <see cref="AsepriteFile"/> instances.
/// </summary>
public static class AsepriteFileExtensions
{
    /// <summary>
    /// Extracts pixel data from a specific cel in a specified frame of an <see cref="AsepriteFile"/> and returns it as
    /// a <see cref="Texture"/>.
    /// </summary>
    /// <remarks>
    /// Cel index of a frame starts at zero with the bottom most and goes up.  If a layer in a frame does not have cel
    /// data, then the frame does not have that cel.  For instance, if your Aseprite file has 10 layers, but frame 0
    /// does not have pixels on layer 2, then frame 0 will only have 9 cels and not the full 10 cels.
    ///
    /// Due to this, it may be easier to use the <see cref="ExtractCel(AsepriteFile, int, string, string?)"/> method
    /// to specify the layer name to extract the cel from.
    /// </remarks>
    /// <param name="file">
    /// The <see cref="AsepriteFile"/> containing the frame and cel from which to extract the pixel data.
    /// </param>
    /// <param name="frameIndex">
    /// The index of the frame containing the cel from which to extract the pixel data.
    /// </param>
    /// <param name="celIndex">
    /// The index of the cel within the specified frame from which to extract the pixel data.
    /// </param>
    /// <param name="name">
    /// Optional name for the extracted texture. If not provided, a default name is generated.
    /// </param>
    /// <returns>A <see cref="Texture"/> object containing the extracted pixel data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input <see cref="AsepriteFile"/> is null.</exception>
    public static Texture ExtractCel(this AsepriteFile file, int frameIndex, int celIndex, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        name ??= $"{file.Name}_frame{frameIndex}_cel{celIndex}";
        AsepriteCel cel = file.Frames[frameIndex].Cels[celIndex];
        return cel.ExtractCel(name);

    }

    /// <summary>
    /// Extracts pixel data from a specific layer in a specified frame of an <see cref="AsepriteFile"/> and returns it
    /// as a <see cref="Texture"/>.
    /// </summary>
    /// <param name="file">
    /// The <see cref="AsepriteFile"/> containing the frame and layer from which to extract the pixel data.
    /// </param>
    /// <param name="frameIndex">The index of the frame from which to extract the pixel data.</param>
    /// <param name="layerName">The name of the layer from which to extract the pixel data.</param>
    /// <param name="name">Optional name for the extracted texture. If not provided, a default name is generated.</param>
    /// <returns>A <see cref="Texture"/>. object containing the extracted pixel data.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input <see cref="AsepriteFile"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when the specified layer cannot be located.</exception>
    public static Texture ExtractCel(this AsepriteFile file, int frameIndex, string layerName, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(file);
        name ??= $"{file.Name}_frame{frameIndex}_{layerName}_cel";
        AsepriteCel? cel = null;
        AsepriteFrame frame = file.Frames[frameIndex];
        Parallel.For(0, frame.Cels.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i, loopState) =>
        {
            if (frame.Cels[i].Layer.Name.Equals(layerName, StringComparison.Ordinal))
            {
                cel = frame.Cels[i];
                loopState.Break();
            }
        });

        if (cel is null)
        {
            throw new ArgumentException($"Unable to locate cel in frame {frameIndex} on a layer called '{layerName}'", layerName);
        }

        return cel.ExtractCel(name);
    }
}
