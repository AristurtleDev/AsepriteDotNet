// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsepriteDotNet;

public static class AseColorExtensions
{
    public static T As<T>(this AseColor color, Func<AseColor, T> converter) where T : struct
    {
        AseColor[] colors = new AseColor[12];

        ArgumentNullException.ThrowIfNull(converter);
        return converter(color);
    }

    public static T[] As<T>(this AseColor[] colors, Func<AseColor, T> converter) where T : struct
    {
        ArgumentNullException.ThrowIfNull(colors);
        ArgumentNullException.ThrowIfNull(converter);

        T[] converted = new T[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            converted[i] = converter(colors[i]);
        }
        return converted;
    }

    public static unsafe T[] AsUnsafe<T>(this AseColor[] colors, Func<AseColor, T> converter) where T : struct
    {
        ArgumentNullException.ThrowIfNull(colors);
        ArgumentNullException.ThrowIfNull(converter);

        T[] converted = new T[colors.Length];

        fixed (AseColor* pColors = colors)
        fixed (T* pConverted = converted)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                *(pConverted + i) = converter(*(pColors + i));
            }
        }

        return converted;
    }
}
