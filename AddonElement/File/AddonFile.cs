using System.IO;

namespace AddonElement
{
    public class AddonFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
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
            string file = WidgetManager.RegisterAddonFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }

        public AddonFile(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(FilePath);
        }
    }
}
