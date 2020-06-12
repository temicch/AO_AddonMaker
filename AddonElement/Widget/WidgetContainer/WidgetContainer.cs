using System.Windows.Media;

namespace AddonElement
{
    abstract public class WidgetContainer : WidgetPanel
    {
        public href border { get; set; }
        public WidgetsArrangement widgetsArrangement { get; set; }
        public WidgetContainer()
        {

        }
        public override ImageSource Bitmap => (border?.File as Widget)?.Bitmap;
    }
}
