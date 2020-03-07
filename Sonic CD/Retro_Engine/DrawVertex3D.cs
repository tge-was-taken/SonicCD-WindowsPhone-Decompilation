using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Retro_Engine
{
	// Token: 0x02000028 RID: 40
	public struct DrawVertex3D : IVertexType
	{
		// Token: 0x060000E3 RID: 227 RVA: 0x0002229B File Offset: 0x0002049B
		public DrawVertex3D(Vector3 position, Vector2 texCoord, Color color)
		{
			this.position = position;
			this.texCoord = texCoord;
			this.color = color;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000222B2 File Offset: 0x000204B2
		VertexDeclaration IVertexType.VertexDeclaration
		{
			get
			{
				return DrawVertex3D.VertexDeclaration;
			}
		}

		// Token: 0x04000202 RID: 514
		public Vector3 position;

		// Token: 0x04000203 RID: 515
		public Vector2 texCoord;

		// Token: 0x04000204 RID: 516
		public Color color;

		// Token: 0x04000205 RID: 517
		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[]
		{
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new VertexElement(20, VertexElementFormat.Color, VertexElementUsage.Color, 0)
		});
	}
}
