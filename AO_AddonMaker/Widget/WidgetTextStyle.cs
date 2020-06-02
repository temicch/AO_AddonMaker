namespace AO_AddonMaker
{
    public class WidgetTextStyle
    {
        public bool multiline { get; set; } = false;
        public bool wrapText { get; set; } = true;
        public bool showClippedSymbol { get; set; } = false;
        public bool showClippedLine { get; set; } = false;
        public int lineSpacing { get; set; } = 0;
        public bool ellipsis { get; set; } = true;
        public AlignY Align { get; set; } = AlignY.ALIGNY_DEFAULT;
        public Blend_Effect blendEffect { get; set; } = Blend_Effect.BLEND_EFFECT_ALPHABLND;
    }
}
