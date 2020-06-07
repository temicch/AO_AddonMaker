using AndBurn.DDSReader;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Texture
{
    public class Texture
    {
        private readonly List<MipData> _mips = new List<MipData>();

        public const int DXT1_FOURCC = 827611204;
        public const int DXT3_FOURCC = 861165636;
        public const int DXT5_FOURCC = 894720068;

        //private bool _overrideSize;

        public List<Size> Resolutions { get; private set; } = new List<Size>();

        public int CurrentResolutionIndex
        {
            get
            {
                return Resolutions.FindIndex(s => s.Width == Width && s.Height == Height);
            }
        }

        public Format CurrentFormat { get; private set; }

        public int Width { get; private set; } = 1;
        public int Height { get; private set; } = 1;

        public int MipsCount
        {
            get
            {
                return _mips.Count;
            }
        }

        public void SetResolution(int index)
        {
            Size resolution = Resolutions[index];
            Width = resolution.Width;
            Height = resolution.Height;
            //_overrideSize = true;
        }

        public void AddMipData(int level, int size, byte[] data)
        {
            if (level >= _mips.Count)
            {
                for (int count = _mips.Count; count <= level; ++count)
                    _mips.Add(null);
            }
            _mips[level] = new MipData()
            {
                data = data,
                size = size
            };
        }

        public MipData GetMip(int level)
        {
            return _mips[level];
        }

        public void PatchMips()
        {
            while (_mips[0] == null)
                _mips.RemoveAt(0);
        }

        private void FillResolutionsMap()
        {
            Resolutions.Clear();
            int width1 = Width;
            int height1 = Height;
            int num;
            for (num = width1 * height1; width1 * height1 == num && width1 >= 4 && (height1 <= 4096 && (Utils.IsPowerOf2(width1) && Utils.IsPowerOf2(height1))); height1 *= 2)
            {
                Resolutions.Add(new Size(width1, height1));
                width1 /= 2;
            }
            int width2 = Width;
            for (int height2 = Height; width2 * height2 == num && width2 <= 4096 && (height2 >= 4 && (Utils.IsPowerOf2(width2) && Utils.IsPowerOf2(height2))); height2 /= 2)
            {
                Resolutions.Add(new Size(width2, height2));
                width2 *= 2;
            }
            Resolutions = Resolutions.OrderByDescending(s => s.Width).Distinct().ToList();
        }

        public Format DeductFormat()
        {
            int dxt3Count = 0;
            int dxt5Count = 0;
            for (int index = 0; index < this._mips.Count; ++index)
            {
                if (_mips[index] == null)
                    return Format.Unknown;
                DXTHeuristics.CountHits(_mips[index].data, ref dxt3Count, ref dxt5Count);
            }
            Format format = Format.DXT1;
            if (dxt3Count > 0 || dxt5Count > 0)
                format = dxt3Count > dxt5Count ? Format.DXT3 : Format.DXT5;
            return format;
        }

        private void PatchSize()
        {
            if (this._mips.Count > 1)
            {
                int maxSize = (int)Math.Pow(2.0, (double)(this._mips.Count + 1));
                int index = this.Resolutions.FindIndex((Predicate<Size>)(s => Math.Min(s.Width, s.Height) == maxSize));
                if (index == -1)
                    return;
                this.SetResolution(index);
                //this._overrideSize = false;
            }
            else
            {
                int index = Resolutions.Count / 2;
                if (Resolutions.Count % 2 == 0)
                    --index;
                SetResolution(index);
                //this._overrideSize = false;
            }
        }

        //public bool CalculateSize()
        //{
        //    if (this.CurrentFormat == Texture.Format.Unknown)
        //        return false;
        //    int num1 = this.CurrentFormat == Texture.Format.DXT1 ? this._mips[0].data.Length * 2 : this._mips[0].data.Length;
        //    if (!this._overrideSize)
        //    {
        //        int num2 = (int)Math.Sqrt((double)num1);
        //        if (Utils.IsPowerOf2(num2))
        //        {
        //            this.Width = num2;
        //            this.Height = num2;
        //        }
        //        else
        //        {
        //            int num3 = Utils.NextPowerOf2(num2);
        //            int x = num1 / num3;
        //            if (!Utils.IsPowerOf2(x))
        //                return false;
        //            this.Width = num3;
        //            this.Height = x;
        //        }
        //        this.FillResolutionsMap();
        //        this.PatchSize();
        //    }
        //    return true;
        //}

        public void SetFormat(Texture.Format fmt)
        {
            if (this.CurrentFormat == fmt)
                return;
            this.CurrentFormat = fmt;
            //this._overrideSize = false;
            //this.CalculateSize();
        }

        public static Texture Read(ZipEntry e, string path)
        {
            using (MemoryStream input = new MemoryStream())
            {
                e.Extract(input);
                return Read(Utils.UnZLib(input), path);
            }
        }

        public static Texture Read(Stream input, string path)
        {
            Texture texture = null;
            using (BinaryReader binaryReader = new BinaryReader(input))
            {
                texture = new Texture();
                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    int level = binaryReader.ReadInt32();
                    int num = binaryReader.ReadInt32();
                    texture.AddMipData(level, num, binaryReader.ReadBytes(num));
                }
            }
            texture.PatchMips();
            //int resolutionIndex = -1;
            //Texture.Format format = Texture.Format.Unknown;
            //if (Database.Instance.Get(path, out resolutionIndex, out format))
            //{
            //    texture.SetFormat(format);
            //    texture.SetResolution(resolutionIndex);
            //}
            //else
                texture.SetFormat(texture.DeductFormat());
            return texture;
        }

        public void SaveTo(Stream output)
        {
            BinaryWriter binaryWriter = new BinaryWriter(output);
            binaryWriter.Write(542327876);
            binaryWriter.Write(124);
            int num = 528391;
            if (this._mips.Count > 1)
                num |= 131072;
            binaryWriter.Write(num);
            binaryWriter.Write(this.Height);
            binaryWriter.Write(this.Width);
            binaryWriter.Write(this._mips[0].data.Length);
            binaryWriter.Write(0);
            binaryWriter.Write(this._mips.Count > 1 ? this._mips.Count : 0);
            for (int index = 0; index < 11; ++index)
                binaryWriter.Write(0);
            binaryWriter.Write(32);
            binaryWriter.Write(4);
            switch (this.CurrentFormat)
            {
                case Texture.Format.DXT1:
                    binaryWriter.Write(827611204);
                    break;
                case Texture.Format.DXT3:
                    binaryWriter.Write(861165636);
                    break;
                case Texture.Format.DXT5:
                    binaryWriter.Write(894720068);
                    break;
                default:
                    throw new Exception("Unknown format!");
            }
            for (int index = 0; index < 5; ++index)
                binaryWriter.Write(0);
            binaryWriter.Write(4096);
            for (int index = 0; index < 4; ++index)
                binaryWriter.Write(0);
            for (int index = 0; index < this._mips.Count; ++index)
                binaryWriter.Write(this._mips[index].data);
            binaryWriter.Flush();
        }

        //public XmlElement CreateElementWithValue(
        //  XmlDocument doc,
        //  XmlNode parent,
        //  string name,
        //  string value)
        //{
        //    XmlElement element = doc.CreateElement(name);
        //    parent.AppendChild((XmlNode)element);
        //    element.InnerText = value;
        //    return element;
        //}

        //public void ExportTo(string filename)
        //{
        //    string str1 = Path.ChangeExtension(filename, "");
        //    string str2 = str1 + "(UITexture).bin";
        //    string str3 = str1 + "(UITexture).xdb";
        //    string singleTexturePath = str1 + "(UISingleTexture).xdb";
        //    int mipsCount = 0;
        //    long binSize = 0;
        //    using (FileStream fileStream = new FileStream(str2, FileMode.Create))
        //    {
        //        using (BinaryWriter binaryWriter = new BinaryWriter((Stream)fileStream))
        //        {
        //            for (int index = 0; index < this._mips.Count; ++index)
        //            {
        //                if (this._mips[index] != null)
        //                {
        //                    binaryWriter.Write(mipsCount++);
        //                    binaryWriter.Write(this._mips[index].size);
        //                    binaryWriter.Write(this._mips[index].data);
        //                }
        //            }
        //            binSize = fileStream.Position;
        //        }
        //    }
        //    this.WriteBaseXdb(str3, str2, binSize, mipsCount);
        //    this.WriteSingleTextureXdb(singleTexturePath, str3);
        //}

        //private void WriteBaseXdb(string xdbPath, string binPath, long binSize, int mipsCount)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.AppendChild((XmlNode)doc.CreateXmlDeclaration("1.0", "UTF-8", (string)null));
        //    XmlNode parent = doc.AppendChild((XmlNode)doc.CreateElement("UITexture"));
        //    this.CreateElementWithValue(doc, parent, "mipSW", "-1");
        //    this.CreateElementWithValue(doc, parent, "mipsNumber", mipsCount.ToString());
        //    this.CreateElementWithValue(doc, parent, "generateMipChain", "false");
        //    this.CreateElementWithValue(doc, parent, "type", this.CurrentFormat.ToString());
        //    this.CreateElementWithValue(doc, parent, "width", this.Width.ToString());
        //    this.CreateElementWithValue(doc, parent, "height", this.Height.ToString());
        //    this.CreateElementWithValue(doc, parent, "realWidth", this.Width.ToString());
        //    this.CreateElementWithValue(doc, parent, "realHeight", this.Height.ToString());
        //    this.CreateElementWithValue(doc, parent, "disableLODControl", "false");
        //    this.CreateElementWithValue(doc, parent, "alphaTex", "true");
        //    this.CreateElementWithValue(doc, parent, "binaryFileSize", binSize.ToString());
        //    XmlElement element1 = doc.CreateElement("binaryFile");
        //    parent.AppendChild((XmlNode)element1);
        //    element1.SetAttribute("href", Path.GetFileName(binPath));
        //    this.CreateElementWithValue(doc, parent, "binaryFileSize2", "0");
        //    XmlElement element2 = doc.CreateElement("binaryFile2");
        //    parent.AppendChild((XmlNode)element2);
        //    element2.SetAttribute("href", "");
        //    this.CreateElementWithValue(doc, parent, "wrap", "false");
        //    parent.AppendChild((XmlNode)doc.CreateElement("LocalizationInfo"));
        //    this.CreateElementWithValue(doc, parent, "atlasPart", "true");
        //    this.CreateElementWithValue(doc, parent, "pool", "UNDEFINED");
        //    doc.Save(xdbPath);
        //}

        //private void WriteSingleTextureXdb(string singleTexturePath, string baseXdbPath)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.AppendChild((XmlNode)doc.CreateXmlDeclaration("1.0", "UTF-8", (string)null));
        //    XmlNode parent = doc.AppendChild((XmlNode)doc.CreateElement("UISingleTexture"));
        //    XmlElement element = doc.CreateElement("singleTexture");
        //    parent.AppendChild((XmlNode)element);
        //    element.SetAttribute("href", Path.GetFileName(baseXdbPath) + "#xpointer(/UITexture)");
        //    this.CreateElementWithValue(doc, parent, "permanentCache", "0");
        //    doc.Save(singleTexturePath);
        //}

        public Bitmap GetBitmap()
        {
            using (MemoryStream memoryStream = new MemoryStream(this._mips[0].size + this._mips[0].size / 2 + 150))
            {
                SaveTo(memoryStream);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                return DDS.LoadImage(memoryStream, true);
            }
        }

        public enum Format
        {
            Unknown,
            DXT1,
            DXT3,
            DXT5,
        }

        public class MipData
        {
            public int size;
            public byte[] data;
        }
    }

}
