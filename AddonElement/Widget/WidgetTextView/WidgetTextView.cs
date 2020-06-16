using System.Collections.Generic;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetTextView : Widget
    {
        public WidgetTextView()
        {
            minWidth = 0.0f;
            maxWidth = 0.0f;
            pickObjectsOnly = false;
            isHtmlEscaping = false;
        }

        public string FormatFileRef { get; set; }
        public List<WidgetTextTaggedValue> TextValues { get; set; }
        public string DefaultTag { get; set; }
        public WidgetTextStyle TextStyle { get; set; }
        public float minWidth { get; set; }
        public float maxWidth { get; set; }

        [XmlIgnore] 
        public bool isHtmlEscaping { get; set; }

        [XmlElement("isHtmlEscaping")]
        public string _isHtmlEscaping
        {
            get => isHtmlEscaping.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    isHtmlEscaping = result;
            }
        }

        [XmlIgnore] 
        public bool pickObjectsOnly { get; set; } = true;

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
    }
}