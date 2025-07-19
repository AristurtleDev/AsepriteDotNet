using System.Drawing;

namespace AsepriteDotNet.Core;

/// <summary>
/// Represents rendered pixel data and dimensions from Aseprite content.
/// </summary>
/// <remarks>
/// This class contains the final rendered output, whether from extracting a single cel's pixel data
/// or from flattening multiple cels into a composite frame image.
/// </remarks>
public sealed class RenderedImage
{
    private readonly Rgba32[] _pixels;

    /// <summary>
    /// Gets an empty <see cref="RenderedImage"/> instance with zero dimensions and no pixel data.
    /// </summary>
    public static RenderedImage Empty { get; } = new RenderedImage(Size.Empty, []);

    /// <summary>
    /// Gets the size of the image in pixels.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets the pixel data ordered from left-to-right, top-to-bottom.
    /// </summary>
    /// <value>
    /// The total number of pixels equals <see cref="Size.Width"/> × <see cref="Size.Height"/>.
    /// </value>
    public ReadOnlySpan<Rgba32> Pixels => _pixels;

    /// <summary>
    /// Gets a value indicating whether this image contains no pixel data.
    /// </summary>
    public bool IsEmpty => Size.IsEmpty || _pixels.Length == 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderedImage"/> class.
    /// </summary>
    /// <param name="size">The dimensions of the image in pixels.</param>
    /// <param name="pixels">The pixel data for the image.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the pixel array length does not match the expected size (width × height).
    /// </exception>
    public RenderedImage(Size size, Rgba32[] pixels)
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
    /// Gets the pixel at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate (0-based from left).</param>
    /// <param name="y">The y-coordinate (0-based from top).</param>
    /// <returns>The pixel color at the specified position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the coordinates are outside the image bounds.
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
    /// Attempts to get the pixel at the specified coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate (0-based from left).</param>
    /// <param name="y">The y-coordinate (0-based from top).</param>
    /// <param name="pixel">When this method returns <see langword="true"/>, contains the pixel color; otherwise, a default value.</param>
    /// <returns><see langword="true"/> if the coordinates are within bounds; otherwise, <see langword="false"/>.</returns>
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
    /// <returns>A new array containing a copy of all pixel data.</returns>
    public Rgba32[] ToArray() => _pixels.ToArray();

    /// <summary>
    /// Deconstructs this image into its size and pixel data.
    /// </summary>
    /// <param name="size">The size of the image.</param>
    /// <param name="pixels">The pixel data of the image.</param>
    public void Deconstruct(out Size size, out ReadOnlySpan<Rgba32> pixels)
    {
        size = Size;
        pixels = Pixels;
    }
}
