using System.IO;

namespace AO_AddonMaker
{
    public class AddonFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }

        public AddonFile()
        {
            string file = WidgetManager.RegisterAddonFile(this);
            FilePath = Path.GetDirectoryName(file);
            FileName = Path.GetFileName(file);
        }
    }
}
