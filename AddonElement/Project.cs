using System.IO;
using Application.BL.Files;
using Application.BL.Widgets;
using File = System.IO.File;

namespace Application.BL
{
    /// <summary>
    ///     A class that encapsulates work with the project
    /// </summary>
    public class Project
    {
        public Project(IFileManager fileManager)
        {
            FileManager = fileManager;
        }

        private IFileManager FileManager { get; }

        /// <summary>
        ///     Project root file
        /// </summary>
        public IUIElement RootWidget => FileManager.RootFile as IUIElement;

        /// <summary>
        ///     Load the project at the specified file path
        /// </summary>
        /// <param name="rootFilePath">Path to root file</param>
        public void Load(string rootFilePath)
        {
            if (!File.Exists(rootFilePath))
                throw new FileNotFoundException();

            FileManager.Clear();
            FileManager.Load(rootFilePath);
        }
    }
}