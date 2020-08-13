using System.IO;
using System.Windows.Media;
using System.Xml.Serialization;
using Application.BL.Files.Provider;
using Application.BL.Texture;
using File = Application.BL.Files.File;

namespace Application.BL.Widgets
{
    public class UITexture : File
    {
        [XmlElement("mipSW")]
        public int MipSW { get; set; }

        [XmlElement("mipsNumber")]
        public int MipsNumber { get; set; }

        [XmlIgnore]
        public bool GenerateMipChain { get; set; }

        [XmlElement("generateMipChain")]
        public string _GenerateMipChain
        {
            get => GenerateMipChain.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    GenerateMipChain = result;
            }
        }

        [XmlElement("type")]
        public Format Type { get; set; }

        [XmlElement("width")]
        public int Width { get; set; }

        [XmlElement("height")]
        public int Height { get; set; }

        [XmlElement("realWidth")]
        public int RealWidth { get; set; }

        [XmlElement("realHeight")]
        public int RealHeight { get; set; }

        [XmlIgnore]
        public bool DisableLODControl { get; set; }

        [XmlElement("disableLODControl")]
        public string _DisableLODControl
        {
            get => DisableLODControl.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    DisableLODControl = result;
            }
        }

        [XmlIgnore]
        public bool AlphaTex { get; set; }

        [XmlElement("alphaTex")]
        public string _AlphaTex
        {
            get => AlphaTex.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    AlphaTex = result;
            }
        }

        [XmlElement("binaryFileSize")]
        public int BinaryFileSize { get; set; }

        [XmlElement("binaryFile")]
        public Reference<BlankFileProvider> BinaryFile { get; set; }

        [XmlElement("binaryFileSize2")]
        public int BinaryFileSize2 { get; set; }

        [XmlElement("binaryFile2")]
        public Reference<BlankFileProvider> BinaryFile2 { get; set; }

        [XmlElement("sourceFile")]
        public Reference<BlankFileProvider> SourceFile { get; set; }

        [XmlElement("sourceFileCRC")]
        public long SourceFileCrc { get; set; }


        [XmlIgnore]
        public bool Wrap { get; set; }

        [XmlElement("wrap")]
        public string _Wrap
        {
            get => Wrap.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    Wrap = result;
            }
        }

        public Reference<BlankFileProvider> LocalizationInfo { get; set; }

        [XmlIgnore]
        public bool AtlasPart { get; set; }

        [XmlElement("atlasPart")]
        public string _AtlasPart
        {
            get => AtlasPart.ToString().ToLower();
            set
            {
                if (bool.TryParse(value, out var result))
                    AtlasPart = result;
            }
        }

        [XmlElement("pool")]
        public string Pool { get; set; }

        [XmlIgnore]
        protected ImageSource bitmap { get; set; }

        [XmlIgnore]
        public ImageSource Bitmap => GetBitmap();

        protected ImageSource GetBitmap()
        {
            if (bitmap != null)
                return bitmap;
            using (var binaryFileStream = new StreamReader(BinaryFile.File.FullPath))
            {
                var texture = new Texture.Texture(binaryFileStream.BaseStream, Width, Height, Type);
                bitmap = texture.Bitmap;
            }

            return bitmap;
        }
    }
}