using AddonElement;
using System.IO;

namespace AO_AddonMaker
{
    public class Project
    {
        private readonly string filePath;

        public IUIElement RootWidget
        {
            get => FileManager.RootFile as IUIElement;
        }

        public Project(string rootFilePath)
        {
            if (!System.IO.File.Exists(rootFilePath))
                throw new FileNotFoundException();
            filePath = rootFilePath;
        }

        public void Load()
        {
            FileManager.Load(filePath);
        }
    }
}
