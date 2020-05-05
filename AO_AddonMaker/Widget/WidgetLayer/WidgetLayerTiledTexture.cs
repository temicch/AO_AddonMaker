namespace AO_AddonMaker
{
    public class WidgetLayerTiledLayout
    {
        public int LeftX;
        public int MiddleX;
        public int RightX;
        public int TopY;
        public int MiddleY;
        public int BottomY;
    }

    public class WidgetLayerTiledTexture : WidgetLayer
    {
        public href textureItem;

        public WidgetLayerTiledLayout Layout;

        public WidgetLayerTiledLayoutType layoutTypeX;
        public WidgetLayerTiledLayoutType layoutTypeY;

        public WidgetLayerTiledTexture() : base()
        {

        }
    }
}