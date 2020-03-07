using System;

namespace Retro_Engine
{
	// Token: 0x02000017 RID: 23
	public class CollisionMask16x16
	{
		// Token: 0x0400017C RID: 380
		public sbyte[] floorMask = new sbyte[16384];

		// Token: 0x0400017D RID: 381
		public sbyte[] leftWallMask = new sbyte[16384];

		// Token: 0x0400017E RID: 382
		public sbyte[] rightWallMask = new sbyte[16384];

		// Token: 0x0400017F RID: 383
		public sbyte[] roofMask = new sbyte[16384];

		// Token: 0x04000180 RID: 384
		public uint[] angle = new uint[1024];

		// Token: 0x04000181 RID: 385
		public byte[] flags = new byte[1024];
	}
}
