using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class WidgetScrollableContainer : WidgetContainer
    {
        public WidgetScrollableContainer()
        {
            ElementsInterval = 0;
        }

        [XmlElement("scrollBar")] public Reference<XmlFileProvider> ScrollBar { get; set; }

        [XmlElement("elementsInterval")] public int ElementsInterval { get; set; }
    }
}