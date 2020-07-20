using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public abstract class WidgetScrollBar : Widget
    {
        [XmlElement("decButton")] public Reference<XmlFileProvider> DecButton { get; set; }
        [XmlElement("incButton")] public Reference<XmlFileProvider> IncButton { get; set; }
    }
}