using Application.BL.Files.Provider;
using Application.BL.Widgets.enums;
using System.Xml.Serialization;

namespace Application.BL.Widgets
{
    public abstract class WidgetSlider : Widget
    {
        [XmlElement("sliderButton")] public Reference<XmlFileProvider> SliderButton { get; set; }
        [XmlElement("moveArrangement")] public WidgetsArrangement MoveArrangement { get; set; }
    }
}