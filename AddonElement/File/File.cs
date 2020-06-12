using System.IO;

namespace AddonElement
{
    public class File
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FullPath { get => $"{FilePath}\\{FileName}"; }
        public string FileType 
        { 
            get
            {
                string type = GetType().Name;
                if (type.StartsWith("Widget"))
                    type = type.Substring(6);
                if (type == nameof(File))
                    type = "Other";
                return $"[{type}]";
            }
        }

        public File()
        {
            string file = FileManager.RegisterFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }

        public File(string filePath)
        {
            FilePath = Path.GetDirectoryName(filePath);
            FileName = Path.GetFileName(filePath);
        }
    }
}
