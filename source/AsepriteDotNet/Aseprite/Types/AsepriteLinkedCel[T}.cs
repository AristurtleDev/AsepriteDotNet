//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Document;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of an Aseprite cel that is linked to another cel.
/// </summary>
public sealed class AsepriteLinkedCel<TColor> : AsepriteCel<TColor> where TColor : struct, IColor<TColor>
{
    /// <summary>
    /// Gets the cel that this linked cel is linked to
    /// </summary>
    public AsepriteCel<TColor> Cel { get; }

    internal AsepriteLinkedCel(AsepriteCelProperties celProperties, AsepriteCel<TColor> otherCel) : base(celProperties, otherCel.Layer)
    {
        Cel = otherCel;
    }
}
