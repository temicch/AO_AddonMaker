using System.Xml.Serialization;
using AddonElement.File;

namespace AddonElement.Widgets
{
    public class href
    {
        [XmlAttribute("href")]
        public string Path
        {
            get => File?.FilePath;
            set
            {
                if (value != string.Empty) File = FileManager.CurrentWorkingManager.GetFile(value);
            }
        }

        [XmlIgnore] public IFile File { get; set; }
    }
}