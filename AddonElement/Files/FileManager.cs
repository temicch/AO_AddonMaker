using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Application.BL.Widgets;
using Microsoft.Extensions.Logging;

namespace Application.BL.Files
{
    public class FileManager : IFileManager
    {
        private readonly IDictionary<string, IFile> paths;

        public FileManager(ILogger<FileManager> logger)
        {
            paths = new Dictionary<string, IFile>();
            CurrentWorkingManager = this;
            Logger = logger;
        }

        private string CurrentWorkingFile { get; set; }
        internal static IFileManager CurrentWorkingManager { get; set; }
        public ILogger<FileManager> Logger { get; }

        public IFile RootFile { get; set; }

        public string RegisterFile(IFile file)
        {
            return RegisterFile(file, CurrentWorkingFile);
        }

        public string RegisterFile(IFile file, string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            if (paths.ContainsKey(filePath))
                return filePath; //new InvalidOperationException("This file is already exist");
            paths[filePath] = file;
            return filePath;
        }

        public IFile Load(string filePath)
        {
            Clear();
            RootFile = GetFile(filePath);
            return RootFile;
        }

        public IFile GetFile(string filePath)
        {
            if (filePath == null)
                return null;
            filePath = Path.GetFullPath(filePath.RemoveXPointer());
            return paths.ContainsKey(filePath) ? paths[filePath] : Add(filePath);
        }

        public IFile GetEmptyFile(string filePath)
        {
            return filePath == null ? null : CreateFileIfNotExists(filePath);
        }

        public void Clear()
        {
            paths.Clear();
            RootFile = null;
        }

        private IFile Add(string filePath)
        {
            if (filePath == null)
                return null;

            IFile newUiElement = null;

            if (paths.ContainsKey(filePath))
            {
                Logger.LogWarning($"[{filePath}] Trying add the element which already exist");
                return paths[filePath];
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            try
            {
                newUiElement = CreateUiElement(filePath, currentDirectory);
            }
            catch (ArgumentNullException)
            {
                Logger.LogWarning($"[{Path.GetFullPath(filePath)}]: Unknown type");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            catch (InvalidOperationException exception)
            {
                Logger.LogWarning($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                if (exception.InnerException != null)
                    Logger.LogWarning($" - {exception.InnerException.Message}");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            catch (XmlException)
            {
                Logger.LogWarning($"[{Path.GetFullPath(filePath)}] can't read as XML file");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            catch (IOException)
            {
                Logger.LogWarning($"[{Path.GetFullPath(filePath)}] file not found");
            }
            catch (Exception exception)
            {
                Logger.LogWarning($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }

            return newUiElement;
        }

        private IFile CreateFileIfNotExists(string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            return !paths.ContainsKey(filePath) ? new File(filePath) : paths[filePath];
        }

        private IFile CreateUiElement(string filePath, string currentDirectory)
        {
            IFile newUiElement;
            using (var xmlReaderStream = XmlReader.Create(filePath))
            {
                xmlReaderStream.MoveToContent();

                if (!string.IsNullOrEmpty(currentDirectory))
                    Directory.SetCurrentDirectory(currentDirectory);

                CurrentWorkingFile = Path.GetFullPath(filePath);

                var type = Type.GetType($"{typeof(Widget).Namespace}.{xmlReaderStream.Name}");

                var xmlSerializer = new XmlSerializer(type);

                newUiElement = xmlSerializer.Deserialize(xmlReaderStream) as IFile;
                (newUiElement as Widget)?.Widgets?.RemoveAll(x => x.File == null);
            }

            return newUiElement;
        }
    }
}