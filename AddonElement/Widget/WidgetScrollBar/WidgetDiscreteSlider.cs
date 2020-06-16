namespace AddonElement
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