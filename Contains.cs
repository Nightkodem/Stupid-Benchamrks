using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Benchmarking;

/*
 * Comment:     WTF is this Enumerable doing that it is so fast
 * Conclusion:  For less than 10 elements array is good enough,
 *              for more than that use plain Enumerable or bset - obviously HashSet
 *             
|                     Method |      Mean |    Error |   StdDev | Allocated |
|--------------------------- |----------:|---------:|---------:|----------:|
|   Enumerable10ContainsTest |  46.56 ns | 0.093 ns | 0.078 ns |         - |
|    ArrLinear10ContainsTest |  29.27 ns | 0.075 ns | 0.070 ns |         - |
|    ArrBinary10ContainsTest |  80.18 ns | 0.422 ns | 0.374 ns |         - |
|          Set10ContainsTest |  33.99 ns | 0.084 ns | 0.078 ns |         - |
|                            |           |          |          |           |
|  Enumerable100ContainsTest |  43.83 ns | 0.591 ns | 0.553 ns |         - |
|   ArrLinear100ContainsTest | 192.17 ns | 0.421 ns | 0.352 ns |         - |
|   ArrBinary100ContainsTest | 120.07 ns | 0.599 ns | 0.560 ns |         - |
|         Set100ContainsTest |  33.64 ns | 0.110 ns | 0.103 ns |         - |
|                            |           |          |          |           |
| Enumerable1000ContainsTest |  41.50 ns | 0.559 ns | 0.523 ns |         - |
|  ArrLinear1000ContainsTest | 825.73 ns | 1.423 ns | 1.188 ns |         - |
|  ArrBinary1000ContainsTest | 154.49 ns | 0.292 ns | 0.259 ns |         - |
|        Set1000ContainsTest |  34.01 ns | 0.092 ns | 0.077 ns |         - | 
*/

[MemoryDiagnoser]
public class Contains
{
    public static readonly int[] Elements = new[] { 1, 7, 17, 89, 243, 954, 1300 };

    public static readonly IEnumerable<int> Range10 = Enumerable.Range(0, 10);
    public static readonly int[] ArrRange10 = Range10.ToArray();
    public static readonly HashSet<int> SetRange10 = new(Range10);

    public static readonly IEnumerable<int> Range100 = Enumerable.Range(0, 100);
    public static readonly int[] ArrRange100 = Range100.ToArray();
    public static readonly HashSet<int> SetRange100 = new(Range100);

    public static readonly IEnumerable<int> Range1000 = Enumerable.Range(0, 1000);
    public static readonly int[] ArrRange1000 = Range1000.ToArray();
    public static readonly HashSet<int> SetRange1000 = new(Range1000);

    public volatile bool contains = false;

    public void Test()
    {
        Console.WriteLine(String.Join(' ', ArrRange10));
    }

    [Benchmark]
    public void Enumerable10ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = EnumerableContains(SetRange10, element);
        }
    }

    [Benchmark]
    public void ArrLinear10ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = ArrLinearContains(ArrRange10, element);
        }
    }
    [Benchmark]
    public void ArrBinary10ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = ArrBinaryContains(ArrRange10, element);
        }
    }

    [Benchmark]
    public void Set10ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = SetContains(SetRange10, element);
        }
    }

    [Benchmark]
    public void Enumerable100ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = EnumerableContains(SetRange100, element);
        }
    }

    [Benchmark]
    public void ArrLinear100ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = ArrLinearContains(ArrRange100, element);
        }
    }

    [Benchmark]
    public void ArrBinary100ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = ArrBinaryContains(ArrRange100, element);
        }
    }

    [Benchmark]
    public void Set100ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = SetContains(SetRange100, element);
        }
    }

    [Benchmark]
    public void Enumerable1000ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = EnumerableContains(SetRange1000, element);
        }
    }

    [Benchmark]
    public void ArrLinear1000ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = ArrLinearContains(ArrRange1000, element);
        }
    }


    [Benchmark]
    public void ArrBinary1000ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = ArrBinaryContains(ArrRange1000, element);
        }
    }

    [Benchmark]
    public void Set1000ContainsTest()
    {
        foreach (int element in Elements)
        {
            contains = SetContains(SetRange1000, element);
        }
    }


    private static bool EnumerableContains(IEnumerable<int> collection, int x)
    {
        return collection.Contains(x);
    }

    private static bool ArrLinearContains(int[] arr, int x)
    {
        foreach (int i in arr)
        {
            if (i == x) return true;
        }
        return false;
    }

    private static bool ArrBinaryContains(int[] arr, int x)
    {
        return Array.BinarySearch(arr, x) >= 0;
    }

    private static bool SetContains(HashSet<int> set, int x)
    {
        return set.Contains(x);
    }
}