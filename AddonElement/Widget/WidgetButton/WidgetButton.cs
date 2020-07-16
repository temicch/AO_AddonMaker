using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;

namespace AddonElement.Widgets
{
    public class WidgetButton : WidgetPanel
    {
        public WidgetButton()
        {
            useDefaultSounds = true;
        }

        public string TextTag { get; set; }

        [XmlArrayItem("Item")] public List<WidgetButtonVariant> Variants { get; set; }

        public WidgetTextStyle TextStyle { get; set; }

        [XmlIgnore] public bool useDefaultSounds { get; set; }

        [XmlElement("useDefaultSounds")]
        public string _useDefaultSounds
        {
            get => useDefaultSounds.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    useDefaultSounds = result;
            }
        }

        public List<BindSection> pushingBindSections { get; set; }

        public override ImageSource Bitmap
        {
            get
            {
                var backLayer = BackLayer?.File as WidgetLayer;
                if (backLayer?.Bitmap != null)
                    return backLayer.Bitmap;
                if (Variants?.Count > 0)
                    return (Variants?[0]?.StateNormal?.LayerMain?.File as WidgetLayer)?.Bitmap;
                return null;
            }
        }
    }
}