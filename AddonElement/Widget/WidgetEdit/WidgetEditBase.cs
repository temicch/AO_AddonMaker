using System.Xml.Serialization;

namespace AddonElement
{
    abstract public class WidgetEditBase : Widget
    {
        [XmlIgnore]
        public bool canPaste { get; set; }
        [XmlElement("canPaste")]
        public string _canPaste
        {
            get => canPaste.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                    canPaste = result;
            }
        }

        public href Cursor1Layer { get; set; }
        public href Cursor2Layer { get; set; }
        public int CursorWidth { get; set; }
        public int CursorChangeTimeMs { get; set; }
        public int maxSymbolsCount { get; set; }
        public WidgetTextStyle TextStyle { get; set; }
        public string globalClassName { get; set; }
        public string selectionClassName { get; set; }
        public href selectionLayer { get; set; }
        //public string filterAlias: string - название фильтра, разрешающего только буквы, перечисленные в нём.Значения: "RUSSIAN", "NUMBERS", "INTEGER". См.EditBaseTextFilter

        public string ReactionEsc { get; set; }
        public string ReactionChanged { get; set; }
        public string reactionFocusChanged { get; set; }
        public string reactionPaste { get; set; }
        public string reactionCapsLock { get; set; }

        public WidgetEditBase()
        {
            CursorWidth = 2;
            CursorChangeTimeMs = 500;
            maxSymbolsCount = -1;
            canPaste = true;
        }
    }
}
