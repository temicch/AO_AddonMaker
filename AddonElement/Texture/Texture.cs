using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application.BL.Texture.Extensions;

namespace Application.BL.Texture
{
    public class Texture
    {
        private readonly List<MipData> _mips = new List<MipData>();

        public Texture(Stream binaryFileStream, int realWidth, int realHeight, Format type)
        {
            Read(binaryFileStream.UnZLib());
            binaryFileStream.Dispose();
            Width = realWidth;
            Height = realHeight;
            TextureFormat = type;
            Bitmap = GetBitmap();
        }

        public ImageSource Bitmap { get; }

        public Format TextureFormat { get; }
        public int Width { get; }
        public int Height { get; }

        private void AddMipData(int level, int size, byte[] data)
        {
            if (level >= _mips.Count)
                for (var count = _mips.Count; count <= level; ++count)
                    _mips.Add(null);
            _mips[level] = new MipData
            {
                Data = data,
                Size = size
            };
        }

        private void Read(Stream input)
        {
            using (var binaryReader = new BinaryReader(input))
            {
                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    var level = binaryReader.ReadInt32();
                    var num = binaryReader.ReadInt32();
                    AddMipData(level, num, binaryReader.ReadBytes(num));
                }
            }

            input.Dispose();
        }

        private BinaryWriter SaveTo(Stream output)
        {
            var binaryWriter = new BinaryWriter(output);
            binaryWriter.Write(542327876);
            binaryWriter.Write(124);
            var num = 528391;
            if (_mips.Count > 1)
                num |= 131072;
            binaryWriter.Write(num);
            binaryWriter.Write(Height);
            binaryWriter.Write(Width);
            binaryWriter.Write(_mips[0].Data.Length);
            binaryWriter.Write(0);
            binaryWriter.Write(_mips.Count > 1 ? _mips.Count : 0);
            for (var index = 0; index < 11; ++index)
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

            for (var index = 0; index < 5; ++index)
                binaryWriter.Write(0);
            binaryWriter.Write(4096);
            for (var index = 0; index < 4; ++index)
                binaryWriter.Write(0);
            for (var index = 0; index < _mips.Count; ++index)
                binaryWriter.Write(_mips[index].Data);
            binaryWriter.Flush();
            return binaryWriter;
        }

        private ImageSource GetBitmap()
        {
            BitmapImage bitmap = null;
            using (var textureStream = new MemoryStream())
            {
                using (SaveTo(textureStream))
                {
                    textureStream.Seek(0L, SeekOrigin.Begin);

                    bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = textureStream;
                    bitmap.EndInit();
                    //bitmap.Freeze();
                }
            }

            return bitmap;
        }
    }
}