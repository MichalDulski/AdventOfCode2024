using Day02;

namespace Benchmarking;

using BenchmarkDotNet.Attributes;
using System.IO;
using System.Linq;

public class BenchmarkTest
{
    private IEnumerable<IEnumerable<int>> input;

    [GlobalSetup]
    public void Setup()
    {
        input = File.ReadAllLines("../../../../../../../../Day02/input.txt").Select(s => s.Split(' ').Select(int.Parse));
    }

    [Benchmark]
    public void RunOriginalCode()
    {
        MySolution.Run(input);
    }

}