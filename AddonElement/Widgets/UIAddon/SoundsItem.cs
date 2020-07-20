using System.Xml.Serialization;
using Addon.Files.Provider;

namespace Addon.Widgets
{
    public class SoundsItem
    {
        [XmlElement("sounds")] public Reference<BlankFileProvider> Sounds;
        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}