namespace Addon.Files.Provider
{
    public interface IFileProvider
    {
        IFile GetFile();
        IFileProvider SetFullFilePath(string filePath);
    }
}