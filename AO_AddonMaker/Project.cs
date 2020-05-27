using System.IO;

namespace AO_AddonMaker
{
    class Project
    {
        private readonly string filePath;

        public IUIElement RootWidget { get; set; }

        public Project(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();
            this.filePath = filePath;
        }

        public void Load()
        {
            WidgetManager.Clear();
            WidgetManager.Add(filePath);
            RootWidget = WidgetManager.GetRootWidget();
        }
    }
}
