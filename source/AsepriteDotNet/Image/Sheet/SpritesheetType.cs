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
namespace AsepriteDotNet.Image.Sheet;

public enum SpritesheetPackingMethod
{
    /// <summary>
    ///     Defines that the spritesheet should use a horizontal packing method
    ///     where there is only 1 row and each frame is contained on a unique
    ///     column.
    /// </summary>
    HorizontalStrip = 0,

    /// <summary>
    ///     Defines that the spritesheet should use a vertical strip packing
    ///     method where there is only 1 column and each frame is contained on
    ///     a unique row.
    /// </summary>
    VerticalStrip = 1,

    /// <summary>
    ///     Defines that the spritesheet should use a square packing method 
    ///     where where an equal number of columns and rows is created.
    /// </summary>
    SquarePacked = 2
}