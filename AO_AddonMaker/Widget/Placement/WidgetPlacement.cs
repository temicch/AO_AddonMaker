namespace AO_AddonMaker
{
    public class WidgetPlacement
    {
        public WidgetAlign Align;
        public WidgetSizing Sizing;
        public int Pos;
        public int HighPos;
        public int Size;
        public WidgetPlacement()
        {
            Align = WidgetAlign.WIDGET_ALIGN_LOW;
            Sizing = WidgetSizing.WIDGET_SIZING_DEFAULT;
        }
    }
}