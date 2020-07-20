using System.Windows.Media;
using System.Xml.Serialization;

namespace Addon.Widgets
{
    public class WidgetLayerTiledTexture : WidgetLayer
    {
        [XmlElement("textureItem")]
        public Href<UITextureItem> TextureItem { get; set; }

        public override ImageSource Bitmap => (TextureItem?.File as UISingleTexture)?.Bitmap;

        public WidgetLayerTiledLayout Layout { get; set; }

        [XmlElement("layoutTypeX")]
        public WidgetLayerTiledLayoutType LayoutTypeX { get; set; }
        [XmlElement("layoutTypeY")]
        public WidgetLayerTiledLayoutType LayoutTypeY { get; set; }
    }
}