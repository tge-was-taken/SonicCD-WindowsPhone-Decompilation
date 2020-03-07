using System;

namespace Retro_Engine
{
	// Token: 0x02000026 RID: 38
	public class Quad2D
	{
		// Token: 0x060000DF RID: 223 RVA: 0x000221E0 File Offset: 0x000203E0
		public Quad2D()
		{
			for (int i = 0; i < 4; i++)
			{
				this.vertex[i] = default(Vertex2D);
			}
		}

		// Token: 0x040001FD RID: 509
		public Vertex2D[] vertex = new Vertex2D[4];
	}
}
