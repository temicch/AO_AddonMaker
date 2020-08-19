namespace Application.BL.Files
{
    /// <summary>
    ///     Functional for working with files
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        ///     Root file
        /// </summary>
        IFile RootFile { get; }

        /// <summary>
        ///     Register a file with a path specified by the file manager
        /// </summary>
        /// <param name="file">File Instance</param>
        /// <returns>Absolute path to file</returns>
        string RegisterFile(IFile file);

        /// <summary>
        ///     Register a file with a specified path
        /// </summary>
        /// <param name="file">File instance</param>
        /// <param name="filePath">File relative path</param>
        /// <returns>Absolute path to file</returns>
        string RegisterFile(IFile file, string filePath);

        /// <summary>
        ///     Returns true if specified file exist in the file manager
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns></returns>
        bool IsFileExist(string filePath);

        /// <summary>
        ///     Load the file at the specified path as the root file of the manager
        /// </summary>
        /// <param name="rootFilePath">Path to root file</param>
        /// <returns></returns>
        IFile Load(string rootFilePath);

        /// <summary>
        ///     Get an instance of the file at the specified path. If this file does not exist in the file manager, it will be
        ///     created automatically.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns><see cref="IFile"/> instance if file exist, null otherwise</returns>
        IFile GetFile(string filePath);

        /// <summary>
        ///     Get an instance of the stub file at the specified path. This file will not have any behavior, only information
        ///     about its location. If the file at the specified location already exists in the file manager, its instance will be
        ///     returned.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns></returns>
        IFile GetEmptyFile(string filePath);

        /// <summary>
        ///     Clear file configuration information
        /// </summary>
        void Clear();

        /// <summary>
        /// Number of files contained in the File manager
        /// </summary>
        int Count { get; }
    }
}