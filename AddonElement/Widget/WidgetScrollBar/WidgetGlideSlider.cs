namespace AddonElement.Widgets
{
    public class WidgetGlideSlider : WidgetSlider
    {
        public WidgetGlideSlider()
        {
            discreteStep = 10;
        }

        public int discreteStep { get; set; }
    }
}