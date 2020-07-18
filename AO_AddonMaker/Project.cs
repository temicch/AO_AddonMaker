using System;
using System.IO;
using AddonElement.File;
using AddonElement.Widgets;

namespace AO_AddonMaker
{
    public class Project
    {
        private string filePath;
        public IFileManager FileManager { get; private set; }
        private readonly Action<string> debugWriter;

        public IUIElement RootWidget => FileManager?.RootFile as IUIElement;

        public Project(Action<string> debugWriter)
        {
            this.debugWriter = debugWriter;
        }

        public void Load(string rootFilePath)
        {
            if (!System.IO.File.Exists(rootFilePath))
                throw new FileNotFoundException();

            filePath = rootFilePath;
            Clear();
            FileManager.Load(filePath);
        }

        public void Clear()
        {
            FileManager?.Clear();
            if (FileManager == null)
            {
                FileManager = new FileManager();
                FileManager.OnDebug += debugWriter;
            }
        }
    }
}