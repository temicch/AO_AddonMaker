using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetDiscreteScrollBar : WidgetScrollBar
    {
        [XmlElement("slider")]
        public Href Slider { get; set; }
    }
}