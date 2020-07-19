using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class SoundsItem
    {
        [XmlElement("sounds")]
        public Href Sounds;
        [XmlElement("groupName")]
        public string GroupName { get; set; }
    }
}