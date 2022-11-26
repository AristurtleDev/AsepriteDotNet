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

public class Slice : IUserData, IEnumerable<SliceKey>
{
    private List<SliceKey> _keys = new();

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Slice"/> and its keys are nine patch.
    /// </summary>
    public bool IsNinePatch { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="bool"/> value that indicates whether this
    ///     <see cref="Slice"/> and it's keys have pivot information.
    /// </summary>
    public bool HasPivot { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="string"/> containing the name of this
    ///     <see cref="Slice"/>.
    /// </summary>
    public string Name { get; set; } = "Slice";

    public UserData UserData { get; set; } = new();

    /// <summary>
    ///     Creates a new instance of the <see cref="Slice"/> class.
    /// </summary>
    public Slice() { }

    /// <summary>
    ///     Adds the given <paramref name="key"/> to this <see cref="Slice"/>.
    /// </summary>
    /// <param name="key">
    ///     The <see cref="SliceKey"/> to add.
    /// </param>
    /// <returns>
    ///     Returns <see langword="true"/> if the given <paramref name="key"/>
    ///     was added successfully.  If the <paramref name="key"/>  has
    ///     already been added, then this will return <see langword="false"/>.
    /// </returns>
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