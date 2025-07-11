using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace AsepriteDotNet.Core;

/// <summary>
/// Represents the result of extracting pixel data from a cel, including its size and pixel array.
/// </summary>
public readonly struct ImageData : IEquatable<ImageData>
{
    private readonly Rgba32[] _pixels;

    public static readonly ImageData Empty;

    /// <summary>
    /// Gets the size of the extracted cel in pixels.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets the pixel data for the extracted cel, ordered from left-to-right, top-to-bottom.
    /// </summary>
    public ReadOnlySpan<Rgba32> Pixels => _pixels;

    public bool IsEmpty => Equals(Empty);

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> struct.
    /// </summary>
    /// <param name="size">The size of the extracted cel in pixels.</param>
    /// <param name="pixels">The pixel data for the extracted cel.</param>
    public ImageData(Size size, Rgba32[] pixels)
    {
        Size = size;
        _pixels = pixels;
    }

    /// <summary>
    /// Deconstructs this result into its size and pixel data.
    /// </summary>
    /// <param name="size">The size of the extracted cel.</param>
    /// <param name="pixels">The pixel data for the extracted cel.</param>
    public void Deconstruct(out Size size, out ReadOnlySpan<Rgba32> pixels)
    {
        size = Size;
        pixels = Pixels;
    }

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ImageData other && Equals(other);

    /// <inheritdoc />
    public bool Equals(ImageData other) => Pixels.SequenceEqual(other.Pixels) &&
                                                  Size.Equals(other.Size);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(_pixels, Size);

    /// <summary>
    /// Determines whether two <see cref="ImageData"/> are equal.
    /// </summary>
    /// <param name="lhs">The first <see cref="ImageData"/> to compare.</param>
    /// <param name="rhs">The second <see cref="ImageData"/> to compare.</param>
    /// <returns><see langword="true"/> if the colors are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ImageData lhs, ImageData rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Determines whether two <see cref="ImageData"/> are not equal.
    /// </summary>
    /// <param name="lhs">The first <see cref="ImageData"/> to compare.</param>
    /// <param name="rhs">The second <see cref="ImageData"/> to compare.</param>
    /// <returns><see langword="true"/> if the colors are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ImageData lhs, ImageData rhs) => !lhs.Equals(rhs);
}
