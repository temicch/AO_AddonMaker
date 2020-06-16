using System.Xml.Serialization;

namespace AddonElement
{
    public class href
    {
        [XmlAttribute("href")]
        public string Path
        {
            get => File?.FilePath;
            set
            {
                if (value != string.Empty) File = FileManager.GetFile(value);
            }
        }

        [XmlIgnore] 
        public File File { get; set; }
    }
}