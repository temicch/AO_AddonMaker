using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Application.BL.Texture.Extensions;

namespace Application.BL.Texture;

/// <summary>
///     Class for working with textures
/// </summary>
public class Texture
{
    private readonly List<MipData> mips = new();

    /// <summary>
    ///     Create texture
    /// </summary>
    /// <param name="binaryFileStream">A stream that encapsulates texture information</param>
    /// <param name="realWidth">Width of texture</param>
    /// <param name="realHeight">Height of texture</param>
    /// <param name="type">Type of texture</param>
    public Texture(Stream binaryFileStream, int realWidth, int realHeight, Format type)
    {
        Read(binaryFileStream.UnZLib());

        Width = realWidth;
        Height = realHeight;
        TextureFormat = type;
        Bitmap = GetBitmap();
    }

    /// <summary>
    ///     Provides a specialized <see cref="BitmapSource" /> that is optimized for loading images using Extensible
    ///     Application Markup Language (XAML).
    /// </summary>
    public virtual ImageSource Bitmap { get; }

    /// <summary>
    ///     Texture <seealso cref="Format" />
    /// </summary>
    public Format TextureFormat { get; }

    /// <summary>
    ///     Texture real width
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Texture real height
    /// </summary>
    public int Height { get; }

    private void AddMipData(int level, int size, byte[] data)
    {
        if (level >= mips.Count)
            for (var count = mips.Count; count <= level; ++count)
                mips.Add(null);
        mips[level] = new MipData
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
    }

    private Stream GetTextureStream()
    {
        Stream outputStream = new MemoryStream();

        using (var binaryWriter = new BinaryWriter(outputStream, Encoding.Default, true))
        {
            binaryWriter.Write(542327876);
            binaryWriter.Write(124);
            var num = 528391;
            if (mips.Count > 1)
                num |= 131072;
            binaryWriter.Write(num);
            binaryWriter.Write(Height);
            binaryWriter.Write(Width);
            binaryWriter.Write(mips[0].Data.Length);
            binaryWriter.Write(0);
            binaryWriter.Write(mips.Count > 1 ? mips.Count : 0);
            for (var index = 0; index < 11; ++index)
                binaryWriter.Write(0);
            binaryWriter.Write(32);
            binaryWriter.Write(4);

            WriteFormatData(binaryWriter);

            for (var index = 0; index < 5; ++index)
                binaryWriter.Write(0);
            binaryWriter.Write(4096);
            for (var index = 0; index < 4; ++index)
                binaryWriter.Write(0);
            foreach (var mip in mips)
                binaryWriter.Write(mip.Data);
            binaryWriter.Flush();
        }

        return outputStream;
    }

    private void WriteFormatData(BinaryWriter binaryWriter)
    {
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
    }

    private ImageSource GetBitmap()
    {
        BitmapImage bitmap = null;
        using (var textureStream = GetTextureStream())
        {
            textureStream.Seek(0L, SeekOrigin.Begin);

            bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = textureStream;
            bitmap.EndInit();
            //bitmap.Freeze();
        }

        return bitmap;
    }
}
