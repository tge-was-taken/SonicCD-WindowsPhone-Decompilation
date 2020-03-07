using System;

namespace Retro_Engine
{
	// Token: 0x0200001F RID: 31
	public static class Scene3D
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00018E8C File Offset: 0x0001708C
		static Scene3D()
		{
			for (int i = 0; i < Scene3D.vertexBuffer.Length; i++)
			{
				Scene3D.vertexBuffer[i] = default(Vertex3D);
			}
			for (int i = 0; i < Scene3D.vertexBufferT.Length; i++)
			{
				Scene3D.vertexBufferT[i] = default(Vertex3D);
			}
			for (int i = 0; i < Scene3D.indexBuffer.Length; i++)
			{
				Scene3D.indexBuffer[i] = default(Face3D);
			}
			for (int i = 0; i < Scene3D.drawList.Length; i++)
			{
				Scene3D.drawList[i] = default(SortList);
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00018FB4 File Offset: 0x000171B4
		public static void SetIdentityMatrix(ref int[] m)
		{
			m[0] = 256;
			m[1] = 0;
			m[2] = 0;
			m[3] = 0;
			m[4] = 0;
			m[5] = 256;
			m[6] = 0;
			m[7] = 0;
			m[8] = 0;
			m[9] = 0;
			m[10] = 256;
			m[11] = 0;
			m[12] = 0;
			m[13] = 0;
			m[14] = 0;
			m[15] = 256;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00019028 File Offset: 0x00017228
		public static void MatrixMultiply(ref int[] a, ref int[] b)
		{
			int[] array = new int[16];
			for (uint num = 0U; num < 16U; num += 1U)
			{
				uint num2 = num & 3U;
				uint num3 = num & 12U;
				array[(int)((UIntPtr)num)] = (b[(int)((UIntPtr)num2)] * a[(int)((UIntPtr)num3)] >> 8) + (b[(int)((UIntPtr)(num2 + 4U))] * a[(int)((UIntPtr)(num3 + 1U))] >> 8) + (b[(int)((UIntPtr)(num2 + 8U))] * a[(int)((UIntPtr)(num3 + 2U))] >> 8) + (b[(int)((UIntPtr)(num2 + 12U))] * a[(int)((UIntPtr)(num3 + 3U))] >> 8);
			}
			for (uint num = 0U; num < 16U; num += 1U)
			{
				a[(int)((UIntPtr)num)] = array[(int)((UIntPtr)num)];
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000190B4 File Offset: 0x000172B4
		public static void MatrixTranslateXYZ(ref int[] m, int xPos, int yPos, int zPos)
		{
			m[0] = 256;
			m[1] = 0;
			m[2] = 0;
			m[3] = 0;
			m[4] = 0;
			m[5] = 256;
			m[6] = 0;
			m[7] = 0;
			m[8] = 0;
			m[9] = 0;
			m[10] = 256;
			m[11] = 0;
			m[12] = xPos;
			m[13] = yPos;
			m[14] = zPos;
			m[15] = 256;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00019128 File Offset: 0x00017328
		public static void MatrixScaleXYZ(ref int[] m, int xScale, int yScale, int zScale)
		{
			m[0] = xScale;
			m[1] = 0;
			m[2] = 0;
			m[3] = 0;
			m[4] = 0;
			m[5] = yScale;
			m[6] = 0;
			m[7] = 0;
			m[8] = 0;
			m[9] = 0;
			m[10] = zScale;
			m[11] = 0;
			m[12] = 0;
			m[13] = 0;
			m[14] = 0;
			m[15] = 256;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00019190 File Offset: 0x00017390
		public static void MatrixRotateX(ref int[] m, int angle)
		{
			if (angle < 0)
			{
				angle = 512 - angle;
			}
			angle &= 511;
			int num = GlobalAppDefinitions.SinValue512[angle] >> 1;
			int num2 = GlobalAppDefinitions.CosValue512[angle] >> 1;
			m[0] = 256;
			m[1] = 0;
			m[2] = 0;
			m[3] = 0;
			m[4] = 0;
			m[5] = num2;
			m[6] = num;
			m[7] = 0;
			m[8] = 0;
			m[9] = -num;
			m[10] = num2;
			m[11] = 0;
			m[12] = 0;
			m[13] = 0;
			m[14] = 0;
			m[15] = 256;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00019228 File Offset: 0x00017428
		public static void MatrixRotateY(ref int[] m, int angle)
		{
			if (angle < 0)
			{
				angle = 512 - angle;
			}
			angle &= 511;
			int num = GlobalAppDefinitions.SinValue512[angle] >> 1;
			int num2 = GlobalAppDefinitions.CosValue512[angle] >> 1;
			m[0] = num2;
			m[1] = 0;
			m[2] = num;
			m[3] = 0;
			m[4] = 0;
			m[5] = 256;
			m[6] = 0;
			m[7] = 0;
			m[8] = -num;
			m[9] = 0;
			m[10] = num2;
			m[11] = 0;
			m[12] = 0;
			m[13] = 0;
			m[14] = 0;
			m[15] = 256;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000192C0 File Offset: 0x000174C0
		public static void MatrixRotateZ(ref int[] m, int angle)
		{
			if (angle < 0)
			{
				angle = 512 - angle;
			}
			angle &= 511;
			int num = GlobalAppDefinitions.SinValue512[angle] >> 1;
			int num2 = GlobalAppDefinitions.CosValue512[angle] >> 1;
			m[0] = num2;
			m[1] = 0;
			m[2] = num;
			m[3] = 0;
			m[4] = 0;
			m[5] = 256;
			m[6] = 0;
			m[7] = 0;
			m[8] = -num;
			m[9] = 0;
			m[10] = num2;
			m[11] = 0;
			m[12] = 0;
			m[13] = 0;
			m[14] = 0;
			m[15] = 256;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00019358 File Offset: 0x00017558
		public static void MatrixRotateXYZ(ref int[] m, int angleX, int angleY, int angleZ)
		{
			if (angleX < 0)
			{
				angleX = 512 - angleX;
			}
			angleX &= 511;
			if (angleY < 0)
			{
				angleY = 512 - angleY;
			}
			angleY &= 511;
			if (angleZ < 0)
			{
				angleZ = 512 - angleZ;
			}
			angleZ &= 511;
			int num = GlobalAppDefinitions.SinValue512[angleX] >> 1;
			int num2 = GlobalAppDefinitions.CosValue512[angleX] >> 1;
			int num3 = GlobalAppDefinitions.SinValue512[angleY] >> 1;
			int num4 = GlobalAppDefinitions.CosValue512[angleY] >> 1;
			int num5 = GlobalAppDefinitions.SinValue512[angleZ] >> 1;
			int num6 = GlobalAppDefinitions.CosValue512[angleZ] >> 1;
			m[0] = (num4 * num6 >> 8) + ((num * num3 >> 8) * num5 >> 8);
			m[1] = (num4 * num5 >> 8) - ((num * num3 >> 8) * num6 >> 8);
			m[2] = num2 * num3 >> 8;
			m[3] = 0;
			m[4] = -num2 * num5 >> 8;
			m[5] = num2 * num6 >> 8;
			m[6] = num;
			m[7] = 0;
			m[8] = ((num * num4 >> 8) * num5 >> 8) - (num3 * num6 >> 8);
			m[9] = (-num3 * num5 >> 8) - ((num * num4 >> 8) * num6 >> 8);
			m[10] = num2 * num4 >> 8;
			m[11] = 0;
			m[12] = 0;
			m[13] = 0;
			m[14] = 0;
			m[15] = 256;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00019494 File Offset: 0x00017694
		public static void TransformVertexBuffer()
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 16; i++)
			{
				Scene3D.matFinal[i] = Scene3D.matWorld[i];
			}
			Scene3D.MatrixMultiply(ref Scene3D.matFinal, ref Scene3D.matView);
			for (int i = 0; i < Scene3D.numVertices; i++)
			{
				Vertex3D vertex3D = Scene3D.vertexBuffer[num];
				Scene3D.vertexBufferT[num2].x = (Scene3D.matFinal[0] * vertex3D.x >> 8) + (Scene3D.matFinal[4] * vertex3D.y >> 8) + (Scene3D.matFinal[8] * vertex3D.z >> 8) + Scene3D.matFinal[12];
				Scene3D.vertexBufferT[num2].y = (Scene3D.matFinal[1] * vertex3D.x >> 8) + (Scene3D.matFinal[5] * vertex3D.y >> 8) + (Scene3D.matFinal[9] * vertex3D.z >> 8) + Scene3D.matFinal[13];
				Scene3D.vertexBufferT[num2].z = (Scene3D.matFinal[2] * vertex3D.x >> 8) + (Scene3D.matFinal[6] * vertex3D.y >> 8) + (Scene3D.matFinal[10] * vertex3D.z >> 8) + Scene3D.matFinal[14];
				if (Scene3D.vertexBufferT[num2].z < 1 && Scene3D.vertexBufferT[num2].z > 0)
				{
					Scene3D.vertexBufferT[num2].z = 1;
				}
				num++;
				num2++;
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00019624 File Offset: 0x00017824
		public static void TransformVertices(ref int[] m, int vStart, int vEnd)
		{
			int num = 0;
			Vertex3D vertex3D = default(Vertex3D);
			vEnd++;
			for (int i = vStart; i < vEnd; i++)
			{
				Vertex3D vertex3D2 = Scene3D.vertexBuffer[i];
				vertex3D.x = (m[0] * vertex3D2.x >> 8) + (m[4] * vertex3D2.y >> 8) + (m[8] * vertex3D2.z >> 8) + m[12];
				vertex3D.y = (m[1] * vertex3D2.x >> 8) + (m[5] * vertex3D2.y >> 8) + (m[9] * vertex3D2.z >> 8) + m[13];
				vertex3D.z = (m[2] * vertex3D2.x >> 8) + (m[6] * vertex3D2.y >> 8) + (m[10] * vertex3D2.z >> 8) + m[14];
				Scene3D.vertexBuffer[i].x = vertex3D.x;
				Scene3D.vertexBuffer[i].y = vertex3D.y;
				Scene3D.vertexBuffer[i].z = vertex3D.z;
				num++;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0001975C File Offset: 0x0001795C
		public static void Sort3DDrawList()
		{
			for (int i = 0; i < Scene3D.numFaces; i++)
			{
				Scene3D.drawList[i].z = Scene3D.vertexBufferT[Scene3D.indexBuffer[i].a].z + Scene3D.vertexBufferT[Scene3D.indexBuffer[i].b].z + Scene3D.vertexBufferT[Scene3D.indexBuffer[i].c].z + Scene3D.vertexBufferT[Scene3D.indexBuffer[i].d].z >> 2;
				Scene3D.drawList[i].index = i;
			}
			for (int i = 0; i < Scene3D.numFaces; i++)
			{
				for (int j = Scene3D.numFaces - 1; j > i; j--)
				{
					if (Scene3D.drawList[j].z > Scene3D.drawList[j - 1].z)
					{
						int index = Scene3D.drawList[j].index;
						int z = Scene3D.drawList[j].z;
						Scene3D.drawList[j].index = Scene3D.drawList[j - 1].index;
						Scene3D.drawList[j].z = Scene3D.drawList[j - 1].z;
						Scene3D.drawList[j - 1].index = index;
						Scene3D.drawList[j - 1].z = z;
					}
				}
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00019904 File Offset: 0x00017B04
		public static void Draw3DScene(int surfaceNum)
		{
			Quad2D quad2D = new Quad2D();
			for (int i = 0; i < Scene3D.numFaces; i++)
			{
				Face3D face3D = Scene3D.indexBuffer[Scene3D.drawList[i].index];
				switch (face3D.flag)
				{
				case 0:
					if (Scene3D.vertexBufferT[face3D.a].z > 256 && Scene3D.vertexBufferT[face3D.b].z > 256 && Scene3D.vertexBufferT[face3D.c].z > 256 && Scene3D.vertexBufferT[face3D.d].z > 256)
					{
						quad2D.vertex[0].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.a].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.a].z;
						quad2D.vertex[0].y = 120 - Scene3D.vertexBufferT[face3D.a].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.a].z;
						quad2D.vertex[1].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.b].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.b].z;
						quad2D.vertex[1].y = 120 - Scene3D.vertexBufferT[face3D.b].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.b].z;
						quad2D.vertex[2].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.c].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.c].z;
						quad2D.vertex[2].y = 120 - Scene3D.vertexBufferT[face3D.c].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.c].z;
						quad2D.vertex[3].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.d].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.d].z;
						quad2D.vertex[3].y = 120 - Scene3D.vertexBufferT[face3D.d].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.d].z;
						quad2D.vertex[0].u = Scene3D.vertexBuffer[face3D.a].u;
						quad2D.vertex[0].v = Scene3D.vertexBuffer[face3D.a].v;
						quad2D.vertex[1].u = Scene3D.vertexBuffer[face3D.b].u;
						quad2D.vertex[1].v = Scene3D.vertexBuffer[face3D.b].v;
						quad2D.vertex[2].u = Scene3D.vertexBuffer[face3D.c].u;
						quad2D.vertex[2].v = Scene3D.vertexBuffer[face3D.c].v;
						quad2D.vertex[3].u = Scene3D.vertexBuffer[face3D.d].u;
						quad2D.vertex[3].v = Scene3D.vertexBuffer[face3D.d].v;
						GraphicsSystem.DrawTexturedQuad(quad2D, surfaceNum);
					}
					break;
				case 1:
					quad2D.vertex[0].x = Scene3D.vertexBuffer[face3D.a].x;
					quad2D.vertex[0].y = Scene3D.vertexBuffer[face3D.a].y;
					quad2D.vertex[1].x = Scene3D.vertexBuffer[face3D.b].x;
					quad2D.vertex[1].y = Scene3D.vertexBuffer[face3D.b].y;
					quad2D.vertex[2].x = Scene3D.vertexBuffer[face3D.c].x;
					quad2D.vertex[2].y = Scene3D.vertexBuffer[face3D.c].y;
					quad2D.vertex[3].x = Scene3D.vertexBuffer[face3D.d].x;
					quad2D.vertex[3].y = Scene3D.vertexBuffer[face3D.d].y;
					quad2D.vertex[0].u = Scene3D.vertexBuffer[face3D.a].u;
					quad2D.vertex[0].v = Scene3D.vertexBuffer[face3D.a].v;
					quad2D.vertex[1].u = Scene3D.vertexBuffer[face3D.b].u;
					quad2D.vertex[1].v = Scene3D.vertexBuffer[face3D.b].v;
					quad2D.vertex[2].u = Scene3D.vertexBuffer[face3D.c].u;
					quad2D.vertex[2].v = Scene3D.vertexBuffer[face3D.c].v;
					quad2D.vertex[3].u = Scene3D.vertexBuffer[face3D.d].u;
					quad2D.vertex[3].v = Scene3D.vertexBuffer[face3D.d].v;
					GraphicsSystem.DrawTexturedQuad(quad2D, surfaceNum);
					break;
				case 2:
					if (Scene3D.vertexBufferT[face3D.a].z > 256 && Scene3D.vertexBufferT[face3D.b].z > 256 && Scene3D.vertexBufferT[face3D.c].z > 256 && Scene3D.vertexBufferT[face3D.d].z > 256)
					{
						quad2D.vertex[0].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.a].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.a].z;
						quad2D.vertex[0].y = 120 - Scene3D.vertexBufferT[face3D.a].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.a].z;
						quad2D.vertex[1].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.b].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.b].z;
						quad2D.vertex[1].y = 120 - Scene3D.vertexBufferT[face3D.b].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.b].z;
						quad2D.vertex[2].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.c].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.c].z;
						quad2D.vertex[2].y = 120 - Scene3D.vertexBufferT[face3D.c].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.c].z;
						quad2D.vertex[3].x = GlobalAppDefinitions.SCREEN_CENTER + Scene3D.vertexBufferT[face3D.d].x * Scene3D.projectionX / Scene3D.vertexBufferT[face3D.d].z;
						quad2D.vertex[3].y = 120 - Scene3D.vertexBufferT[face3D.d].y * Scene3D.projectionY / Scene3D.vertexBufferT[face3D.d].z;
						GraphicsSystem.DrawQuad(quad2D, face3D.color);
					}
					break;
				case 3:
					quad2D.vertex[0].x = Scene3D.vertexBuffer[face3D.a].x;
					quad2D.vertex[0].y = Scene3D.vertexBuffer[face3D.a].y;
					quad2D.vertex[1].x = Scene3D.vertexBuffer[face3D.b].x;
					quad2D.vertex[1].y = Scene3D.vertexBuffer[face3D.b].y;
					quad2D.vertex[2].x = Scene3D.vertexBuffer[face3D.c].x;
					quad2D.vertex[2].y = Scene3D.vertexBuffer[face3D.c].y;
					quad2D.vertex[3].x = Scene3D.vertexBuffer[face3D.d].x;
					quad2D.vertex[3].y = Scene3D.vertexBuffer[face3D.d].y;
					GraphicsSystem.DrawQuad(quad2D, face3D.color);
					break;
				}
			}
		}

		// Token: 0x040001B4 RID: 436
		public static Vertex3D[] vertexBuffer = new Vertex3D[4096];

		// Token: 0x040001B5 RID: 437
		public static Vertex3D[] vertexBufferT = new Vertex3D[4096];

		// Token: 0x040001B6 RID: 438
		public static Face3D[] indexBuffer = new Face3D[1024];

		// Token: 0x040001B7 RID: 439
		public static SortList[] drawList = new SortList[1024];

		// Token: 0x040001B8 RID: 440
		public static int numVertices = 0;

		// Token: 0x040001B9 RID: 441
		public static int numFaces = 0;

		// Token: 0x040001BA RID: 442
		public static int projectionX = 136;

		// Token: 0x040001BB RID: 443
		public static int projectionY = 160;

		// Token: 0x040001BC RID: 444
		public static int[] matWorld = new int[16];

		// Token: 0x040001BD RID: 445
		public static int[] matView = new int[16];

		// Token: 0x040001BE RID: 446
		public static int[] matFinal = new int[16];

		// Token: 0x040001BF RID: 447
		public static int[] matTemp = new int[16];
	}
}
