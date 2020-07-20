using System;
using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class Href<T> where T: IFile
    {
        //[XmlIgnore]
        //public T Reference { get; set; }

        [XmlAttribute("href")]
        public string Path
        {
            get => File?.FilePath;
            set
            {
                if (value == string.Empty)
                    return;
                File = FileManager.CurrentWorkingManager.GetFile(value, typeof(T));
            }
        }

        [XmlIgnore] 
        public IFile File { get; set; }
    }
}