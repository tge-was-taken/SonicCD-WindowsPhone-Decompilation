using System;

namespace Retro_Engine
{
	// Token: 0x02000010 RID: 16
	public static class TextSystem
	{
		// Token: 0x06000073 RID: 115 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		static TextSystem()
		{
			for (int i = 0; i < TextSystem.fontCharacterList.Length; i++)
			{
				TextSystem.fontCharacterList[i] = default(FontCharacter);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000C934 File Offset: 0x0000AB34
		public static void LoadFontFile(char[] fileName)
		{
			int num = 0;
			FileData fData = new FileData();
			if (FileIO.LoadFile(fileName, fData))
			{
				while (!FileIO.ReachedEndOfFile())
				{
					byte b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].id = (int)b;
					b = FileIO.ReadByte();
					FontCharacter[] array = TextSystem.fontCharacterList;
					int num2 = num;
					array[num2].id = array[num2].id + ((int)b << 8);
					b = FileIO.ReadByte();
					FontCharacter[] array2 = TextSystem.fontCharacterList;
					int num3 = num;
					array2[num3].id = array2[num3].id + ((int)b << 16);
					b = FileIO.ReadByte();
					FontCharacter[] array3 = TextSystem.fontCharacterList;
					int num4 = num;
					array3[num4].id = array3[num4].id + ((int)b << 24);
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].left = (short)b;
					b = FileIO.ReadByte();
					FontCharacter[] array4 = TextSystem.fontCharacterList;
					int num5 = num;
					array4[num5].left = (short)(array4[num5].left + (short)(b << 8));
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].top = (short)b;
					b = FileIO.ReadByte();
					FontCharacter[] array5 = TextSystem.fontCharacterList;
					int num6 = num;
					array5[num6].top = (short)(array5[num6].top + (short)(b << 8));
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].xSize = (short)b;
					b = FileIO.ReadByte();
					FontCharacter[] array6 = TextSystem.fontCharacterList;
					int num7 = num;
					array6[num7].xSize = (short)(array6[num7].xSize + (short)(b << 8));
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].ySize = (short)b;
					b = FileIO.ReadByte();
					FontCharacter[] array7 = TextSystem.fontCharacterList;
					int num8 = num;
					array7[num8].ySize = (short)(array7[num8].ySize + (short)(b << 8));
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].xPivot = (short)b;
					b = FileIO.ReadByte();
					if (b > 128)
					{
						FontCharacter[] array8 = TextSystem.fontCharacterList;
						int num9 = num;
						array8[num9].xPivot = (short)(array8[num9].xPivot + (short)(b - 128 << 8));
						TextSystem.fontCharacterList[num].xPivot = (short)(-(short)(32768 - (int)TextSystem.fontCharacterList[num].xPivot));
					}
					else
					{
						FontCharacter[] array9 = TextSystem.fontCharacterList;
						int num10 = num;
						array9[num10].xPivot = (short)(array9[num10].xPivot + (short)(b << 8));
					}
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].yPivot = (short)b;
					b = FileIO.ReadByte();
					if (b > 128)
					{
						FontCharacter[] array10 = TextSystem.fontCharacterList;
						int num11 = num;
						array10[num11].yPivot = (short)(array10[num11].yPivot + (short)(b - 128 << 8));
						TextSystem.fontCharacterList[num].yPivot = (short)(-(short)(32768 - (int)TextSystem.fontCharacterList[num].xPivot));
					}
					else
					{
						FontCharacter[] array11 = TextSystem.fontCharacterList;
						int num12 = num;
						array11[num12].yPivot = (short)(array11[num12].yPivot + (short)(b << 8));
					}
					b = FileIO.ReadByte();
					TextSystem.fontCharacterList[num].xAdvance = (short)b;
					b = FileIO.ReadByte();
					if (b > 128)
					{
						FontCharacter[] array12 = TextSystem.fontCharacterList;
						int num13 = num;
						array12[num13].xAdvance = (short)(array12[num13].xAdvance + (short)(b - 128 << 8));
						TextSystem.fontCharacterList[num].xAdvance = (short)(-(short)(32768 - (int)TextSystem.fontCharacterList[num].xAdvance));
					}
					else
					{
						FontCharacter[] array13 = TextSystem.fontCharacterList;
						int num14 = num;
						array13[num14].xAdvance = (short)(array13[num14].xAdvance + (short)(b << 8));
					}
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					num++;
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000CC80 File Offset: 0x0000AE80
		public static void LoadTextFile(TextMenu tMenu, char[] fileName, byte mapCode)
		{
			bool flag = false;
			FileData fData = new FileData();
			if (FileIO.LoadFile(fileName, fData))
			{
				tMenu.textDataPos = 0;
				tMenu.numRows = 0;
				tMenu.entryStart[(int)tMenu.numRows] = tMenu.textDataPos;
				tMenu.entrySize[(int)tMenu.numRows] = 0;
				byte b = FileIO.ReadByte();
				if (b == 255)
				{
					b = FileIO.ReadByte();
					while (!flag)
					{
						b = FileIO.ReadByte();
						ushort num = (ushort)b;
						b = FileIO.ReadByte();
						num += (ushort)(b << 8);
						ushort num2 = num;
						if (num2 != 10)
						{
							if (num2 == 13)
							{
								tMenu.numRows += 1;
								if (tMenu.numRows > 511)
								{
									flag = true;
								}
								else
								{
									tMenu.entryStart[(int)tMenu.numRows] = tMenu.textDataPos;
									tMenu.entrySize[(int)tMenu.numRows] = 0;
								}
							}
							else
							{
								if (mapCode == 1)
								{
									int i = 0;
									while (i < 1024)
									{
										if (TextSystem.fontCharacterList[i].id == (int)num)
										{
											num = (ushort)i;
											i = 1025;
										}
										else
										{
											i++;
										}
									}
									if (i == 1024)
									{
										num = 0;
									}
								}
								tMenu.textData[tMenu.textDataPos] = (char)num;
								tMenu.textDataPos++;
								tMenu.entrySize[(int)tMenu.numRows]++;
							}
						}
						if (!flag)
						{
							flag = FileIO.ReachedEndOfFile();
							if (tMenu.textDataPos >= 10240)
							{
								flag = true;
							}
						}
					}
				}
				else
				{
					ushort num = (ushort)b;
					ushort num3 = num;
					if (num3 != 10)
					{
						if (num3 == 13)
						{
							tMenu.numRows += 1;
							tMenu.entryStart[(int)tMenu.numRows] = tMenu.textDataPos;
							tMenu.entrySize[(int)tMenu.numRows] = 0;
						}
						else
						{
							if (mapCode == 1)
							{
								int i = 0;
								while (i < 1024)
								{
									if (TextSystem.fontCharacterList[i].id == (int)num)
									{
										num = (ushort)i;
										i = 1025;
									}
									else
									{
										i++;
									}
								}
								if (i == 1024)
								{
									num = 0;
								}
							}
							tMenu.textData[tMenu.textDataPos] = (char)num;
							tMenu.textDataPos++;
							tMenu.entrySize[(int)tMenu.numRows]++;
						}
					}
					while (!flag)
					{
						b = FileIO.ReadByte();
						num = (ushort)b;
						ushort num4 = num;
						if (num4 != 10)
						{
							if (num4 == 13)
							{
								tMenu.numRows += 1;
								if (tMenu.numRows > 511)
								{
									flag = true;
								}
								else
								{
									tMenu.entryStart[(int)tMenu.numRows] = tMenu.textDataPos;
									tMenu.entrySize[(int)tMenu.numRows] = 0;
								}
							}
							else
							{
								if (mapCode == 1)
								{
									int i = 0;
									while (i < 1024)
									{
										if (TextSystem.fontCharacterList[i].id == (int)num)
										{
											num = (ushort)i;
											i = 1025;
										}
										else
										{
											i++;
										}
									}
									if (i == 1024)
									{
										num = 0;
									}
								}
								tMenu.textData[tMenu.textDataPos] = (char)num;
								tMenu.textDataPos++;
								tMenu.entrySize[(int)tMenu.numRows]++;
							}
						}
						if (!flag)
						{
							flag = FileIO.ReachedEndOfFile();
							if (tMenu.textDataPos >= 10240)
							{
								flag = true;
							}
						}
					}
				}
				tMenu.numRows += 1;
				FileIO.CloseFile();
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000CFC4 File Offset: 0x0000B1C4
		public static void DrawBitmapText(TextMenu tMenu, int xPos, int yPos, int scale, int spacing, int rowStart, int numRows)
		{
			int num = yPos << 9;
			if (numRows < 0)
			{
				numRows = (int)tMenu.numRows;
			}
			if (rowStart + numRows > (int)tMenu.numRows)
			{
				numRows = (int)tMenu.numRows - rowStart;
			}
			while (numRows > 0)
			{
				int num2 = 0;
				int i = tMenu.entrySize[rowStart];
				int num3 = xPos << 9;
				while (i > 0)
				{
					char c = tMenu.textData[tMenu.entryStart[rowStart] + num2];
					GraphicsSystem.DrawScaledChar(0, num3 >> 5, num >> 5, (int)(-(int)TextSystem.fontCharacterList[(int)c].xPivot), (int)(-(int)TextSystem.fontCharacterList[(int)c].yPivot), scale, scale, (int)TextSystem.fontCharacterList[(int)c].xSize, (int)TextSystem.fontCharacterList[(int)c].ySize, (int)TextSystem.fontCharacterList[(int)c].left, (int)TextSystem.fontCharacterList[(int)c].top, TextSystem.textMenuSurfaceNo);
					num3 += (int)TextSystem.fontCharacterList[(int)c].xAdvance * scale;
					num2++;
					i--;
				}
				num += spacing * scale;
				rowStart++;
				numRows--;
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000D0E8 File Offset: 0x0000B2E8
		public static void SetupTextMenu(TextMenu tMenu, int numRows)
		{
			tMenu.textDataPos = 0;
			tMenu.numRows = (ushort)numRows;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		public static void AddTextMenuEntry(TextMenu tMenu, char[] inputTxt)
		{
			int i = 0;
			tMenu.entryStart[(int)tMenu.numRows] = tMenu.textDataPos;
			tMenu.entrySize[(int)tMenu.numRows] = 0;
			while (i < inputTxt.Length)
			{
				if (inputTxt[i] != '\0')
				{
					tMenu.textData[tMenu.textDataPos] = inputTxt[i];
					tMenu.textDataPos++;
					tMenu.entrySize[(int)tMenu.numRows]++;
					i++;
				}
				else
				{
					i = inputTxt.Length;
				}
			}
			tMenu.numRows += 1;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000D190 File Offset: 0x0000B390
		public static void AddTextMenuEntryMapped(TextMenu tMenu, char[] inputTxt)
		{
			int i = 0;
			tMenu.entryStart[(int)tMenu.numRows] = tMenu.textDataPos;
			tMenu.entrySize[(int)tMenu.numRows] = 0;
			while (i < inputTxt.Length)
			{
				if (inputTxt[i] != '\0')
				{
					ushort num = (ushort)inputTxt[i];
					int j = 0;
					while (j < 1024)
					{
						if (TextSystem.fontCharacterList[j].id == (int)num)
						{
							num = (ushort)j;
							j = 1025;
						}
						else
						{
							j++;
						}
					}
					if (j == 1024)
					{
						num = 0;
					}
					tMenu.textData[tMenu.textDataPos] = (char)num;
					tMenu.textDataPos++;
					tMenu.entrySize[(int)tMenu.numRows]++;
					i++;
				}
				else
				{
					i = inputTxt.Length;
				}
			}
			tMenu.numRows += 1;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000D264 File Offset: 0x0000B464
		public static void SetTextMenuEntry(TextMenu tMenu, char[] inputTxt, int rowNum)
		{
			int i = 0;
			tMenu.entryStart[rowNum] = tMenu.textDataPos;
			tMenu.entrySize[rowNum] = 0;
			while (i < inputTxt.Length)
			{
				if (inputTxt[i] != '\0')
				{
					tMenu.textData[tMenu.textDataPos] = inputTxt[i];
					tMenu.textDataPos++;
					tMenu.entrySize[rowNum]++;
					i++;
				}
				else
				{
					i = inputTxt.Length;
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
		public static void EditTextMenuEntry(TextMenu tMenu, char[] inputTxt, int rowNum)
		{
			int i = 0;
			int num = tMenu.entryStart[rowNum];
			tMenu.entrySize[rowNum] = 0;
			while (i < inputTxt.Length)
			{
				if (inputTxt[i] != '\0')
				{
					tMenu.textData[num] = inputTxt[i];
					num++;
					tMenu.entrySize[rowNum]++;
					i++;
				}
				else
				{
					i = inputTxt.Length;
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000D338 File Offset: 0x0000B538
		public static void DrawTextMenuEntry(TextMenu tMenu, int rowNum, int xPos, int yPos, int textHighL)
		{
			int num = tMenu.entryStart[rowNum];
			for (int i = 0; i < tMenu.entrySize[rowNum]; i++)
			{
				GraphicsSystem.DrawSprite(xPos + (i << 3), yPos, 8, 8, (int)((int)(tMenu.textData[num] & '\u000f') << 3), (int)((int)(tMenu.textData[num] >> 4) << 3) + textHighL, TextSystem.textMenuSurfaceNo);
				num++;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000D394 File Offset: 0x0000B594
		public static void DrawStageTextEntry(TextMenu tMenu, int rowNum, int xPos, int yPos, int textHighL)
		{
			int num = tMenu.entryStart[rowNum];
			for (int i = 0; i < tMenu.entrySize[rowNum]; i++)
			{
				if (i == tMenu.entrySize[rowNum] - 1)
				{
					GraphicsSystem.DrawSprite(xPos + (i << 3), yPos, 8, 8, (int)((int)(tMenu.textData[num] & '\u000f') << 3), (int)((int)(tMenu.textData[num] >> 4) << 3), TextSystem.textMenuSurfaceNo);
				}
				else
				{
					GraphicsSystem.DrawSprite(xPos + (i << 3), yPos, 8, 8, (int)((int)(tMenu.textData[num] & '\u000f') << 3), (int)((int)(tMenu.textData[num] >> 4) << 3) + textHighL, TextSystem.textMenuSurfaceNo);
				}
				num++;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000D42C File Offset: 0x0000B62C
		public static void DrawBlendedTextMenuEntry(TextMenu tMenu, int rowNum, int xPos, int yPos, int textHighL)
		{
			int num = tMenu.entryStart[rowNum];
			for (int i = 0; i < tMenu.entrySize[rowNum]; i++)
			{
				GraphicsSystem.DrawBlendedSprite(xPos + (i << 3), yPos, 8, 8, (int)((int)(tMenu.textData[num] & '\u000f') << 3), (int)((int)(tMenu.textData[num] >> 4) << 3) + textHighL, TextSystem.textMenuSurfaceNo);
				num++;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000D488 File Offset: 0x0000B688
		public static void DrawTextMenu(TextMenu tMenu, int xPos, int yPos)
		{
			int num;
			if (tMenu.numVisibleRows > 0)
			{
				num = (int)(tMenu.numVisibleRows + tMenu.visibleRowOffset);
			}
			else
			{
				tMenu.visibleRowOffset = 0;
				num = (int)tMenu.numRows;
			}
			if (tMenu.numSelections == 3)
			{
				tMenu.selection2 = -1;
				for (int i = 0; i < tMenu.selection1 + 1; i++)
				{
					if (tMenu.entryHighlight[i] == 1)
					{
						tMenu.selection2 = i;
					}
				}
			}
			switch (tMenu.alignment)
			{
			case 0:
				for (int i = (int)tMenu.visibleRowOffset; i < num; i++)
				{
					switch (tMenu.numSelections)
					{
					case 1:
						if (i == tMenu.selection1)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos, yPos, 0);
						}
						break;
					case 2:
						if (i == tMenu.selection1 || i == tMenu.selection2)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos, yPos, 0);
						}
						break;
					case 3:
						if (i == tMenu.selection1)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos, yPos, 0);
						}
						if (i == tMenu.selection2 && i != tMenu.selection1)
						{
							TextSystem.DrawStageTextEntry(tMenu, i, xPos, yPos, 128);
						}
						break;
					}
					yPos += 8;
				}
				return;
			case 1:
				for (int i = (int)tMenu.visibleRowOffset; i < num; i++)
				{
					int xPos2 = xPos - (tMenu.entrySize[i] << 3);
					switch (tMenu.numSelections)
					{
					case 1:
						if (i == tMenu.selection1)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 0);
						}
						break;
					case 2:
						if (i == tMenu.selection1 || i == tMenu.selection2)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 0);
						}
						break;
					case 3:
						if (i == tMenu.selection1)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 0);
						}
						if (i == tMenu.selection2 && i != tMenu.selection1)
						{
							TextSystem.DrawStageTextEntry(tMenu, i, xPos2, yPos, 128);
						}
						break;
					}
					yPos += 8;
				}
				return;
			case 2:
				for (int i = (int)tMenu.visibleRowOffset; i < num; i++)
				{
					int xPos2 = xPos - (tMenu.entrySize[i] >> 1 << 3);
					switch (tMenu.numSelections)
					{
					case 1:
						if (i == tMenu.selection1)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 0);
						}
						break;
					case 2:
						if (i == tMenu.selection1 || i == tMenu.selection2)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 0);
						}
						break;
					case 3:
						if (i == tMenu.selection1)
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 128);
						}
						else
						{
							TextSystem.DrawTextMenuEntry(tMenu, i, xPos2, yPos, 0);
						}
						if (i == tMenu.selection2 && i != tMenu.selection1)
						{
							TextSystem.DrawStageTextEntry(tMenu, i, xPos2, yPos, 128);
						}
						break;
					}
					yPos += 8;
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		public static void LoadConfigListText(TextMenu tMenu, int listNo)
		{
			FileData fData = new FileData();
			char[] array = new char[32];
			if (FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fData))
			{
				byte b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				byte b3 = FileIO.ReadByte();
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					for (int i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
				}
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					for (int i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
				}
				b3 = FileIO.ReadByte();
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					byte b2;
					for (int i = 0; i < (int)b; i++)
					{
						b2 = FileIO.ReadByte();
					}
					b2 = FileIO.ReadByte();
					b2 = FileIO.ReadByte();
					b2 = FileIO.ReadByte();
					b2 = FileIO.ReadByte();
				}
				b3 = FileIO.ReadByte();
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					for (int i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
				}
				b3 = FileIO.ReadByte();
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					int i;
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						array[i] = (char)b2;
					}
					array[i] = '\0';
					if (listNo == 0)
					{
						TextSystem.AddTextMenuEntry(tMenu, array);
					}
				}
				for (int k = 1; k < 5; k++)
				{
					b3 = FileIO.ReadByte();
					for (int j = 0; j < (int)b3; j++)
					{
						b = FileIO.ReadByte();
						int i;
						byte b2;
						for (i = 0; i < (int)b; i++)
						{
							b2 = FileIO.ReadByte();
						}
						b = FileIO.ReadByte();
						for (i = 0; i < (int)b; i++)
						{
							b2 = FileIO.ReadByte();
						}
						b = FileIO.ReadByte();
						for (i = 0; i < (int)b; i++)
						{
							b2 = FileIO.ReadByte();
							array[i] = (char)b2;
						}
						array[i] = '\0';
						b2 = FileIO.ReadByte();
						if (listNo == k)
						{
							tMenu.entryHighlight[j] = b2;
							TextSystem.AddTextMenuEntry(tMenu, array);
						}
					}
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x0400010C RID: 268
		public static int textMenuSurfaceNo;

		// Token: 0x0400010D RID: 269
		public static FontCharacter[] fontCharacterList = new FontCharacter[1024];
	}
}
