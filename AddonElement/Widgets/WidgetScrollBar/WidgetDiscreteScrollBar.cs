using System.Xml.Serialization;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class WidgetDiscreteScrollBar : WidgetScrollBar
    {
        [XmlElement("slider")] public Reference<XmlFileProvider> Slider { get; set; }
    }
}