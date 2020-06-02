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

        private static IUIElement rootWidget { get; set; }
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
            AddonFile newUIElement = null;

            if (paths.ContainsKey(filePath))
            {
                DebugOutput.Write(string.Format("[{0}] Trying add the element which already exist", filePath));
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
                DebugOutput.Write(string.Format("[{0}] {1}: Unknown type", Path.GetFullPath(filePath), xmlReader.Name));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch (InvalidOperationException exception)
            {
                DebugOutput.Write(string.Format("[{0}]: {1}", Path.GetFullPath(filePath), exception.InnerException.Message));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch (XmlException)
            {
                DebugOutput.Write(string.Format("[{0}] can't read as XML file", Path.GetFullPath(filePath)));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch(FileNotFoundException)
            {
                DebugOutput.Write(string.Format("[{0}] file not found", Path.GetFullPath(filePath)));
            }
            catch (Exception exception)
            {
                DebugOutput.Write(string.Format("[{0}]: {1}", Path.GetFullPath(filePath), exception.InnerException?.Message));
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
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
