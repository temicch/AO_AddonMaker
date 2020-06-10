using System.Collections.Generic;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetPlacementXY
    {
        [XmlIgnore]
        public bool quantumScale { get; set; }
        [XmlElement("QuantumScale")]
        public string _quantumScale
        {
            get => quantumScale.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                    quantumScale = result;
            }
        }

        public href sizingWidget { get; set; }

        [XmlArrayItem("Item")]
        public List<href> sizingWidgets { get; set; }

        public WidgetPlacement X { get; set; }
        public WidgetPlacement Y { get; set; }

        public WidgetPlacementXY()
        {
            quantumScale = false;
            X = new WidgetPlacement();
            Y = new WidgetPlacement();
        }
    }
}