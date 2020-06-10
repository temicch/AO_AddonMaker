using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetButtonVariant
    {
        [XmlIgnore]
        public bool ReactionOnUp { get; set; }
        [XmlElement("ReactionOnUp")]
        public string _ReactionOnUp
        {
            get => ReactionOnUp.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                    ReactionOnUp = result;
            }
        }

        public string TextFileRef { get; set; }
        public href LayerHighlight { get; set; }

        public vec2 PushedOffset { get; set; }

        public WidgetButtonState StateNormal { get; set; }
        public WidgetButtonState StatePushed { get; set; }
        public WidgetButtonState StateHighlighted { get; set; }
        public WidgetButtonState StatePushedHighlighted { get; set; }
        public WidgetButtonState StateDisabled { get; set; }
        public WidgetSoundBase soundOver { get; set; }
        public WidgetSoundBase soundPress { get; set; }

        public WidgetButtonVariant()
        {
            ReactionOnUp = false;
        }
    }
}
