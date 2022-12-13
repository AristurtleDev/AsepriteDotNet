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
using AsepriteDotNet.IO.Image;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Document;

/// <summary>
///     Represents a frame in an Aseprite image.
/// </summary>
public sealed class Frame : IEnumerable<Cel>
{
    private readonly List<Cel> _cels;

    /// <summary>
    ///     Gets the width and height of this <see cref="Frame"/>.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    ///     Gets the duration, in milliseconds, of this <see cref="Frame"/> when
    ///     used as part of an animation.
    /// </summary>
    public int Duration { get; }

    /// <summary>
    ///     Gets the <see cref="Cel"/> in this frame at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Cel"/> to retrieve.
    /// </param>
    /// <returns>
    ///     The <see cref="Cel"/> at the specified index in this 
    ///     <see cref="Frame"/>.
    /// </returns>
    public Cel this[int index]
    {
        get => _cels[index];
    }

    /// <summary>
    ///     Gets a read-only collection of all <see cref="Cel"/> instances in
    ///     this <see cref="Frame"/>.
    /// </summary>
    public ReadOnlyCollection<Cel> Cels { get; }

    internal Frame(int duration, List<Cel> cels, Size size)
    {
        Duration = duration;
        _cels = cels;
        Cels = _cels.AsReadOnly();
        Size = size;
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

    IEnumerator IEnumerable.GetEnumerator() => _cels.GetEnumerator();

    /// <summary>
    ///     Flattens this <see cref="Frame"/> by blending each <see cref="Cel"/>
    ///     staring with the top most <see cref="Cel"/> and blending down.  Te
    ///     result is an <see cref="Array"/> of <see cref="Color"/> elements
    ///     representing the final flattened image of this <see cref="Frame"/>.
    /// </summary>
    /// <param name="onlyVisibleLayers">
    ///     Whether only the <see cref="Cel"/> elements that are on a
    ///     <see cref="Layer"/> that is visible should be included.
    /// </param>
    /// <returns>
    ///     A new <see cref="Array"/> of <see cref="Color"/> elements that
    ///     represent the flattened image of this <see cref="Frame"/>.
    /// </returns>
    public Color[] FlattenFrame(bool onlyVisibleLayers = true)
    {
        Color[] result = new Color[Size.Width * Size.Height];

        for (int celNum = 0; celNum < Cels.Count; celNum++)
        {
            Cel cel = Cels[celNum];

            if (onlyVisibleLayers && !cel.Layer.IsVisible)
            {
                continue;
            }

            ImageCel imageCel;

            if (cel is ImageCel)
            {
                imageCel = (ImageCel)cel;
            }
            else if (cel is LinkedCel linkedCel && linkedCel.Cel is ImageCel)
            {
                imageCel = (ImageCel)linkedCel.Cel;
            }
            else
            {

                continue;
            }

            byte opacity = Color.MUL_UN8(imageCel.Opacity, imageCel.Layer.Opacity);

            for (int pixelNum = 0; pixelNum < imageCel.Pixels.Length; pixelNum++)
            {
                int x = (pixelNum % imageCel.Size.Width) + imageCel.Position.X;
                int y = (pixelNum / imageCel.Size.Width) + imageCel.Position.Y;
                int index = y * Size.Width + x;

                //  Sometimes a cell can have a negative x and/or y value. This
                //  is caused by selecting an area within aseprite and then 
                //  moving a portion of the selected pixels outside the canvas. 
                //  We don't care about these pixels so if the index is outside
                //  the range of the array to store them in then we'll just
                //  ignore them.
                if (index < 0 || index >= result.Length) { continue; }

                Color backdrop = result[index];
                Color source = imageCel.Pixels[pixelNum];
                result[index] = Color.Blend(imageCel.Layer.BlendMode, backdrop, source, opacity);
            }
        }

        return result;
    }

    /// <summary>
    ///     Writes the pixel data for this <see cref="Frame"/> to disk as a .png
    ///     file.
    /// </summary>
    /// <param name="path">
    ///     The absolute file path to save the generated .png file to.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Whether only the <see cref="Cel"/> elements that are on a 
    ///     <see cref="Layer"/> that is visible should be included.
    /// </param>
    public void ToPng(string path, bool onlyVisibleLayers = true)
    {
        Color[] frame = FlattenFrame(onlyVisibleLayers);
        PngWriter.SaveTo(path, Size, frame);
    }
}