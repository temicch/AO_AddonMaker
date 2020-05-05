namespace AO_AddonMaker
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        public href textureItem;
        public href textureMask;
        public bool Scaling;

        public WidgetLayerSimpleTexture() : base()
        {
            Scaling = false;
        }
    }
}