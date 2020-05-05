namespace AO_AddonMaker
{
    public class WidgetTextContainer : WidgetContainer
    {
        public href scrollBar;
        public string formatFileRef;
        public string defaultTag;
        public int elementsInterval;
        public bool pickObjectsOnly;

        public WidgetTextContainer() : base()
        {
            elementsInterval = 0;
        }
    }
}
