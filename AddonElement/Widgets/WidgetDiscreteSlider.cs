using System.Xml.Serialization;

namespace Application.BL.Widgets
{
    public class WidgetDiscreteSlider : WidgetSlider
    {
        public WidgetDiscreteSlider()
        {
            StepsCount = 0;
        }

        [XmlElement("stepsCount")] public int StepsCount { get; set; }
    }
}