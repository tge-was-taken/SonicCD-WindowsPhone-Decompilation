using System;

namespace Retro_Engine
{
	// Token: 0x02000019 RID: 25
	public class LineScrollParallax
	{
		// Token: 0x04000190 RID: 400
		public int[] parallaxFactor = new int[256];

		// Token: 0x04000191 RID: 401
		public int[] scrollSpeed = new int[256];

		// Token: 0x04000192 RID: 402
		public int[] scrollPosition = new int[256];

		// Token: 0x04000193 RID: 403
		public int[] linePos = new int[256];

		// Token: 0x04000194 RID: 404
		public byte[] deformationEnabled = new byte[256];

		// Token: 0x04000195 RID: 405
		public byte numEntries;
	}
}
