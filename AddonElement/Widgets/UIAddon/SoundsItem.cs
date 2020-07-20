using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class SoundsItem
    {
        [XmlElement("sounds")]
        public Href<BlankFile> Sounds;
        [XmlElement("groupName")]
        public string GroupName { get; set; }
    }
}