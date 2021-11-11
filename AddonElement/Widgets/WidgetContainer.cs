using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Files.Provider;
using Application.BL.Widgets.enums;

namespace Application.BL.Widgets;

public abstract class WidgetContainer : WidgetPanel
{
    [XmlElement("border")] public Reference<XmlFileProvider> Border { get; set; }

    [XmlElement("widgetsArrangement")] public WidgetsArrangement WidgetsArrangement { get; set; }

    protected override ImageSource GetBitmap()
    {
        return (Border?.File as Widget)?.Bitmap;
    }
}
