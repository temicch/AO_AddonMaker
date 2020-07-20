using System;

namespace Addon.Files
{
    public interface IFileManager
    {
        IFile RootFile { get; }

        event Action<string> OnError;

        string RegisterFile(IFile file);
        string RegisterFile(IFile file, string filePath);

        IFile Load(string filePath);
        IFile GetFile(string filePath);
        IFile GetEmptyFile(string filePath);

        void Clear();
    }
}