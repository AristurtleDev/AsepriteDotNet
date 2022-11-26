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
using System.Drawing;

namespace AsepriteDotNet.Document;

public class SliceKey
{
    private Rectangle _center;
    private Point _pivot;

    /// <summary>
    ///     Gets the <see cref="Slice"/> this <see cref="SliceKey"/> belongs
    ///     too.
    /// </summary>
    public Slice Slice { get; }

    /// <summary>
    ///     Gets or Sets an <see cref="int"/> value that indicates the frame
    ///     number this <see cref="SliceKey"/> is valid for starting from to
    ///     the end fo the animation.
    /// </summary>
    public int Frame { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="Rectangle"/> value that defines the bounds
    ///     of this <see cref="SliceKey"/>
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    ///     Gets or Sets a <see cref="Rectangle"/> value that defines the bounds
    ///     of the nine patch center rectangle of this <see cref="SliceKey"/>,
    ///     or <see langword="null"/> if it is not a nine patch slice.
    /// </summary>
    public Rectangle CenterBounds
    {
        get => Slice.IsNinePatch ? _center : Bounds;

        set
        {
            if(Slice.IsNinePatch)
            {
                _center = value;
            }
        }
    }

    /// <summary>
    ///     Gets or Sets a <see cref="Pivot"/> value that defines the pivot
    ///     point of this <see cref="SliceKey"/> relative to the origin, or
    ///     <see langword="null"/> if it has no pivot information.
    /// </summary>
    /// <remarks>
    ///     The origin in this context is always the X and Y components of the
    ///     <see cref="Bounds"/> value.
    /// </remarks>
    public Point Pivot
    {
        get => Slice.HasPivot ? _pivot : new(0, 0);
        set
        {
            if(Slice.HasPivot)
            {
                _pivot = value;
            }
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SliceKey"/> class.
    /// </summary>
    public SliceKey(Slice slice)
    {
        Slice = slice;
        slice.AddKey(this);
    }
}