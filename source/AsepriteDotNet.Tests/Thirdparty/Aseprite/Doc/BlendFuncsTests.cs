// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2022 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */
// using System.Drawing;

// using AsepriteDotNet.Document;
// using AsepriteDotNet.ThirdParty.Aseprite.Doc;

// namespace AsepriteDotNet.Tests;

// public sealed class BlendFuncsTests
// {
//     private const int LEN = 16;
//     private const int OPACITY = 255;
//     private readonly static Color _blue = Color.FromArgb(255, 1, 136, 165);
//     private readonly static Color _yelw = Color.FromArgb(255, 255, 208, 128);
//     private readonly static Color _tran = Color.FromArgb(0, 0, 0, 0);

//     //  4x4 color array
//     //
//     //  x = _blue
//     //
//     //  [x, x, x, x]
//     //  [x, x, x, x]
//     //  [x, x, x, x]
//     //  [x, x, x, x]
//     private readonly static Color[] _bottomLayer = new Color[]
//     {
//         _blue, _blue, _blue, _blue,
//         _blue, _blue, _blue, _blue,
//         _blue, _blue, _blue, _blue,
//         _blue, _blue, _blue, _blue
//     };

//     //  4x4 color array, only center 4 pixels have color
//     //
//     //  x = _yelw
//     //  _ = _tran
//     //
//     //  [_, _, _, _]
//     //  [_, x, x, _]
//     //  [_, x, x, _]
//     //  [_, _, _, _]
//     private readonly static Color[] _topLayer = new Color[]
//     {
//         _tran, _tran, _tran, _tran,
//         _tran, _yelw, _yelw, _tran,
//         _tran, _yelw, _yelw, _tran,
//         _tran, _tran, _tran, _tran
//     };

//     private static Color[] BlendTopAndBottom(BlendMode mode)
//     {
//         Color[] blended = new Color[LEN];

//         for (int i = 0; i < LEN; i++)
//         {
//             blended[i] = BlendFuncs.Blend(_bottomLayer[i], _topLayer[i], OPACITY, OPACITY, mode);
//         }

//         return blended;
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Normal_Test()
//     {

//         Color[] expected = new Color[]
//         {
//             _blue, _blue, _blue, _blue,
//             _blue, _yelw, _yelw, _blue,
//             _blue, _yelw, _yelw, _blue,
//             _blue, _blue, _blue, _blue
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.Normal);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Darken_Test()
//     {
//         Color a = Color.FromArgb(255, 1, 136, 165);
//         Color b = Color.FromArgb(255, 1, 136, 128);

//         Color[] expected = new Color[]
//         {
//             a, a, a, a,
//             a, b, b, a,
//             a, b, b, a,
//             a, a, a, a
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.Darken);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Multiply_Test()
//     {
//         Color a = Color.FromArgb(255, 1, 136, 165);
//         Color b = Color.FromArgb(255, 1, 111, 83);

//         Color[] expected = new Color[]
//         {
//             a, a, a, a,
//             a, b, b, a,
//             a, b, b, a,
//             a, a, a, a
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.Multiply);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_ColorBurn_Test()
//     {
//         Color a = Color.FromArgb(255, 1, 136, 165);
//         Color b = Color.FromArgb(255, 1, 109, 76);

//         Color[] expected = new Color[]
//         {
//             a, a, a, a,
//             a, b, b, a,
//             a, b, b, a,
//             a, a, a, a
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.ColorBurn);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Lighten_Test()
//     {
//         Color a = Color.FromArgb(255, 1, 136, 165);
//         Color b = Color.FromArgb(255, 255, 208, 165);

//         Color[] expected = new Color[]
//         {
//             a, a, a, a,
//             a, b, b, a,
//             a, b, b, a,
//             a, a, a, a
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.Lighten);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Screen_Test()
//     {
//         Color a = Color.FromArgb(255, 1, 136, 165);
//         Color b = Color.FromArgb(255, 255, 233, 210);

//         Color[] expected = new Color[]
//         {
//             a, a, a, a,
//             a, b, b, a,
//             a, b, b, a,
//             a, a, a, a
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.Screen);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_ColorDodge_Test()
//     {
//         Color a = Color.FromArgb(255, 1, 136, 165);
//         Color b = Color.FromArgb(255, 255, 255, 255);

//         Color[] expected = new Color[]
//         {
//             a, a, a, a,
//             a, b, b, a,
//             a, b, b, a,
//             a, a, a, a
//         };

//         Color[] actual = BlendTopAndBottom(BlendMode.ColorDodge);

//         Assert.Equal(expected, actual);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Addition_Test()
//     {
//         byte Br = 1;
//         byte Bg = 2;
//         byte Bb = 3;
//         byte Ba = 255;

//         byte Sr = 4;
//         byte Sg = 5;
//         byte Sb = 6;
//         byte Sa = 255;

//         byte r = 5;
//         byte g = 7;
//         byte b = 9;
//         byte a = 255;


//         Color backdrop = Color.FromArgb(Ba, Br, Bg, Bb);
//         Color source = Color.FromArgb(Sa, Sr, Sg, Sb);
//         Color expected = Color.FromArgb(a, r, g, b);

//         Color actual = BlendFuncs.Blend(backdrop, source, 255, 255, BlendMode.Addition);
//     }

//     [Fact]
//     public void BlendFuncs_BlendMode_Subtract_Test()
//     {
//         byte Br = 1;
//         byte Bg = 2;
//         byte Bb = 3;
//         byte Ba = 255;

//         byte Sr = 4;
//         byte Sg = 5;
//         byte Sb = 6;
//         byte Sa = 255;

//         byte r = 3;
//         byte g = 3;
//         byte b = 3;
//         byte a = 255;


//         Color backdrop = Color.FromArgb(Ba, Br, Bg, Bb);
//         Color source = Color.FromArgb(Sa, Sr, Sg, Sb);
//         Color expected = Color.FromArgb(a, r, g, b);

//         Color actual = BlendFuncs.Blend(backdrop, source, 255, 255, BlendMode.Subtract);
//     }




// }