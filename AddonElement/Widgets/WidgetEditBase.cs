using System.Xml.Serialization;
using Application.BL.Files.Provider;

namespace Application.BL.Widgets
{
    public abstract class WidgetEditBase : Widget
    {
        public WidgetEditBase()
        {
            CursorWidth = 2;
            CursorChangeTimeMs = 500;
            MaxSymbolsCount = -1;
            CanPaste = true;
        }

        [XmlIgnore]
        public bool CanPaste { get; set; }

        [XmlElement("canPaste")]
        public string _CanPaste
        {
            get => CanPaste.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    CanPaste = result;
            }
        }

        public Reference<XmlFileProvider> Cursor1Layer { get; set; }
        public Reference<XmlFileProvider> Cursor2Layer { get; set; }
        public int CursorWidth { get; set; }
        public int CursorChangeTimeMs { get; set; }

        [XmlElement("maxSymbolsCount")]
        public int MaxSymbolsCount { get; set; }

        public WidgetTextStyle TextStyle { get; set; }

        [XmlElement("globalClassName")]
        public string GlobalClassName { get; set; }

        [XmlElement("selectionClassName")]
        public string SelectionClassName { get; set; }

        [XmlElement("selectionLayer")]
        public Reference<XmlFileProvider> SelectionLayer { get; set; }

        public string ReactionEsc { get; set; }
        public string ReactionChanged { get; set; }

        [XmlElement("reactionFocusChanged")]
        public string ReactionFocusChanged { get; set; }

        [XmlElement("reactionPaste")]
        public string ReactionPaste { get; set; }

        [XmlElement("reactionCapsLock")]
        public string ReactionCapsLock { get; set; }
    }
}