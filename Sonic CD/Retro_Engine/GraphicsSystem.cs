using System;
using Microsoft.Xna.Framework;

namespace Retro_Engine
{
	// Token: 0x02000023 RID: 35
	public static class GraphicsSystem
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x0001A410 File Offset: 0x00018610
		static GraphicsSystem()
		{
			for (int i = 0; i < GraphicsSystem.gfxSurface.Length; i++)
			{
				GraphicsSystem.gfxSurface[i] = new GfxSurfaceDesc();
			}
			for (int i = 0; i < GraphicsSystem.gfxPolyList.Length; i++)
			{
				GraphicsSystem.gfxPolyList[i] = default(DrawVertex);
			}
			for (int i = 0; i < GraphicsSystem.polyList3D.Length; i++)
			{
				GraphicsSystem.polyList3D[i] = default(DrawVertex3D);
			}
			for (int i = 0; i < GraphicsSystem.tilePalette.Length; i++)
			{
				GraphicsSystem.tilePalette[i] = default(PaletteEntry);
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0001A5D4 File Offset: 0x000187D4
		public static void SetScreenRenderSize(int gfxWidth, int gfxPitch)
		{
			GlobalAppDefinitions.SCREEN_XSIZE = gfxWidth;
			GlobalAppDefinitions.SCREEN_CENTER = GlobalAppDefinitions.SCREEN_XSIZE / 2;
			GlobalAppDefinitions.SCREEN_SCROLL_LEFT = GlobalAppDefinitions.SCREEN_CENTER - 8;
			GlobalAppDefinitions.SCREEN_SCROLL_RIGHT = GlobalAppDefinitions.SCREEN_CENTER + 8;
			GlobalAppDefinitions.OBJECT_BORDER_X1 = 128;
			GlobalAppDefinitions.OBJECT_BORDER_X2 = GlobalAppDefinitions.SCREEN_XSIZE + 128;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0001A625 File Offset: 0x00018825
		public static ushort RGB_16BIT5551(byte r, byte g, byte b, byte a)
		{
			return (ushort)(((int)a << 15) + (r >> 3 << 10) + (g >> 3 << 5) + (b >> 3));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0001A640 File Offset: 0x00018840
		public static void LoadPalette(char[] fileName, int paletteNum, int destPoint, int startPoint, int endPoint)
		{
			char[] filePath = new char[64];
			char[] array = "Data/Palettes/".ToCharArray();
			FileIO.StrCopy(ref filePath, ref array);
			FileIO.StrAdd(ref filePath, ref fileName);
			FileData fData = new FileData();
			byte[] array2 = new byte[3];
			if (FileIO.LoadFile(filePath, fData))
			{
				FileIO.SetFilePosition((uint)(startPoint * 3));
				if (paletteNum < 0 || paletteNum > 7)
				{
					paletteNum = 0;
				}
				if (paletteNum == 0)
				{
					for (int i = startPoint; i < endPoint; i++)
					{
						FileIO.ReadByteArray(ref array2, 3);
						GraphicsSystem.tilePalette16_Data[0, destPoint] = GraphicsSystem.RGB_16BIT5551(array2[0], array2[1], array2[2], 1);
						GraphicsSystem.tilePalette[destPoint].red = array2[0];
						GraphicsSystem.tilePalette[destPoint].green = array2[1];
						GraphicsSystem.tilePalette[destPoint].blue = array2[2];
						destPoint++;
					}
					GraphicsSystem.tilePalette16_Data[0, 0] = GraphicsSystem.RGB_16BIT5551(array2[0], array2[1], array2[2], 0);
				}
				else
				{
					for (int i = startPoint; i < endPoint; i++)
					{
						FileIO.ReadByteArray(ref array2, 3);
						GraphicsSystem.tilePalette16_Data[paletteNum, destPoint] = GraphicsSystem.RGB_16BIT5551(array2[0], array2[1], array2[2], 1);
						destPoint++;
					}
					GraphicsSystem.tilePalette16_Data[paletteNum, 0] = GraphicsSystem.RGB_16BIT5551(array2[0], array2[1], array2[2], 0);
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0001A794 File Offset: 0x00018994
		public static byte AddGraphicsFile(char[] fileName)
		{
			byte b = 0;
			char[] array = new char[64];
			char[] array2 = "Data/Sprites/".ToCharArray();
			FileIO.StrCopy(ref array, ref array2);
			FileIO.StrAdd(ref array, ref fileName);
			while (b < 24)
			{
				if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[(int)b].fileName) <= 0)
				{
					int num = FileIO.StringLength(ref array) - 1;
					char c = array[num];
					if (c != 'f')
					{
						if (c == 'p')
						{
							GraphicsSystem.LoadBMPFile(array, (int)b);
						}
					}
					else
					{
						GraphicsSystem.LoadGIFFile(array, (int)b);
					}
					return b;
				}
				if (FileIO.StringComp(ref GraphicsSystem.gfxSurface[(int)b].fileName, ref array))
				{
					return b;
				}
				b += 1;
			}
			return 0;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0001A830 File Offset: 0x00018A30
		public static void RemoveGraphicsFile(char[] fileName, int surfaceNum)
		{
			uint num;
			if (surfaceNum < 0)
			{
				for (num = 0U; num < 24U; num += 1U)
				{
					if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[(int)((UIntPtr)num)].fileName) > 0 && FileIO.StringComp(ref GraphicsSystem.gfxSurface[(int)((UIntPtr)num)].fileName, ref fileName))
					{
						surfaceNum = (int)num;
					}
				}
			}
			if (surfaceNum < 0)
			{
				return;
			}
			if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[surfaceNum].fileName) == 0)
			{
				return;
			}
			FileIO.StrClear(ref GraphicsSystem.gfxSurface[surfaceNum].fileName);
			num = GraphicsSystem.gfxSurface[surfaceNum].dataStart;
			uint num2 = (uint)((ulong)GraphicsSystem.gfxSurface[surfaceNum].dataStart + (ulong)((long)(GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height)));
			for (uint num3 = 2097152U - num2; num3 > 0U; num3 -= 1U)
			{
				GraphicsSystem.graphicData[(int)((UIntPtr)num)] = GraphicsSystem.graphicData[(int)((UIntPtr)num2)];
				num += 1U;
				num2 += 1U;
			}
			GraphicsSystem.gfxDataPosition -= (uint)(GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
			for (num = 0U; num < 24U; num += 1U)
			{
				if (GraphicsSystem.gfxSurface[(int)((UIntPtr)num)].dataStart > GraphicsSystem.gfxSurface[surfaceNum].dataStart)
				{
					GraphicsSystem.gfxSurface[(int)((UIntPtr)num)].dataStart -= (uint)(GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
				}
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0001A980 File Offset: 0x00018B80
		public static void ClearGraphicsData()
		{
			for (int i = 0; i < 24; i++)
			{
				FileIO.StrClear(ref GraphicsSystem.gfxSurface[i].fileName);
			}
			GraphicsSystem.gfxDataPosition = 0U;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0001A9B4 File Offset: 0x00018BB4
		public static bool CheckSurfaceSize(int size)
		{
			for (int i = 2; i < 2048; i <<= 1)
			{
				if (i == size)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0001A9D8 File Offset: 0x00018BD8
		public static void SetupPolygonLists()
		{
			int num = 0;
			for (int i = 0; i < 8192; i++)
			{
				GraphicsSystem.gfxPolyListIndex[num] = (short)(i << 2);
				num++;
				GraphicsSystem.gfxPolyListIndex[num] = (short)((i << 2) + 1);
				num++;
				GraphicsSystem.gfxPolyListIndex[num] = (short)((i << 2) + 2);
				num++;
				GraphicsSystem.gfxPolyListIndex[num] = (short)((i << 2) + 1);
				num++;
				GraphicsSystem.gfxPolyListIndex[num] = (short)((i << 2) + 3);
				num++;
				GraphicsSystem.gfxPolyListIndex[num] = (short)((i << 2) + 2);
				num++;
			}
			for (int i = 0; i < 8192; i++)
			{
				GraphicsSystem.gfxPolyList[i].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[i].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[i].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[i].color.A = byte.MaxValue;
			}
			for (int i = 0; i < 6404; i++)
			{
				GraphicsSystem.polyList3D[i].color.R = byte.MaxValue;
				GraphicsSystem.polyList3D[i].color.G = byte.MaxValue;
				GraphicsSystem.polyList3D[i].color.B = byte.MaxValue;
				GraphicsSystem.polyList3D[i].color.A = byte.MaxValue;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0001AB4C File Offset: 0x00018D4C
		public static void UpdateTextureBufferWithTiles()
		{
			int num = 0;
			int num3;
			if (GraphicsSystem.texBufferMode == 0)
			{
				for (int i = 0; i < 512; i += 16)
				{
					for (int j = 0; j < 512; j += 16)
					{
						int num2 = num << 8;
						num++;
						num3 = j + (i << 10);
						for (int k = 0; k < 16; k++)
						{
							for (int l = 0; l < 16; l++)
							{
								if (GraphicsSystem.tileGfx[num2] > 0)
								{
									GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
								}
								else
								{
									GraphicsSystem.texBuffer[num3] = 0;
								}
								num3++;
								num2++;
							}
							num3 += 1008;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < 504; i += 18)
				{
					for (int j = 0; j < 504; j += 18)
					{
						int num2 = num << 8;
						num++;
						if (num == 783)
						{
							num = 1023;
						}
						num3 = j + (i << 10);
						if (GraphicsSystem.tileGfx[num2] > 0)
						{
							GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
						}
						else
						{
							GraphicsSystem.texBuffer[num3] = 0;
						}
						num3++;
						for (int l = 0; l < 15; l++)
						{
							if (GraphicsSystem.tileGfx[num2] > 0)
							{
								GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
							}
							else
							{
								GraphicsSystem.texBuffer[num3] = 0;
							}
							num3++;
							num2++;
						}
						if (GraphicsSystem.tileGfx[num2] > 0)
						{
							GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
							num3++;
							GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
						}
						else
						{
							GraphicsSystem.texBuffer[num3] = 0;
							num3++;
							GraphicsSystem.texBuffer[num3] = 0;
						}
						num3++;
						num2 -= 15;
						num3 += 1006;
						for (int k = 0; k < 16; k++)
						{
							if (GraphicsSystem.tileGfx[num2] > 0)
							{
								GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
							}
							else
							{
								GraphicsSystem.texBuffer[num3] = 0;
							}
							num3++;
							for (int l = 0; l < 15; l++)
							{
								if (GraphicsSystem.tileGfx[num2] > 0)
								{
									GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
								}
								else
								{
									GraphicsSystem.texBuffer[num3] = 0;
								}
								num3++;
								num2++;
							}
							if (GraphicsSystem.tileGfx[num2] > 0)
							{
								GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
								num3++;
								GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
							}
							else
							{
								GraphicsSystem.texBuffer[num3] = 0;
								num3++;
								GraphicsSystem.texBuffer[num3] = 0;
							}
							num3++;
							num2++;
							num3 += 1006;
						}
						num2 -= 16;
						if (GraphicsSystem.tileGfx[num2] > 0)
						{
							GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
						}
						else
						{
							GraphicsSystem.texBuffer[num3] = 0;
						}
						num3++;
						for (int l = 0; l < 15; l++)
						{
							if (GraphicsSystem.tileGfx[num2] > 0)
							{
								GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
							}
							else
							{
								GraphicsSystem.texBuffer[num3] = 0;
							}
							num3++;
							num2++;
						}
						if (GraphicsSystem.tileGfx[num2] > 0)
						{
							GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
							num3++;
							GraphicsSystem.texBuffer[num3] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.tileGfx[num2]];
						}
						else
						{
							GraphicsSystem.texBuffer[num3] = 0;
							num3++;
							GraphicsSystem.texBuffer[num3] = 0;
						}
						num3++;
						num3 += 1006;
					}
				}
			}
			num3 = 0;
			for (int k = 0; k < 16; k++)
			{
				for (int l = 0; l < 16; l++)
				{
					GraphicsSystem.texBuffer[num3] = GraphicsSystem.RGB_16BIT5551(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
					num3++;
				}
				num3 += 1008;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0001AFD8 File Offset: 0x000191D8
		public static void UpdateTextureBufferWithSortedSprites()
		{
			byte b = 0;
			byte[] array = new byte[24];
			bool flag = true;
			for (int i = 0; i < 24; i++)
			{
				GraphicsSystem.gfxSurface[i].texStartX = -1;
			}
			for (int i = 0; i < 24; i++)
			{
				int j = 0;
				sbyte b2 = -1;
				for (int k = 0; k < 24; k++)
				{
					if (FileIO.StringLength(ref GraphicsSystem.gfxSurface[k].fileName) > 0 && GraphicsSystem.gfxSurface[k].texStartX == -1)
					{
						if (GraphicsSystem.CheckSurfaceSize(GraphicsSystem.gfxSurface[k].width) && GraphicsSystem.CheckSurfaceSize(GraphicsSystem.gfxSurface[k].height))
						{
							if (GraphicsSystem.gfxSurface[k].width + GraphicsSystem.gfxSurface[k].height > j)
							{
								j = GraphicsSystem.gfxSurface[k].width + GraphicsSystem.gfxSurface[k].height;
								b2 = (sbyte)k;
							}
						}
						else
						{
							GraphicsSystem.gfxSurface[k].texStartX = 0;
						}
					}
				}
				if (b2 == -1)
				{
					i = 24;
				}
				else
				{
					GraphicsSystem.gfxSurface[(int)b2].texStartX = 0;
					array[(int)b] = (byte)b2;
					b += 1;
				}
			}
			for (int i = 0; i < 24; i++)
			{
				GraphicsSystem.gfxSurface[i].texStartX = -1;
			}
			for (int i = 0; i < (int)b; i++)
			{
				sbyte b2 = (sbyte)array[i];
				GraphicsSystem.gfxSurface[(int)b2].texStartX = 0;
				GraphicsSystem.gfxSurface[(int)b2].texStartY = 0;
				int j = 0;
				while (j == 0)
				{
					j = 1;
					if (GraphicsSystem.gfxSurface[(int)b2].height == 1024)
					{
						flag = false;
					}
					if (flag)
					{
						if (GraphicsSystem.gfxSurface[(int)b2].texStartX < 512 && GraphicsSystem.gfxSurface[(int)b2].texStartY < 512)
						{
							j = 0;
							GraphicsSystem.gfxSurface[(int)b2].texStartX += GraphicsSystem.gfxSurface[(int)b2].width;
							if (GraphicsSystem.gfxSurface[(int)b2].texStartX + GraphicsSystem.gfxSurface[(int)b2].width > 1024)
							{
								GraphicsSystem.gfxSurface[(int)b2].texStartX = 0;
								GraphicsSystem.gfxSurface[(int)b2].texStartY += GraphicsSystem.gfxSurface[(int)b2].height;
							}
						}
						else
						{
							for (int k = 0; k < 24; k++)
							{
								if (GraphicsSystem.gfxSurface[k].texStartX > -1 && k != (int)b2 && GraphicsSystem.gfxSurface[(int)b2].texStartX < GraphicsSystem.gfxSurface[k].texStartX + GraphicsSystem.gfxSurface[k].width && GraphicsSystem.gfxSurface[(int)b2].texStartX >= GraphicsSystem.gfxSurface[k].texStartX && GraphicsSystem.gfxSurface[(int)b2].texStartY < GraphicsSystem.gfxSurface[k].texStartY + GraphicsSystem.gfxSurface[k].height)
								{
									j = 0;
									GraphicsSystem.gfxSurface[(int)b2].texStartX += GraphicsSystem.gfxSurface[(int)b2].width;
									if (GraphicsSystem.gfxSurface[(int)b2].texStartX + GraphicsSystem.gfxSurface[(int)b2].width > 1024)
									{
										GraphicsSystem.gfxSurface[(int)b2].texStartX = 0;
										GraphicsSystem.gfxSurface[(int)b2].texStartY += GraphicsSystem.gfxSurface[(int)b2].height;
									}
									k = 24;
								}
							}
						}
					}
					else if (GraphicsSystem.gfxSurface[(int)b2].width < 1024)
					{
						if (GraphicsSystem.gfxSurface[(int)b2].texStartX < 16 && GraphicsSystem.gfxSurface[(int)b2].texStartY < 16)
						{
							j = 0;
							GraphicsSystem.gfxSurface[(int)b2].texStartX += GraphicsSystem.gfxSurface[(int)b2].width;
							if (GraphicsSystem.gfxSurface[(int)b2].texStartX + GraphicsSystem.gfxSurface[(int)b2].width > 1024)
							{
								GraphicsSystem.gfxSurface[(int)b2].texStartX = 0;
								GraphicsSystem.gfxSurface[(int)b2].texStartY += GraphicsSystem.gfxSurface[(int)b2].height;
							}
						}
						else
						{
							for (int k = 0; k < 24; k++)
							{
								if (GraphicsSystem.gfxSurface[k].texStartX > -1 && k != (int)b2 && GraphicsSystem.gfxSurface[(int)b2].texStartX < GraphicsSystem.gfxSurface[k].texStartX + GraphicsSystem.gfxSurface[k].width && GraphicsSystem.gfxSurface[(int)b2].texStartX >= GraphicsSystem.gfxSurface[k].texStartX && GraphicsSystem.gfxSurface[(int)b2].texStartY < GraphicsSystem.gfxSurface[k].texStartY + GraphicsSystem.gfxSurface[k].height)
								{
									j = 0;
									GraphicsSystem.gfxSurface[(int)b2].texStartX += GraphicsSystem.gfxSurface[(int)b2].width;
									if (GraphicsSystem.gfxSurface[(int)b2].texStartX + GraphicsSystem.gfxSurface[(int)b2].width > 1024)
									{
										GraphicsSystem.gfxSurface[(int)b2].texStartX = 0;
										GraphicsSystem.gfxSurface[(int)b2].texStartY += GraphicsSystem.gfxSurface[(int)b2].height;
									}
									k = 24;
								}
							}
						}
					}
				}
				if (GraphicsSystem.gfxSurface[(int)b2].texStartY + GraphicsSystem.gfxSurface[(int)b2].height <= 1024)
				{
					int num = (int)GraphicsSystem.gfxSurface[(int)b2].dataStart;
					int num2 = GraphicsSystem.gfxSurface[(int)b2].texStartX + (GraphicsSystem.gfxSurface[(int)b2].texStartY << 10);
					for (j = 0; j < GraphicsSystem.gfxSurface[(int)b2].height; j++)
					{
						for (int k = 0; k < GraphicsSystem.gfxSurface[(int)b2].width; k++)
						{
							if (GraphicsSystem.graphicData[num] > 0)
							{
								GraphicsSystem.texBuffer[num2] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.graphicData[num]];
							}
							else
							{
								GraphicsSystem.texBuffer[num2] = 0;
							}
							num2++;
							num++;
						}
						num2 += 1024 - GraphicsSystem.gfxSurface[(int)b2].width;
					}
				}
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0001B5DC File Offset: 0x000197DC
		public static void UpdateTextureBufferWithSprites()
		{
			for (int i = 0; i < 24; i++)
			{
				if (GraphicsSystem.gfxSurface[i].texStartY + GraphicsSystem.gfxSurface[i].height <= 1024 && GraphicsSystem.gfxSurface[i].texStartX > -1)
				{
					int num = (int)GraphicsSystem.gfxSurface[i].dataStart;
					int num2 = GraphicsSystem.gfxSurface[i].texStartX + (GraphicsSystem.gfxSurface[i].texStartY << 10);
					for (int j = 0; j < GraphicsSystem.gfxSurface[i].height; j++)
					{
						for (int k = 0; k < GraphicsSystem.gfxSurface[i].width; k++)
						{
							if (GraphicsSystem.graphicData[num] > 0)
							{
								GraphicsSystem.texBuffer[num2] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)GraphicsSystem.graphicData[num]];
							}
							else
							{
								GraphicsSystem.texBuffer[num2] = 0;
							}
							num2++;
							num++;
						}
						num2 += 1024 - GraphicsSystem.gfxSurface[i].width;
					}
				}
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x0001B6DC File Offset: 0x000198DC
		public static void LoadBMPFile(char[] fileName, int surfaceNum)
		{
			FileData fileData = new FileData();
			if (FileIO.LoadFile(fileName, fileData))
			{
				FileIO.StrCopy(ref GraphicsSystem.gfxSurface[surfaceNum].fileName, ref fileName);
				FileIO.SetFilePosition(18U);
				byte b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].width = (int)b;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].width += (int)b << 8;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].width += (int)b << 16;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].width += (int)b << 24;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].height = (int)b;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].height += (int)b << 8;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].height += (int)b << 16;
				b = FileIO.ReadByte();
				GraphicsSystem.gfxSurface[surfaceNum].height += (int)b << 24;
				FileIO.SetFilePosition((uint)((ulong)fileData.fileSize - (ulong)((long)(GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height))));
				GraphicsSystem.gfxSurface[surfaceNum].dataStart = GraphicsSystem.gfxDataPosition;
				int num = (int)GraphicsSystem.gfxSurface[surfaceNum].dataStart;
				num += GraphicsSystem.gfxSurface[surfaceNum].width * (GraphicsSystem.gfxSurface[surfaceNum].height - 1);
				for (int i = 0; i < GraphicsSystem.gfxSurface[surfaceNum].height; i++)
				{
					for (int j = 0; j < GraphicsSystem.gfxSurface[surfaceNum].width; j++)
					{
						b = FileIO.ReadByte();
						GraphicsSystem.graphicData[num] = b;
						num++;
					}
					num -= GraphicsSystem.gfxSurface[surfaceNum].width << 1;
				}
				GraphicsSystem.gfxDataPosition += (uint)(GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
				if (GraphicsSystem.gfxDataPosition >= 4194304U)
				{
					GraphicsSystem.gfxDataPosition = 0U;
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0001B8DC File Offset: 0x00019ADC
		public static void LoadGIFFile(char[] fileName, int surfaceNum)
		{
			FileData fData = new FileData();
			byte[] array = new byte[3];
			bool interlaced = false;
			if (FileIO.LoadFile(fileName, fData))
			{
				FileIO.StrCopy(ref GraphicsSystem.gfxSurface[surfaceNum].fileName, ref fileName);
				FileIO.SetFilePosition(6U);
				array[0] = FileIO.ReadByte();
				int num = (int)array[0];
				array[0] = FileIO.ReadByte();
				num += (int)array[0] << 8;
				array[0] = FileIO.ReadByte();
				int num2 = (int)array[0];
				array[0] = FileIO.ReadByte();
				num2 += (int)array[0] << 8;
				array[0] = FileIO.ReadByte();
				array[0] = FileIO.ReadByte();
				array[0] = FileIO.ReadByte();
				for (int i = 0; i < 256; i++)
				{
					FileIO.ReadByteArray(ref array, 3);
				}
				array[0] = FileIO.ReadByte();
				while (array[0] != 44)
				{
					array[0] = FileIO.ReadByte();
				}
				if (array[0] == 44)
				{
					FileIO.ReadByteArray(ref array, 2);
					FileIO.ReadByteArray(ref array, 2);
					FileIO.ReadByteArray(ref array, 2);
					FileIO.ReadByteArray(ref array, 2);
					array[0] = FileIO.ReadByte();
					if ((array[0] & 64) >> 6 == 1)
					{
						interlaced = true;
					}
					if ((array[0] & 128) >> 7 == 1)
					{
						for (int i = 128; i < 256; i++)
						{
							FileIO.ReadByteArray(ref array, 3);
						}
					}
					GraphicsSystem.gfxSurface[surfaceNum].width = num;
					GraphicsSystem.gfxSurface[surfaceNum].height = num2;
					GraphicsSystem.gfxSurface[surfaceNum].dataStart = GraphicsSystem.gfxDataPosition;
					GraphicsSystem.gfxDataPosition += (uint)(GraphicsSystem.gfxSurface[surfaceNum].width * GraphicsSystem.gfxSurface[surfaceNum].height);
					if (GraphicsSystem.gfxDataPosition >= 4194304U)
					{
						GraphicsSystem.gfxDataPosition = 0U;
					}
					else
					{
						GifLoader.ReadGifPictureData(num, num2, interlaced, ref GraphicsSystem.graphicData, (int)GraphicsSystem.gfxSurface[surfaceNum].dataStart);
					}
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0001BAA0 File Offset: 0x00019CA0
		public static void LoadStageGIFFile(int zNumber)
		{
			FileData fData = new FileData();
			byte[] array = new byte[3];
			bool interlaced = false;
			if (FileIO.LoadStageFile("16x16Tiles.gif".ToCharArray(), zNumber, fData))
			{
				FileIO.SetFilePosition(6U);
				array[0] = FileIO.ReadByte();
				int num = (int)array[0];
				array[0] = FileIO.ReadByte();
				num += (int)array[0] << 8;
				array[0] = FileIO.ReadByte();
				int num2 = (int)array[0];
				array[0] = FileIO.ReadByte();
				num2 += (int)array[0] << 8;
				array[0] = FileIO.ReadByte();
				array[0] = FileIO.ReadByte();
				array[0] = FileIO.ReadByte();
				for (int i = 128; i < 256; i++)
				{
					FileIO.ReadByteArray(ref array, 3);
				}
				for (int i = 128; i < 256; i++)
				{
					FileIO.ReadByteArray(ref array, 3);
					GraphicsSystem.tilePalette[i].red = array[0];
					GraphicsSystem.tilePalette[i].green = array[1];
					GraphicsSystem.tilePalette[i].blue = array[2];
					GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, i] = GraphicsSystem.RGB_16BIT5551(array[0], array[1], array[2], 1);
				}
				array[0] = FileIO.ReadByte();
				if (array[0] == 44)
				{
					FileIO.ReadByteArray(ref array, 2);
					FileIO.ReadByteArray(ref array, 2);
					FileIO.ReadByteArray(ref array, 2);
					FileIO.ReadByteArray(ref array, 2);
					array[0] = FileIO.ReadByte();
					if ((array[0] & 64) >> 6 == 1)
					{
						interlaced = true;
					}
					if ((array[0] & 128) >> 7 == 1)
					{
						for (int i = 128; i < 256; i++)
						{
							FileIO.ReadByteArray(ref array, 3);
						}
					}
					GifLoader.ReadGifPictureData(num, num2, interlaced, ref GraphicsSystem.tileGfx, 0);
					array[0] = GraphicsSystem.tileGfx[0];
					for (int i = 0; i < 262144; i++)
					{
						if (GraphicsSystem.tileGfx[i] == array[0])
						{
							GraphicsSystem.tileGfx[i] = 0;
						}
					}
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0001BC88 File Offset: 0x00019E88
		public static void Copy16x16Tile(int tDest, int tSource)
		{
			tSource <<= 2;
			tDest <<= 2;
			GraphicsSystem.tileUVArray[tDest] = GraphicsSystem.tileUVArray[tSource];
			GraphicsSystem.tileUVArray[tDest + 1] = GraphicsSystem.tileUVArray[tSource + 1];
			GraphicsSystem.tileUVArray[tDest + 2] = GraphicsSystem.tileUVArray[tSource + 2];
			GraphicsSystem.tileUVArray[tDest + 3] = GraphicsSystem.tileUVArray[tSource + 3];
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0001BCE4 File Offset: 0x00019EE4
		public static void ClearScreen(byte clearColour)
		{
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.tilePalette[(int)clearColour].red;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.tilePalette[(int)clearColour].green;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.tilePalette[(int)clearColour].blue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0f;
			GraphicsSystem.gfxVertexSize += 1;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(GlobalAppDefinitions.SCREEN_XSIZE << 4);
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.tilePalette[(int)clearColour].red;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.tilePalette[(int)clearColour].green;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.tilePalette[(int)clearColour].blue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0f;
			GraphicsSystem.gfxVertexSize += 1;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = 3840f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.tilePalette[(int)clearColour].red;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.tilePalette[(int)clearColour].green;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.tilePalette[(int)clearColour].blue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0f;
			GraphicsSystem.gfxVertexSize += 1;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(GlobalAppDefinitions.SCREEN_XSIZE << 4);
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = 3840f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.tilePalette[(int)clearColour].red;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.tilePalette[(int)clearColour].green;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.tilePalette[(int)clearColour].blue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0f;
			GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0f;
			GraphicsSystem.gfxVertexSize += 1;
			GraphicsSystem.gfxIndexSize += 2;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0001C17C File Offset: 0x0001A37C
		public static void DrawSprite(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int surfaceNum)
		{
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0001C6A8 File Offset: 0x0001A8A8
		public static void DrawSpriteFlipped(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int direction, int surfaceNum)
		{
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				switch (direction)
				{
				case 0:
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					break;
				case 1:
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					break;
				case 2:
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					break;
				case 3:
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					break;
				}
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0001DA48 File Offset: 0x0001BC48
		public static void DrawBlendedSprite(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int surfaceNum)
		{
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = 128;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = 128;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = 128;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = 128;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0001DF74 File Offset: 0x0001C174
		public static void DrawAlphaBlendedSprite(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int alpha, int surfaceNum)
		{
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0001E498 File Offset: 0x0001C698
		public static void DrawAdditiveBlendedSprite(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int alpha, int surfaceNum)
		{
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0001E9BC File Offset: 0x0001CBBC
		public static void DrawSubtractiveBlendedSprite(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int alpha, int surfaceNum)
		{
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0001EEE0 File Offset: 0x0001D0E0
		public static void DrawRectangle(int xPos, int yPos, int xSize, int ySize, int r, int g, int b, int alpha)
		{
			if (alpha > 255)
			{
				alpha = 255;
			}
			if (GraphicsSystem.gfxVertexSize < 8192)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = (byte)r;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = (byte)g;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = (byte)b;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xSize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = (byte)r;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = (byte)g;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = (byte)b;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0.01f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + ySize << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = (byte)r;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = (byte)g;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = (byte)b;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0.01f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = (byte)r;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = (byte)g;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = (byte)b;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)alpha;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0.01f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0.01f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0001F32D File Offset: 0x0001D52D
		public static void DrawTintRectangle(int xPos, int yPos, int xSize, int ySize)
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0001F32F File Offset: 0x0001D52F
		public static void DrawTintSpriteMask(int xPos, int yPos, int xSize, int ySize, int xBegin, int yBegin, int tableNo, int surfaceNum)
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0001F331 File Offset: 0x0001D531
		public static void DrawScaledTintMask(byte direction, int xPos, int yPos, int xPivot, int yPivot, int xScale, int yScale, int xSize, int ySize, int xBegin, int yBegin, int surfaceNum)
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0001F334 File Offset: 0x0001D534
		public static void DrawScaledSprite(byte direction, int xPos, int yPos, int xPivot, int yPivot, int xScale, int yScale, int xSize, int ySize, int xBegin, int yBegin, int surfaceNum)
		{
			if (GraphicsSystem.gfxVertexSize < 8192 && xPos > -512 && xPos < 872 && yPos > -512 && yPos < 752)
			{
				xScale <<= 2;
				yScale <<= 2;
				xPos -= xPivot * xScale >> 11;
				xScale = xSize * xScale >> 11;
				yPos -= yPivot * yScale >> 11;
				yScale = ySize * yScale >> 11;
				if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1)
				{
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xScale << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + yScale << 4);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxIndexSize += 2;
				}
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0001F89C File Offset: 0x0001DA9C
		public static void DrawScaledChar(byte direction, int xPos, int yPos, int xPivot, int yPivot, int xScale, int yScale, int xSize, int ySize, int xBegin, int yBegin, int surfaceNum)
		{
			if (GraphicsSystem.gfxVertexSize < 8192 && xPos > -8192 && xPos < 13951 && yPos > -1024 && yPos < 4864)
			{
				xPos -= xPivot * xScale >> 5;
				xScale = xSize * xScale >> 5;
				yPos -= yPivot * yScale >> 5;
				yScale = ySize * yScale >> 5;
				if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 4096)
				{
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)xPos;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)yPos;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + xScale);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)yPos;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)xPos;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + yScale);
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].position.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxIndexSize += 2;
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0001FDF4 File Offset: 0x0001DFF4
		public static void DrawRotatedSprite(byte direction, int xPos, int yPos, int xPivot, int yPivot, int xBegin, int yBegin, int xSize, int ySize, int rotAngle, int surfaceNum)
		{
			xPos <<= 4;
			yPos <<= 4;
			rotAngle -= rotAngle >> 9 << 9;
			if (rotAngle < 0)
			{
				rotAngle += 512;
			}
			if (rotAngle != 0)
			{
				rotAngle = 512 - rotAngle;
			}
			int num = GlobalAppDefinitions.SinValue512[rotAngle];
			int num2 = GlobalAppDefinitions.CosValue512[rotAngle];
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -8192 && xPos < 13952 && yPos > -8192 && yPos < 12032)
			{
				int num3;
				int num4;
				if (direction == 0)
				{
					num3 = -xPivot;
					num4 = -yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					num3 = xSize - xPivot;
					num4 = -yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					num3 = -xPivot;
					num4 = ySize - yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					num3 = xSize - xPivot;
					num4 = ySize - yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxIndexSize += 2;
					return;
				}
				num4 = -yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (xPivot * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - xPivot * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				num3 = xPivot - xSize;
				num4 = -yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				num4 = ySize - yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (xPivot * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - xPivot * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				num3 = xPivot - xSize;
				num4 = ySize - yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000208A0 File Offset: 0x0001EAA0
		public static void DrawRotoZoomSprite(byte direction, int xPos, int yPos, int xPivot, int yPivot, int xBegin, int yBegin, int xSize, int ySize, int rotAngle, int scale, int surfaceNum)
		{
			xPos <<= 4;
			yPos <<= 4;
			rotAngle -= rotAngle >> 9 << 9;
			if (rotAngle < 0)
			{
				rotAngle += 512;
			}
			if (rotAngle != 0)
			{
				rotAngle = 512 - rotAngle;
			}
			int num = GlobalAppDefinitions.SinValue512[rotAngle] * scale >> 9;
			int num2 = GlobalAppDefinitions.CosValue512[rotAngle] * scale >> 9;
			if (GraphicsSystem.gfxSurface[surfaceNum].texStartX > -1 && GraphicsSystem.gfxVertexSize < 8192 && xPos > -8192 && xPos < 13952 && yPos > -8192 && yPos < 12032)
			{
				int num3;
				int num4;
				if (direction == 0)
				{
					num3 = -xPivot;
					num4 = -yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					num3 = xSize - xPivot;
					num4 = -yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					num3 = -xPivot;
					num4 = ySize - yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
					GraphicsSystem.gfxVertexSize += 1;
					num3 = xSize - xPivot;
					num4 = ySize - yPivot;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
					GraphicsSystem.gfxVertexSize += 1;
					GraphicsSystem.gfxIndexSize += 2;
					return;
				}
				num4 = -yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (xPivot * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - xPivot * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				num3 = xPivot - xSize;
				num4 = -yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + xBegin + xSize) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				num4 = ySize - yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (xPivot * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - xPivot * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + yBegin + ySize) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				num3 = xPivot - xSize;
				num4 = ySize - yPivot;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(xPos + (num3 * num2 + num4 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(yPos + (num4 * num2 - num3 * num >> 5));
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00021358 File Offset: 0x0001F558
		public static void DrawQuad(Quad2D face, int rgbVal)
		{
			if (GraphicsSystem.gfxVertexSize < 8192)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[0].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[0].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = (byte)(rgbVal >> 16 & 255);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = (byte)(rgbVal >> 8 & 255);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = (byte)(rgbVal & 255);
				rgbVal = (rgbVal & 2130706432) >> 23;
				if (rgbVal > 253)
				{
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				}
				else
				{
					GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = (byte)rgbVal;
				}
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0.01f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0.01f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[1].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[1].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.R;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.G;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.B;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.A;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0.01f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0.01f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[2].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[2].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.R;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.G;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.B;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.A;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0.01f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0.01f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[3].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[3].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.R;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.G;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.B;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].color.A;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = 0.01f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = 0.01f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00021934 File Offset: 0x0001FB34
		public static void DrawTexturedQuad(Quad2D face, int surfaceNum)
		{
			if (GraphicsSystem.gfxVertexSize < 8192)
			{
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[0].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[0].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[0].u) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[0].v) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[1].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[1].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[1].u) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[1].v) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[2].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[2].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[2].u) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[2].v) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(face.vertex[3].x << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(face.vertex[3].y << 4);
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartX + face.vertex[3].u) * 0.0009765625f;
				GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = (float)(GraphicsSystem.gfxSurface[surfaceNum].texStartY + face.vertex[3].v) * 0.0009765625f;
				GraphicsSystem.gfxVertexSize += 1;
				GraphicsSystem.gfxIndexSize += 2;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00021ECC File Offset: 0x000200CC
		public static void SetPaletteEntry(byte entryPos, byte cR, byte cG, byte cB)
		{
			if (entryPos > 0)
			{
				GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)entryPos] = GraphicsSystem.RGB_16BIT5551(cR, cG, cB, 1);
			}
			else
			{
				GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)entryPos] = GraphicsSystem.RGB_16BIT5551(cR, cG, cB, 0);
			}
			GraphicsSystem.tilePalette[(int)entryPos].red = cR;
			GraphicsSystem.tilePalette[(int)entryPos].green = cG;
			GraphicsSystem.tilePalette[(int)entryPos].blue = cB;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00021F44 File Offset: 0x00020144
		public static void SetFade(byte clrR, byte clrG, byte clrB, ushort clrA)
		{
			GraphicsSystem.fadeMode = 1;
			if (clrA > 255)
			{
				clrA = 255;
			}
			GraphicsSystem.fadeR = clrR;
			GraphicsSystem.fadeG = clrG;
			GraphicsSystem.fadeB = clrB;
			GraphicsSystem.fadeA = (byte)clrA;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00021F74 File Offset: 0x00020174
		public static void SetLimitedFade(byte paletteNum, byte clrR, byte clrG, byte clrB, ushort clrA, int fStart, int fEnd)
		{
			byte[] array = new byte[3];
			GraphicsSystem.paletteMode = paletteNum;
			if (paletteNum < 8)
			{
				if (clrA > 255)
				{
					clrA = 255;
				}
				if (fEnd < 256)
				{
					fEnd++;
				}
				for (int i = fStart; i < fEnd; i++)
				{
					array[0] = (byte)((ushort)GraphicsSystem.tilePalette[i].red * (255 - clrA) + clrA * (ushort)clrR >> 8);
					array[1] = (byte)((ushort)GraphicsSystem.tilePalette[i].green * (255 - clrA) + clrA * (ushort)clrG >> 8);
					array[2] = (byte)((ushort)GraphicsSystem.tilePalette[i].blue * (255 - clrA) + clrA * (ushort)clrB >> 8);
					GraphicsSystem.tilePalette16_Data[0, i] = GraphicsSystem.RGB_16BIT5551(array[0], array[1], array[2], 1);
				}
				GraphicsSystem.tilePalette16_Data[0, 0] = GraphicsSystem.RGB_16BIT5551(array[0], array[1], array[2], 0);
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0002206A File Offset: 0x0002026A
		public static void SetActivePalette(byte paletteNum, int minY, int maxY)
		{
			if (paletteNum < 8)
			{
				GraphicsSystem.texPaletteNum = (int)paletteNum;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00022078 File Offset: 0x00020278
		public static void CopyPalette(byte paletteSource, byte paletteDest)
		{
			if (paletteSource < 8 && paletteDest < 8)
			{
				for (int i = 0; i < 256; i++)
				{
					GraphicsSystem.tilePalette16_Data[(int)paletteDest, i] = GraphicsSystem.tilePalette16_Data[(int)paletteSource, i];
				}
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000220B8 File Offset: 0x000202B8
		public static void RotatePalette(byte pStart, byte pEnd, byte pDirection)
		{
			switch (pDirection)
			{
			case 0:
			{
				ushort num = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)pStart];
				for (byte b = pStart; b < pEnd; b += 1)
				{
					GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)b] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)(b + 1)];
				}
				GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)pEnd] = num;
				return;
			}
			case 1:
			{
				ushort num = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)pEnd];
				for (byte b = pEnd; b > pStart; b -= 1)
				{
					GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)b] = GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)(b - 1)];
				}
				GraphicsSystem.tilePalette16_Data[GraphicsSystem.texPaletteNum, (int)pStart] = num;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0002217C File Offset: 0x0002037C
		public static void GenerateBlendLookupTable()
		{
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					GraphicsSystem.blendLookupTable[num] = (ushort)(j * i >> 8);
					GraphicsSystem.subtractiveLookupTable[num] = (ushort)((31 - j) * i >> 8);
					num++;
				}
			}
		}

		// Token: 0x040001CD RID: 461
		public const int NUM_SPRITESHEETS = 24;

		// Token: 0x040001CE RID: 462
		public const int GRAPHIC_DATASIZE = 2097152;

		// Token: 0x040001CF RID: 463
		public const int VERTEX_LIMIT = 8192;

		// Token: 0x040001D0 RID: 464
		public const int INDEX_LIMIT = 49152;

		// Token: 0x040001D1 RID: 465
		public static bool render3DEnabled = false;

		// Token: 0x040001D2 RID: 466
		public static byte fadeMode = 0;

		// Token: 0x040001D3 RID: 467
		public static byte fadeR = 0;

		// Token: 0x040001D4 RID: 468
		public static byte fadeG = 0;

		// Token: 0x040001D5 RID: 469
		public static byte fadeB = 0;

		// Token: 0x040001D6 RID: 470
		public static byte fadeA = 0;

		// Token: 0x040001D7 RID: 471
		public static byte paletteMode = 0;

		// Token: 0x040001D8 RID: 472
		public static byte colourMode = 0;

		// Token: 0x040001D9 RID: 473
		public static ushort[] texBuffer = new ushort[1048576];

		// Token: 0x040001DA RID: 474
		public static byte texBufferMode = 0;

		// Token: 0x040001DB RID: 475
		public static byte[] tileGfx = new byte[262144];

		// Token: 0x040001DC RID: 476
		public static byte[] graphicData = new byte[2097152];

		// Token: 0x040001DD RID: 477
		public static GfxSurfaceDesc[] gfxSurface = new GfxSurfaceDesc[24];

		// Token: 0x040001DE RID: 478
		public static uint gfxDataPosition;

		// Token: 0x040001DF RID: 479
		public static DrawVertex[] gfxPolyList = new DrawVertex[8192];

		// Token: 0x040001E0 RID: 480
		public static DrawVertex3D[] polyList3D = new DrawVertex3D[6404];

		// Token: 0x040001E1 RID: 481
		public static short[] gfxPolyListIndex = new short[49152];

		// Token: 0x040001E2 RID: 482
		public static ushort gfxVertexSize = 0;

		// Token: 0x040001E3 RID: 483
		public static ushort gfxVertexSizeOpaque = 0;

		// Token: 0x040001E4 RID: 484
		public static ushort gfxIndexSize = 0;

		// Token: 0x040001E5 RID: 485
		public static ushort gfxIndexSizeOpaque = 0;

		// Token: 0x040001E6 RID: 486
		public static ushort vertexSize3D = 0;

		// Token: 0x040001E7 RID: 487
		public static ushort indexSize3D = 0;

		// Token: 0x040001E8 RID: 488
		public static float[] tileUVArray = new float[4096];

		// Token: 0x040001E9 RID: 489
		public static Vector3 floor3DPos = default(Vector3);

		// Token: 0x040001EA RID: 490
		public static float floor3DAngle;

		// Token: 0x040001EB RID: 491
		public static ushort[] blendLookupTable = new ushort[8192];

		// Token: 0x040001EC RID: 492
		public static ushort[] subtractiveLookupTable = new ushort[8192];

		// Token: 0x040001ED RID: 493
		public static PaletteEntry[] tilePalette = new PaletteEntry[256];

		// Token: 0x040001EE RID: 494
		public static ushort[,] tilePalette16_Data = new ushort[8, 256];

		// Token: 0x040001EF RID: 495
		public static int texPaletteNum = 0;

		// Token: 0x040001F0 RID: 496
		public static int waterDrawPos = 320;

		// Token: 0x040001F1 RID: 497
		public static bool videoPlaying = false;

		// Token: 0x040001F2 RID: 498
		public static int currentVideoFrame;
	}
}
