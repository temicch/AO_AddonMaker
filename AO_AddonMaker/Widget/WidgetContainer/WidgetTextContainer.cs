namespace AO_AddonMaker
{
    public class WidgetTextContainer : WidgetContainer
    {
        public href scrollBar { get; set; }
        public string formatFileRef { get; set; }
        public string defaultTag { get; set; }
        public int elementsInterval { get; set; }
        public bool pickObjectsOnly { get; set; }

        public WidgetTextContainer()
        {
            elementsInterval = 0;
        }
    }
}
