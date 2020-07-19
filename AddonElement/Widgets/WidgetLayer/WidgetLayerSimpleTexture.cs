using System.Windows.Media;
using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        public WidgetLayerSimpleTexture()
        {
            Scaling = false;
        }

        public Href textureItem { get; set; }
        public Href textureMask { get; set; }

        [XmlIgnore] public bool Scaling { get; set; }

        [XmlElement("Scaling")]
        public string _Scaling
        {
            get => Scaling.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    Scaling = result;
            }
        }

        public override ImageSource Bitmap => (textureItem?.File as UISingleTexture)?.Bitmap;
    }
}