using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Benchmarking;

public delegate int GCDMethod(int a, int b);

/*
|                            Method |              Mean |          Error |         StdDev |  Gen 0 | Allocated |
|---------------------------------- |------------------:|---------------:|---------------:|-------:|----------:|
|               BruteforceGCDCaller | 15,841,952.927 us | 65,913.2686 us | 61,655.3123 us |      - |     824 B |
|        EuclidesGCDRecursiveCaller |          3.646 us |      0.0032 us |      0.0025 us | 0.0343 |     232 B |
|        EuclidesGCDIterativeCaller |          4.119 us |      0.0476 us |      0.0445 us | 0.0305 |     232 B |
| EuclidesGCDIterativeNoAllocCaller |          3.607 us |      0.0039 us |      0.0035 us | 0.0343 |     232 B |    THE WINNER
|           SteinGCDRecursiveCaller |          6.124 us |      0.0155 us |      0.0130 us | 0.0305 |     232 B |
|           SteinGCDIterativeCaller |          4.275 us |      0.0068 us |      0.0063 us | 0.0305 |     232 B |
*/

[MemoryDiagnoser]
public class GCD
{
    private const int Iterations = 10;
    static readonly int[] a = { 4, 0, 12, 4324, 859359382, 57835 };
    static readonly int[] b = { 2, 12, 9, 124, 759359378, 0 };

    public void Test()
    {
        Console.WriteLine("======================================================");
        //Console.WriteLine($"BruteforceGCD: Average of GCDs = {BruteforceGCDCaller().Average()}");
        Console.WriteLine($"EuclidesGCDRecursive: Average of GCDs = {EuclidesGCDRecursiveCaller().Average()}");
        Console.WriteLine($"EuclidesGCDIterative: Average of GCDs = {EuclidesGCDIterativeCaller().Average()}");
        Console.WriteLine($"EuclidesGCDIterativeNoAlloc: Average of GCDs = {EuclidesGCDIterativeNoAllocCaller().Average()}");
        Console.WriteLine($"SteinGCDRecursive: Average of GCDs = {SteinGCDRecursiveCaller().Average()}");
        Console.WriteLine($"SteinGCDIterative: Average of GCDs = {SteinGCDIterativeCaller().Average()}");
        Console.WriteLine("======================================================");

        /*int a = 0, b = 12;

        Console.WriteLine($"BruteforceGCD: GCD({a},{b}) = {BruteforceGCD(a, b)}");
        Console.WriteLine($"EuclidesGCDRecursive: GCD({a},{b}) = {EuclidesGCDRecursive(a, b)}");
        Console.WriteLine($"EuclidesGCDIterative GCD({a},{b}) = {EuclidesGCDIterative(a, b)}");
        Console.WriteLine($"EuclidesGCDIterativeNoAlloc: GCD({a},{b}) = {EuclidesGCDIterativeNoAlloc(a, b)}");
        Console.WriteLine($"SteinGCD: GCD({a},{b}) = {SteinGCDRecursive(a, b)}");
        Console.WriteLine($"SteinGCD: GCD({a},{b}) = {SteinGCDIterative(a, b)}");*/
    }

    [Benchmark]
    public int[] BruteforceGCDCaller()
    {
        return Caller(BruteforceGCD);
    }

    [Benchmark]
    public int[] EuclidesGCDRecursiveCaller()
    {
        return Caller(EuclidesGCDRecursive);
    }

    [Benchmark]
    public int[] EuclidesGCDIterativeCaller()
    {
        return Caller(EuclidesGCDIterative);
    }

    [Benchmark]
    public int[] EuclidesGCDIterativeNoAllocCaller()
    {
        return Caller(EuclidesGCDIterativeNoAlloc);
    }

    [Benchmark]
    public int[] SteinGCDRecursiveCaller()
    {
        return Caller(SteinGCDRecursive);
    }

    [Benchmark]
    public int[] SteinGCDIterativeCaller()
    {
        return Caller(SteinGCDIterative);
    }

    public int[] Caller(GCDMethod method)
    {
        int[] res = new int[a.Length * b.Length];
        for (int k = 0; k < Iterations; k++)
        {
            int counter = 0;
            for (int i = 0; i < a.Length; i++)
            {
                int divident = a[i];
                for (int j = 0; j < b.Length; j++)
                {
                    res[counter] = method(divident, b[j]);
                    counter++;
                }
            }
        }
        return res;
    }


    public int BruteforceGCD(int a, int b)
    {
        if (a == 0) return b;
        if (b == 0) return a;
        int lower = a < b ? a : b;
        int max = 1;
        for (var i = 1; i <= lower; i++)
        {
            if (a % i == 0 && b % i == 0) max = i;
        }
        return max;
    }

    public int EuclidesGCDRecursive(int a, int b)
    {
        if (b == 0) return a;
        else return EuclidesGCDRecursive(b, a % b);
    }

    public int EuclidesGCDIterative(int a, int b)
    {
        int divident = a;
        int divisor = b;
        int temp;
        while (divisor > 0)
        {
            temp = divisor;
            divisor = divident % temp;
            divident = temp;
        }
        return divident;
    }

    public int EuclidesGCDIterativeNoAlloc(int a, int b) // THE WINNER
    {
        int temp;
        while (b > 0)
        {
            temp = b;
            b = a % temp;
            a = temp;
        }
        return a;
    }

    public int SteinGCDRecursive(int a, int b)
    {
        if (a == b) return a;
        if (a == 0) return b;
        if (b == 0) return a;

        if ((a & 1) == 0)
        {
            if ((b & 1) == 1) return SteinGCDRecursive(a >> 1, b);
            else return SteinGCDRecursive(a >> 1, b >> 1) << 1;
        }

        if ((b & 1) == 0) return SteinGCDRecursive(a, b >> 1);

        if (a > b) return SteinGCDRecursive((a - b) >> 1, b);

        return SteinGCDRecursive((b - a) >> 1, a);
    }

    public int SteinGCDIterative(int a, int b)
    {
        if (a == 0) return b;
        if (b == 0) return a;

        int gPot;
        for (gPot = 0; ((a | b) & 1) == 0; ++gPot)
        {
            a >>= 1;
            b >>= 1;
        }

        while ((a & 1) == 0) a >>= 1;

        do
        {
            while ((b & 1) == 0) b >>= 1;

            if (a > b)
            {
                (a, b) = (b, a);
            }

            b -= a;
        } while (b > 0);

        return a << gPot;
    }
}