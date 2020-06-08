using System.Collections.Generic;

namespace AO_AddonMaker
{
    public class WidgetTextView : Widget
    {
        public string FormatFileRef { get; set; }
        public List<WidgetTextTaggedValue> TextValues { get; set; }
        public string DefaultTag { get; set; }
        public WidgetTextStyle TextStyle { get; set; }
        public float minWidth { get; set; }
        public float maxWidth { get; set; }
        public bool pickObjectsOnly { get; set; }
        public bool isHtmlEscaping { get; set; }

        public WidgetTextView()
        {
            minWidth = 0.0f;
            maxWidth = 0.0f;
            pickObjectsOnly = false;
            isHtmlEscaping = false;
        }
    }
}
