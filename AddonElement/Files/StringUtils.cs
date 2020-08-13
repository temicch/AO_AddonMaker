using System;

namespace Application.BL.Files
{
    internal static class StringUtils
    {
        /// <summary>
        ///     Remove the expression "#xpointer" from the file name
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns></returns>
        public static string RemoveXPointer(this string filePath)
        {
            var indexOf = filePath.IndexOf("#xpointer", StringComparison.Ordinal);
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
            return filePath;
        }
    }
}