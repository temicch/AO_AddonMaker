using System.IO;

namespace AddonElement.File
{
    public class File : IFile
    {
        private const string prefixFileWidget = "Widget";
        private const string otherType = "Other";

        public File()
        {
            var file = FileManager.RegisterFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }

        public File(string filePath)
        {
            FilePath = Path.GetDirectoryName(filePath);
            FileName = Path.GetFileName(filePath);
        }

        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FullPath => $"{FilePath}\\{FileName}";

        public string FileType
        {
            get
            {
                var type = GetType().Name;
                if (type.StartsWith(prefixFileWidget))
                    type = type.Substring(prefixFileWidget.Length);
                if (type == nameof(File))
                    type = otherType;
                return $"[{type}]";
            }
        }
    }
}