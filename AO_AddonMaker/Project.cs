using System;
using System.IO;
using AddonElement.File;
using AddonElement.Widgets;
using File = System.IO.File;

namespace AO_AddonMaker
{
    public class Project
    {
        private readonly Action<string> debugWriter;
        private string filePath;

        public Project(Action<string> debugWriter)
        {
            this.debugWriter = debugWriter;
        }

        public IFileManager FileManager { get; private set; }

        public IUIElement RootWidget => FileManager?.RootFile as IUIElement;

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
            FileManager?.Clear();
            if (FileManager == null)
            {
                FileManager = new FileManager();
                FileManager.OnDebug += debugWriter;
            }
        }
    }
}