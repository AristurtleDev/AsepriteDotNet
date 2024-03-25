```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4170/22H2/2022Update)
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
.NET SDK 8.0.202
  [Host]   : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2

Job=.NET 8.0  Runtime=.NET 8.0  Server=False  

```
| Method            | Mean     | Error     | StdDev    | Ratio | RatioSD |
|------------------ |---------:|----------:|----------:|------:|--------:|
| ForLoopBenchmark  | 8.538 ms | 0.1844 ms | 0.5438 ms |  1.00 |    0.00 |
| PLINQBenchmark    | 6.267 ms | 0.1200 ms | 0.1284 ms |  0.74 |    0.05 |
| ParallelBenchmark | 5.395 ms | 0.1060 ms | 0.1828 ms |  0.64 |    0.05 |
| UnsafeAsBenchmark | 8.206 ms | 0.1802 ms | 0.5172 ms |  0.97 |    0.08 |
