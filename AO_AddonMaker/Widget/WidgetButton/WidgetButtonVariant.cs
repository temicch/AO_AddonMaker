namespace AO_AddonMaker
{
    public class WidgetButtonVariant
    {
        public string TextFileRef { get; set; }
        public bool ReactionOnUp { get; set; }
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
