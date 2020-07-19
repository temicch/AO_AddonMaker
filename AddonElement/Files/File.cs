using System.IO;

namespace Addon.Files
{
    public class File : IFile
    {
        private const string PrefixFileWidget = "Widget";
        private const string OtherType = "Other";

        internal File()
        {
            var file = FileManager.CurrentWorkingManager.RegisterFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }

        public File(string filePath)
        {
            var file = FileManager.CurrentWorkingManager.RegisterFile(this, filePath);
            FilePath = Path.GetDirectoryName(file);
            if (!file.Equals(filePath))
            {

            }
            FileName = Path.GetFileName(file);
        }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FullPath => $"{FilePath}{Path.DirectorySeparatorChar}{FileName}";

        public string FileType
        {
            get
            {
                var type = GetType().Name;
                if (type.StartsWith(PrefixFileWidget))
                    type = type.Substring(PrefixFileWidget.Length);
                if (type == nameof(File))
                    type = OtherType;
                return $"[{type}]";
            }
        }
    }
}