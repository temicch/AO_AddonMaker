namespace Addon.Files.Provider
{
    public class BlankFileProvider : IFileProvider
    {
        public IFile File { get; protected set; }

        public virtual void SetFullFilePath(string filePath)
        {
            File = FileManager.CurrentWorkingManager.GetEmptyFile(filePath);
        }

        public IFile GetFile()
        {
            return File;
        }
    }
}