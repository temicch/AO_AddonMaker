using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class TextsItem
    {
        [XmlElement("texts")]
        public Href<BlankFile> Texts;

        [XmlElement("groupName")]
        public string GroupName { get; set; }
    }
}