using System;

namespace Retro_Engine
{
	// Token: 0x02000014 RID: 20
	public class GifDecoder
	{
		// Token: 0x0400012E RID: 302
		public int depth;

		// Token: 0x0400012F RID: 303
		public int clearCode;

		// Token: 0x04000130 RID: 304
		public int eofCode;

		// Token: 0x04000131 RID: 305
		public int runningCode;

		// Token: 0x04000132 RID: 306
		public int runningBits;

		// Token: 0x04000133 RID: 307
		public int prevCode;

		// Token: 0x04000134 RID: 308
		public int currentCode;

		// Token: 0x04000135 RID: 309
		public int maxCodePlusOne;

		// Token: 0x04000136 RID: 310
		public int stackPtr;

		// Token: 0x04000137 RID: 311
		public int shiftState;

		// Token: 0x04000138 RID: 312
		public int fileState;

		// Token: 0x04000139 RID: 313
		public int position;

		// Token: 0x0400013A RID: 314
		public int bufferSize;

		// Token: 0x0400013B RID: 315
		public uint shiftData;

		// Token: 0x0400013C RID: 316
		public uint pixelCount;

		// Token: 0x0400013D RID: 317
		public byte[] buffer = new byte[256];

		// Token: 0x0400013E RID: 318
		public byte[] stack = new byte[4096];

		// Token: 0x0400013F RID: 319
		public byte[] suffix = new byte[4096];

		// Token: 0x04000140 RID: 320
		public uint[] prefix = new uint[4096];
	}
}
