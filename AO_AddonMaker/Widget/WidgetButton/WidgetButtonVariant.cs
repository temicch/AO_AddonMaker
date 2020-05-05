namespace AO_AddonMaker
{
    public class WidgetButtonVariant
    {
        public string TextFileRef;
        public bool ReactionOnUp;
        public href LayerHighlight;

        public vec2 PushedOffset;

        public WidgetButtonState StateNormal;
        public WidgetButtonState StatePushed;
        public WidgetButtonState StateHighlighted;
        public WidgetButtonState StatePushedHighlighted;
        public WidgetButtonState StateDisabled;
        public WidgetSoundBase soundOver;
        public WidgetSoundBase soundPress;

        public WidgetButtonVariant()
        {
            ReactionOnUp = false;
        }
    }
}
