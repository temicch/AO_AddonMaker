using Ionic.Zlib;
using System.IO;

namespace Texture
{
    internal static class Utils
    {
        public static bool IsPowerOf2(int x)
        {
            return (x & x - 1) == 0;
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

        public static MemoryStream UnZLib(MemoryStream input)
        {
            input.Position = 0L;
            MemoryStream memoryStream = new MemoryStream();
            using (ZlibStream zlibStream = new ZlibStream(input, (CompressionMode)1))
            {
                byte[] buffer = new byte[16384];
                for (int count = ((Stream)zlibStream).Read(buffer, 0, 16384); count > 0; count = ((Stream)zlibStream).Read(buffer, 0, 16384))
                    memoryStream.Write(buffer, 0, count);
            }
            memoryStream.Position = 0L;
            return memoryStream;
        }

        //public static MemoryStream ZLib(MemoryStream input)
        //{
        //    input.Position = 0L;
        //    MemoryStream memoryStream = new MemoryStream();
        //    using (ZlibStream zlibStream = new ZlibStream((Stream)input, (CompressionMode)0))
        //    {
        //        byte[] buffer = new byte[16384];
        //        for (int count = ((Stream)zlibStream).Read(buffer, 0, 16384); count > 0; count = ((Stream)zlibStream).Read(buffer, 0, 16384))
        //            memoryStream.Write(buffer, 0, count);
        //    }
        //    memoryStream.Position = 0L;
        //    return memoryStream;
        //}
    }
}
