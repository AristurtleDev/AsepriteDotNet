//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a <see cref="AsepriteCel"/> that is linked to another <see cref="AsepriteCel"/>.
/// </summary>
public sealed class AsepriteLinkedCel : AsepriteCel
{
    /// <summary>
    /// The <see cref="AsepriteCel"/> that this <see cref="AsepriteLinkedCel"/> is linked to.
    /// </summary>
    public AsepriteCel Cel { get; }

    internal AsepriteLinkedCel(AsepriteCel cel) : base(cel.Layer, cel.Position, cel.Opacity, cel.UserData)
    {
        Cel = cel;
    }
}
