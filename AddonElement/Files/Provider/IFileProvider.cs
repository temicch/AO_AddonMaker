namespace Application.BL.Files.Provider
{
    public interface IFileProvider
    {
        IFile GetFile();
        IFileProvider SetFullFilePath(string filePath);
    }
}