using System.IO;
using Addon.Files;
using Addon.Widgets;
using File = System.IO.File;

namespace AO_AddonMaker
{
    public class Project
    {
        private string filePath;

        public Project(IFileManager fileManager)
        {
            FileManager = fileManager;
        }

        public IFileManager FileManager { get; }

        public IUIElement RootWidget => FileManager.RootFile as IUIElement;

        public void Load(string rootFilePath)
        {
            if (!File.Exists(rootFilePath))
                throw new FileNotFoundException();

            filePath = rootFilePath;
            Clear();
            FileManager.Load(filePath);
        }

        public void Clear()
        {
            FileManager.Clear();
        }
    }
}