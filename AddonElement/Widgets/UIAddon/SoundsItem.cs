using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class SoundsItem
    {
        [XmlElement("sounds")] public Reference<BlankFileProvider> Sounds;
        [XmlElement("groupName")] public string GroupName { get; set; }
    }
}