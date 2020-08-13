using Application.BL.Files;
using Application.BL.Files.Provider;
using System.Xml.Serialization;

namespace Application.BL.Widgets
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
                IFileProvider fileProvider = new T()
                    .SetFullFilePath(value);
                File = fileProvider.GetFile();
            }
        }

        [XmlIgnore] 
        public IFile File { get; set; }
    }
}