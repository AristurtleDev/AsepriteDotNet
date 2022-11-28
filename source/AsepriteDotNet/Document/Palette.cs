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
namespace AsepriteDotNet.Document;

using System.Collections;
using System.Drawing;

/// <summary>
///     Represents the palette of colors in an Aseprite image.
/// </summary>
public sealed class Palette : IEnumerable<AsepriteColor>
{
    private AsepriteColor[] _colors = Array.Empty<AsepriteColor>();

    /// <summary>
    ///     Gets the <see cref="AsepriteColor"/> element at the specified
    ///     <paramref name="index"/> from this <see cref="Palette"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="AsepriteColor"/> element.
    /// </param>
    /// <returns>
    ///     The <see cref="AsepriteColor"/> element at the specified 
    ///     <paramref name="index"/> from this <see cref="Palette"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if <paramref name="index"/> is less than zero or 
    ///     <paramref name="index"/> is equal to or greater than
    ///     <see cref="Count"/>.
    /// </exception>
    public AsepriteColor this[int index]
    {
        get
        {
            if (index < 0 || index >= _colors.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _colors[index];
        }

        internal set
        {
            if (index < 0 || index >= _colors.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _colors[index] = value;
        }
    }

    /// <summary>
    ///     Gets the index of the <see cref="AsepriteColor"/> element in this
    ///     <see cref="Palette"/> that represents a transparent pixel.
    /// </summary>
    /// <remarks>
    ///     This value is only valid when the Asprite image used a color depth
    ///     mode of <see cref="ColorDepth.Indexed"/>.
    /// </remarks>
    public int TransparentIndex { get; internal set; }

    /// <summary>
    ///     Get the total number of <see cref="AsepriteColor"/> elements in this
    ///     <see cref="Palette"/>.
    /// </summary>
    public int Count => _colors.Length;

    internal void Resize(int newSize)
    {
        AsepriteColor[] newColors = new AsepriteColor[newSize];
        Array.Copy(_colors, newColors, _colors.Length);
        _colors = newColors;
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="AsepriteColor"/>
    ///     elements in this <see cref="Palette"/> instance.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through the <see cref="AsepriteColor"/> elements
    ///     in this <see cref="Palette"/> instance.
    /// </returns>
    public IEnumerator<AsepriteColor> GetEnumerator()
    {
        for (int i = 0; i < _colors.Length; i++)
        {
            yield return _colors[i];
        }
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="AsepriteColor"/>
    ///     elements in this <see cref="Palette"/> instance.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through the <see cref="AsepriteColor"/> elements
    ///     in this <see cref="Palette"/> instance.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}