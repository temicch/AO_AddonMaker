using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetScrollableContainer : WidgetContainer
    {
        public WidgetScrollableContainer()
        {
            ElementsInterval = 0;
        }

        [XmlElement("scrollBar")]
        public Href<WidgetGlideScrollBar> ScrollBar { get; set; }

        [XmlElement("elementsInterval")]
        public int ElementsInterval { get; set; }
    }
}