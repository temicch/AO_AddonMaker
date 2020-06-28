using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;

namespace AddonElement
{
    public class WidgetLayerAnimatedTexture : WidgetLayer
    {
        public WidgetLayerAnimatedTexture()
        {
            scaling = false;
            delayMs = 0;
            repeatForever = true;
            playImmidiatly = true;
        }

        [XmlIgnore] public bool playImmidiatly { get; set; }

        [XmlElement("playImmidiatly")]
        public string _playImmidiatly
        {
            get => playImmidiatly.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    playImmidiatly = result;
            }
        }

        [XmlIgnore] public bool repeatForever { get; set; }

        [XmlElement("repeatForever")]
        public string _repeatForever
        {
            get => repeatForever.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    repeatForever = result;
            }
        }

        [XmlIgnore] public bool scaling { get; set; }

        [XmlElement("scaling")]
        public string _scaling
        {
            get => scaling.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    scaling = result;
            }
        }

        public int delayMs { get; set; }
        public List<Frame> frames { get; set; }

        public override ImageSource Bitmap => throw new NotImplementedException();
    }
}