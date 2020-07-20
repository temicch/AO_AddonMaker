using System.Xml.Serialization;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class TexturesItem
    {
        [XmlElement("textures")] public Reference<BlankFileProvider> Textures;
        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}