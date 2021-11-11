using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets;

public class WidgetTextContainer : WidgetContainer
{
    public WidgetTextContainer()
    {
        ElementsInterval = 0;
    }

    [XmlIgnore] public bool PickObjectsOnly { get; set; }

    [XmlElement("pickObjectsOnly")]
    public string _PickObjectsOnly
    {
        get => PickObjectsOnly.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                PickObjectsOnly = result;
        }
    }

    [XmlElement("scrollBar")] public Reference<XmlFileProvider> ScrollBar { get; set; }

    [XmlElement("formatFileRef")] public string FormatFileRef { get; set; }

    [XmlElement("defaultTag")] public string DefaultTag { get; set; }

    [XmlElement("elementsInterval")] public int ElementsInterval { get; set; }
}
