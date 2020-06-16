using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetTextStyle
    {
        [XmlIgnore] 
        public bool multiline { get; set; }

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

        [XmlIgnore] 
        public bool wrapText { get; set; } = true;

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

        [XmlIgnore] 
        public bool showClippedSymbol { get; set; }

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

        [XmlIgnore] 
        public bool showClippedLine { get; set; }

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

        [XmlIgnore] 
        public bool ellipsis { get; set; } = true;

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

        public int lineSpacing { get; set; } = 0;
        public AlignY Align { get; set; } = AlignY.ALIGNY_DEFAULT;
        public Blend_Effect blendEffect { get; set; } = Blend_Effect.BLEND_EFFECT_ALPHABLND;
    }
}