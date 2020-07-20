using System.Windows.Media;
using System.Xml.Serialization;
using Addon.Files;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public abstract class UITextureItem : File
    {
        [XmlElement("singleTexture")] public Reference<XmlFileProvider> SingleTexture { get; set; }

        [XmlElement("permanentCache")] public int PermanentCache { get; set; }

        [XmlElement("sourceFile")] public Reference<BlankFileProvider> SourceFile { get; set; }

        [XmlElement("sourceFileCRC")] public long SourceFileCrc { get; set; }

        public ImageSource Bitmap => (SingleTexture?.File as UITexture)?.Bitmap;
    }
}