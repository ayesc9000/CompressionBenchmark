using BenchmarkDotNet.Running;

namespace CompressionBenchmark;

/// <summary>
/// A set of benchmarks that test the performance of System.Compression streams.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main program entry point.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<CompressBenchmark>();
        BenchmarkRunner.Run<DecompressBenchmark>();
        Console.WriteLine($"Size of test data used: {Program.DataSize / 1000000} MB.");
        Console.WriteLine("\nPress enter to quit.");
        Console.ReadLine();
    }

    /// <summary>
    /// Size of the data to generate for all benchmarks.
    /// </summary>
    public static int DataSize => 128000000;
}
