// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Drawing;

namespace AsepriteDotNet;

/// <summary>
/// Represents a rendered image with RGBA pixel data stored in a linear array.
/// </summary>
/// <remarks>
/// Pixels are stored in row-major order where each row is stored sequentially.
/// The coordinate system uses (0,0) at the top-left corner with positive X extending right and positive Y extending down.
/// </remarks>
public sealed class RenderedImage
{
    private readonly Rgba32[] _pixels;

    /// <summary>
    /// Gets an empty rendered image with zero dimensions and no pixel data.
    /// </summary>
    public static RenderedImage Empty { get; } = new RenderedImage(Size.Empty, []);

    /// <summary>
    /// Gets the dimensions of the image in pixels.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets the pixel data as a read-only span.
    /// </summary>
    public ReadOnlySpan<Rgba32> Pixels => _pixels;

    /// <summary>
    /// Gets a value indicating whether this image contains no pixel data.
    /// </summary>
    /// <value><c>true</c> if the image has zero area or no pixels; otherwise, <c>false</c>.</value>
    public bool IsEmpty => Size.IsEmpty || _pixels.Length == 0;

    internal RenderedImage(Size size, Rgba32[] pixels)
    {
        ArgumentNullException.ThrowIfNull(pixels);

        int expectedLength = size.Width * size.Height;
        if (pixels.Length != expectedLength)
        {
            throw new ArgumentException(
                $"Pixel array length ({pixels.Length}) does not match expected size ({expectedLength}) for dimensions {size.Width}×{size.Height}.",
                nameof(pixels));
        }

        Size = size;
        _pixels = pixels;
    }

    /// <summary>
    /// Gets the pixel color at the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate (0-based, left to right).</param>
    /// <param name="y">The Y coordinate (0-based, top to bottom).</param>
    /// <returns>The <see cref="Rgba32"/> color value at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="x"/> or <paramref name="y"/> is negative or exceeds the image bounds.
    /// </exception>
    public Rgba32 GetPixel(int x, int y)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(x, Size.Width);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(y, Size.Height);

        return _pixels[y * Size.Width + x];
    }

    /// <summary>
    /// Attempts to get the pixel color at the specified coordinates.
    /// </summary>
    /// <param name="x">The X coordinate (0-based, left to right).</param>
    /// <param name="y">The Y coordinate (0-based, top to bottom).</param>
    /// <param name="pixel">When this method returns, contains the pixel color if the coordinates are valid; otherwise, the default value.</param>
    /// <returns><c>true</c> if the coordinates are within the image bounds; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method provides bounds-safe pixel access without throwing exceptions for invalid coordinates.
    /// </remarks>
    public bool TryGetPixel(int x, int y, out Rgba32 pixel)
    {
        pixel = default;

        if (x < 0 || y < 0 || x >= Size.Width || y >= Size.Height)
        {
            return false;
        }

        pixel = _pixels[y * Size.Width + x];
        return true;
    }

    /// <summary>
    /// Creates a copy of the pixel data as a new array.
    /// </summary>
    /// <returns>A new array containing a copy of all pixel data in row-major order.</returns>
    public Rgba32[] ToArray() => _pixels.ToArray();

    /// <summary>
    /// Deconstructs this image into its size and pixel data components.
    /// </summary>
    /// <param name="size">When this method returns, contains the image dimensions.</param>
    /// <param name="pixels">When this method returns, contains the pixel data as a read-only span.</param>
    public void Deconstruct(out Size size, out ReadOnlySpan<Rgba32> pixels)
    {
        size = Size;
        pixels = Pixels;
    }
}
