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

namespace AsepriteDotNet.Document;

public class GroupLayer : Layer, IEnumerable<Layer>
{
    private readonly List<Layer> _children = new();
    private readonly HashSet<Layer> _childLookup = new();
    /// <summary>
    ///     Gets the child level of this <see cref="Layer"/>.
    /// </summary>
    /// <remarks>
    ///     A <see cref="GroupLayer"/> will always have a child level of
    ///     zero since it cannot be added as a child of another group.
    /// </remarks>
    public override int ChildLevel
    {
        get => base.ChildLevel;
        internal set => throw new InvalidOperationException();
    }

    /// <summary>
    ///     Gets a <see cref="ReadOnlyCollection{T}"/> of this child
    ///     <see cref="Layer"/> elements of this <see cref="GroupLayer"/>.
    /// </summary>
    public ReadOnlyCollection<Layer> Children { get; }


    /// <summary>
    ///     Initializes a new instance of the <see cref="GroupLayer"/> class.
    /// </summary>
    public GroupLayer()
    {
        Children = _children.AsReadOnly();
    }

    /// <summary>
    ///     Adds the given <paramref name="layer"/> as a child of this
    ///     <see cref="GroupLayer"/>.
    /// </summary>
    /// <remarks>
    ///     A <see cref="GroupLayer"/> instnace cannot be added as a child
    ///     of another <see cref="GroupLayer"/>.
    /// </remarks>
    /// <param name="layer">
    ///     The <see cref="Layer"/> to add.
    /// </param>
    /// <returns>
    ///     Returns <see langword="true"/> if the given <paramref name="layer"/>
    ///     was added successfully.  If the <paramref name="layer"/> is a
    ///     <see cref="GroupLayer"/>, or if the <paramref name="layer"/> has
    ///     already been added, then this will return <see langword="false"/>.
    /// </returns>
    public bool AddChild(Layer layer)
    {
        if (layer is not GroupLayer && !_childLookup.Contains(layer))
        {
            _children.Add(layer);
            _childLookup.Add(layer);
            layer.ChildLevel = _children.Count - 1;
            return true;

        }

        return false;
    }

    /// <summary>
    ///     Removes the specified <paramref name="layer"/> from this
    ///     <see cref="GroupLayer"/>.
    /// </summary>
    /// <param name="layer">
    ///     The <see cref="Layer"/> to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="layer"/> was removed
    ///     successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public bool RemoveChild(Layer layer)
    {
        if (layer is not GroupLayer && _childLookup.Contains(layer))
        {
            _children.Remove(layer);
            _childLookup.Remove(layer);
            layer.ChildLevel = 0;
            return true;
        }

        return false;
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