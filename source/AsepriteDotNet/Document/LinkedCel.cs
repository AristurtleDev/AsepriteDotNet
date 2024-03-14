//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Document;

/// <summary>
/// Defines the properties of an Aseprite cel that is linked to another cel.
/// </summary>
public sealed class LinkedCel : Cel
{
    /// <summary>
    /// Gets the cel that this linked cel is linked to
    /// </summary>
    public Cel Cel { get; }

    internal LinkedCel(CelProperties celProperties, Cel otherCel) : base(celProperties, otherCel.Layer)
    {
        Cel = otherCel;
    }
}
