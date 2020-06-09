﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AddonElement
{
    public static class WidgetManager
    {
        public delegate void DebugHandler(string message);
        public static event DebugHandler OnDebug;

        public static Dictionary<string, AddonFile> paths;
        public static string CurrentWorkingFile { get; private set; } = null;

        public static AddonFile RootFile { get; set; }

        private static XmlReader xmlReader;

        static WidgetManager()
        {
            paths = new Dictionary<string, AddonFile>();
        }

        static void RemovePointer(ref string filePath)
        {
            int indexOf = filePath.IndexOf("#xpointer");
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
        }

        public static string RegisterAddonFile(AddonFile file)
        {
            paths[CurrentWorkingFile] = file;
            return CurrentWorkingFile;
        }

        public static AddonFile Load(string filePath)
        {
            RootFile = Add(filePath);
            return RootFile;
        }

        public static AddonFile Add(string filePath)
        {
            if (filePath == null)
                return null;

            RemovePointer(ref filePath);
            AddonFile newUIElement = null;

            if (paths.ContainsKey(filePath))
            {
                DebugOutput(string.Format("[{0}] Trying add the element which already exist", filePath));
                return paths[filePath];
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            try
            {
                xmlReader = XmlReader.Create(filePath);
                xmlReader.MoveToContent();

                if (currentDirectory != string.Empty)
                    Directory.SetCurrentDirectory(currentDirectory);

                filePath = Path.GetFileName(filePath);

                Type type = Type.GetType(string.Format("{0}.{1}", typeof(WidgetManager).Namespace, xmlReader.Name));

                XmlSerializer xmlSerializer = new XmlSerializer(type);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    CurrentWorkingFile = stream.Name;
                    newUIElement = xmlSerializer.Deserialize(stream) as AddonFile;
                }
            }
            catch (ArgumentNullException)
            {
                DebugOutput(string.Format("[{0}] {1}: Unknown type", Path.GetFullPath(filePath), xmlReader.Name));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch (InvalidOperationException exception)
            {
                DebugOutput(string.Format("[{0}]: {1}", Path.GetFullPath(filePath), exception.Message));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch (XmlException)
            {
                DebugOutput(string.Format("[{0}] can't read as XML file", Path.GetFullPath(filePath)));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch(DirectoryNotFoundException)
            {
                DebugOutput(string.Format("[{0}] file not found", Path.GetFullPath(filePath)));
            }
            catch (FileNotFoundException)
            {
                DebugOutput(string.Format("[{0}] file not found", Path.GetFullPath(filePath)));
            }
            catch (Exception exception)
            {
                DebugOutput(string.Format("[{0}]: {1}", Path.GetFullPath(filePath), exception.Message));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }
            return newUIElement;
        }

        public static AddonFile GetAddonFile(string filePath)
        {
            if (filePath == null)
                return null;
            if (paths.ContainsKey(filePath))
                return paths[filePath];
            return Add(filePath);
        }

        public static void Clear()
        {
            paths.Clear();
        }

        public static void DebugOutput(string msg)
        {
            OnDebug?.Invoke(msg);
        }
    }
}
