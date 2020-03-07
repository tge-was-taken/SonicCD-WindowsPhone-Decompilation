using System;

namespace Retro_Engine
{
	// Token: 0x0200002D RID: 45
	public class ObjectScript
	{
		// Token: 0x04000247 RID: 583
		public int numFrames;

		// Token: 0x04000248 RID: 584
		public byte surfaceNum;

		// Token: 0x04000249 RID: 585
		public int mainScript;

		// Token: 0x0400024A RID: 586
		public int playerScript;

		// Token: 0x0400024B RID: 587
		public int drawScript;

		// Token: 0x0400024C RID: 588
		public int startupScript;

		// Token: 0x0400024D RID: 589
		public int mainJumpTable;

		// Token: 0x0400024E RID: 590
		public int playerJumpTable;

		// Token: 0x0400024F RID: 591
		public int drawJumpTable;

		// Token: 0x04000250 RID: 592
		public int startupJumpTable;

		// Token: 0x04000251 RID: 593
		public int frameListOffset;

		// Token: 0x04000252 RID: 594
		public AnimationFileList animationFile;
	}
}
