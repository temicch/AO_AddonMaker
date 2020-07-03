using System.IO;
using System.IO.Compression;

namespace Texture
{
    internal static class Utils
    {
        public static bool IsPowerOf2(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public static int NextPowerOf2(int n)
        {
            --n;
            n |= n >> 1;
            n |= n >> 2;
            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;
            ++n;
            return n;
        }

        public static MemoryStream UnZLib(Stream input)
        {
            input.Position = 0L;
            var memoryStream = new MemoryStream();
            using (var zlibStream = new GZipStream(input, CompressionMode.Decompress))
            {
                var buffer = new byte[16384];
                for (var count = zlibStream.Read(buffer, 0, 16384);
                    count > 0;
                    count = zlibStream.Read(buffer, 0, 16384))
                    memoryStream.Write(buffer, 0, count);
            }

            memoryStream.Position = 0L;
            return memoryStream;
        }
    }
}