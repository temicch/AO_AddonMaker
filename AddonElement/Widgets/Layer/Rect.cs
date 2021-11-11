using System.Xml.Serialization;

namespace Application.BL.Widgets.Layer;

public class Rect
{
    [XmlElement("offsetX")] public int OffsetX { get; set; }

    [XmlElement("offsetY")] public int OffsetY { get; set; }

    [XmlElement("sizeX")] public int SizeX { get; set; }

    [XmlElement("sizeY")] public int SizeY { get; set; }

    [XmlElement("centerOffsetX")] public float CenterOffsetX { get; set; }

    [XmlElement("centerOffsetY")] public float CenterOffsetY { get; set; }
}
