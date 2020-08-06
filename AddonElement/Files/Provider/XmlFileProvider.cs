namespace Addon.Files.Provider
{
    public class XmlFileProvider : BlankFileProvider
    {
        public override IFileProvider SetFullFilePath(string filePath)
        {
            File = FileManager.CurrentWorkingManager.GetFile(filePath);
            return this;
        }
    }
}