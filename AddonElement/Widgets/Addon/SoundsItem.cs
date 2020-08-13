using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Addon
{
    public class SoundsItem
    {
        [XmlElement("sounds")] public Reference<BlankFileProvider> Sounds;
        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}