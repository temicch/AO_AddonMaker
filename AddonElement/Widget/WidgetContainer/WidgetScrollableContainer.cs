namespace AddonElement
{
    public class WidgetScrollableContainer : WidgetContainer
    {
        public href scrollBar { get; set; }
        public int elementsInterval { get; set; }

        public WidgetScrollableContainer()
        {
            elementsInterval = 0;
        }
    }
}
