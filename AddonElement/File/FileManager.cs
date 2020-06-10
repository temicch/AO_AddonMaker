using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AddonElement
{
    public static class FileManager
    {
        public delegate void DebugHandler(string message);
        public static event DebugHandler OnDebug;

        private static Dictionary<string, AddonFile> paths;
        public static string CurrentWorkingFile { get; private set; } = null;

        public static AddonFile RootFile { get; set; }
        
        static FileManager()
        {
            paths = new Dictionary<string, AddonFile>();
        }

        static void RemovePointer(ref string filePath)
        {
            int indexOf = filePath.IndexOf("#xpointer", StringComparison.Ordinal);
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
        }

        public static string RegisterFile(AddonFile file)
        {
            paths[CurrentWorkingFile] = file;
            return CurrentWorkingFile;
        }

        public static AddonFile Load(string filePath)
        {
            RootFile = Add(filePath);
            return RootFile;
        }

        private static AddonFile Add(string filePath)
        {
            if (filePath == null)
                return null;

            RemovePointer(ref filePath);
            AddonFile newUIElement = null;

            if (paths.ContainsKey(filePath))
            {
                DebugOutput($"[{filePath}] Trying add the element which already exist");
                return paths[filePath];
            }

            var previousDirectory = Directory.GetCurrentDirectory();
            var currentDirectory = Path.GetDirectoryName(filePath);

            try
            {
                var xmlReader = XmlReader.Create(filePath);
                xmlReader.MoveToContent();

                if (!string.IsNullOrEmpty(currentDirectory))
                    Directory.SetCurrentDirectory(currentDirectory);

                filePath = Path.GetFileName(filePath);

                Type type = Type.GetType($"{typeof(FileManager).Namespace}.{xmlReader.Name}");

                XmlSerializer xmlSerializer = new XmlSerializer(type);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    CurrentWorkingFile = stream.Name;
                    newUIElement = xmlSerializer.Deserialize(stream) as AddonFile;
                    (newUIElement as Widget)?.Children?.RemoveAll(x => x.File == null);
                }
            }
            catch (ArgumentNullException)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}]: Unknown type");
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch (InvalidOperationException exception)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                if(exception.InnerException != null)
                    DebugOutput($" - {exception.InnerException.Message}");
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch (XmlException)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}] can't read as XML file");
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            catch(DirectoryNotFoundException)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}] file not found");
            }
            catch (FileNotFoundException)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}] file not found");
            }
            catch (Exception exception)
            {
                DebugOutput($"[{Path.GetFullPath(filePath)}]: {exception.Message}");
                newUIElement = new AddonFile(Path.GetFullPath(filePath));
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }
            return newUIElement;
        }

        public static AddonFile GetFile(string filePath)
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
