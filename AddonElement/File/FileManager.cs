using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AddonElement
{
    public static class FileManager
    {
        private static readonly Dictionary<string, File> paths;

        static FileManager()
        {
            paths = new Dictionary<string, File>();
        }

        public static string CurrentWorkingFile { get; private set; }

        public static File RootFile { get; set; }
        public static event Action<string> OnDebug;

        private static void RemovePointer(ref string filePath)
        {
            var indexOf = filePath.IndexOf("#xpointer", StringComparison.Ordinal);
            if (indexOf > 0)
                filePath = filePath.Remove(indexOf);
        }

        public static string RegisterFile(File file)
        {
            paths[CurrentWorkingFile] = file;
            return CurrentWorkingFile;
        }

        public static File Load(string filePath)
        {
            Clear();
            RootFile = Add(filePath);
            return RootFile;
        }

        private static File Add(string filePath)
        {
            if (filePath == null)
                return null;

            RemovePointer(ref filePath);
            File newUIElement = null;

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

                var type = Type.GetType($"{typeof(FileManager).Namespace}.{xmlReader.Name}");

                var xmlSerializer = new XmlSerializer(type);

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    CurrentWorkingFile = stream.Name;
                    newUIElement = xmlSerializer.Deserialize(stream) as File;
                    (newUIElement as Widget)?.Children?.RemoveAll(x => x.File == null);
                }
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
            catch (DirectoryNotFoundException)
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
                newUIElement = new File(Path.GetFullPath(filePath));
            }
            finally
            {
                Directory.SetCurrentDirectory(previousDirectory);
            }

            return newUIElement;
        }

        public static File GetFile(string filePath)
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