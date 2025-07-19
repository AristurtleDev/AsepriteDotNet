// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.IO;

/// <summary>
/// Represents errors that occur during PNG image processing operations.
/// </summary>
public class PngException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PngException"/> class.
    /// </summary>
    internal PngException() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PngException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the PNG processing error.</param>
    internal PngException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="PngException"/> class with a specified error message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the PNG processing error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    internal PngException(string message, Exception innerException)
        : base(message, innerException) { }
}
