using System.IO;
using System.IO.Compression;

namespace Texture.Extensions
{
    internal static class StreamExtensions
    {
        public static MemoryStream UnZLib(this Stream input)
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