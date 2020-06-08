namespace AO_AddonMaker
{
    public class UISingleTexture : AddonFile
    {
        protected href _singleTexture { get; set; }
        public href singleTexture { get; set; }
		public int permanentCache { get; set; }
		public href sourceFile { get; set; }
		public long sourceFileCRC { get; set; }
    }
}