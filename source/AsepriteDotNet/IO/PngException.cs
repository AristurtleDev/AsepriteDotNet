// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.IO;

/// <summary>
/// Represents an exception that is thrown when saving data as a PNG file.
/// </summary>
/// <remarks>
/// This acts as a top-level exception wrapper around the different
/// exceptions that can be thrown when saving data as a PNG file.  Refer to
/// the inner exception for details on the cause of the error.
/// </remarks>
public class PngException : Exception
{
    internal PngException():base() {}
    internal PngException(string message):base(message) {}
    internal PngException(string message, Exception innerException)
        : base(message, innerException) { }
}
