﻿using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetButtonVariant
    {
        public WidgetButtonVariant()
        {
            ReactionOnUp = false;
        }

        [XmlIgnore] 
        public bool ReactionOnUp { get; set; }

        [XmlElement("ReactionOnUp")]
        public string _ReactionOnUp
        {
            get => ReactionOnUp.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    ReactionOnUp = result;
            }
        }

        public string TextFileRef { get; set; }
        public Href LayerHighlight { get; set; }

        public vec2 PushedOffset { get; set; }

        public WidgetButtonState StateNormal { get; set; }
        public WidgetButtonState StatePushed { get; set; }
        public WidgetButtonState StateHighlighted { get; set; }
        public WidgetButtonState StatePushedHighlighted { get; set; }
        public WidgetButtonState StateDisabled { get; set; }

        [XmlElement("soundOver")]
        public WidgetSoundBase SoundOver { get; set; }

        [XmlElement("soundPress")]
        public WidgetSoundBase SoundPress { get; set; }
    }
}