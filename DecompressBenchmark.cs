using BenchmarkDotNet.Attributes;
using System.IO.Compression;

namespace CompressionBenchmark;

/// <summary>
/// Decompression benchmark to test data decompression speeds.
/// </summary>
[InvocationCount(10)]
public class DecompressBenchmark
{
    readonly byte[] _data;
    readonly MemoryStream _brotliData;
    readonly MemoryStream _gzipData;
    readonly MemoryStream _zlibData;
    readonly MemoryStream _deflateData;

    /// <summary>
    /// Initialize the test data for the benchmark.
    /// </summary>
    public DecompressBenchmark()
    {
        _data = new byte[Program.DataSize];
        _brotliData = new MemoryStream();
        _gzipData = new MemoryStream();
        _zlibData = new MemoryStream();
        _deflateData = new MemoryStream();

        Console.WriteLine($"Generating {Program.DataSize / 1000000} MB of test data, please stand by...");
        Random random = new();
        random.NextBytes(_data);
    }

    /// <summary>
    /// Compress the test data for the Brotli benchmark.
    /// </summary>
    [GlobalSetup(Targets = new[] { nameof(Brotli) })]
    public void BrotliSetup()
    {
        Console.WriteLine("Compressing test data (Brotli), please stand by...");
        using BrotliStream brotli = new(_brotliData, CompressionLevel.Optimal, true);
        brotli.Write(_data);
        brotli.Flush();
    }

    /// <summary>
    /// Compress the test data for the GZip benchmark.
    /// </summary>
    [GlobalSetup(Targets = new[] { nameof(GZip) })]
    public void GZipSetup()
    {
        Console.WriteLine("Compressing test data (GZip), please stand by...");
        using GZipStream gzip = new(_gzipData, CompressionLevel.Optimal, true);
        gzip.Write(_data);
        gzip.Flush();
    }

    /// <summary>
    /// Compress the test data for the ZLib benchmark.
    /// </summary>
    [GlobalSetup(Targets = new[] { nameof(ZLib) })]
    public void ZLibSetup()
    {
        Console.WriteLine("Compressing test data (ZLib), please stand by...");
        using ZLibStream zlib = new(_zlibData, CompressionLevel.Optimal, true);
        zlib.Write(_data);
        zlib.Flush();
    }

    /// <summary>
    /// Compress the test data for the Deflate benchmark.
    /// </summary>
    [GlobalSetup(Targets = new[] { nameof(Deflate) })]
    public void DeflateSetup()
    {
        Console.WriteLine("Compressing test data (Deflate), please stand by...");
        using DeflateStream deflate = new(_deflateData, CompressionLevel.Optimal, true);
        deflate.Write(_data);
        deflate.Flush();
    }

    /// <summary>
    /// Brotli decompression benchmark.
    /// </summary>
    /// <returns>Decompressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] Brotli()
    {
        _brotliData.Seek(0, SeekOrigin.Begin); // This is not ideal, but somehow the position changes
                                               // by 1 even when nothing should touch this when I
                                               // seek from the setup method.
        using MemoryStream output = new();
        using BrotliStream decompressor = new(_brotliData, CompressionMode.Decompress, true);
        decompressor.CopyTo(output);
        return output.ToArray();
    }

    /// <summary>
    /// GZip decompression benchmark.
    /// </summary>
    /// <returns>Decompressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] GZip()
    {
        _gzipData.Seek(0, SeekOrigin.Begin);
        using MemoryStream output = new();
        using GZipStream decompressor = new(_gzipData, CompressionMode.Decompress, true);
        decompressor.CopyTo(output);
        return output.ToArray();
    }

    /// <summary>
    /// ZLib decompression benchmark.
    /// </summary>
    /// <returns>Decompressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] ZLib()
    {
        _zlibData.Seek(0, SeekOrigin.Begin);
        using MemoryStream output = new();
        using ZLibStream decompressor = new(_zlibData, CompressionMode.Decompress, true);
        decompressor.CopyTo(output);
        return output.ToArray();
    }

    /// <summary>
    /// Deflate decompression benchmark.
    /// </summary>
    /// <returns>Decompressed test data.</returns>
    [Benchmark]
    [ArgumentsSource(nameof(Array))]
    public byte[] Deflate()
    {
        _deflateData.Seek(0, SeekOrigin.Begin);
        using MemoryStream output = new();
        using DeflateStream decompressor = new(_deflateData, CompressionMode.Decompress, true);
        decompressor.CopyTo(output);
        return output.ToArray();
    }
}
