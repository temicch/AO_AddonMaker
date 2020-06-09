using AddonElement;
using System.IO;

namespace AO_AddonMaker
{
    public class Project
    {
        private readonly string filePath;

        public IUIElement RootWidget
        {
            get => AddonFileManager.RootFile as IUIElement;
        }

        public Project(string rootFilePath)
        {
            AddonFileManager.Clear();
            if (!File.Exists(rootFilePath))
                throw new FileNotFoundException();
            filePath = rootFilePath;
        }

        public void Load()
        {
            AddonFileManager.Clear();
            AddonFileManager.Load(filePath);
        }
    }
}
