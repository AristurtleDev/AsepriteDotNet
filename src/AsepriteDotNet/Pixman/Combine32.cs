// -----------------------------------------------------------------------------
//  This contains a port of the MUL_UN8 and DIV_UN8 functions from Pixman
//
//  https://gitlab.freedesktop.org/pixman/pixman
//
//  Pixman is licensed under the MIT license which can be found at
//
//  https://gitlab.freedesktop.org/pixman/pixman/-/blob/master/COPYING
//
//  You can also find the license text from Pixman as of 01/12/2024 below
//
// -------------------------BEGIN PIXMAN LICENSE TEXT---------------------------
//
//  The following is the MIT license, agreed upon by most contributors.
//  Copyright holders of new code should use this license statement where
//  possible. They may also add themselves to the list below.
//
//
//  Copyright 1987, 1988, 1989, 1998  The Open Group
//  Copyright 1987, 1988, 1989 Digital Equipment Corporation
//  Copyright 1999, 2004, 2008 Keith Packard
//  Copyright 2000 SuSE, Inc.
//  Copyright 2000 Keith Packard, member of The XFree86 Project, Inc.
//  Copyright 2004, 2005, 2007, 2008, 2009, 2010 Red Hat, Inc.
//  Copyright 2004 Nicholas Miell
//  Copyright 2005 Lars Knoll & Zack Rusin, Trolltech
//  Copyright 2005 Trolltech AS
//  Copyright 2007 Luca Barbato
//  Copyright 2008 Aaron Plattner, NVIDIA Corporation
//  Copyright 2008 Rodrigo Kumpera
//  Copyright 2008 André Tupinambá
//  Copyright 2008 Mozilla Corporation
//  Copyright 2008 Frederic Plourde
//  Copyright 2009, Oracle and/or its affiliates. All rights reserved.
//  Copyright 2009, 2010 Nokia Corporation
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice (including the next
//  paragraph) shall be included in all copies or substantial portions of the
//  Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
// --------------------------END PIXMAN LICENSE TEXT----------------------------

namespace AsepriteDotNet.Pixman;

internal static class Combine32
{
    private const byte ONE_HALF = 0x80;
    private const byte MASK = 0xFF;

    internal static byte MUL_UN8(int a, int b)
    {
        int t = (a * b) + ONE_HALF;
        return (byte)(((t >> 8) + t) >> 8);
    }

    internal static byte DIV_UN8(int a, int b)
    {
         return (byte)(((ushort)a * MASK + (b / 2)) / b);
    }
}
