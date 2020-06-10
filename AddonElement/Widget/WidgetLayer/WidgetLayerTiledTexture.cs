using System.Windows.Media;

namespace AddonElement
{
    public class WidgetLayerTiledLayout
    {
        public int LeftX { get; set; }
        public int MiddleX { get; set; }
        public int RightX { get; set; }
        public int TopY { get; set; }
        public int MiddleY { get; set; }
        public int BottomY { get; set; }
    }

    public class WidgetLayerTiledTexture : WidgetLayer
    {
        public href textureItem { get; set; }

        public override ImageSource Bitmap
        {
            get => (textureItem?.File as UISingleTexture)?.Bitmap;
        }

        public WidgetLayerTiledLayout Layout { get; set; }

        public WidgetLayerTiledLayoutType layoutTypeX { get; set; }
        public WidgetLayerTiledLayoutType layoutTypeY { get; set; }
    }
}