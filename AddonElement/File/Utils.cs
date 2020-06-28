using System;

namespace AddonElement
{
    internal class Utils
    {
        internal static void RemovePointer(ref string filePath)
        {
            var indexOf = filePath.IndexOf("#xpointer", StringComparison.Ordinal);
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
        }
    }
}
