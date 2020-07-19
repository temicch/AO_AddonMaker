using System;

namespace Addon.Files
{
    internal static class StringUtils
    {
        public static string RemoveXPointer(this string  filePath)
        {
            var indexOf = filePath.IndexOf("#xpointer", StringComparison.Ordinal);
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
            return filePath;
        }
    }
}