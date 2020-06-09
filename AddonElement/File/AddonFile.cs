using System.IO;

namespace AddonElement
{
    public class AddonFile
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FullPath { get => $"{FilePath}//{FileName}"; }
        public string FileType 
        { 
            get
            {
                string type = GetType().Name;
                if (type.StartsWith("Widget"))
                    type = type.Substring(6);
                if (type == "AddonFile")
                    type = "Other";
                return $"[{type}]";
            }
        }

        public AddonFile()
        {
            string file = AddonFileManager.RegisterAddonFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }

        public AddonFile(string filePath)
        {
            FilePath = Path.GetDirectoryName(filePath);
            FileName = Path.GetFileName(filePath);
        }
    }
}
