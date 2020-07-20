using Addon.Files;
using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetDiscreteScrollBar : WidgetScrollBar
    {
        [XmlElement("slider")] public Reference<XmlFileProvider> Slider { get; set; }
    }
}