using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Widgets.Button;

namespace Application.BL.Widgets
{
    public class WidgetButton : WidgetPanel
    {
        public WidgetButton()
        {
            UseDefaultSounds = true;
        }

        public string TextTag { get; set; }

        [XmlArrayItem("Item")]
        public List<WidgetButtonVariant> Variants { get; set; }

        public WidgetTextStyle TextStyle { get; set; }

        [XmlIgnore]
        public bool UseDefaultSounds { get; set; }

        [XmlElement("useDefaultSounds")]
        public string _UseDefaultSounds
        {
            get => UseDefaultSounds.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    UseDefaultSounds = result;
            }
        }

        [XmlArray("pushingBindSections")]
        public List<BindSection> PushingBindSections { get; set; }

        protected override ImageSource GetBitmap()
        {
            var backLayer = BackLayer?.File as WidgetLayer;
            if (backLayer?.Bitmap != null)
                return backLayer.Bitmap;
            return Variants?.Count > 0 ? (Variants?[0]?.StateNormal?.LayerMain?.File as WidgetLayer)?.Bitmap : null;
        }
    }
}