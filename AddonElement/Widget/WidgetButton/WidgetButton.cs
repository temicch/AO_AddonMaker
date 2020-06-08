using System.Collections.Generic;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetButton : Widget
    {
        public string TextTag { get; set; }
        [XmlArrayItem("Item")]
        public List<WidgetButtonVariant> Variants { get; set; }
        public WidgetTextStyle TextStyle { get; set; }
        public bool useDefaultSounds { get; set; }
        public List<BindSection> pushingBindSections { get; set; }

        public WidgetButton()
        {
            useDefaultSounds = true;
        }
    }
}
