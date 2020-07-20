using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public abstract class WidgetSlider : Widget
    {
        [XmlElement("sliderButton")] public Reference<XmlFileProvider> SliderButton { get; set; }
        [XmlElement("moveArrangement")] public WidgetsArrangement MoveArrangement { get; set; }
    }
}