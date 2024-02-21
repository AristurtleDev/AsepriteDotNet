//  Copyright (c) Christopher Whitley. All rights reserved.
//  Licensed under the MIT license.
//  See LICENSE file in the project root for full license information.

namespace AsepriteDotNet;

/// <summary>
/// Represents a single frame in an Aseprite file.
/// </summary>
public sealed class AsepriteFrame
{
    private readonly AsepriteCel[] _cels;

    /// <summary>
    /// The collection of <see cref="AsepriteCel"/> elements that make up this <see cref="AsepriteFrame"/>.  The order
    /// of <see cref="AsepriteCel"/> elements are from bottom most layer to top most layer within the
    /// <see cref="AsepriteFrame"/>
    /// </summary>
    public ReadOnlySpan<AsepriteCel> Cels => _cels;

    /// <summary>
    /// The name of this <see cref="AsepriteFrame"/>.
    /// The name of this frame.  This name is autogenerated based on the name of the Aseprite file.
    /// </summary>
    /// <remarks>
    /// Frames do not natively have a name in Aseprite.  A name is autogenerated for each <see cref="AsepriteFrame"/>
    /// when the AsepriteFile is parsed based on the name of the Aseprite file, without extension, appended with the
    /// zero-based index of the frame. (e.g. sprite0).
    /// </remarks>
    public string Name { get; }

    /// <summary>
    /// Gets the width of this <see cref="AsepriteFrame"/>, in pixels.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of this <see cref="AsepriteFrame"/>, in pixels.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// The duration that this <see cref="AsepriteFrame"/> should be displayed when used as part of an animation.
    /// </summary>
    public TimeSpan Duration { get; }

    internal AsepriteFrame(string name, int width, int height, int duration, List<AsepriteCel> cels)
    {
        Name = name;
        Width = width;
        Height = height;
        Duration = TimeSpan.FromMilliseconds(duration);
        _cels = [.. cels];
    }
}

