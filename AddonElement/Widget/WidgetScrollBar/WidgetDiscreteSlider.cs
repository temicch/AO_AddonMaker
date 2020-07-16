namespace AddonElement.Widgets
{
    public class WidgetDiscreteSlider : WidgetSlider
    {
        public WidgetDiscreteSlider()
        {
            stepsCount = 0;
        }

        public int stepsCount { get; set; }
    }
}