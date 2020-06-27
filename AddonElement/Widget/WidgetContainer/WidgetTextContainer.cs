using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetTextContainer : WidgetContainer
    {
        public WidgetTextContainer()
        {
            elementsInterval = 0;
        }

        [XmlIgnore] public bool pickObjectsOnly { get; set; }

        [XmlElement("pickObjectsOnly")]
        public string _pickObjectsOnly
        {
            get => pickObjectsOnly.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    pickObjectsOnly = result;
            }
        }

        public href scrollBar { get; set; }
        public string formatFileRef { get; set; }
        public string defaultTag { get; set; }
        public int elementsInterval { get; set; }
    }
}