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
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Image;

public class Packer
{
    public enum PackMethod
    {
        HorizontalStrip,
        VerticalStrip,
        Box
    }

    private readonly Size _frameSize;
    private readonly int _frameLen;
    private List<Color[]> _frames = new();
    private bool _mergeDuplicates;
    private int _borderPadding;
    private int _borderSpacing;
    private int _innerPadding;


    public Packer(Size frameSize)
    {
        if (frameSize.Width <= 0)
        {
            throw new ArgumentException(nameof(frameSize));
        }

        if (frameSize.Height <= 0)
        {
            throw new ArgumentException(nameof(frameSize));
        }


        _frameSize = frameSize;
        _frameLen = _frameSize.Width * _frameSize.Height;
    }

    public int AddFrame(Color[] pixels)
    {
        if (pixels.Length != _frameLen)
        {
            throw new InvalidOperationException();
        }

        if (_mergeDuplicates)
        {
            for (int i = 0; i < _frames.Count; i++)
            {
                if (_frames[i].SequenceEqual(pixels))
                {
                    return i;
                }
            }
        }

        _frames.Add(pixels);
        return _frames.Count - 1;
    }

    public void Pack(PackMethod method)
    {
        int width, height;
        int columns, rows;

        if (method == PackMethod.HorizontalStrip)
        {
            columns = _frames.Count;
            rows = 1;
        }
        else if (method == PackMethod.VerticalStrip)
        {
            columns = 1;
            rows = _frames.Count;
        }
        else
        {
            //  To get the total amount of columns and rows needed, we'll use
            //  a super basic packing method.
            //
            // https://en.wikipedia.org/wiki/Square_packing_in_a_square            
            double sqrt = Math.Sqrt(_frames.Count);
            columns = (int)Math.Floor(sqrt);
            if (Math.Abs(sqrt % 1) >= double.Epsilon)
            {
                columns++;
            }

            rows = _frames.Count / columns;
            if (_frames.Count % columns != 0)
            {
                rows++;
            }

            width = (columns * _frameSize.Width) +
                     (_borderPadding * 2) +
                     (_borderSpacing * (columns - 1)) +
                     (_innerPadding * 2 * columns);

            height = (rows * _frameSize.Height) +
                     (_borderPadding * 2) +
                     (_borderSpacing * (rows - 1)) +
                     (_innerPadding * 2 * rows);

        }
    }


}