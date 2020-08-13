using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets
{
    public class WidgetDiscreteScrollBar : WidgetScrollBar
    {
        [XmlElement("slider")]
        public Reference<XmlFileProvider> Slider { get; set; }
    }
}