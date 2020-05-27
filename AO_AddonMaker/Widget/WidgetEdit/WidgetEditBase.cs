namespace AO_AddonMaker
{
    abstract public class WidgetEditBase : Widget
    {
        public href Cursor1Layer;
        public href Cursor2Layer;
        public int CursorWidth;
        public int CursorChangeTimeMs;
        public int maxSymbolsCount;
        public bool canPaste;
        public WidgetTextStyle TextStyle;
        public string globalClassName;
        public string selectionClassName;
        public href selectionLayer;
        //public string filterAlias: string - название фильтра, разрешающего только буквы, перечисленные в нём.Значения: "RUSSIAN", "NUMBERS", "INTEGER". См.EditBaseTextFilter

        public string ReactionEsc;
        public string ReactionChanged;
        public string reactionFocusChanged;
        public string reactionPaste;
        public string reactionCapsLock;

        public WidgetEditBase()
        {
            CursorWidth = 2;
            CursorChangeTimeMs = 500;
            maxSymbolsCount = -1;
            canPaste = true;
        }
    }
}
