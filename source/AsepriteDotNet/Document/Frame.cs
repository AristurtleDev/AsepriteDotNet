/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Collections;
using System.Collections.ObjectModel;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a frame in an Aseprite image.
/// </summary>
public sealed class Frame : IEnumerable<Cel>
{
    private readonly List<Cel> _cels;

    /// <summary>
    ///     Gets the duration, in milliseconds, of this <see cref="Frame"/> when
    ///     used as part of an animation.
    /// </summary>
    public int Duration { get; set; } = 100;

    public Cel this[int index]
    {
        get => _cels[index];
    }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Cel"/> instances in
    ///     this <see cref="Frame"/>.
    /// </summary>
    public ReadOnlyCollection<Cel> Cels { get; }

    internal Frame()
    {
        _cels = new();
        Cels = _cels.AsReadOnly();
    }

    internal void AddCel(Cel cel) => _cels.Add(cel);


    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="Cel"/>
    ///     elements in this <see cref="Frame"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through the <see cref="Cel"/> elements
    ///     in this <see cref="Frame"/>.
    /// </returns>
    public IEnumerator<Cel> GetEnumerator() => _cels.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="Cel"/>
    ///     elements in this <see cref="Frame"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through the <see cref="Cel"/> elements
    ///     in this <see cref="Frame"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => _cels.GetEnumerator();
}