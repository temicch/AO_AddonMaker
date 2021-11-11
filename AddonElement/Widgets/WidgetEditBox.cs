using Application.BL.Files.Provider;

namespace Application.BL.Widgets;

public class WidgetEditBox : WidgetEditBase
{
    public Reference<XmlFileProvider> scrollBar { get; set; }
}
