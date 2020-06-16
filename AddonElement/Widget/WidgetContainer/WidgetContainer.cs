using System.Windows.Media;

namespace AddonElement
{
    public abstract class WidgetContainer : WidgetPanel
    {
        public href border { get; set; }
        public WidgetsArrangement widgetsArrangement { get; set; }
        public override ImageSource Bitmap => (border?.File as Widget)?.Bitmap;
    }
}