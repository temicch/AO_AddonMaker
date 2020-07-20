using System.Xml.Serialization;

namespace Addon.Widgets
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