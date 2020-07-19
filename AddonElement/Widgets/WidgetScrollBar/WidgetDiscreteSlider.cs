using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetDiscreteSlider : WidgetSlider
    {
        public WidgetDiscreteSlider()
        {
            StepsCount = 0;
        }

        [XmlElement("stepsCount")]
        public int StepsCount { get; set; }
    }
}