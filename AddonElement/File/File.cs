using System.IO;

namespace AddonElement
{
    public class File
    {
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
                if (type.StartsWith("Widget"))
                    type = type.Substring(6);
                if (type == nameof(File))
                    type = "Other";
                return $"[{type}]";
            }
        }
    }
}