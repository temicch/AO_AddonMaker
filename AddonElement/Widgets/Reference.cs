using Application.BL.Files;
using Application.BL.Files.Provider;
using System.Xml.Serialization;

namespace Application.BL.Widgets
{
    /// <summary>
    /// Link to the specified file
    /// </summary>
    /// <typeparam name="T">File provider</typeparam>
    public class Reference<T> where T: IFileProvider, new()
    {
        /// <summary>
        /// The path to the file. It is important to consider that when the value changes, the file instance also changes
        /// </summary>
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
        /// <summary>
        /// File instance
        /// </summary>
        [XmlIgnore] 
        public IFile File { get; set; }
    }
}