using BenchmarkDotNet.Attributes;

namespace Benchmarking;

/*
|             Method |     Mean |   Error |  StdDev | Allocated |
|------------------- |---------:|--------:|--------:|----------:|
|     IterateWithFor | 435.9 us | 2.45 us | 2.17 us |         - |
| IterateWithForeach | 327.4 us | 1.74 us | 1.63 us |         - | THE WINNER
*/

[MemoryDiagnoser]
public class ForVsForeach
{
    private const int N = 1000000;
    private static readonly bool[] array = new bool[N];

    [Benchmark]
    public bool IterateWithFor()
    {
        bool result = false;

        for (int i = 0; i < array.Length; i++)
        {
            result |= array[i];
        }

        return result;
    }

    [Benchmark]
    public bool IterateWithForeach()
    {
        bool result = false;

        foreach (var b in array)
        {
            result |= b;
        }

        return result;
    }
}
