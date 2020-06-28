using System.Windows.Media;

namespace AddonElement
{
    public class WidgetLayerTiledTexture : WidgetLayer
    {
        public href textureItem { get; set; }

        public override ImageSource Bitmap => (textureItem?.File as UISingleTexture)?.Bitmap;

        public WidgetLayerTiledLayout Layout { get; set; }

        public WidgetLayerTiledLayoutType layoutTypeX { get; set; }
        public WidgetLayerTiledLayoutType layoutTypeY { get; set; }
    }
}