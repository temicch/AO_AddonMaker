using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class TextsItem
    {
        [XmlElement("texts")]
        public Href Texts;

        [XmlElement("groupName")]
        public string GroupName { get; set; }
    }
}