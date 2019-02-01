using System.IO;
using System.IO.Compression;

namespace Exercise3.Compressor
{
    public static class StreamCompressor
    {    
        public static void Compress(ref Stream inputStream, ref Stream outputStream, ref CompressorData timingData)
        {                       
            using (inputStream)
            {
                timingData.OriginalFileSize = inputStream.Length;                
                using (outputStream)
                {                    
                    using (var compressor = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        inputStream.CopyTo(compressor);                        
                        timingData.CompressedFileSize = outputStream.Length;
                    }
                }
            }            
        }            

        public static void Decompress(ref Stream inputStream, ref Stream outputStream, ref CompressorData timingData)
        {                        
            using (inputStream)
            {
                timingData.OriginalFileSize = inputStream.Length;                
                using (outputStream)
                {                    
                    using (var compressor = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        compressor.CopyTo(outputStream);                        
                        timingData.CompressedFileSize = outputStream.Length;
                    }
                }
            }                        
        }

        public class CompressorData
        {
            public long OriginalFileSize; // in bytes
            public long CompressedFileSize;
            public long TimeToReadStream; // in milliseconds
            public long TimeToWriteStream;
            public long AllOperationsComplete;

            private static int CompressionEfficiency(long x, long y)
            {
                if (y == 0)
                {
                    return 0;
                }
                return (int)(x / y * 100);
            }

            private long TimeToCompressOrDecompress() => AllOperationsComplete - (TimeToWriteStream + TimeToReadStream); 

            public string GetCompressionData() =>
                $"Original size: {OriginalFileSize / 1000} kB" +
                $"\nCompressed size: {CompressedFileSize / 1000} kB" +
                $"\nEfficiency: {CompressionEfficiency(OriginalFileSize, CompressedFileSize)}% smaller size" +
                $"\nTime to open read stream from source file: {TimeToReadStream} ms" +
                $"\nTime to open write stream from output file: {TimeToWriteStream} ms" +
                $"\nTime to compress file: {TimeToCompressOrDecompress()} ms" +
                $"\nTotal time taken: {AllOperationsComplete} ms";

            public string GetDecompressionData() =>
                $"Original size: {OriginalFileSize / 1000} kB" +
                $"\nUncompressed size: {CompressedFileSize / 1000} kB" +
                $"\nIncrease in size: {CompressionEfficiency(CompressedFileSize, OriginalFileSize)}% larger size" +
                $"\nTime to open read stream from source file: {TimeToReadStream} ms" +
                $"\nTime to open write stream from output file: {TimeToWriteStream} ms" +
                $"\nTime to decompress file: {TimeToCompressOrDecompress()} ms" +
                $"\nTotal time taken: {AllOperationsComplete} ms";
        }
    }
}
