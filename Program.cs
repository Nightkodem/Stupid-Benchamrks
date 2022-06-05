using System;
using BenchmarkDotNet.Running;

namespace Benchmarking;

class Program
{
    static void Main()
    {
        /*new Contains().Test();
        Console.ReadLine();*/
        var summary = BenchmarkRunner.Run<Contains>();
    }
}
