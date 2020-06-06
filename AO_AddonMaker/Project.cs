using System.IO;

namespace AO_AddonMaker
{
    public class Project
    {
        private readonly string filePath;

        public IUIElement RootWidget
        {
            get => WidgetManager.RootFile as IUIElement;
        }

        public Project(string rootFilePath)
        {
            WidgetManager.Clear();
            if (!File.Exists(rootFilePath))
                throw new FileNotFoundException();
            filePath = rootFilePath;
        }

        public void Load()
        {
            WidgetManager.Clear();
            WidgetManager.Load(filePath);
        }
    }
}
