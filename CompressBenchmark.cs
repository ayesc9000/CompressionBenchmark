using BenchmarkDotNet.Attributes;
using System.IO.Compression;

namespace CompressionBenchmark;

/// <summary>
/// Compression benchmark to test data compression speeds.
/// </summary>
/// <remarks>
/// I elected to only perform one invocation per iteration because the compression methods are
/// slow as fuck and I'm impatient. Feel free to increase this if you feel like waiting around
/// for slightly more accuracy or if you have a much better CPU than I do.
/// </remarks>
[InvocationCount(1)]
public class CompressBenchmark
{
    readonly byte[] _data;

    /// <summary>
    /// Initialize the test data for the benchmark.
    /// </summary>
    public CompressBenchmark()
    {
        _data = new byte[Program.DataSize];

        Console.WriteLine($"Generating {Program.DataSize / 1000000} MB of test data, please stand by...");
        Random random = new();
        random.NextBytes(_data);
    }

    /// <summary>
    /// Brotli compression benchmark.
    /// </summary>
    /// <returns>Compressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] Brotli()
    {
        using MemoryStream output = new();
        using BrotliStream compressor = new(output, Level, true);
        compressor.Write(_data);
        return output.ToArray();
    }

    /// <summary>
    /// GZip compression benchmark.
    /// </summary>
    /// <returns>Compressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] GZip()
    {
        using MemoryStream output = new();
        using GZipStream compressor = new(output, Level, true);
        compressor.Write(_data);
        return output.ToArray();
    }

    /// <summary>
    /// ZLib compression benchmark.
    /// </summary>
    /// <returns>Compressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] ZLib()
    {
        using MemoryStream output = new();
        using ZLibStream compressor = new(output, Level, true);
        compressor.Write(_data);
        return output.ToArray();
    }

    /// <summary>
    /// Deflate compression benchmark.
    /// </summary>
    /// <returns>Compressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] Deflate()
    {
        using MemoryStream output = new();
        using DeflateStream compressor = new(output, Level, true);
        compressor.Write(_data);
        return output.ToArray();
    }

    /// <summary>
    /// Defines the level of compression to use. This is automated by
    /// BenchmarkDotNet.
    /// </summary>
    [Params(CompressionLevel.Fastest, CompressionLevel.Optimal, CompressionLevel.SmallestSize)]
    public CompressionLevel Level { get; set; }
}
