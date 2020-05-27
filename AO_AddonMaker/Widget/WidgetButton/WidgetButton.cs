using System.Collections.Generic;

namespace AO_AddonMaker
{
    public class WidgetButton : Widget
    {
        public string TextTag;
        public List<WidgetButtonVariant> Variants;
        public WidgetTextStyle TextStyle;
        public bool useDefaultSounds;
        public List<BindSection> pushingBindSections;

        public WidgetButton()
        {
            useDefaultSounds = true;
        }
    }
}
