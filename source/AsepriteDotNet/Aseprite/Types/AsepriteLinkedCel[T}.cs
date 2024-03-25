//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of an Aseprite cel that is linked to another cel.
/// </summary>
public sealed class AsepriteLinkedCel<T> : AsepriteCel<T> where T: IColor, new()
{
    /// <summary>
    /// Gets the cel that this linked cel is linked to
    /// </summary>
    public AsepriteCel<T> Cel { get; }

    internal AsepriteLinkedCel(AsepriteCelProperties celProperties, AsepriteCel<T> otherCel) : base(celProperties, otherCel.Layer)
    {
        Cel = otherCel;
    }
}
