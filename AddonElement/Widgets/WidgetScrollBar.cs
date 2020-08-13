using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets
{
    public abstract class WidgetScrollBar : Widget
    {
        [XmlElement("decButton")]
        public Reference<XmlFileProvider> DecButton { get; set; }

        [XmlElement("incButton")]
        public Reference<XmlFileProvider> IncButton { get; set; }
    }
}