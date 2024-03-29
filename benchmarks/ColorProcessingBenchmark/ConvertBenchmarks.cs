﻿// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Drawing;
using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Common;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ColorProcessingBenchmark
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    [InProcess]
    [ReturnValueValidator]
    [GcServer(false)]
    public class ConvertBenchmarks
    {
        private Rgba32[] _colors = new Rgba32[1280 * 720];

        [GlobalSetup]
        public void Setup()
        {
            _colors = new Rgba32[1280 * 720];
            Span<byte> rgba = stackalloc byte[4];

            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = new Rgba32((byte)i, (byte)i, (byte)i, (byte)i);
            }
        }

        [Benchmark(Baseline = true)]
        public Color[] PLINQ_With_Func_AsT()
        {
            return _colors.As<Color>(aseColor => Color.FromArgb(aseColor.A, aseColor.R, aseColor.G, aseColor.B));
        }
    }
}
