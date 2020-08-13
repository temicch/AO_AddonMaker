namespace Application.BL.Files.Provider
{
    /// <summary>
    ///     An empty file provider containing only file information
    /// </summary>
    public class BlankFileProvider : IFileProvider
    {
        public IFile File { get; protected set; }

        public virtual IFileProvider SetFullFilePath(string filePath)
        {
            File = FileManager.CurrentWorkingManager.GetEmptyFile(filePath);
            return this;
        }

        public IFile GetFile()
        {
            return File;
        }
    }
}