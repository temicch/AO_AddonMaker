using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class TexturesItem
    {
        [XmlElement("textures")]
        public Href Textures;
        [XmlElement("groupName")]
        public string GroupName { get; set; }
    }
}