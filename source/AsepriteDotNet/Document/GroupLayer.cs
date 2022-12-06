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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a group layer that contains child layers in an Aseprite
///     image.
/// </summary>
public sealed class GroupLayer : Layer, IEnumerable<Layer>
{
    private readonly List<Layer> _children;

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Layer"/> elements that
    ///     are children of this <see cref="GroupLayer"/>.
    /// </summary>
    public ReadOnlyCollection<Layer> Children { get; }

    internal GroupLayer(bool isVisible, bool isBackground, bool isReference, int childLevel, BlendMode blend, int opacity, string name)
        : base(isVisible, isBackground, isReference, childLevel, blend, opacity, name)
    {
        _children = new();
        Children = _children.AsReadOnly();
    }

    internal void AddChild(Layer layer)
    {
        Debug.Assert(layer is not GroupLayer, "Cannot add group layer as child");
        _children.Add(layer);
    }

    /// <summary>
    ///     Returns an enumerator that itereates through the child
    ///     <see cref="Layer"/> elements in this <see cref="GroupLayer"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that itereates through the child <see cref="Layer"/>
    ///     elements in this <see cref="GroupLayer"/>.
    /// </returns>
    public IEnumerator<Layer> GetEnumerator() => _children.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator that itereates through the child
    ///     <see cref="Layer"/> elements in this <see cref="GroupLayer"/>.
    /// </summary>
    /// <returns>
    ///     An enumerator that itereates through the child <see cref="Layer"/>
    ///     elements in this <see cref="GroupLayer"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => _children.GetEnumerator();
}