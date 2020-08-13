using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets.Addon
{
    public class TexturesItem
    {
        [XmlElement("textures")] public Reference<BlankFileProvider> Textures;
        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}