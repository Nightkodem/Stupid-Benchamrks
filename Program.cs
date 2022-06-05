using System;
using BenchmarkDotNet.Running;

namespace Benchmarking;

class Program
{
    static void Main()
    {
        /*new IntPower().Test();
        Console.ReadLine();*/
        var summary = BenchmarkRunner.Run<IntPower>();
    }
}
