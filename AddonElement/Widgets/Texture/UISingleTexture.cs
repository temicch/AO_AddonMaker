using System.Windows.Media;
using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class UISingleTexture : File
    {
        [XmlElement("singleTexture")]
        public Href SingleTexture { get; set; }

        [XmlElement("permanentCache")]
        public int PermanentCache { get; set; }

        [XmlElement("sourceFile")]
        public Href SourceFile { get; set; }

        [XmlElement("sourceFileCRC")]
        public long SourceFileCrc { get; set; }

        public ImageSource Bitmap => (SingleTexture?.File as UITexture)?.Bitmap;
    }
}