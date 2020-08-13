using System.Xml.Serialization;

namespace Application.BL.Widgets
{
    public class WidgetGlideSlider : WidgetSlider
    {
        public WidgetGlideSlider()
        {
            DiscreteStep = 10;
        }

        [XmlElement("discreteStep")] public int DiscreteStep { get; set; }
    }
}