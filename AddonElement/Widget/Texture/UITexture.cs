using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace AddonElement
{
	public class UITexture : AddonFile
	{
		public int mipSW { get; set; }
		public int mipsNumber { get; set; }
		public bool generateMipChain { get; set; }
		public Texture.Texture.Format type { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int realWidth { get; set; }
		public int realHeight { get; set; }
		public bool disableLODControl { get; set; }
		public bool alphaTex { get; set; }
		public int binaryFileSize { get; set; }
		public href binaryFile { get; set; }
		public int binaryFileSize2 { get; set; }
		public href binaryFile2 { get; set; }
		public href sourceFile { get; set; }
		public long sourceFileCRC { get; set; }
		public bool wrap { get; set; }
		//public href LocalizationInfo { get; set; }
		public bool atlasPart { get; set; }
		public string pool { get; set; }

		[XmlIgnore]
		protected Bitmap bitmap { get; set; }
		[XmlIgnore]
		public Bitmap Bitmap
        {
			get => GetBitmap();
        }

		protected Bitmap GetBitmap()
        {
			if (bitmap == null)
				using (var binaryFileStream = new StreamReader(binaryFile.Path))
				{
					var Texture = new Texture.Texture(binaryFileStream.BaseStream, realWidth, realHeight, type);
					bitmap = Texture.GetBitmap();
				}
			return bitmap;
		}
	}
}