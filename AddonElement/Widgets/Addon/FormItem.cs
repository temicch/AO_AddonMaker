using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Addon;

public class FormItem
{
    public Reference<XmlFileProvider> Form;
    public string Id { get; set; }
}
