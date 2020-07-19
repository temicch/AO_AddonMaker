﻿using System.Windows.Media;
using System.Xml.Serialization;
using Addon.Files;
using Textures;

namespace Addon.Widgets
{
    public class UITexture : File
    {
        [XmlElement("mipSW")]
        public int MipSW { get; set; }

        [XmlElement("mipsNumber")]
        public int MipsNumber { get; set; }

        [XmlIgnore] public bool GenerateMipChain { get; set; }

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
        public Href BinaryFile { get; set; }

        [XmlElement("binaryFileSize2")]
        public int BinaryFileSize2 { get; set; }

        [XmlElement("binaryFile2")]
        public Href BinaryFile2 { get; set; }

        [XmlElement("sourceFile")]
        public Href SourceFile { get; set; }

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

        public Href LocalizationInfo { get; set; }

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
            using (var binaryFileStream = new System.IO.StreamReader(BinaryFile.File.FullPath))
            {
                var texture = new Texture(binaryFileStream.BaseStream, Width, Height, Type);
                bitmap = texture.Bitmap;
            }

            return bitmap;
        }
    }
}