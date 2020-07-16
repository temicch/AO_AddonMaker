namespace AddonElement.Widgets
{
    public class WidgetScrollableContainer : WidgetContainer
    {
        public WidgetScrollableContainer()
        {
            elementsInterval = 0;
        }

        public href scrollBar { get; set; }
        public int elementsInterval { get; set; }
    }
}