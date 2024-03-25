// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

//---------------------------------------------------------------------------------------------------------------------
/*
    Description:
        This benchmark is to measure different methods of taking an array of IColor<T> values and converting them to an
        array of T[] values for the consume to use.

    Last Run:
        March 23, 2004

   Specs:
        OS Name:                   Microsoft Windows 10 Home
        OS Version:                10.0.19045 N/A Build 19045
        Processor(s):              1 Processor(s) Installed.
                                   [01]: Intel64 Family 6 Model 94 Stepping 3 GenuineIntel ~3192 Mhz
        Total Physical Memory:     16,344 MB

    Results:
        --------------------------------------------------------------------------------
        | Method             | Mean      | Error      | StdDev     | Ratio  | RatioSD  |
        | ------------------ | ---------:| ----------:| ----------:| ------:| --------:|
        | ForLoopBenchmark   | 8.538 ms  | 0.1844 ms  | 0.5438 ms  | 1.00   | 0.00     |
        | PLINQBenchmark     | 6.267 ms  | 0.1200 ms  | 0.1284 ms  | 0.74   | 0.05     |
        | ParallelBenchmark  | 5.395 ms  | 0.1060 ms  | 0.1828 ms  | 0.64   | 0.05     |
        | UnsafeAsBenchmark  | 8.206 ms  | 0.1802 ms  | 0.5172 ms  | 0.97   | 0.08     |
        --------------------------------------------------------------------------------

    Decision:
        Based on the benchmark results, the Parallel method was chosen for the IColor<T>[].As<T>() extension method
*/
//---------------------------------------------------------------------------------------------------------------------

using System.Drawing;
using System.Runtime.CompilerServices;
using AsepriteDotNet.Common;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace ColorProcessingBenchmark
{
    [SimpleJob(RuntimeMoniker.Net80)]
    [ReturnValueValidator]
    [GcServer(false)]
    public class ConvertBenchmarks
    {
        private SystemColor[] _colors = new SystemColor[1280 * 720];

        [GlobalSetup]
        public void Setup()
        {
            _colors = new SystemColor[1280 * 720];
            Span<byte> rgba = stackalloc byte[4];

            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = new SystemColor((byte)i, (byte)i, (byte)i, (byte)i);
            }
        }

        [Benchmark(Baseline = true)]
        public Color[] ForLoopBenchmark()
        {
            Color[] colors = new Color[_colors.Length];
            for(int i = 0; i < _colors.Length; i++)
            {
                colors[i] = _colors[i].Value;
            }
            return colors;
        }

        [Benchmark]
        public Color[] PLINQBenchmark()
        {
            return _colors.AsParallel()
                          .AsOrdered()
                          .WithDegreeOfParallelism(Environment.ProcessorCount)
                          .Select((color, i) => color.Value)
                          .ToArray();
        }

        [Benchmark]
        public Color[] ParallelBenchmark()
        {
            Color[] colors = new Color[_colors.Length];
            Parallel.For(0, _colors.Length, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, (i) =>
            {
                colors[i] = _colors[i].Value;
            });
            return colors;
        }

        [Benchmark]
        public unsafe Color[] UnsafeAsBenchmark()
        {
            Color[] colors = new Color[_colors.Length];
            fixed (Color* pColors = colors)
            {
                for (int i = 0; i < _colors.Length; i++)
                {
                    Unsafe.AsRef(pColors[i]) = _colors[i].Value;
                }
            }
            return colors;
        }
    }
}
