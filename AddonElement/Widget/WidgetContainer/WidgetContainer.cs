using System.Windows.Media;

namespace AddonElement.Widgets
{
    public abstract class WidgetContainer : WidgetPanel
    {
        public href border { get; set; }
        public WidgetsArrangement widgetsArrangement { get; set; }

        protected override ImageSource GetBitmap()
        {
            return (border?.File as Widget)?.Bitmap;
        }        
    }
}