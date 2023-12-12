using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using System.Text;

namespace InputUtilBenchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchmark>();
    }
}

[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.NativeAot70)]
public class Benchmark
{
    private static void RedirectStdIn(string input)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(input + Environment.NewLine));
        Console.SetIn(new StreamReader(stream));
    }

    [IterationSetup(Targets = new[]
    {
        nameof(Old_1Int),
        nameof(Current_1Int),
        nameof(Simple_1Int)
    })]
    public void Setup1()
    {
        RedirectStdIn("100");
    }

    [Benchmark]
    public int Old_1Int()
    {
        return InputUtil.Old.MyInputUtil.ReadInput<int>();
    }

    [Benchmark]
    public int Current_1Int()
    {
        return InputUtil.Current.InputUtil.ReadLine<int>();
    }

    [Benchmark]
    public int Simple_1Int()
    {
        return InputUtil.Simple.InputUtil.ReadLine<int>();
    }

    [IterationSetup(Targets = new[]
    {
        nameof(Old_3Int),
        nameof(Current_3Int),
        nameof(Simple_3Int)
    })]
    public void Setup2()
    {
        RedirectStdIn("100 200 300");
    }

    [Benchmark]
    public int Old_3Int()
    {
        var (a, b, c) = InputUtil.Old.MyInputUtil.ReadInput3<int>();
        return a + b + c;
    }

    [Benchmark]
    public int Current_3Int()
    {
        var (a, b, c) = InputUtil.Current.InputUtil.ReadLine<int>();
        return a + b + c;
    }

    [Benchmark]
    public int Simple_3Int()
    {
        var (a, b, c) = InputUtil.Simple.InputUtil.ReadLine<int>();
        return a + b + c;
    }

    [IterationSetup(Targets = new[]
    {
        nameof(Old_3Decimal),
        nameof(Current_3Decimal),
        nameof(Simple_3Decimal)
    })]
    public void Setup3()
    {
        RedirectStdIn("0.1 0.2 0.3");
    }

    [Benchmark]
    public decimal Old_3Decimal()
    {
        var (a, b, c) = InputUtil.Old.MyInputUtil.ReadInput3<decimal>();
        return a + b + c;
    }

    [Benchmark]
    public decimal Current_3Decimal()
    {
        var (a, b, c) = InputUtil.Current.InputUtil.ReadLine<decimal>();
        return a + b + c;
    }

    [Benchmark]
    public decimal Simple_3Decimal()
    {
        var (a, b, c) = InputUtil.Simple.InputUtil.ReadLine<decimal>();
        return a + b + c;
    }

    [IterationSetup(Targets = new[]
    {
        nameof(Old_Array),
        nameof(Current_Array),
        nameof(Current_Loop),
        nameof(Simple_Array),
        nameof(Simple_Loop)
    })]
    public void Setup4()
    {
        RedirectStdIn(string.Join(' ', Enumerable.Repeat(Enumerable.Range(0, 10_000), 20).SelectMany(x => x)));
    }

    [Benchmark]
    public int Old_Array()
    {
        return InputUtil.Old.MyInputUtil.ReadArray<int>().Sum();
    }

    [Benchmark]
    public int Current_Array()
    {
        return InputUtil.Current.InputUtil.ReadLine<int>().ToArray().Sum();
    }

    [Benchmark]
    public int Current_Loop()
    {
        var sum = 0;
        foreach (var i in InputUtil.Current.InputUtil.ReadLine<int>())
        {
            sum += i;
        }
        return sum;
    }

    [Benchmark]
    public int Simple_Array()
    {
        return InputUtil.Simple.InputUtil.ReadLine<int>().ToArray().Sum();
    }

    [Benchmark]
    public int Simple_Loop()
    {
        var sum = 0;
        foreach (var i in InputUtil.Simple.InputUtil.ReadLine<int>())
        {
            sum += i;
        }
        return sum;
    }
}