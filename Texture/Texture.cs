﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Texture
{
    public class Texture
    {
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

        private MemoryStream textureStream;

        private readonly List<MipData> _mips = new List<MipData>();

        public Format TextureFormat { get; private set; }
        public int Width { get; private set; } = 1;
        public int Height { get; private set; } = 1;

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

        public Texture(Stream binaryFileStream, int realWidth, int realHeight, Format type)
        {
            Read(Utils.UnZLib(binaryFileStream));
            Width = realWidth;
            Height = realHeight;
            TextureFormat = type;
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

        ~Texture()
        {
            GCCollectTexture();
        }

        protected void GCCollectTexture()
        {
            //if (textureStream != null)
            //    textureStream.Dispose();
            //textureStream = null;
        }

        public ImageSource GetBitmap()
        {
            GCCollectTexture();

            textureStream = new MemoryStream(_mips[0].size + _mips[0].size / 2 + 150);

            SaveTo(textureStream);
            textureStream.Seek(0L, SeekOrigin.Begin);

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = textureStream;
            bitmap.EndInit();

            return bitmap;
        }
    }
}
