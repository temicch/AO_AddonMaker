using System.Collections.Generic;

namespace AO_AddonMaker
{
    public class WidgetButton : Widget
    {
        public string TextTag { get; set; }
        public List<WidgetButtonVariant> Variants { get; set; }
        public WidgetTextStyle TextStyle { get; set; }
        public bool useDefaultSounds { get; set; }
        public List<BindSection> pushingBindSections { get; set; }

        public WidgetButton()
        {
            useDefaultSounds = true;
        }
    }
}
