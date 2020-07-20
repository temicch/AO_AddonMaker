using System.Xml.Serialization;
using Addon.Files;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class Reference<T> where T: IFileProvider, new()
    {
        [XmlAttribute("href")]
        public string Path
        {
            get => File?.FilePath;
            set
            {
                if (value == string.Empty)
                    return;
                IFileProvider fileProvider = new T();
                fileProvider.SetFullFilePath(value);
                File = fileProvider.GetFile();
            }
        }

        [XmlIgnore] 
        public IFile File { get; set; }
    }
}