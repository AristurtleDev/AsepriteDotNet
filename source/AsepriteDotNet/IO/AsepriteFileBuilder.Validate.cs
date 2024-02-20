
namespace AsepriteDotNet.IO;

internal partial class AsepriteFileBuilder
{
    internal static void ValidateFrameCount(int frameCount)
    {
        if (frameCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(frameCount), $"Invalid frame count in file header: {frameCount}.  Must be greater than zero");
        }
    }

    internal static void ValidateCanvasWidth(int width)
    {
        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), $"Invalid canvas height in file header: {width}.  Must be greater than zero");
        }
    }

    internal static void ValidateCanvasHeight(int height)
    {
        if (height < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(height), $"Invalid canvas height in file header: {height}.  Must be greater than zero");
        }
    }

    internal static void ValidateColorDepth(int depth)
    {
        if (!Enum.IsDefined(typeof(AsepriteColorDepth), depth))
        {
            throw new ArgumentException($"Unknown color depth mode in file header: {depth}", nameof(depth));
        }
    }

    internal static void ValidateFileHeaderFlags(int flags)
    {
        if (flags < 0 || flags > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(flags), $"Invalid flags field in file header: {flags}");
        }
    }

    internal static void ValidateTransparentIndex(int transparentIndex)
    {
        if (transparentIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(transparentIndex), $"Invalid palette transparent index field in file header: {transparentIndex}");
        }
    }

    internal static void ValidateNumberOfColors(int nColors)
    {
        if (nColors < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(nColors), $"Invalid number of colors field in file header: {nColors}");
        }
    }
}