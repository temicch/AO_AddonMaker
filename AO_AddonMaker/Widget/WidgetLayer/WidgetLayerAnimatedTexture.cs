using System.Collections.Generic;

namespace AO_AddonMaker
{
    public class Rect
    {
        public int offsetX;
        public int offsetY;
        public int sizeX;
        public int sizeY;
        public float centerOffsetX;
        public float centerOffsetY;
    }

    public class Frame
    {
        public href textureItem;
        public List<Rect> rects;
    }

    public class WidgetLayerAnimatedTexture : WidgetLayer
    {
        public bool scaling;
        public int delayMs;
        public bool repeatForever;
        public bool playImmidiatly;
        public List<Frame> frames;

        public WidgetLayerAnimatedTexture()
        {
            scaling = false;
            delayMs = 0;
            repeatForever = true;
            playImmidiatly = true;
        }
    }
}