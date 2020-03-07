using System;

namespace Retro_Engine
{
	// Token: 0x02000007 RID: 7
	public class FileData
	{
		// Token: 0x04000063 RID: 99
		public char[] fileName = new char[64];

		// Token: 0x04000064 RID: 100
		public uint fileSize;

		// Token: 0x04000065 RID: 101
		public uint position;

		// Token: 0x04000066 RID: 102
		public uint bufferPos;

		// Token: 0x04000067 RID: 103
		public uint virtualFileOffset;

		// Token: 0x04000068 RID: 104
		public byte eStringPosA;

		// Token: 0x04000069 RID: 105
		public byte eStringPosB;

		// Token: 0x0400006A RID: 106
		public byte eStringNo;

		// Token: 0x0400006B RID: 107
		public bool eNybbleSwap;
	}
}
