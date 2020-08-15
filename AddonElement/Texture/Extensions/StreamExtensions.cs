using Ionic.Zlib;
using System.IO;

namespace Application.BL.Texture.Extensions
{
    internal static class StreamExtensions
    {
        /// <summary>
        ///     Uncompress streaming data
        /// </summary>
        /// <param name="input">Stream</param>
        /// <returns></returns>
        public static Stream UnZLib(this Stream input)
        {
            input.Position = 0L;

            Stream memoryStream = new MemoryStream();

            using (Stream zlibStream = new ZlibStream(input, CompressionMode.Decompress))
            {
                zlibStream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0L;
            return memoryStream;
        }
    }
}