using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Retro_Engine
{
	// Token: 0x02000027 RID: 39
	public struct DrawVertex : IVertexType
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x0002221D File Offset: 0x0002041D
		public DrawVertex(Vector2 position, Vector2 texCoord, Color color)
		{
			this.position = position;
			this.texCoord = texCoord;
			this.color = color;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00022234 File Offset: 0x00020434
		VertexDeclaration IVertexType.VertexDeclaration
		{
			get
			{
				return DrawVertex.VertexDeclaration;
			}
		}

		// Token: 0x040001FE RID: 510
		public Vector2 position;

		// Token: 0x040001FF RID: 511
		public Vector2 texCoord;

		// Token: 0x04000200 RID: 512
		public Color color;

		// Token: 0x04000201 RID: 513
		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
			new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
		});
	}
}
