namespace Addon.Files
{
    public interface IFileManager
    {
        IFile RootFile { get; }

        string RegisterFile(IFile file);
        string RegisterFile(IFile file, string filePath);

        IFile Load(string filePath);
        IFile GetFile(string filePath);
        IFile GetEmptyFile(string filePath);

        void Clear();
    }
}