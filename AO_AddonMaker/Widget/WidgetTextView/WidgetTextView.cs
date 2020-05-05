using System.Collections.Generic;

namespace AO_AddonMaker
{
    public class WidgetTextView : Widget
    {
        public string FormatFileRef;
        public List<WidgetTextTaggedValue> TextValues;
        public string DefaultTag;
        public WidgetTextStyle TextStyle;
        public float minWidth;
        public float maxWidth;
        public bool pickObjectsOnly;
        public bool isHtmlEscaping;

        public WidgetTextView() : base()
        {
            minWidth = 0.0f;
            maxWidth = 0.0f;
            pickObjectsOnly = false;
            isHtmlEscaping = false;
        }
    }
}
