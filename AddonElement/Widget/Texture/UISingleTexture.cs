﻿using System.Windows.Media;

namespace AddonElement
{
    public class UISingleTexture : AddonFile
    {
        public href singleTexture { get; set; }
		public int permanentCache { get; set; }
		public href sourceFile { get; set; }
		public long sourceFileCRC { get; set; }

        public ImageSource Bitmap
        {
            get => (singleTexture?.File as UITexture)?.Bitmap;
        }
    }
}