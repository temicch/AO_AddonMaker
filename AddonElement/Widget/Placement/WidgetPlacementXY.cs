using System.Collections.Generic;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetPlacementXY
    {
        public bool QuantumScale { get; set; }
        public href sizingWidget { get; set; }

        [XmlArrayItem("Item")]
        public List<href> sizingWidgets { get; set; }

        public WidgetPlacement X { get; set; }
        public WidgetPlacement Y { get; set; }

        public WidgetPlacementXY()
        {
            QuantumScale = false;
            X = new WidgetPlacement();
            Y = new WidgetPlacement();
        }
    }
}