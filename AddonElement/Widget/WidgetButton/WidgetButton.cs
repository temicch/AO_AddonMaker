using System.Collections.Generic;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetButton : WidgetPanel
    {
        public string TextTag { get; set; }
        [XmlArrayItem("Item")]
        public List<WidgetButtonVariant> Variants { get; set; }
        public WidgetTextStyle TextStyle { get; set; }

        [XmlIgnore]
        public bool useDefaultSounds { get; set; }
        [XmlElement("useDefaultSounds")]
        public string _useDefaultSounds
        {
            get => useDefaultSounds.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                    useDefaultSounds = result;
            }
        }

        public List<BindSection> pushingBindSections { get; set; }

        public WidgetButton()
        {
            useDefaultSounds = true;
        }
    }
}
