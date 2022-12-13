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
namespace AsepriteDotNet.Image;

/// <summary>
///     Represents a single frame within a spritesheet.
/// </summary>
public sealed class SpritesheetFrame
{
    private Dictionary<string, List<SpritesheetSlice>> _sliceLookup = new();

    /// <summary>
    ///     Gets the bounds of this <see cref="SpritesheetFrame"/> relative to
    ///     the overall spritesheet.
    /// </summary>
    public Rectangle SourceRectangle { get; }

    /// <summary>
    ///     Gets the duration of this <see cref="SpritesheetFrame"/> when used
    ///     in an animation.
    /// </summary>
    public int Duration { get; }

    internal SpritesheetFrame(Rectangle source, int duration)
    {
        SourceRectangle = source;
        Duration = duration;
    }

    /// <summary>
    ///     Retrieves a collection of all <see cref="SpritesheetSlice"/>
    ///     elements in this <see cref="SpritesheetFrame"/>
    /// </summary>
    /// <returns>
    ///     A collection of all <see cref="SpritesheetSlice"/> elements in this
    ///     <see cref="SpritesheetFrame"/>.
    /// </returns>
    public List<SpritesheetSlice> GetSlices()
    {
        List<SpritesheetSlice> slices = new();

        foreach (var slice in _sliceLookup)
        {
            slices.AddRange(slice.Value);
        }

        return slices;
    }

    /// <summary>
    ///     Retrieves a collection of <see cref="SpritesheetSlice"/> elements
    ///     in this <see cref="SpritesheetFrame"/>> that have the specified 
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpritesheetSlice"/> elements to get.
    /// </param>
    /// <returns>
    ///     A new collection containing all <see cref="SpritesheetSlice"/>
    ///     elements with the given name.
    /// </returns>
    public List<SpritesheetSlice> GetSlices(string name)
    {
        List<SpritesheetSlice> slices = new();

        if (!string.IsNullOrEmpty(name) && _sliceLookup.ContainsKey(name))
        {
            slices.AddRange(_sliceLookup[name]);
        }

        return slices;
    }

    internal void AddSlice(SpritesheetSlice slice)
    {
        if (_sliceLookup.ContainsKey(slice.Name))
        {
            _sliceLookup[slice.Name].Add(slice);
        }
        else
        {
            _sliceLookup.Add(slice.Name, new List<SpritesheetSlice>() { slice });
        }
    }

    internal SpritesheetFrame CreateCopy()
    {
        SpritesheetFrame frame = new(SourceRectangle, Duration);
        return frame;
    }
}