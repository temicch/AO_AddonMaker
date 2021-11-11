using System.Xml.Serialization;
using Application.BL.Widgets.enums;

namespace Application.BL.Widgets;

public class WidgetTextStyle
{
    public WidgetTextStyle()
    {
        WrapText = true;
        Ellipsis = true;
        LineSpacing = 0;
        Align = AlignY.ALIGNY_DEFAULT;
        BlendEffect = Blend_Effect.BLEND_EFFECT_ALPHABLND;
    }

    [XmlIgnore] public bool Multiline { get; set; }

    [XmlElement("multiline")]
    public string _Multiline
    {
        get => Multiline.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                Multiline = result;
        }
    }

    [XmlIgnore] public bool WrapText { get; set; }

    [XmlElement("wrapText")]
    public string _WrapText
    {
        get => WrapText.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                WrapText = result;
        }
    }

    [XmlIgnore] public bool ShowClippedSymbol { get; set; }

    [XmlElement("showClippedSymbol")]
    public string _ShowClippedSymbol
    {
        get => ShowClippedSymbol.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                ShowClippedSymbol = result;
        }
    }

    [XmlIgnore] public bool ShowClippedLine { get; set; }

    [XmlElement("showClippedLine")]
    public string _ShowClippedLine
    {
        get => ShowClippedLine.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                ShowClippedLine = result;
        }
    }

    [XmlIgnore] public bool Ellipsis { get; set; }

    [XmlElement("ellipsis")]
    public string _Ellipsis
    {
        get => Ellipsis.ToString().ToLower();
        set
        {
            if (bool.TryParse(value, out var result))
                Ellipsis = result;
        }
    }

    [XmlElement("lineSpacing")] public int LineSpacing { get; set; }

    public AlignY Align { get; set; }

    [XmlElement("blendEffect")] public Blend_Effect BlendEffect { get; set; }
}
