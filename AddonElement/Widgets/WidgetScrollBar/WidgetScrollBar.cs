using System.Xml.Serialization;

namespace Addon.Widgets
{
    public abstract class WidgetScrollBar : Widget
    {
        [XmlElement("decButton")]
        public Href DecButton { get; set; }
        [XmlElement("incButton")]
        public Href IncButton { get; set; }
    }
}