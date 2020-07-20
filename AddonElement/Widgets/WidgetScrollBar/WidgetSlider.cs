using System.Xml.Serialization;

namespace Addon.Widgets
{
    public abstract class WidgetSlider : Widget
    {
        [XmlElement("sliderButton")]
        public Href<WidgetButton> SliderButton { get; set; }
        [XmlElement("moveArrangement")]
        public WidgetsArrangement MoveArrangement { get; set; }
    }
}