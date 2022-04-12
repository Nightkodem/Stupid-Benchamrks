using BenchmarkDotNet.Attributes;
using System;

namespace Benchmarking;

/*
|           Method |     Mean |    Error |   StdDev | Allocated |
|----------------- |---------:|---------:|---------:|----------:|
|       EqualsMath | 60.12 ns | 0.412 ns | 0.365 ns |         - | THE WINNER
| EqualsSubstitute | 75.52 ns | 0.714 ns | 0.596 ns |         - |
| EqualsComparison | 68.40 ns | 0.292 ns | 0.274 ns |         - |
*/

[MemoryDiagnoser]
public class FloatEqals
{
    private static readonly float[] lhss = new float[] { 2f, 1246f, 0.0035f, -12.5f, 320f };
    private static readonly float[] rhss = new float[] { 1.9f, 1255f, 0.00345f, -12.8f, 334f };
    private static readonly float[] precisions = new float[] { 0.2f, 10f, 0.0001f, 0.4f, 20f };
    private static readonly int length = 5;
    private static readonly int repeats = 10;

    [Benchmark]
    public bool EqualsMath()
    {
        bool result = true;

        for (int i = 0; i < repeats; i++)
        {
            for (int j = 0; j < length; j++)
            {
                result &= EqualsMathMethod(lhss[j], rhss[j], precisions[j]);
            }
        }

        return result;
    }

    [Benchmark]
    public bool EqualsSubstitute()
    {
        bool result = true;

        for (int i = 0; i < repeats; i++)
        {
            for (int j = 0; j < length; j++)
            {
                result &= EqualsSubstituteMethod(lhss[j], rhss[j], precisions[j]);
            }
        }

        return result;
    }

    [Benchmark]
    public bool EqualsComparison()
    {
        bool result = true;

        for (int i = 0; i < repeats; i++)
        {
            for (int j = 0; j < length; j++)
            {
                result &= EqualsComparisonMethod(lhss[j], rhss[j], precisions[j]);
            }
        }

        return result;
    }

    public bool EqualsMathMethod(float lhs, float rhs, float precision)
    {
        return Math.Abs(lhs - rhs) <= precision;
    }

    public bool EqualsSubstituteMethod(float lhs, float rhs, float precision)
    {
        float diff = lhs - rhs;

        if (diff < 0f) diff = -diff;

        return diff <= precision;
    }

    public bool EqualsComparisonMethod(float lhs, float rhs, float precision)
    {
        if (lhs >= rhs) return lhs - rhs <= precision;
        else return rhs - lhs <= precision;
    }
}
