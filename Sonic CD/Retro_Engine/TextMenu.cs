using System;

namespace Retro_Engine
{
	// Token: 0x02000011 RID: 17
	public class TextMenu
	{
		// Token: 0x0400010E RID: 270
		public char[] textData = new char[10240];

		// Token: 0x0400010F RID: 271
		public int[] entryStart = new int[512];

		// Token: 0x04000110 RID: 272
		public int[] entrySize = new int[512];

		// Token: 0x04000111 RID: 273
		public byte[] entryHighlight = new byte[512];

		// Token: 0x04000112 RID: 274
		public int textDataPos;

		// Token: 0x04000113 RID: 275
		public int selection1;

		// Token: 0x04000114 RID: 276
		public int selection2;

		// Token: 0x04000115 RID: 277
		public ushort numRows;

		// Token: 0x04000116 RID: 278
		public ushort numVisibleRows;

		// Token: 0x04000117 RID: 279
		public ushort visibleRowOffset;

		// Token: 0x04000118 RID: 280
		public byte alignment;

		// Token: 0x04000119 RID: 281
		public byte numSelections;

		// Token: 0x0400011A RID: 282
		public sbyte timer;
	}
}
