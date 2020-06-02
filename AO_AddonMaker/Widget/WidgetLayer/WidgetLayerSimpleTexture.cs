namespace AO_AddonMaker
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        public href textureItem { get; set; }
        public href textureMask { get; set; }
        public bool Scaling { get; set; }

        public WidgetLayerSimpleTexture()
        {
            Scaling = false;
        }
    }
}