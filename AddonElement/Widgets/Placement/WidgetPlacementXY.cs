using System.Collections.Generic;
using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Placement;

public class WidgetPlacementXY
{
    public WidgetPlacementXY()
    {
        QuantumScale = false;
        X = new WidgetPlacement();
        Y = new WidgetPlacement();
    }

    [XmlIgnore] public bool QuantumScale { get; set; }

    [XmlElement("QuantumScale")]
    public string _QuantumScale
    {
        get => QuantumScale.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                QuantumScale = result;
        }
    }

    [XmlElement("sizingWidget")] public Reference<XmlFileProvider> SizingWidget { get; set; }

    [XmlArrayItem("Item")]
    [XmlArray("sizingWidgets")]
    public List<Reference<XmlFileProvider>> SizingWidgets { get; set; }

    public WidgetPlacement X { get; set; }
    public WidgetPlacement Y { get; set; }
}
