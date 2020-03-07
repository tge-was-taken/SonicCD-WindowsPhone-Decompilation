using System;

namespace Retro_Engine
{
	// Token: 0x02000016 RID: 22
	public class Mappings128x128
	{
		// Token: 0x04000177 RID: 375
		public int[] gfxDataPos = new int[32768];

		// Token: 0x04000178 RID: 376
		public ushort[] tile16x16 = new ushort[32768];

		// Token: 0x04000179 RID: 377
		public byte[] direction = new byte[32768];

		// Token: 0x0400017A RID: 378
		public byte[] visualPlane = new byte[32768];

		// Token: 0x0400017B RID: 379
		public byte[,] collisionFlag = new byte[2, 32768];
	}
}
