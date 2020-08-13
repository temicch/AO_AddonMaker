using System.Xml.Serialization;
using Application.BL.Files.Provider;
using Application.BL.Widgets.enums;

namespace Application.BL.Widgets
{
    public abstract class WidgetSlider : Widget
    {
        [XmlElement("sliderButton")]
        public Reference<XmlFileProvider> SliderButton { get; set; }

        [XmlElement("moveArrangement")]
        public WidgetsArrangement MoveArrangement { get; set; }
    }
}