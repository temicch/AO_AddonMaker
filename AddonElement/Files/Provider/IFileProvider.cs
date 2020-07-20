namespace Addon.Files.Provider
{
    public interface IFileProvider
    {
        IFile GetFile();
        void SetFullFilePath(string filePath);
    }
}