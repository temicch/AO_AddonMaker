using System.IO;
using AddonElement.File;
using AddonElement.Widgets;
using File = System.IO.File;

namespace AO_AddonMaker
{
    public class Project
    {
        private readonly string filePath;

        public Project(string rootFilePath)
        {
            if (!File.Exists(rootFilePath))
                throw new FileNotFoundException();
            filePath = rootFilePath;
        }

        public IUIElement RootWidget => FileManager.RootFile as IUIElement;

        public void Load()
        {
            FileManager.Load(filePath);
        }
    }
}