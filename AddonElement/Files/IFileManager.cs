using System;

namespace AddonElement.File
{
    public interface IFileManager
    {
        IFile RootFile { get; }

        event Action<string> OnDebug;

        string RegisterFile(IFile file);

        IFile Load(string filePath);
        IFile GetFile(string filePath);

        void Clear();
    }
}
