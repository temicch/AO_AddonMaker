using AndBurn.DDSReader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Texture
{
    public class Texture
    {
        private readonly List<MipData> _mips = new List<MipData>();

        //public List<Size> Resolutions { get; private set; } = new List<Size>();

        //public int CurrentResolutionIndex
        //{
        //    get
        //    {
        //        return Resolutions.FindIndex(s => s.Width == Width && s.Height == Height);
        //    }
        //}

        public Format TextureFormat { get; private set; }
        public int Width { get; private set; } = 1;
        public int Height { get; private set; } = 1;

        public int MipsCount
        {
            get
            {
                return _mips.Count;
            }
        }

        //public void SetResolution(int index)
        //{
        //    Size resolution = Resolutions[index];
        //    Width = resolution.Width;
        //    Height = resolution.Height;
        //}

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

        //private void FillResolutionsMap()
        //{
        //    Resolutions.Clear();
        //    int width1 = Width;
        //    int height1 = Height;
        //    int num;
        //    for (num = width1 * height1; width1 * height1 == num && width1 >= 4 && (height1 <= 4096 && (Utils.IsPowerOf2(width1) && Utils.IsPowerOf2(height1))); height1 *= 2)
        //    {
        //        Resolutions.Add(new Size(width1, height1));
        //        width1 /= 2;
        //    }
        //    int width2 = Width;
        //    for (int height2 = Height; width2 * height2 == num && width2 <= 4096 && (height2 >= 4 && (Utils.IsPowerOf2(width2) && Utils.IsPowerOf2(height2))); height2 /= 2)
        //    {
        //        Resolutions.Add(new Size(width2, height2));
        //        width2 *= 2;
        //    }
        //    Resolutions = Resolutions.OrderByDescending(s => s.Width).Distinct().ToList();
        //}

        //public Format DeductFormat()
        //{
        //    int dxt3Count = 0;
        //    int dxt5Count = 0;
        //    for (int index = 0; index < _mips.Count; ++index)
        //    {
        //        if (_mips[index] == null)
        //            return Format.Unknown;
        //        DXTHeuristics.CountHits(_mips[index].data, ref dxt3Count, ref dxt5Count);
        //    }
        //    Format format = Format.DXT1;
        //    if (dxt3Count > 0 || dxt5Count > 0)
        //        format = dxt3Count > dxt5Count ? Format.DXT3 : Format.DXT5;
        //    return format;
        //}

        //private void PatchSize()
        //{
        //    if (_mips.Count > 1)
        //    {
        //        int maxSize = (int)Math.Pow(2.0, (double)(_mips.Count + 1));
        //        int index = Resolutions.FindIndex((Predicate<Size>)(s => Math.Min(s.Width, s.Height) == maxSize));
        //        if (index == -1)
        //            return;
        //        SetResolution(index);
        //    }
        //    else
        //    {
        //        int index = Resolutions.Count / 2;
        //        if (Resolutions.Count % 2 == 0)
        //            --index;
        //        SetResolution(index);
        //    }
        //}

        //public bool CalculateSize()
        //{
        //    if (CurrentFormat == Texture.Format.Unknown)
        //        return false;
        //    int num1 = CurrentFormat == Texture.Format.DXT1 ? _mips[0].data.Length * 2 : _mips[0].data.Length;
        //    if (!_overrideSize)
        //    {
        //        int num2 = (int)Math.Sqrt((double)num1);
        //        if (Utils.IsPowerOf2(num2))
        //        {
        //            Width = num2;
        //            Height = num2;
        //        }
        //        else
        //        {
        //            int num3 = Utils.NextPowerOf2(num2);
        //            int x = num1 / num3;
        //            if (!Utils.IsPowerOf2(x))
        //                return false;
        //            Width = num3;
        //            Height = x;
        //        }
        //        FillResolutionsMap();
        //        PatchSize();
        //    }
        //    return true;
        //}

        //public void SetFormat(Format fmt)
        //{
        //    if (TextureFormat == fmt)
        //        return;
        //    TextureFormat = fmt;
        //    //CalculateSize();
        //}

        //public static Texture Read(ZipEntry e)
        //{
        //    using (MemoryStream input = new MemoryStream())
        //    {
        //        e.Extract(input);
        //        return Read(Utils.UnZLib(input));
        //    }
        //}

        public Texture(Stream binaryFileStream, int realWidth, int realHeight, Format type)
        {
            Read(Utils.UnZLib(binaryFileStream));
            Width = realWidth;
            Height = realHeight;
            TextureFormat = type;
            GetBitmap();
        }

        public void Read(Stream input)
        {
            using (BinaryReader binaryReader = new BinaryReader(input))
            {
                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    int level = binaryReader.ReadInt32();
                    int num = binaryReader.ReadInt32();
                    AddMipData(level, num, binaryReader.ReadBytes(num));
                }
            }
            PatchMips();
        }

        public void SaveTo(Stream output)
        {
            BinaryWriter binaryWriter = new BinaryWriter(output);
            binaryWriter.Write(542327876);
            binaryWriter.Write(124);
            int num = 528391;
            if (_mips.Count > 1)
                num |= 131072;
            binaryWriter.Write(num);
            binaryWriter.Write(Height);
            binaryWriter.Write(Width);
            binaryWriter.Write(_mips[0].data.Length);
            binaryWriter.Write(0);
            binaryWriter.Write(_mips.Count > 1 ? _mips.Count : 0);
            for (int index = 0; index < 11; ++index)
                binaryWriter.Write(0);
            binaryWriter.Write(32);
            binaryWriter.Write(4);
            switch (TextureFormat)
            {
                case Format.DXT1:
                    binaryWriter.Write(827611204);
                    break;
                case Format.DXT3:
                    binaryWriter.Write(861165636);
                    break;
                case Format.DXT5:
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
            for (int index = 0; index < _mips.Count; ++index)
                binaryWriter.Write(_mips[index].data);
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
        //            for (int index = 0; index < _mips.Count; ++index)
        //            {
        //                if (_mips[index] != null)
        //                {
        //                    binaryWriter.Write(mipsCount++);
        //                    binaryWriter.Write(_mips[index].size);
        //                    binaryWriter.Write(_mips[index].data);
        //                }
        //            }
        //            binSize = fileStream.Position;
        //        }
        //    }
        //    WriteBaseXdb(str3, str2, binSize, mipsCount);
        //    WriteSingleTextureXdb(singleTexturePath, str3);
        //}

        //private void WriteBaseXdb(string xdbPath, string binPath, long binSize, int mipsCount)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.AppendChild((XmlNode)doc.CreateXmlDeclaration("1.0", "UTF-8", (string)null));
        //    XmlNode parent = doc.AppendChild((XmlNode)doc.CreateElement("UITexture"));
        //    CreateElementWithValue(doc, parent, "mipSW", "-1");
        //    CreateElementWithValue(doc, parent, "mipsNumber", mipsCount.ToString());
        //    CreateElementWithValue(doc, parent, "generateMipChain", "false");
        //    CreateElementWithValue(doc, parent, "type", CurrentFormat.ToString());
        //    CreateElementWithValue(doc, parent, "width", Width.ToString());
        //    CreateElementWithValue(doc, parent, "height", Height.ToString());
        //    CreateElementWithValue(doc, parent, "realWidth", Width.ToString());
        //    CreateElementWithValue(doc, parent, "realHeight", Height.ToString());
        //    CreateElementWithValue(doc, parent, "disableLODControl", "false");
        //    CreateElementWithValue(doc, parent, "alphaTex", "true");
        //    CreateElementWithValue(doc, parent, "binaryFileSize", binSize.ToString());
        //    XmlElement element1 = doc.CreateElement("binaryFile");
        //    parent.AppendChild((XmlNode)element1);
        //    element1.SetAttribute("href", Path.GetFileName(binPath));
        //    CreateElementWithValue(doc, parent, "binaryFileSize2", "0");
        //    XmlElement element2 = doc.CreateElement("binaryFile2");
        //    parent.AppendChild((XmlNode)element2);
        //    element2.SetAttribute("href", "");
        //    CreateElementWithValue(doc, parent, "wrap", "false");
        //    parent.AppendChild((XmlNode)doc.CreateElement("LocalizationInfo"));
        //    CreateElementWithValue(doc, parent, "atlasPart", "true");
        //    CreateElementWithValue(doc, parent, "pool", "UNDEFINED");
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
        //    CreateElementWithValue(doc, parent, "permanentCache", "0");
        //    doc.Save(singleTexturePath);
        //}

        public Bitmap GetBitmap()
        {
            using (MemoryStream memoryStream = new MemoryStream(_mips[0].size + _mips[0].size / 2 + 150))
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
