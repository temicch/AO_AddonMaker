﻿using System.IO;

namespace AddonElement.File
{
    public class File : IFile
    {
        private const string prefixFileWidget = "Widget";
        private const string otherType = "Other";

        internal File()
        {
            var file = FileManager.CurrentWorkingManager.RegisterFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }

        public File(string filePath)
        {
            var file = FileManager.CurrentWorkingManager.RegisterFile(this, filePath);
            FilePath = Path.GetDirectoryName(filePath);
            FileName = Path.GetFileName(filePath);
        }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FullPath => $"{FilePath}{Path.DirectorySeparatorChar}{FileName}";

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