using System.Windows.Media;
using System.Xml.Serialization;

namespace Addon.Widgets
{
    public abstract class WidgetContainer : WidgetPanel
    {
        [XmlElement("border")]
        public Href Border { get; set; }

        [XmlElement("widgetsArrangement")]
        public WidgetsArrangement WidgetsArrangement { get; set; }

        protected override ImageSource GetBitmap()
        {
            return (Border?.File as Widget)?.Bitmap;
        }
    }
}