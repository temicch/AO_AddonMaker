using Application.BL.Files;
using Application.BL.Widgets;
using System.IO;
using File = System.IO.File;

namespace Application.BL
{
    public class Project
    {
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

            FileManager.Clear();
            FileManager.Load(rootFilePath);
        }
    }
}