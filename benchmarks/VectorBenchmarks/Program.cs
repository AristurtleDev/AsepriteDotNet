using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run(typeof(VectorBenchmark));

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
[InProcess]
[ReturnValueValidator]
[GcServer(false)]
public class VectorBenchmark
{
    private AsepriteFile _aseFile;

    [GlobalSetup]
    public void Setup()
    {
        _aseFile = AsepriteFileLoader.FromFile("adventurer.aseprite");
    }

    [Benchmark(Baseline = true)]
    public Rgba32[] FlattenFrame()
    {
        return _aseFile.Frames[0].FlattenFrame();
    }

    public Rgba32[] FlattenFrameVectorized()
    {
        return _aseFile.Frames[0].FlattenFrameVectorized();
    }
}
