using System.Collections.Generic;
using System.Xml.Serialization;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class Frame
    {
        [XmlElement("textureItem")] public Reference<XmlFileProvider> TextureItem { get; set; }
        [XmlArray("rects")] public List<Rect> Rects { get; set; }
    }
}