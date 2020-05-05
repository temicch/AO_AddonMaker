using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    static class WidgetManager
    {
        static IUIElement rootWidget;
        public static Dictionary<string, IUIElement> paths;

        static WidgetManager()
        {
            paths = new Dictionary<string, IUIElement>();
        }

        public static IUIElement GetRootWidget() => rootWidget;

        static void RemovePointer(ref string filePath)
        {
            int indexOf = filePath.IndexOf("#xpointer");
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
        }

        public static IUIElement Add(string filePath)
        {
            RemovePointer(ref filePath);

            if (paths.ContainsKey(filePath))
            {
                DebugController.Write(string.Format("[{0}] Trying add the element which exist", filePath));
                return paths[filePath];
            }

            XmlReader xmlReader;

            try
            {
                xmlReader = XmlReader.Create(filePath);
                xmlReader.MoveToContent();
            }
            catch (Exception)
            {
                DebugController.Write(string.Format("[{0}] can't read as XML file", filePath));
                return null;
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            if (currentDirectory != string.Empty)
                Directory.SetCurrentDirectory(currentDirectory);

            IUIElement newUIElement = null;
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
                        newUIElement = xmlSerializer.Deserialize(stream) as IUIElement;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(string.Format("Error {0}: {1}", Path.GetFullPath(filePath), exception.InnerException.Message));
                    }
                }

                paths[xmlReader.BaseURI] = newUIElement;
            }
            catch(Exception exception)
            {
                DebugController.Write(exception.Message);
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

        public static IUIElement GetUIElement(string filePath)
        {
            if (paths.ContainsKey(filePath))
                return paths[filePath];
            return Add(filePath);
        }

        public static void Clear()
        {
            if (rootWidget != null)
                rootWidget.Dispose();
            rootWidget = null;
            paths.Clear();
        }
    }
}
