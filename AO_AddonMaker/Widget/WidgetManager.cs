using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    static class WidgetManager
    {
        public static Dictionary<string, AddonFile> paths;
        public static string CurrentWorkingFile { get; private set; } = null;

        private static IUIElement rootWidget;
        private static XmlReader xmlReader;

        static WidgetManager()
        {
            paths = new Dictionary<string, AddonFile>();
        }

        public static IUIElement GetRootWidget() => rootWidget;

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

        public static AddonFile Add(string filePath)
        {
            RemovePointer(ref filePath);

            if (paths.ContainsKey(filePath))
            {
                DebugOutput.Write(string.Format("[{0}] Trying add the element which already exist", filePath));
                return paths[filePath];
            }

            try
            {
                xmlReader = XmlReader.Create(filePath);
                xmlReader.MoveToContent();
            }
            catch (Exception)
            {
                DebugOutput.Write(string.Format("[{0}] can't read as XML file", filePath));
                return null;
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            if (currentDirectory != string.Empty)
                Directory.SetCurrentDirectory(currentDirectory);

            AddonFile newUIElement = null;

            try
            {
                filePath = Path.GetFileName(filePath);

                Type type = Type.GetType(string.Format("{0}.{1}", typeof(WidgetManager).Namespace, xmlReader.Name));

                if (type == null)
                {
                    throw new Exception(string.Format("[{0}]: Unknown type", xmlReader.Name));
                }

                XmlSerializer xmlSerializer = new XmlSerializer(type);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        CurrentWorkingFile = stream.Name;
                        newUIElement = xmlSerializer.Deserialize(stream) as AddonFile;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(string.Format("Error {0}: {1}", Path.GetFullPath(filePath), exception.InnerException.Message));
                    }
                }
            }
            catch(Exception exception)
            {
                DebugOutput.Write(exception.Message);
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }
            return newUIElement;
        }

        public static void SetRootWidget(IUIElement uIElement)
        {
            if (rootWidget == null)
                rootWidget = uIElement;
        }

        public static AddonFile GetAddonFile(string filePath)
        {
            if (paths.ContainsKey(filePath))
                return paths[filePath];
            return Add(filePath);
        }

        public static void Clear()
        {
            rootWidget = null;
            paths.Clear();
        }
    }
}
