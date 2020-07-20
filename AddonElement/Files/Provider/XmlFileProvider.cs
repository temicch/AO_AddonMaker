namespace Addon.Files.Provider
{
    public class XmlFileProvider : BlankFileProvider
    {
        public override void SetFullFilePath(string filePath)
        {
            File = FileManager.CurrentWorkingManager.GetFile(filePath);
        }
    }
}