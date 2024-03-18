using BenchmarkDotNet.Running;
using ColorProcessingBenchmark;

var summary = BenchmarkRunner.Run(typeof(ConvertBenchmarks));
