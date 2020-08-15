using System.IO;
using System.IO.Compression;

namespace Application.BL.Texture.Extensions
{
    internal static class StreamExtensions
    {
        /// <summary>
        ///     Uncompress streaming data
        /// </summary>
        /// <param name="input">Stream</param>
        /// <returns></returns>
        public static MemoryStream UnZLib(this Stream input)
        {
            input.Position = 0L;
            var inputLength = (int)input.Length;

            var memoryStream = new MemoryStream();
            using (var zlibStream = new GZipStream(input, CompressionMode.Decompress))
            {
                var buffer = new byte[inputLength];
                for (var count = zlibStream.Read(buffer, 0, inputLength);
                    count > 0;
                    count = zlibStream.Read(buffer, 0, inputLength))
                    memoryStream.Write(buffer, 0, count);
            }

            memoryStream.Position = 0L;
            return memoryStream;
        }
    }
}