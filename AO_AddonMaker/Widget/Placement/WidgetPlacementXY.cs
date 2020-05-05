using System.Collections.Generic;
using System.Xml.Serialization;

namespace AO_AddonMaker
{
    public class WidgetPlacementXY
    {
        public bool QuantumScale;
        public href sizingWidget;

        [XmlArrayItem("Item")]
        public List<href> sizingWidgets;

        public WidgetPlacement X;
        public WidgetPlacement Y;

        public WidgetPlacementXY()
        {
            QuantumScale = false;
            X = new WidgetPlacement();
            Y = new WidgetPlacement();
        }
    }
}