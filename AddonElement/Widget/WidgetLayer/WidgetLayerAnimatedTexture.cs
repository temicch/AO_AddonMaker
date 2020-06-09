using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace AddonElement
{
    public class Rect
    {
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public int sizeX { get; set; }
        public int sizeY { get; set; }
        public float centerOffsetX { get; set; }
        public float centerOffsetY { get; set; }
    }

    public class Frame
    {
        public href textureItem { get; set; }
        public List<Rect> rects { get; set; }
    }

    public class WidgetLayerAnimatedTexture : WidgetLayer
    {
        public bool scaling { get; set; }
        public int delayMs { get; set; }
        public bool repeatForever { get; set; }
        public bool playImmidiatly { get; set; }
        public List<Frame> frames { get; set; }

        public override ImageSource Bitmap
        {
            get => throw new NotImplementedException();
        }

        public WidgetLayerAnimatedTexture()
        {
            scaling = false;
            delayMs = 0;
            repeatForever = true;
            playImmidiatly = true;
        }
    }
}