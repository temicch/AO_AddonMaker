using System.Windows.Media;
using System.Xml.Serialization;
using Addon.Files;

namespace Addon.Widgets
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        public WidgetLayerSimpleTexture()
        {
            Scaling = false;
        }

        public Reference<XmlFileProvider> textureItem { get; set; }
        public Reference<XmlFileProvider> textureMask { get; set; }

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

        public override ImageSource Bitmap
        {
            get
            {
                var file = textureItem?.File as UISingleTexture;
                return file?.Bitmap;
            }
        }
    }
}