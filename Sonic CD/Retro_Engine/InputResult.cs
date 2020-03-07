using System;

namespace Retro_Engine
{
	// Token: 0x02000031 RID: 49
	public class InputResult
	{
		// Token: 0x04000269 RID: 617
		public byte up;

		// Token: 0x0400026A RID: 618
		public byte down;

		// Token: 0x0400026B RID: 619
		public byte left;

		// Token: 0x0400026C RID: 620
		public byte right;

		// Token: 0x0400026D RID: 621
		public byte buttonA;

		// Token: 0x0400026E RID: 622
		public byte buttonB;

		// Token: 0x0400026F RID: 623
		public byte buttonC;

		// Token: 0x04000270 RID: 624
		public byte start;

		// Token: 0x04000271 RID: 625
		public byte[] touchDown = new byte[4];

		// Token: 0x04000272 RID: 626
		public int[] touchX = new int[4];

		// Token: 0x04000273 RID: 627
		public int[] touchY = new int[4];

		// Token: 0x04000274 RID: 628
		public int[] touchID = new int[4];

		// Token: 0x04000275 RID: 629
		public int touches;
	}
}
