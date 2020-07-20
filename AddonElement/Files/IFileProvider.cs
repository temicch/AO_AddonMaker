namespace Addon.Files
{
    public interface IFileProvider
    {
        IFile GetFile();
        void SetFullFilePath(string filePath);
    }
}