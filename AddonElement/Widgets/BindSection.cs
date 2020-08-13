using System.Collections.Generic;
using System.Xml.Serialization;

namespace Application.BL.Widgets
{
    public class BindSection
    {
        [XmlElement("bindSection")] public string Name { get; set; }

        [XmlArray("bindedReactions")] public List<string> BindedReactions { get; set; }
    }
}