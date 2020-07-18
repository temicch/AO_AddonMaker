﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AddonElement.Widgets;

namespace AddonElement.File
{
    public class FileManager: IFileManager
    {
        private readonly Dictionary<string, IFile> paths;

        FileManager()
        {
            paths = new Dictionary<string, IFile>();
        }

        private string CurrentWorkingFile { get; set; }

        public IFile RootFile { get; set; }
        public event Action<string> OnDebug;

        public string RegisterFile(IFile file)
        {
            if (paths.ContainsKey(CurrentWorkingFile))
                throw new InvalidOperationException("This file is already exist");
            paths[CurrentWorkingFile] = file;
            return CurrentWorkingFile;
        }

        public IFile Load(string filePath)
        {
            Clear();
            RootFile = Add(filePath);
            return RootFile;
        }

        private IFile Add(string filePath)
        {
            if (filePath == null)
                return null;

            Utils.RemovePointer(ref filePath);
            IFile newUIElement = null;

            if (paths.ContainsKey(filePath))
            {
                DebugOutput($"[{filePath}] Trying add the element which already exist");
                return paths[filePath];
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            try
            {
                newUIElement = CreateUiElement(ref filePath, currentDirectory);
            }
            catch (ArgumentNullException)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}]: Unknown type");
                newUIElement = new File(Path.GetFullPath(filePath));
            }
            catch (InvalidOperationException exception)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                if (exception.InnerException != null)
                    DebugOutput($" - {exception.InnerException.Message}");
                newUIElement = new File(Path.GetFullPath(filePath));
            }
            catch (XmlException)
            {
                // Need to rewrite (many false positives)
                //DebugOutput($"[{Path.GetFullPath(filePath)}] can't read as XML file");
                newUIElement = new File(Path.GetFullPath(filePath));
            }
            catch (IOException)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}] file not found");
            }
            catch (Exception exception)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                newUIElement = new File(Path.GetFullPath(filePath));
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }

            return newUIElement;
        }

        private IFile CreateUiElement(ref string filePath, string currentDirectory)
        {
            IFile newUIElement;
            using (var xmlReaderStream = XmlReader.Create(filePath))
            {
                xmlReaderStream.MoveToContent();

                if (!string.IsNullOrEmpty(currentDirectory))
                    Directory.SetCurrentDirectory(currentDirectory);

                filePath = Path.GetFileName(filePath);

                var type = Type.GetType($"{typeof(Widget).Namespace}.{xmlReaderStream.Name}");

                var xmlSerializer = new XmlSerializer(type);

                CurrentWorkingFile = xmlReaderStream.BaseURI;
                newUIElement = xmlSerializer.Deserialize(xmlReaderStream) as IFile;
                (newUIElement as Widget)?.Widgets?.RemoveAll(x => x.File == null);
            }

            return newUIElement;
        }

        public IFile GetFile(string filePath)
        {
            if (filePath == null)
                return null;
            if (paths.ContainsKey(filePath))
                return paths[filePath];
            return Add(filePath);
        }

        public void Clear()
        {
            paths.Clear();
            RootFile = null;
        }

        private void DebugOutput(string msg)
        {
            OnDebug?.Invoke(msg);
        }
    }
}