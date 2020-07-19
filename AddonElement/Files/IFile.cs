namespace Addon.Files
{
    public interface IFile
    {
        string FilePath { get; }
        string FileName { get; }
        string FullPath { get; }

        string FileType { get; }
    }
}