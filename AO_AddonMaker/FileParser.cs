using System.IO;

namespace AO_AddonMaker
{
    class FileParser
    {
        string filePath;
        public FileParser(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();
            this.filePath = filePath;
        }
        public void StartParse()
        {
            if (filePath == null)
                throw new FileNotFoundException();
            WidgetManager.Clear();
            WidgetManager.Add(filePath);
        }
    }
}
