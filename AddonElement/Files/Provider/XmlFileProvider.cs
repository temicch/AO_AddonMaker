namespace Application.BL.Files.Provider;

/// <summary>
///     XML file provider. During processing, all child files will be processed
/// </summary>
public class XmlFileProvider : BlankFileProvider
{
    public override IFileProvider SetFullFilePath(string filePath)
    {
        File = FileManager.CurrentWorkingManager.GetFile(filePath);
        return this;
    }
}
