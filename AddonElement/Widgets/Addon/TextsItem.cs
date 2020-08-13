using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Addon
{
    public class TextsItem
    {
        [XmlElement("texts")] public Reference<BlankFileProvider> Texts;

        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}