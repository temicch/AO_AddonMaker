using System.IO;
using System.Xml.Serialization;

namespace Application.BL.Files
{
    /// <summary>
    ///     Class that encapsulates file information
    /// </summary>
    public class File : IFile
    {
        private const string PrefixFileWidget = "Widget";
        private const string OtherType = "Other";

        internal File()
        {
            InitFile(null);
        }

        public File(string filePath)
        {
            InitFile(filePath);
        }

        [XmlIgnore]
        public string FilePath { get; set; }

        [XmlIgnore]
        public string FileName { get; set; }

        [XmlIgnore]
        public string FullPath => $"{FilePath}{Path.DirectorySeparatorChar}{FileName}";

        [XmlIgnore]
        public string FileType { get; private set; }

        private void InitFile(string filePath)
        {
            var file = filePath == null
                ? FileManager.CurrentWorkingManager.RegisterFile(this)
                : FileManager.CurrentWorkingManager.RegisterFile(this, filePath);

            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
            FileType = GetFileType();
        }

        private string GetFileType()
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