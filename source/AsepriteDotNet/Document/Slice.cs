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
using System.Collections;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a slice section in an Aseprite image.
/// </summary>
public class Slice : IUserData, IEnumerable<SliceKey>
{
    private List<SliceKey> _keys = new();

    /// <summary>
    ///     Gets whether the <see cref="SliceKey"/> elements of this
    ///     <see cref="Slice"/> are 9-patch.
    /// </summary>
    public bool IsNinePatch { get; }

    /// <summary>
    ///     Gets whether the <see cref="SliceKey"/> elements of this
    ///     <see cref="Slice"/> have pivot information.
    /// </summary>
    public bool HasPivot { get; }

    /// <summary>
    ///     Gets the name of this <see cref="Slice"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="SliceKey"/> element from this
    ///     <see cref="Slice"/> at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="SliceKey"/> in this slice.
    /// </param>
    /// <returns>
    ///     The <see cref="SliceKey"/> at the specified index.
    /// </returns>
    public SliceKey this[int index]
    {
        get
        {
            return _keys[index];
        }
    }

    /// <summary>
    ///     Gets the total number of <see cref="SliceKey"/> elements in this
    ///     <see cref="Slice"/>.
    /// </summary>
    public int Count => _keys.Count;

    /// <summary>
    ///     Gets the <see cref="UserData"/> for this <see cref="Slice"/>.
    /// </summary>
    public UserData UserData { get; internal set; } = new();

    internal Slice(bool isNinePatch, bool hasPivot, string name) =>
        (IsNinePatch, HasPivot, Name) = (isNinePatch, hasPivot, name);

    internal void AddKey(SliceKey key) => _keys.Add(key);

    /// <summary>
    ///     Returns an enumerator that itereates through each
    ///     <see cref="SliceKey"/> element in this <see cref="Slice"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that itereates through each <see cref="SliceKey"/>
    ///     element in this <see cref="Slice"/>.
    /// </returns>
    public IEnumerator<SliceKey> GetEnumerator() => _keys.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator that itereates through each
    ///     <see cref="SliceKey"/> element in this <see cref="Slice"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that itereates through each <see cref="SliceKey"/>
    ///     element in this <see cref="Slice"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => _keys.GetEnumerator();
}