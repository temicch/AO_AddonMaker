namespace AO_AddonMaker
{
#warning Not implemented
	public class UITexture : AddonFile
	{
		public int mipSW { get; set; }
		public int mipsNumber { get; set; }
		public bool generateMipChain { get; set; }
#warning Need enum type
		public string type { get; set; }
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
	}
}