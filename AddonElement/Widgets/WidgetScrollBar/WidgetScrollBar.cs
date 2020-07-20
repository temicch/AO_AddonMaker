using System.Xml.Serialization;

namespace Addon.Widgets
{
    public abstract class WidgetScrollBar : Widget
    {
        [XmlElement("decButton")]
        public Href<WidgetButton> DecButton { get; set; }
        [XmlElement("incButton")]
        public Href<WidgetButton> IncButton { get; set; }
    }
}