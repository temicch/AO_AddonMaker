namespace Application.BL.Files.Provider
{
    /// <summary>
    ///     Provides processing of files of different types
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        ///     Get an instance of a <seealso cref="IFile" />
        /// </summary>
        /// <returns><seealso cref="IFile" /> instance</returns>
        IFile GetFile();

        /// <summary>
        ///     Set path to file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>This <seealso cref="IFileProvider" /></returns>
        IFileProvider SetFullFilePath(string filePath);
    }
}