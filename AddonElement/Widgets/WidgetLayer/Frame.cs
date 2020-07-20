using System.Collections.Generic;
using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class Frame
    {
        [XmlElement("textureItem")]
        public Href<UISingleTexture> TextureItem { get; set; }
        [XmlArray("rects")]
        public List<Rect> Rects { get; set; }
    }
}