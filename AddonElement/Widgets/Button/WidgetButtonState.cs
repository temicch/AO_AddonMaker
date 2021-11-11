using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Button;

public class WidgetButtonState
{
    public Reference<XmlFileProvider> LayerMain { get; set; }
    public string FormatFileRef { get; set; }
}
