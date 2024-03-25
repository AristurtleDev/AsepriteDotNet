//// Copyright (c) Christopher Whitley. All rights reserved.
//// Licensed under the MIT license.
//// See LICENSE file in the project root for full license information.

//using System.Numerics;
//using System.Runtime.CompilerServices;

//namespace AsepriteDotNet.Common;

///// <summary>
///// Defines a color value with four 8-bit component values, each representing the red, green, blue, and alpha value of
///// the underlying color type.
///// </summary>
///// <typeparam name="T">The underlying color type.</typeparam>
//public interface IColor<T> where T : struct
//{
//    /// <summary>
//    /// Gets the underlying <typeparamref name="T"/> value wrapped by this interface.
//    /// </summary>
//    T Value { get; }


//    /// <summary>
//    /// Converts this <see cref="IColor"/> value to another of type <typeparamref name="T"/>.
//    /// </summary>
//    /// <typeparam name="TOut">The type base type to convert to.</typeparam>
//    /// <returns>The converted <see cref="IColor"/> type. s</returns>
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    TOut To<TOut>() where TOut : IColor, new();
//}
