﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Addon.Widgets;

namespace Addon.Files
{
    public class FileManager : IFileManager
    {
        private readonly Dictionary<string, IFile> paths;

        public FileManager()
        {
            paths = new Dictionary<string, IFile>();
            CurrentWorkingManager = this;
        }

        private string CurrentWorkingFile { get; set; }
        internal static IFileManager CurrentWorkingManager { get; set; }

        public IFile RootFile { get; set; }
        public event Action<string> OnError;

        public string RegisterFile(IFile file)
        {
            return RegisterFile(file, CurrentWorkingFile);
        }

        public string RegisterFile(IFile file, string filePath)
        {
            filePath = Path.GetFullPath(filePath);
            if (paths.ContainsKey(filePath))
                return filePath;//new InvalidOperationException("This file is already exist");
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

        public IFile GetFile(string filePath, Type fileType)
        {
            if (filePath == null)
                return null;
            filePath = Path.GetFullPath(filePath.RemoveXPointer());
            return paths.ContainsKey(filePath) ? paths[filePath] : Add(filePath, fileType);
        }

        public void Clear()
        {
            paths.Clear();
            RootFile = null;
        }

        private IFile Add(string filePath, Type fileType = null)
        {
            if (filePath == null)
                return null;

            if(fileType != null && fileType.IsAssignableFrom(typeof(BlankFile)))
                return new File(filePath);

            IFile newUiElement = null;

            if (paths.ContainsKey(filePath))
            {
                ErrorOutput($"[{filePath}] Trying add the element which already exist");
                return paths[filePath];
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            try
            {
                newUiElement = CreateUiElement(filePath, currentDirectory, fileType);
            }
            catch (ArgumentNullException)
            {
                ErrorOutput($"[{Path.GetFullPath(filePath)}]: Unknown type");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            catch (InvalidOperationException exception)
            {
                ErrorOutput($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                if (exception.InnerException != null)
                    ErrorOutput($" - {exception.InnerException.Message}");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            catch (XmlException)
            {
                // Need to rewrite (many false positives)
                ErrorOutput($"[{Path.GetFullPath(filePath)}] can't read as XML file");
                newUiElement = CreateFileIfNotExists(filePath);
            }
            catch (IOException)
            {
                ErrorOutput($"[{Path.GetFullPath(filePath)}] file not found");
            }
            catch (Exception exception)
            {
                ErrorOutput($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
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

        private IFile CreateUiElement(string filePath, string currentDirectory, Type fileType)
        {
            IFile newUiElement;
            using (var xmlReaderStream = XmlReader.Create(filePath))
            {
                if (fileType != null && fileType.IsAbstract)
                    fileType = null;
                

                xmlReaderStream.MoveToContent();

                if (!string.IsNullOrEmpty(currentDirectory))
                    Directory.SetCurrentDirectory(currentDirectory);

                CurrentWorkingFile = Path.GetFullPath(filePath);
                //filePath = Path.GetFileName(filePath);

                var type = fileType ?? Type.GetType($"{typeof(Widget).Namespace}.{xmlReaderStream.Name}");

                var xmlSerializer = new XmlSerializer(type);

                newUiElement = xmlSerializer.Deserialize(xmlReaderStream) as IFile;
                (newUiElement as Widget)?.Widgets?.RemoveAll(x => x.File == null);
            }

            return newUiElement;
        }

        private void ErrorOutput(string msg)
        {
            OnError?.Invoke(msg);
        }
    }
}