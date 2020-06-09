using System.Windows.Media;

namespace AddonElement
{
    public class WidgetLayerSimpleTexture : WidgetLayer
    {
        public href textureItem { get; set; }
        public href textureMask { get; set; }
        public bool Scaling { get; set; }

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