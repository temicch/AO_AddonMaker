using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetGlideScrollBar : WidgetScrollBar
    {
        [XmlElement("slider")]
        public Href Slider { get; set; }
    }
}