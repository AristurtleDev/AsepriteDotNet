//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information

using System.Diagnostics.CodeAnalysis;

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of custom user data of an element in an Aseprite file.  This class cannot be inherited.
/// </summary>
public sealed class UserData
{
    /// <summary>
    /// Gets a value that indicates whether this user data contains a value for the <see cref="Text"/> property.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Text))]
    public bool HasText => Text is not null;

    /// <summary>
    /// Gets a value that indicates whether this user data contains a value for the <see cref="Color"/> property.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Color))]
    public bool HasColor => Color is not null;

    /// <summary>
    /// Gets the text that was set for this user data in Aseprite.
    /// </summary>
    public string? Text { get; internal set; }

    /// <summary>
    /// Gets the color that was set for this user data in Aseprite.
    /// </summary>
    public AseColor? Color { get; internal set; }

    internal UserData()
    {
        Text = null;
        Color = null;
    }
}
