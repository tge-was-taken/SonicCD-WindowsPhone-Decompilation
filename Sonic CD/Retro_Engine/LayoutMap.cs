using System;

namespace Retro_Engine
{
	// Token: 0x02000018 RID: 24
	public class LayoutMap
	{
		// Token: 0x04000182 RID: 386
		public ushort[] tileMap = new ushort[65536];

		// Token: 0x04000183 RID: 387
		public byte[] lineScrollRef = new byte[32768];

		// Token: 0x04000184 RID: 388
		public int parallaxFactor;

		// Token: 0x04000185 RID: 389
		public int scrollSpeed;

		// Token: 0x04000186 RID: 390
		public int scrollPosition;

		// Token: 0x04000187 RID: 391
		public int angle;

		// Token: 0x04000188 RID: 392
		public int xPos;

		// Token: 0x04000189 RID: 393
		public int yPos;

		// Token: 0x0400018A RID: 394
		public int zPos;

		// Token: 0x0400018B RID: 395
		public int deformationPos;

		// Token: 0x0400018C RID: 396
		public int deformationPosW;

		// Token: 0x0400018D RID: 397
		public byte type;

		// Token: 0x0400018E RID: 398
		public byte xSize;

		// Token: 0x0400018F RID: 399
		public byte ySize;
	}
}
