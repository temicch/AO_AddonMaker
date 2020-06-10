using System.Windows.Media;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        public href textureItem { get; set; }
        public href textureMask { get; set; }

        [XmlIgnore]
        public bool Scaling { get; set; }
        [XmlElement("Scaling")]
        public string _Scaling
        {
            get => Scaling.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out bool result))
                    Scaling = result;
            }
        }

        public override ImageSource Bitmap
        {
            get => (textureItem?.File as UISingleTexture)?.Bitmap;
        }

        public WidgetLayerSimpleTexture()
        {
            Scaling = false;
        }
    }
}