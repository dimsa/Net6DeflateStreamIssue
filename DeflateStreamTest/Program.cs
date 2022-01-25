using System;
using System.IO;
using System.IO.Compression;

namespace DeflateStreamTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting. " + Environment.Version);
            using (var stream = new FileStream(@"stream_test.txt", FileMode.Open))
            {
                stream.Position = 0;

                // .NET implements Deflate (RFC 1951) but not zlib (RFC 1950),
                // so we have to skip the first two bytes.
                stream.ReadByte();
                stream.ReadByte();

                var zipStream = new DeflateStream(stream, CompressionMode.Decompress, true);

                // Hardcoded length from real project. In the previous .Net versions this is size of final result
                long bytesToRead = (long)262 * 350;

                var buffer = new byte[bytesToRead];
                int bytesWereRead = zipStream.Read(buffer, 0, (int)bytesToRead);

                if (bytesWereRead != bytesToRead)
                {
                    throw new Exception("ZIP stream was not fully decompressed.");
                }

                Console.WriteLine("Ok");
                Console.ReadKey();
            }
        }
    }
}
