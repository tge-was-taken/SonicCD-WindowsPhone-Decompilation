using System;
using System.Reflection;

namespace Retro_Engine
{
	// Token: 0x02000009 RID: 9
	public static class GlobalAppDefinitions
	{
		// Token: 0x06000034 RID: 52 RVA: 0x000090CC File Offset: 0x000072CC
		static GlobalAppDefinitions()
		{
			string fullName = Assembly.GetExecutingAssembly().FullName;
			string[] array = fullName.Split(new char[]
			{
				','
			});
			string[] array2 = array[1].Split(new char[]
			{
				'='
			});
			array2[1] = array2[1].Remove(6);
			GlobalAppDefinitions.gameVersion = array2[1].ToCharArray();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00009240 File Offset: 0x00007440
		public static void CalculateTrigAngles()
		{
			for (int i = 0; i < 512; i++)
			{
				GlobalAppDefinitions.SinValueM7[i] = (int)(Math.Sin((double)i / 256.0 * 3.141592654) * 4096.0);
				GlobalAppDefinitions.CosValueM7[i] = (int)(Math.Cos((double)i / 256.0 * 3.141592654) * 4096.0);
			}
			GlobalAppDefinitions.SinValueM7[0] = 0;
			GlobalAppDefinitions.CosValueM7[0] = 4096;
			GlobalAppDefinitions.SinValueM7[128] = 4096;
			GlobalAppDefinitions.CosValueM7[128] = 0;
			GlobalAppDefinitions.SinValueM7[256] = 0;
			GlobalAppDefinitions.CosValueM7[256] = -4096;
			GlobalAppDefinitions.SinValueM7[384] = -4096;
			GlobalAppDefinitions.CosValueM7[384] = 0;
			for (int i = 0; i < 512; i++)
			{
				GlobalAppDefinitions.SinValue512[i] = (int)(Math.Sin((double)((float)i / 256f) * 3.141592654) * 512.0);
				GlobalAppDefinitions.CosValue512[i] = (int)(Math.Cos((double)((float)i / 256f) * 3.141592654) * 512.0);
			}
			GlobalAppDefinitions.SinValue512[0] = 0;
			GlobalAppDefinitions.CosValue512[0] = 512;
			GlobalAppDefinitions.SinValue512[128] = 512;
			GlobalAppDefinitions.CosValue512[128] = 0;
			GlobalAppDefinitions.SinValue512[256] = 0;
			GlobalAppDefinitions.CosValue512[256] = -512;
			GlobalAppDefinitions.SinValue512[384] = -512;
			GlobalAppDefinitions.CosValue512[384] = 0;
			for (int i = 0; i < 256; i++)
			{
				GlobalAppDefinitions.SinValue256[i] = GlobalAppDefinitions.SinValue512[i << 1] >> 1;
				GlobalAppDefinitions.CosValue256[i] = GlobalAppDefinitions.CosValue512[i << 1] >> 1;
			}
			for (int j = 0; j < 256; j++)
			{
				for (int i = 0; i < 256; i++)
				{
					GlobalAppDefinitions.ATanValue256[i, j] = (byte)(Math.Atan2((double)((float)j), (double)((float)i)) * 40.74366542620519);
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00009464 File Offset: 0x00007664
		public static byte ArcTanLookup(int x, int y)
		{
			int i;
			if (x < 0)
			{
				i = -x;
			}
			else
			{
				i = x;
			}
			int j;
			if (y < 0)
			{
				j = -y;
			}
			else
			{
				j = y;
			}
			if (i > j)
			{
				while (i > 255)
				{
					i >>= 4;
					j >>= 4;
				}
			}
			else
			{
				while (j > 255)
				{
					i >>= 4;
					j >>= 4;
				}
			}
			byte result;
			if (x > 0)
			{
				if (y > 0)
				{
					result = GlobalAppDefinitions.ATanValue256[i, j];
				}
				else
				{
					result = (byte)(256 - (int)GlobalAppDefinitions.ATanValue256[i, j]);
				}
			}
			else if (y > 0)
			{
				result = (byte)(128 - GlobalAppDefinitions.ATanValue256[i, j]);
			}
			else
			{
				result = (byte)(128 + GlobalAppDefinitions.ATanValue256[i, j]);
			}
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000950C File Offset: 0x0000770C
		public static void LoadGameConfig(char[] filePath)
		{
			FileData fData = new FileData();
			char[] array = new char[32];
			if (FileIO.LoadFile(filePath, fData))
			{
				byte b = FileIO.ReadByte();
				FileIO.ReadCharArray(ref GlobalAppDefinitions.gameWindowText, (int)b);
				GlobalAppDefinitions.gameWindowText[(int)b] = '\0';
				b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				b = FileIO.ReadByte();
				FileIO.ReadCharArray(ref GlobalAppDefinitions.gameDescriptionText, (int)b);
				GlobalAppDefinitions.gameDescriptionText[(int)b] = '\0';
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
				ObjectSystem.NO_GLOBALVARIABLES = 0;
				for (int j = 0; j < (int)b3; j++)
				{
					ObjectSystem.NO_GLOBALVARIABLES += 1;
					b = FileIO.ReadByte();
					int i;
					byte b2;
					for (i = 0; i < (int)b; i++)
					{
						b2 = FileIO.ReadByte();
						array[i] = (char)b2;
					}
					array[i] = '\0';
					FileIO.StrCopy2D(ref ObjectSystem.globalVariableNames, ref array, j);
					b2 = FileIO.ReadByte();
					ObjectSystem.globalVariables[j] = (int)b2 << 24;
					b2 = FileIO.ReadByte();
					ObjectSystem.globalVariables[j] += (int)b2 << 16;
					b2 = FileIO.ReadByte();
					ObjectSystem.globalVariables[j] += (int)b2 << 8;
					b2 = FileIO.ReadByte();
					ObjectSystem.globalVariables[j] += (int)b2;
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
				FileIO.noPresentationStages = FileIO.ReadByte();
				for (int j = 0; j < (int)FileIO.noPresentationStages; j++)
				{
					b = FileIO.ReadByte();
					int i;
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.pStageList[j].stageFolderName[i] = (char)b2;
					}
					FileIO.pStageList[j].stageFolderName[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.pStageList[j].actNumber[i] = (char)b2;
					}
					FileIO.pStageList[j].actNumber[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
					b = FileIO.ReadByte();
				}
				FileIO.noZoneStages = FileIO.ReadByte();
				for (int j = 0; j < (int)FileIO.noZoneStages; j++)
				{
					b = FileIO.ReadByte();
					int i;
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.zStageList[j].stageFolderName[i] = (char)b2;
					}
					FileIO.zStageList[j].stageFolderName[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.zStageList[j].actNumber[i] = (char)b2;
					}
					FileIO.zStageList[j].actNumber[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
					b = FileIO.ReadByte();
				}
				FileIO.noSpecialStages = FileIO.ReadByte();
				for (int j = 0; j < (int)FileIO.noSpecialStages; j++)
				{
					b = FileIO.ReadByte();
					int i;
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.sStageList[j].stageFolderName[i] = (char)b2;
					}
					FileIO.sStageList[j].stageFolderName[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.sStageList[j].actNumber[i] = (char)b2;
					}
					FileIO.sStageList[j].actNumber[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
					b = FileIO.ReadByte();
				}
				FileIO.noBonusStages = FileIO.ReadByte();
				for (int j = 0; j < (int)FileIO.noBonusStages; j++)
				{
					b = FileIO.ReadByte();
					int i;
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.bStageList[j].stageFolderName[i] = (char)b2;
					}
					FileIO.bStageList[j].stageFolderName[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						FileIO.bStageList[j].actNumber[i] = (char)b2;
					}
					FileIO.bStageList[j].actNumber[i] = '\0';
					b = FileIO.ReadByte();
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
					b = FileIO.ReadByte();
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x0400006E RID: 110
		public const int RETRO_EN = 0;

		// Token: 0x0400006F RID: 111
		public const int RETRO_FR = 1;

		// Token: 0x04000070 RID: 112
		public const int RETRO_IT = 2;

		// Token: 0x04000071 RID: 113
		public const int RETRO_DE = 3;

		// Token: 0x04000072 RID: 114
		public const int RETRO_ES = 4;

		// Token: 0x04000073 RID: 115
		public const int RETRO_JP = 5;

		// Token: 0x04000074 RID: 116
		public const int DEFAULTSCREEN = 0;

		// Token: 0x04000075 RID: 117
		public const int MAINGAME = 1;

		// Token: 0x04000076 RID: 118
		public const int RESETGAME = 2;

		// Token: 0x04000077 RID: 119
		public const int EXITGAME = 3;

		// Token: 0x04000078 RID: 120
		public const int SCRIPTERROR = 4;

		// Token: 0x04000079 RID: 121
		public const int ENTER_HIRESMODE = 5;

		// Token: 0x0400007A RID: 122
		public const int EXIT_HIRESMODE = 6;

		// Token: 0x0400007B RID: 123
		public const int PAUSE_ENGINE = 7;

		// Token: 0x0400007C RID: 124
		public const int PAUSE_WAIT = 8;

		// Token: 0x0400007D RID: 125
		public const int VIDEO_WAIT = 9;

		// Token: 0x0400007E RID: 126
		public const int RETRO_WIN = 0;

		// Token: 0x0400007F RID: 127
		public const int RETRO_OSX = 1;

		// Token: 0x04000080 RID: 128
		public const int RETRO_XBOX_360 = 2;

		// Token: 0x04000081 RID: 129
		public const int RETRO_PS3 = 3;

		// Token: 0x04000082 RID: 130
		public const int RETRO_iOS = 4;

		// Token: 0x04000083 RID: 131
		public const int RETRO_ANDROID = 5;

		// Token: 0x04000084 RID: 132
		public const int RETRO_WP7 = 6;

		// Token: 0x04000085 RID: 133
		public const int MAX_PLAYERS = 2;

		// Token: 0x04000086 RID: 134
		public const int FACING_LEFT = 1;

		// Token: 0x04000087 RID: 135
		public const int FACING_RIGHT = 0;

		// Token: 0x04000088 RID: 136
		public const int GAME_FULL = 0;

		// Token: 0x04000089 RID: 137
		public const int GAME_TRIAL = 1;

		// Token: 0x0400008A RID: 138
		public const int OBJECT_BORDER_Y1 = 256;

		// Token: 0x0400008B RID: 139
		public const int OBJECT_BORDER_Y2 = 496;

		// Token: 0x0400008C RID: 140
		public const double Pi = 3.141592654;

		// Token: 0x0400008D RID: 141
		public static char[] gameWindowText = "Retro-Engine".ToCharArray();

		// Token: 0x0400008E RID: 142
		public static char[] gameVersion;

		// Token: 0x0400008F RID: 143
		public static char[] gameDescriptionText = new char[256];

		// Token: 0x04000090 RID: 144
		public static char[] gamePlatform = "Mobile".ToCharArray();

		// Token: 0x04000091 RID: 145
		public static char[] gameRenderType = "HW_Rendering".ToCharArray();

		// Token: 0x04000092 RID: 146
		public static char[] gameHapticsSetting = "No_Haptics".ToCharArray();

		// Token: 0x04000093 RID: 147
		public static byte gameMode = 1;

		// Token: 0x04000094 RID: 148
		public static byte gameLanguage = 0;

		// Token: 0x04000095 RID: 149
		public static int gameMessage = 0;

		// Token: 0x04000096 RID: 150
		public static byte gameOnlineActive = 1;

		// Token: 0x04000097 RID: 151
		public static byte gameHapticsEnabled = 0;

		// Token: 0x04000098 RID: 152
		public static byte frameCounter = 0;

		// Token: 0x04000099 RID: 153
		public static int frameSkipTimer = -1;

		// Token: 0x0400009A RID: 154
		public static int frameSkipSetting = 0;

		// Token: 0x0400009B RID: 155
		public static int gameSFXVolume = 100;

		// Token: 0x0400009C RID: 156
		public static int gameBGMVolume = 100;

		// Token: 0x0400009D RID: 157
		public static byte gameTrialMode = 0;

		// Token: 0x0400009E RID: 158
		public static int gamePlatformID = 6;

		// Token: 0x0400009F RID: 159
		public static bool HQ3DFloorEnabled = false;

		// Token: 0x040000A0 RID: 160
		public static int SCREEN_XSIZE = 320;

		// Token: 0x040000A1 RID: 161
		public static int SCREEN_CENTER;

		// Token: 0x040000A2 RID: 162
		public static int SCREEN_SCROLL_LEFT;

		// Token: 0x040000A3 RID: 163
		public static int SCREEN_SCROLL_RIGHT;

		// Token: 0x040000A4 RID: 164
		public static int OBJECT_BORDER_X1;

		// Token: 0x040000A5 RID: 165
		public static int OBJECT_BORDER_X2;

		// Token: 0x040000A6 RID: 166
		public static int[] SinValue256 = new int[256];

		// Token: 0x040000A7 RID: 167
		public static int[] CosValue256 = new int[256];

		// Token: 0x040000A8 RID: 168
		public static int[] SinValue512 = new int[512];

		// Token: 0x040000A9 RID: 169
		public static int[] CosValue512 = new int[512];

		// Token: 0x040000AA RID: 170
		public static int[] SinValueM7 = new int[512];

		// Token: 0x040000AB RID: 171
		public static int[] CosValueM7 = new int[512];

		// Token: 0x040000AC RID: 172
		public static byte[,] ATanValue256 = new byte[256, 256];
	}
}
