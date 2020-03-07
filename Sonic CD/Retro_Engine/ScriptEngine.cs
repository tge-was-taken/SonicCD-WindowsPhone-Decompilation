using System;

namespace Retro_Engine
{
	// Token: 0x0200002F RID: 47
	public class ScriptEngine
	{
		// Token: 0x04000255 RID: 597
		public int[] operands = new int[10];

		// Token: 0x04000256 RID: 598
		public int[] tempValue = new int[8];

		// Token: 0x04000257 RID: 599
		public int[] arrayPosition = new int[3];

		// Token: 0x04000258 RID: 600
		public int checkResult;

		// Token: 0x04000259 RID: 601
		public int sRegister;
	}
}
