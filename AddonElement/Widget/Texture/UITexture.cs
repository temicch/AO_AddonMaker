using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;
using Textures;

namespace AddonElement.Widgets
{
    public class UITexture : File.File
    {
        public int mipSW { get; set; }
        public int mipsNumber { get; set; }

        [XmlIgnore] public bool generateMipChain { get; set; }

        [XmlElement("generateMipChain")]
        public string _generateMipChain
        {
            get => generateMipChain.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    generateMipChain = result;
            }
        }

        public Format type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int realWidth { get; set; }
        public int realHeight { get; set; }

        [XmlIgnore] public bool disableLODControl { get; set; }

        [XmlElement("disableLODControl")]
        public string _disableLODControl
        {
            get => disableLODControl.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    disableLODControl = result;
            }
        }

        [XmlIgnore] public bool alphaTex { get; set; }

        [XmlElement("alphaTex")]
        public string _alphaTex
        {
            get => alphaTex.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    alphaTex = result;
            }
        }

        public int binaryFileSize { get; set; }
        public href binaryFile { get; set; }
        public int binaryFileSize2 { get; set; }
        public href binaryFile2 { get; set; }
        public href sourceFile { get; set; }
        public long sourceFileCRC { get; set; }


        [XmlIgnore] public bool wrap { get; set; }

        [XmlElement("wrap")]
        public string _wrap
        {
            get => wrap.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    wrap = result;
            }
        }
        //public href LocalizationInfo { get; set; }

        [XmlIgnore] public bool atlasPart { get; set; }

        [XmlElement("atlasPart")]
        public string _atlasPart
        {
            get => atlasPart.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    atlasPart = result;
            }
        }

        public string pool { get; set; }

        [XmlIgnore] protected ImageSource bitmap { get; set; }

        [XmlIgnore] public ImageSource Bitmap => GetBitmap();

        protected ImageSource GetBitmap()
        {
            if (bitmap == null)
                using (var binaryFileStream = new StreamReader(binaryFile.File.FullPath))
                {
                    var texture = new Texture(binaryFileStream.BaseStream, width, height, type);
                    bitmap = texture.Bitmap;
                }

            return bitmap;
        }
    }
}