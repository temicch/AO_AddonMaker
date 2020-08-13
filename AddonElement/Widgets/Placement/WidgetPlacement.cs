using Application.BL.Widgets.enums;

namespace Application.BL.Widgets.Placement
{
    public class WidgetPlacement
    {
        public WidgetPlacement()
        {
            Align = WidgetAlign.WIDGET_ALIGN_LOW;
            Sizing = WidgetSizing.WIDGET_SIZING_DEFAULT;
        }

        public WidgetAlign Align { get; set; }
        public WidgetSizing Sizing { get; set; }
        public int Pos { get; set; }
        public int HighPos { get; set; }
        public int Size { get; set; }
    }
}