/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
using System.Collections.ObjectModel;

namespace AsepriteDotNet.Image;

/// <summary>
///     Represents the definition for an animation in a spritesheet.
/// </summary>
public sealed class SpritesheetAnimation
{
    private List<SpritesheetFrame> _frames;

    /// <summary>
    ///     Gets a read-only collection of the <see cref="SpritesheetFrame"/>
    ///     elements that make up this animation.  The order of the frames is
    ///     from start to the end of the animation.
    /// </summary>
    public ReadOnlyCollection<SpritesheetFrame> Frames { get; }

    /// <summary>
    ///     Gets the name of this animation.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="LoopDirection"/> used by this animation.
    /// </summary>
    public LoopDirection Direction { get; }

    internal SpritesheetAnimation(List<SpritesheetFrame> frames, string name, LoopDirection direction)
    {
        _frames = frames;
        Frames = frames.AsReadOnly();
        Name = name;
        Direction = direction;
    }
}