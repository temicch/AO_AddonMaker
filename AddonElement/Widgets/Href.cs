using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class Href
    {
        [XmlAttribute("href")]
        public string Path
        {
            get => File?.FilePath;
            set
            {
                if (value != string.Empty) 
                    File = FileManager.CurrentWorkingManager.GetFile(value);
            }
        }

        [XmlIgnore] public IFile File { get; set; }
    }
}