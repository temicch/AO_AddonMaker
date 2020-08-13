using System.Collections.Generic;
using System.Xml.Serialization;
using Application.BL.Widgets.TextView;

namespace Application.BL.Widgets
{
    public class WidgetTextView : Widget
    {
        public WidgetTextView()
        {
            MinWidth = 0.0f;
            MaxWidth = 0.0f;
            PickObjectsOnly = false;
            IsHtmlEscaping = false;
        }

        public string FormatFileRef { get; set; }
        public List<WidgetTextTaggedValue> TextValues { get; set; }
        public string DefaultTag { get; set; }
        public WidgetTextStyle TextStyle { get; set; }

        [XmlElement("minWidth")]
        public float MinWidth { get; set; }

        [XmlElement("maxWidth")]
        public float MaxWidth { get; set; }

        [XmlIgnore]
        public bool IsHtmlEscaping { get; set; }

        [XmlElement("isHtmlEscaping")]
        public string _IsHtmlEscaping
        {
            get => IsHtmlEscaping.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    IsHtmlEscaping = result;
            }
        }

        [XmlIgnore]
        public bool PickObjectsOnly { get; set; }

        [XmlElement("pickObjectsOnly")]
        public string _PickObjectsOnly
        {
            get => PickObjectsOnly.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    PickObjectsOnly = result;
            }
        }
    }
}