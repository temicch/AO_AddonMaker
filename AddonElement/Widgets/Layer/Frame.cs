using System.Collections.Generic;
using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Layer;

public class Frame
{
    [XmlElement("textureItem")] public Reference<XmlFileProvider> TextureItem { get; set; }

    [XmlArray("rects")] public List<Rect> Rects { get; set; }
}
