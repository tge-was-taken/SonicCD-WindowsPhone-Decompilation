using System;

namespace Retro_Engine
{
	// Token: 0x0200001D RID: 29
	public class SpriteAnimation
	{
		// Token: 0x040001AA RID: 426
		public char[] name = new char[16];

		// Token: 0x040001AB RID: 427
		public byte numFrames;

		// Token: 0x040001AC RID: 428
		public byte animationSpeed;

		// Token: 0x040001AD RID: 429
		public byte loopPosition;

		// Token: 0x040001AE RID: 430
		public byte rotationFlag;

		// Token: 0x040001AF RID: 431
		public int frameListOffset;
	}
}
