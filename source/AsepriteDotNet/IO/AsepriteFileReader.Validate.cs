
using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet.IO;

public partial class AsepriteFileReader
{
    private static void ValidateStream([NotNull] Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (!stream.CanRead)
        {
            throw new ArgumentException($"{nameof(stream)} is unreadable", nameof(stream));
        }

        if (!stream.CanSeek)
        {
            throw new ArgumentException($"{nameof(stream)} is not seekable", nameof(stream));
        }
    }

    private static void ValidateDisposed(bool disposed)
    {
        if (disposed)
        {
            throw new ObjectDisposedException(nameof(AsepriteFileReader), $"The {nameof(AsepriteFileReader)} has already been disposed");
        }
    }
}