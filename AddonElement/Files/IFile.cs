namespace Application.BL.Files;

/// <summary>
///     Interface for file information
/// </summary>
public interface IFile
{
    /// <summary>
    ///     File relative path
    /// </summary>
    string FilePath { get; }

    /// <summary>
    ///     File name
    /// </summary>
    string FileName { get; }

    /// <summary>
    ///     Absolute path to file
    /// </summary>
    string FullPath { get; }

    /// <summary>
    ///     Internal file type
    /// </summary>
    string FileType { get; }
}
