using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetTextStyle
    {
        public WidgetTextStyle()
        {
            wrapText = true;
            ellipsis = true;
            lineSpacing = 0;
            Align = AlignY.ALIGNY_DEFAULT;
            blendEffect = Blend_Effect.BLEND_EFFECT_ALPHABLND;
        }

        [XmlIgnore] public bool multiline { get; set; }

        [XmlElement("multiline")]
        public string _multiline
        {
            get => multiline.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    multiline = result;
            }
        }

        [XmlIgnore] public bool wrapText { get; set; }

        [XmlElement("wrapText")]
        public string _wrapText
        {
            get => wrapText.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    wrapText = result;
            }
        }

        [XmlIgnore] public bool showClippedSymbol { get; set; }

        [XmlElement("showClippedSymbol")]
        public string _showClippedSymbol
        {
            get => showClippedSymbol.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    showClippedSymbol = result;
            }
        }

        [XmlIgnore] public bool showClippedLine { get; set; }

        [XmlElement("showClippedLine")]
        public string _showClippedLine
        {
            get => showClippedLine.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    showClippedLine = result;
            }
        }

        [XmlIgnore] public bool ellipsis { get; set; }

        [XmlElement("ellipsis")]
        public string _ellipsis
        {
            get => ellipsis.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    ellipsis = result;
            }
        }

        public int lineSpacing { get; set; }
        public AlignY Align { get; set; }
        public Blend_Effect blendEffect { get; set; }
    }
}