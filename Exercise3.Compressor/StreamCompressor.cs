using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Exercise3.Compressor
{
    public static class StreamCompressor
    {    
        public static CompressorData Compress(ref Stream inputStream, ref Stream outputStream)
        {
            var timingData = new CompressorData();
            var stopwatch = Stopwatch.StartNew();

            using (inputStream)
            {
                timingData.OriginalFileSize = inputStream.Length;
                timingData.TimeToReadStream = stopwatch.ElapsedMilliseconds;
                using (outputStream)
                {
                    timingData.TimeToCreateCompressedFile = stopwatch.ElapsedMilliseconds;
                    using (var compressor = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        inputStream.CopyTo(compressor);

                        timingData.TimeToCompressFinished = stopwatch.ElapsedMilliseconds;
                        timingData.CompressedFileSize = outputStream.Length;
                    }
                }
            }

            stopwatch.Stop();

            return timingData;
        }            

        public static CompressorData Decompress(ref Stream inputStream, ref Stream outputStream)
        {
            var timingData = new CompressorData();
            var stopwatch = Stopwatch.StartNew();

            using (inputStream)
            {
                timingData.OriginalFileSize = inputStream.Length;
                timingData.TimeToReadStream = stopwatch.ElapsedMilliseconds;
                using (outputStream)
                {
                    timingData.TimeToCreateCompressedFile = stopwatch.ElapsedMilliseconds;
                    using (var compressor = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        compressor.CopyTo(outputStream);

                        timingData.TimeToCompressFinished = stopwatch.ElapsedMilliseconds;
                        timingData.CompressedFileSize = outputStream.Length;
                    }
                }
            }

            stopwatch.Stop();

            return timingData;
        }

        public class CompressorData
        {
            public long OriginalFileSize; // in bytes
            public long CompressedFileSize;
            public long TimeToReadStream; // in milliseconds
            public long TimeToCreateCompressedFile;
            public long TimeToCompressFinished;

            private static int CompressionEfficiency(long x, long y)
            {
                if (y == 0)
                {
                    return 0;
                }
                return (int)(x / y * 100);
            }

            public string GetCompressionData() =>
                $"Original size: {OriginalFileSize / 1000} kB" +
                $"\nCompressed size: {CompressedFileSize / 1000} kB" +
                $"\nEfficiency: {CompressionEfficiency(OriginalFileSize, CompressedFileSize)}% smaller" +
                $"\nTime to open stream: {TimeToReadStream} ms" +
                $"\nTime to create new file: {TimeToCreateCompressedFile} ms" +
                $"\nTotal time taken: {TimeToCompressFinished} ms";

            public string GetDecompressionData() =>
                $"Original size: {OriginalFileSize / 1000} kB" +
                $"\nUncompressed size: {CompressedFileSize / 1000} kB" +
                $"\nIncrease in size: {CompressionEfficiency(CompressedFileSize, OriginalFileSize)}% larger" +
                $"\nTime to open stream: {TimeToReadStream} ms" +
                $"\nTime to create new file: {TimeToCreateCompressedFile} ms" +
                $"\nTotal time taken: {TimeToCompressFinished} ms";
        }
    }
}
