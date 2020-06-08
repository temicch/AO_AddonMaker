namespace AddonElement
{
    public class WidgetPlacement
    {
        public WidgetAlign Align { get; set; }
        public WidgetSizing Sizing { get; set; }
        public int Pos { get; set; }
        public int HighPos { get; set; }
        public int Size { get; set; }
        public WidgetPlacement()
        {
            Align = WidgetAlign.WIDGET_ALIGN_LOW;
            Sizing = WidgetSizing.WIDGET_SIZING_DEFAULT;
        }
    }
}