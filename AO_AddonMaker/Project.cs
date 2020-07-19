using System;
using System.IO;
using Addon.Files;
using Addon.Widgets;

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
            if (!System.IO.File.Exists(rootFilePath))
                throw new FileNotFoundException();

            filePath = rootFilePath;
            Clear();
            FileManager.Load(filePath);
        }

        public void Clear()
        {
            FileManager?.Clear();
            if (FileManager != null) 
                return;
            FileManager = new FileManager();
            FileManager.OnError += debugWriter;
        }
    }
}