//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Aseprite.Types;

/// <summary>
/// Defines the properties of a frame in an Aseprite file.  This class cannot be inherited.
/// </summary>
public sealed class AsepriteFrame
{
    private readonly List<AsepriteCel> _cels;

    /// <summary>
    /// Gets the underlying collection of cels elements that are contained within this frame.
    /// Cel elements are ordered from bottom most layer to top most layer within the frame.
    /// </summary>
    public ReadOnlySpan<AsepriteCel> Cels => CollectionsMarshal.AsSpan(_cels);

    /// <summary>
    /// Gets the name of this frame.
    /// </summary>
    /// <remarks>
    /// Frames do not natively have a name in Aseprite.  A name is auto generated for each <see cref="AsepriteFrame"/>
    /// when the AsepriteFile is parsed based on the name of the Aseprite file, without extension, appended with the
    /// zero-based index of the frame. (e.g. sprite0).
    /// </remarks>
    public string Name { get; }

    /// <summary>
    /// Gets the size of this frame, in pixels.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Gets the amount of time that this frame should be displayed when used as part of an animation.
    /// </summary>
    public TimeSpan Duration { get; }

    internal AsepriteFrame(string name, int width, int height, int duration, List<AsepriteCel> cels)
    {
        Name = name;
        Size = new Size(width, height);
        Duration = TimeSpan.FromMilliseconds(duration);
        _cels = cels;
    }

    internal void AddCel(AsepriteCel cel) => _cels.Add(cel);
}
