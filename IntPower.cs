using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace Benchmarking;

public delegate ulong PowerMethod(ulong n, int pow);

/*
|                 Method |      Mean |    Error |   StdDev | Allocated |
|----------------------- |----------:|---------:|---------:|----------:|
|       BuiltinPowerTest | 408.42 ns | 1.032 ns | 0.861 ns |         - |
|    CppFastestPowerTest |  52.59 ns | 0.426 ns | 0.378 ns |         - | THE WINNER
|          EasyPowerTest |  56.11 ns | 0.557 ns | 0.521 ns |         - |
| EasyPowerRecursiveTest | 184.51 ns | 0.987 ns | 0.923 ns |         - |
*/

[MemoryDiagnoser]
public class IntPower
{
    static readonly int Length = 16;
    static readonly ulong Prime = 31ul;

    public void Test()
    {
        ulong[] bases = new ulong[] { 1ul, 4ul, 10ul, 135ul, 15ul };
        int[] powers = new int[] { 4, 6, 12, 3, 16 };

        for (int i = 0; i < bases.Length; i++)
        {
            ulong n = bases[i];
            int power = powers[i];

            Console.WriteLine("======================================================");
            Console.WriteLine($"BuiltinPower: {n}^{power} = {BuiltinPower(n, power)}");
            Console.WriteLine($"CppFastestPower: {n}^{power} = {CppFastestPower(n, power)}");
            Console.WriteLine($"EasyPower: {n}^{power} = {EasyPower(n, power)}");
            Console.WriteLine($"EasyPowerRecursive: {n}^{power} = {EasyPowerRecursive(n, power)}");
            Console.WriteLine("======================================================");

        }
    }

    [Benchmark]
    public ulong BuiltinPowerTest()
    {
        return Caller(BuiltinPower);
    }

    [Benchmark]
    public ulong CppFastestPowerTest()
    {
        return Caller(CppFastestPower);
    }

    [Benchmark]
    public ulong EasyPowerTest()
    {
        return Caller(EasyPower);
    }

    [Benchmark]
    public ulong EasyPowerRecursiveTest()
    {
        return Caller(EasyPowerRecursive);
    }

    public static ulong Caller(PowerMethod method)
    {
        ulong sum = 0;
        for (int i = 0; i < Length; i++)
        {
            sum += method(Prime, i);
        }
        return sum;
    }

    private static ulong BuiltinPower(ulong n, int pow)
    {
        return (ulong)Math.Pow(n, pow);
    }

    private static ulong CppFastestPower(ulong n, int pow)
    {
        ulong result = 1;
        while (pow > 0)
        {
            if ((pow & 1) != 0) result *= n;
            pow >>= 1;
            n *= n;
        }
        return result;
    }

    private static ulong EasyPower(ulong n, int pow)
    {
        ulong result = 1;
        for (int i = 0; i < pow; i++)
        {
            result *= n;
        }
        return result;
    }

    private static ulong EasyPowerRecursive(ulong n, int pow)
    {
        if (pow < 1) return 1;
        else return n * EasyPowerRecursive(n, pow - 1);
    }
}