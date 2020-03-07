using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;

namespace Retro_Engine
{
	// Token: 0x02000006 RID: 6
	public static class FileIO
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00007A74 File Offset: 0x00005C74
		static FileIO()
		{
			for (int i = 0; i < FileIO.pStageList.Length; i++)
			{
				FileIO.pStageList[i] = new StageList();
			}
			for (int i = 0; i < FileIO.zStageList.Length; i++)
			{
				FileIO.zStageList[i] = new StageList();
			}
			for (int i = 0; i < FileIO.bStageList.Length; i++)
			{
				FileIO.bStageList[i] = new StageList();
			}
			for (int i = 0; i < FileIO.sStageList.Length; i++)
			{
				FileIO.sStageList[i] = new StageList();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00007BA4 File Offset: 0x00005DA4
		public static void StrCopy(ref char[] strA, ref char[] strB)
		{
			int i = 0;
			bool flag = true;
			if (i == strB.Length || i == strA.Length)
			{
				flag = false;
			}
			while (flag)
			{
				strA[i] = strB[i];
				if (strB[i] == '\0')
				{
					flag = false;
				}
				i++;
				if (i == strB.Length || i == strA.Length)
				{
					flag = false;
				}
			}
			while (i < strA.Length)
			{
				strA[i] = '\0';
				i++;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00007C00 File Offset: 0x00005E00
		public static void StrClear(ref char[] strA)
		{
			for (int i = 0; i < strA.Length; i++)
			{
				strA[i] = '\0';
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00007C24 File Offset: 0x00005E24
		public static void StrCopy2D(ref char[,] strA, ref char[] strB, int strPos)
		{
			int i = 0;
			bool flag = true;
			if (i == strB.Length || i == strA.GetLength(1))
			{
				flag = false;
			}
			while (flag)
			{
				strA[strPos, i] = strB[i];
				if (strB[i] == '\0')
				{
					flag = false;
				}
				i++;
				if (i == strB.Length || i == strA.GetLength(1))
				{
					flag = false;
				}
			}
			while (i < strA.GetLength(1))
			{
				strA[strPos, i] = '\0';
				i++;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00007C98 File Offset: 0x00005E98
		public static void StrAdd(ref char[] strA, ref char[] strB)
		{
			int i = 0;
			int num = 0;
			bool flag = true;
			while (strA[i] != '\0')
			{
				i++;
			}
			if (num == strB.Length || i == strA.Length)
			{
				flag = false;
			}
			while (flag)
			{
				strA[i] = strB[num];
				if (strB[num] == '\0')
				{
					flag = false;
				}
				i++;
				num++;
				if (num == strB.Length || i == strA.Length)
				{
					flag = false;
				}
			}
			while (i < strA.Length)
			{
				strA[i] = '\0';
				i++;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00007D08 File Offset: 0x00005F08
		public static bool StringComp(ref char[] strA, ref char[] strB)
		{
			bool result = true;
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 1)
			{
				if (strA[num] != strB[num2] && strA[num] != strB[num2] + ' ' && strA[num] != strB[num2] - ' ')
				{
					result = false;
					i = 1;
				}
				else if (strA[num] == '\0')
				{
					i = 1;
				}
				else
				{
					num++;
					num2++;
				}
			}
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00007D60 File Offset: 0x00005F60
		public static int StringLength(ref char[] strA)
		{
			int num = 0;
			if (strA.Length == 0)
			{
				return num;
			}
			while (strA[num] != '\0' && num < strA.Length)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00007D8C File Offset: 0x00005F8C
		public static int FindStringToken(ref char[] strA, ref char[] token, char instance)
		{
			int num = 0;
			int result = -1;
			int num2 = 0;
			while (strA[num] != '\0')
			{
				int num3 = 0;
				bool flag = true;
				while (token[num3] != '\0')
				{
					if (strA[num + num3] == '\0')
					{
						return result;
					}
					if (strA[num + num3] != token[num3])
					{
						flag = false;
					}
					num3++;
				}
				if (flag)
				{
					num2++;
					if (num2 == (int)instance)
					{
						return num;
					}
				}
				num++;
			}
			return result;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public static bool ConvertStringToInteger(ref char[] strA, ref int sValue)
		{
			int num = 0;
			bool flag = false;
			sValue = 0;
			if ((strA[num] > '/' && strA[num] < ':') || strA[num] == '-' || strA[num] == '+')
			{
				int i = FileIO.StringLength(ref strA) - 1;
				if (strA[num] == '-')
				{
					flag = true;
					num++;
					i--;
				}
				else if (strA[num] == '+')
				{
					num++;
					i--;
				}
				while (i > -1)
				{
					if (strA[num] <= '/' || strA[num] >= ':')
					{
						return false;
					}
					if (i > 0)
					{
						int num2 = (int)(strA[num] - '0');
						for (int j = i; j > 0; j--)
						{
							num2 *= 10;
						}
						sValue += num2;
					}
					else
					{
						sValue += (int)(strA[num] - '0');
					}
					i--;
					num++;
				}
				if (flag)
				{
					sValue = -sValue;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00007EB0 File Offset: 0x000060B0
		public static bool CheckRSDKFile()
		{
			FileData fData = new FileData();
			FileIO.fileReader = TitleContainer.OpenStream("Content\\Data.rsdk");
			if (FileIO.fileReader.Length <= 0L)
			{
				FileIO.fileReader.Close();
				FileIO.useRSDKFile = false;
				FileIO.useByteCode = false;
				return false;
			}
			FileIO.useRSDKFile = true;
			FileIO.useByteCode = false;
			FileIO.fileReader.Close();
			if (FileIO.LoadFile("Data/Scripts/ByteCode/GlobalCode.bin".ToCharArray(), fData))
			{
				FileIO.useByteCode = true;
				FileIO.CloseFile();
				return true;
			}
			return false;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00007F30 File Offset: 0x00006130
		public static bool LoadFile(char[] filePath, FileData fData)
		{
			int i = 0;
			bool flag = true;
			while (flag)
			{
				fData.fileName[i] = filePath[i];
				if (filePath[i] == '\0')
				{
					flag = false;
				}
				i++;
				if (i == filePath.Length)
				{
					flag = false;
				}
			}
			while (i < fData.fileName.Length)
			{
				fData.fileName[i] = '\0';
				i++;
			}
			if (FileIO.fileReader.Length > 0L)
			{
				FileIO.fileReader.Close();
			}
			FileIO.fileReader = TitleContainer.OpenStream("Content\\Data.rsdk");
			FileIO.fileSize = (uint)FileIO.fileReader.Length;
			FileIO.bufferPosition = 0U;
			FileIO.readSize = 0U;
			FileIO.readPos = 0U;
			if (!FileIO.ParseVirtualFileSystem(fData.fileName))
			{
				FileIO.fileReader.Close();
				return false;
			}
			fData.fileSize = FileIO.vFileSize;
			fData.virtualFileOffset = FileIO.virtualFileOffset;
			FileIO.bufferPosition = 0U;
			FileIO.readSize = 0U;
			return true;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00008003 File Offset: 0x00006203
		public static void CloseFile()
		{
			if (FileIO.fileReader.Length > 0L)
			{
				FileIO.fileReader.Close();
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00008020 File Offset: 0x00006220
		public static bool CheckCurrentStageFolder(int sNumber)
		{
			switch (FileIO.activeStageList)
			{
			case 0:
				if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.pStageList[sNumber].stageFolderName))
				{
					return true;
				}
				FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.pStageList[sNumber].stageFolderName);
				return false;
			case 1:
				if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.zStageList[sNumber].stageFolderName))
				{
					return true;
				}
				FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.zStageList[sNumber].stageFolderName);
				return false;
			case 2:
				if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.bStageList[sNumber].stageFolderName))
				{
					return true;
				}
				FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.bStageList[sNumber].stageFolderName);
				return false;
			case 3:
				if (FileIO.StringComp(ref FileIO.currentStageFolder, ref FileIO.sStageList[sNumber].stageFolderName))
				{
					return true;
				}
				FileIO.StrCopy(ref FileIO.currentStageFolder, ref FileIO.sStageList[sNumber].stageFolderName);
				return false;
			default:
				return false;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00008118 File Offset: 0x00006318
		public static void ResetCurrentStageFolder()
		{
			char[] array = "".ToCharArray();
			FileIO.StrCopy(ref FileIO.currentStageFolder, ref array);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000813C File Offset: 0x0000633C
		public static bool LoadStageFile(char[] filePath, int sNumber, FileData fData)
		{
			char[] filePath2 = new char[64];
			char[] array = "Data/Stages/".ToCharArray();
			FileIO.StrCopy(ref filePath2, ref array);
			switch (FileIO.activeStageList)
			{
			case 0:
				FileIO.StrAdd(ref filePath2, ref FileIO.pStageList[sNumber].stageFolderName);
				break;
			case 1:
				FileIO.StrAdd(ref filePath2, ref FileIO.zStageList[sNumber].stageFolderName);
				break;
			case 2:
				FileIO.StrAdd(ref filePath2, ref FileIO.bStageList[sNumber].stageFolderName);
				break;
			case 3:
				FileIO.StrAdd(ref filePath2, ref FileIO.sStageList[sNumber].stageFolderName);
				break;
			}
			array = "/".ToCharArray();
			FileIO.StrAdd(ref filePath2, ref array);
			FileIO.StrAdd(ref filePath2, ref filePath);
			return FileIO.LoadFile(filePath2, fData);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000081FC File Offset: 0x000063FC
		public static bool LoadActFile(char[] filePath, int sNumber, FileData fData)
		{
			char[] filePath2 = new char[64];
			char[] array = "Data/Stages/".ToCharArray();
			FileIO.StrCopy(ref filePath2, ref array);
			switch (FileIO.activeStageList)
			{
			case 0:
				FileIO.StrAdd(ref filePath2, ref FileIO.pStageList[sNumber].stageFolderName);
				break;
			case 1:
				FileIO.StrAdd(ref filePath2, ref FileIO.zStageList[sNumber].stageFolderName);
				break;
			case 2:
				FileIO.StrAdd(ref filePath2, ref FileIO.bStageList[sNumber].stageFolderName);
				break;
			case 3:
				FileIO.StrAdd(ref filePath2, ref FileIO.sStageList[sNumber].stageFolderName);
				break;
			}
			array = "/Act".ToCharArray();
			FileIO.StrAdd(ref filePath2, ref array);
			switch (FileIO.activeStageList)
			{
			case 0:
				FileIO.StrAdd(ref filePath2, ref FileIO.pStageList[sNumber].actNumber);
				FileIO.ConvertStringToInteger(ref FileIO.pStageList[sNumber].actNumber, ref FileIO.actNumber);
				break;
			case 1:
				FileIO.StrAdd(ref filePath2, ref FileIO.zStageList[sNumber].actNumber);
				FileIO.ConvertStringToInteger(ref FileIO.zStageList[sNumber].actNumber, ref FileIO.actNumber);
				break;
			case 2:
				FileIO.StrAdd(ref filePath2, ref FileIO.bStageList[sNumber].actNumber);
				FileIO.ConvertStringToInteger(ref FileIO.bStageList[sNumber].actNumber, ref FileIO.actNumber);
				break;
			case 3:
				FileIO.StrAdd(ref filePath2, ref FileIO.sStageList[sNumber].actNumber);
				FileIO.ConvertStringToInteger(ref FileIO.sStageList[sNumber].actNumber, ref FileIO.actNumber);
				break;
			}
			FileIO.StrAdd(ref filePath2, ref filePath);
			return FileIO.LoadFile(filePath2, fData);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000838C File Offset: 0x0000658C
		public static bool ParseVirtualFileSystem(char[] filePath)
		{
			char[] array = new char[64];
			char[] array2 = new char[64];
			char[] array3 = new char[64];
			int num = 0;
			int i = 0;
			FileIO.virtualFileOffset = 0U;
			int j = 0;
			while (filePath[j] != '\0')
			{
				if (filePath[j] == '/')
				{
					num = j;
					i = 0;
				}
				else
				{
					i++;
				}
				array[j] = filePath[j];
				j++;
			}
			num++;
			for (j = 0; j < i; j++)
			{
				array2[j] = filePath[num + j];
			}
			array2[i] = '\0';
			array[num] = '\0';
			FileIO.fileReader.Seek(0L, 0);
			FileIO.useRSDKFile = false;
			FileIO.bufferPosition = 0U;
			FileIO.readSize = 0U;
			FileIO.readPos = 0U;
			byte b = FileIO.ReadByte();
			num = (int)b;
			b = FileIO.ReadByte();
			num += (int)b << 8;
			b = FileIO.ReadByte();
			num += (int)b << 16;
			b = FileIO.ReadByte();
			num += (int)b << 24;
			b = FileIO.ReadByte();
			byte b2 = b;
			b = FileIO.ReadByte();
			b2 += (byte)(b << 8);
			j = 0;
			int num2 = 0;
			while (j < (int)b2)
			{
				b = FileIO.ReadByte();
				for (i = 0; i < (int)b; i++)
				{
					array3[i] = (char)(FileIO.ReadByte() ^ byte.MaxValue - b);
				}
				array3[i] = '\0';
				if (FileIO.StringComp(ref array, ref array3))
				{
					b = 1;
				}
				else
				{
					b = 0;
				}
				if (b == 1)
				{
					j = (int)b2;
					b = FileIO.ReadByte();
					num2 = (int)b;
					b = FileIO.ReadByte();
					num2 += (int)b << 8;
					b = FileIO.ReadByte();
					num2 += (int)b << 16;
					b = FileIO.ReadByte();
					num2 += (int)b << 24;
				}
				else
				{
					num2 = -1;
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					j++;
				}
			}
			if (num2 == -1)
			{
				FileIO.useRSDKFile = true;
				return false;
			}
			FileIO.fileReader.Seek((long)(num + num2), 0);
			FileIO.bufferPosition = 0U;
			FileIO.readSize = 0U;
			FileIO.readPos = 0U;
			FileIO.virtualFileOffset = (uint)(num + num2);
			j = 0;
			while (j < 1)
			{
				b = FileIO.ReadByte();
				FileIO.virtualFileOffset += 1U;
				i = 0;
				while (i < (int)b)
				{
					array3[i] = (char)(FileIO.ReadByte() ^ byte.MaxValue);
					i++;
					FileIO.virtualFileOffset += 1U;
				}
				array3[i] = '\0';
				if (FileIO.StringComp(ref array2, ref array3))
				{
					j = 1;
					b = FileIO.ReadByte();
					i = (int)b;
					b = FileIO.ReadByte();
					i += (int)b << 8;
					b = FileIO.ReadByte();
					i += (int)b << 16;
					b = FileIO.ReadByte();
					i += (int)b << 24;
					FileIO.virtualFileOffset += 4U;
					FileIO.vFileSize = (uint)i;
					FileIO.fileReader.Seek((long)((ulong)FileIO.virtualFileOffset), 0);
					FileIO.bufferPosition = 0U;
					FileIO.readSize = 0U;
					FileIO.readPos = FileIO.virtualFileOffset;
				}
				else
				{
					b = FileIO.ReadByte();
					i = (int)b;
					b = FileIO.ReadByte();
					i += (int)b << 8;
					b = FileIO.ReadByte();
					i += (int)b << 16;
					b = FileIO.ReadByte();
					i += (int)b << 24;
					FileIO.virtualFileOffset += 4U;
					FileIO.virtualFileOffset += (uint)i;
					FileIO.fileReader.Seek((long)((ulong)FileIO.virtualFileOffset), 0);
					FileIO.bufferPosition = 0U;
					FileIO.readSize = 0U;
					FileIO.readPos = FileIO.virtualFileOffset;
				}
			}
			FileIO.eStringNo = (byte)((FileIO.vFileSize & 508U) >> 2);
			FileIO.eStringPosB = 1 + FileIO.eStringNo % 9;
			FileIO.eStringPosA = 1 + FileIO.eStringNo % FileIO.eStringPosB;
			FileIO.eNybbleSwap = false;
			FileIO.useRSDKFile = true;
			return true;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00008728 File Offset: 0x00006928
		public static byte ReadByte()
		{
			byte b = 0;
			if (FileIO.readPos <= FileIO.fileSize)
			{
				if (!FileIO.useRSDKFile)
				{
					if (FileIO.bufferPosition == FileIO.readSize)
					{
						FileIO.FillFileBuffer();
					}
					b = FileIO.fileBuffer[(int)((UIntPtr)FileIO.bufferPosition)];
					FileIO.bufferPosition += 1U;
				}
				else
				{
					if (FileIO.bufferPosition == FileIO.readSize)
					{
						FileIO.FillFileBuffer();
					}
					b = (byte)((char)(FileIO.fileBuffer[(int)((UIntPtr)FileIO.bufferPosition)] ^ FileIO.eStringNo) ^ FileIO.encryptionStringB[(int)FileIO.eStringPosB]);
					if (FileIO.eNybbleSwap)
					{
						b = (byte)((b >> 4) + ((int)(b & 15) << 4));
					}
					b ^= (byte)FileIO.encryptionStringA[(int)FileIO.eStringPosA];
					FileIO.eStringPosA += 1;
					FileIO.eStringPosB += 1;
					if (FileIO.eStringPosA > 19 && FileIO.eStringPosB > 11)
					{
						FileIO.eStringNo += 1;
						FileIO.eStringNo &= 127;
						if (!FileIO.eNybbleSwap)
						{
							FileIO.eNybbleSwap = true;
							FileIO.eStringPosA = 3 + FileIO.eStringNo % 15;
							FileIO.eStringPosB = 1 + FileIO.eStringNo % 7;
						}
						else
						{
							FileIO.eNybbleSwap = false;
							FileIO.eStringPosA = 6 + FileIO.eStringNo % 12;
							FileIO.eStringPosB = 4 + FileIO.eStringNo % 5;
						}
					}
					else
					{
						if (FileIO.eStringPosA > 19)
						{
							FileIO.eStringPosA = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
						if (FileIO.eStringPosB > 11)
						{
							FileIO.eStringPosB = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
					}
					FileIO.bufferPosition += 1U;
				}
			}
			return b;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000088C4 File Offset: 0x00006AC4
		public static void ReadByteArray(ref byte[] byteP, int numBytes)
		{
			int num = 0;
			if (FileIO.readPos <= FileIO.fileSize)
			{
				if (!FileIO.useRSDKFile)
				{
					while (numBytes > 0)
					{
						if (FileIO.bufferPosition == FileIO.readSize)
						{
							FileIO.FillFileBuffer();
						}
						byteP[num] = FileIO.fileBuffer[(int)((UIntPtr)FileIO.bufferPosition)];
						FileIO.bufferPosition += 1U;
						num++;
						numBytes--;
					}
					return;
				}
				while (numBytes > 0)
				{
					if (FileIO.bufferPosition >= FileIO.readSize)
					{
						FileIO.FillFileBuffer();
					}
					byteP[num] = (byte)((char)(FileIO.fileBuffer[(int)((UIntPtr)FileIO.bufferPosition)] ^ FileIO.eStringNo) ^ FileIO.encryptionStringB[(int)FileIO.eStringPosB]);
					if (FileIO.eNybbleSwap)
					{
						byteP[num] = (byte)((byteP[num] >> 4) + ((int)(byteP[num] & 15) << 4));
					}
					byte[] array = byteP;
					int num2 = num;
					array[num2] ^= (byte)FileIO.encryptionStringA[(int)FileIO.eStringPosA];
					FileIO.eStringPosA += 1;
					FileIO.eStringPosB += 1;
					if (FileIO.eStringPosA > 19 && FileIO.eStringPosB > 11)
					{
						FileIO.eStringNo += 1;
						FileIO.eStringNo &= 127;
						if (!FileIO.eNybbleSwap)
						{
							FileIO.eNybbleSwap = true;
							FileIO.eStringPosA = 3 + FileIO.eStringNo % 15;
							FileIO.eStringPosB = 1 + FileIO.eStringNo % 7;
						}
						else
						{
							FileIO.eNybbleSwap = false;
							FileIO.eStringPosA = 6 + FileIO.eStringNo % 12;
							FileIO.eStringPosB = 4 + FileIO.eStringNo % 5;
						}
					}
					else
					{
						if (FileIO.eStringPosA > 19)
						{
							FileIO.eStringPosA = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
						if (FileIO.eStringPosB > 11)
						{
							FileIO.eStringPosB = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
					}
					FileIO.bufferPosition += 1U;
					num++;
					numBytes--;
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00008A9C File Offset: 0x00006C9C
		public static void ReadCharArray(ref char[] charP, int numBytes)
		{
			int num = 0;
			if (FileIO.readPos <= FileIO.fileSize)
			{
				if (!FileIO.useRSDKFile)
				{
					while (numBytes > 0)
					{
						if (FileIO.bufferPosition == FileIO.readSize)
						{
							FileIO.FillFileBuffer();
						}
						charP[num] = (char)FileIO.fileBuffer[(int)((UIntPtr)FileIO.bufferPosition)];
						FileIO.bufferPosition += 1U;
						num++;
						numBytes--;
					}
					return;
				}
				while (numBytes > 0)
				{
					if (FileIO.bufferPosition == FileIO.readSize)
					{
						FileIO.FillFileBuffer();
					}
					byte b = (byte)((char)(FileIO.fileBuffer[(int)((UIntPtr)FileIO.bufferPosition)] ^ FileIO.eStringNo) ^ FileIO.encryptionStringB[(int)FileIO.eStringPosB]);
					if (FileIO.eNybbleSwap)
					{
						b = (byte)((b >> 4) + ((int)(b & 15) << 4));
					}
					b ^= (byte)FileIO.encryptionStringA[(int)FileIO.eStringPosA];
					charP[num] = (char)b;
					FileIO.eStringPosA += 1;
					FileIO.eStringPosB += 1;
					if (FileIO.eStringPosA > 19 && FileIO.eStringPosB > 11)
					{
						FileIO.eStringNo += 1;
						FileIO.eStringNo &= 127;
						if (!FileIO.eNybbleSwap)
						{
							FileIO.eNybbleSwap = true;
							FileIO.eStringPosA = 3 + FileIO.eStringNo % 15;
							FileIO.eStringPosB = 1 + FileIO.eStringNo % 7;
						}
						else
						{
							FileIO.eNybbleSwap = false;
							FileIO.eStringPosA = 6 + FileIO.eStringNo % 12;
							FileIO.eStringPosB = 4 + FileIO.eStringNo % 5;
						}
					}
					else
					{
						if (FileIO.eStringPosA > 19)
						{
							FileIO.eStringPosA = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
						if (FileIO.eStringPosB > 11)
						{
							FileIO.eStringPosB = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
					}
					FileIO.bufferPosition += 1U;
					num++;
					numBytes--;
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00008C5C File Offset: 0x00006E5C
		public static void FillFileBuffer()
		{
			if (FileIO.readPos + 8192U > FileIO.fileSize)
			{
				FileIO.readSize = FileIO.fileSize - FileIO.readPos;
			}
			else
			{
				FileIO.readSize = 8192U;
			}
			FileIO.fileReader.Read(FileIO.fileBuffer, 0, (int)FileIO.readSize);
			FileIO.readPos += FileIO.readSize;
			FileIO.bufferPosition = 0U;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00008CC4 File Offset: 0x00006EC4
		public static void GetFileInfo(FileData fData)
		{
			fData.bufferPos = FileIO.bufferPosition;
			fData.position = FileIO.readPos - FileIO.readSize;
			fData.eStringPosA = FileIO.eStringPosA;
			fData.eStringPosB = FileIO.eStringPosB;
			fData.eStringNo = FileIO.eStringNo;
			fData.eNybbleSwap = FileIO.eNybbleSwap;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00008D1C File Offset: 0x00006F1C
		public static void SetFileInfo(FileData fData)
		{
			if (FileIO.useRSDKFile)
			{
				if (FileIO.fileReader.Length > 0L)
				{
					FileIO.fileReader.Close();
				}
				FileIO.fileReader = TitleContainer.OpenStream("Content\\Data.rsdk");
				FileIO.virtualFileOffset = fData.virtualFileOffset;
				FileIO.vFileSize = fData.fileSize;
				FileIO.fileSize = (uint)FileIO.fileReader.Length;
				FileIO.readPos = fData.position;
				FileIO.fileReader.Seek((long)((ulong)FileIO.readPos), 0);
				FileIO.FillFileBuffer();
				FileIO.bufferPosition = fData.bufferPos;
				FileIO.eStringPosA = fData.eStringPosA;
				FileIO.eStringPosB = fData.eStringPosB;
				FileIO.eStringNo = fData.eStringNo;
				FileIO.eNybbleSwap = fData.eNybbleSwap;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00008DD9 File Offset: 0x00006FD9
		public static uint GetFilePosition()
		{
			if (FileIO.useRSDKFile)
			{
				return FileIO.readPos - FileIO.readSize + FileIO.bufferPosition - FileIO.virtualFileOffset;
			}
			return FileIO.readPos - FileIO.readSize + FileIO.bufferPosition;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00008E0C File Offset: 0x0000700C
		public static void SetFilePosition(uint newFilePos)
		{
			if (FileIO.useRSDKFile)
			{
				FileIO.readPos = newFilePos + FileIO.virtualFileOffset;
				FileIO.eStringNo = (byte)((FileIO.vFileSize & 508U) >> 2);
				FileIO.eStringPosB = 1 + FileIO.eStringNo % 9;
				FileIO.eStringPosA = 1 + FileIO.eStringNo % FileIO.eStringPosB;
				FileIO.eNybbleSwap = false;
				while (newFilePos > 0U)
				{
					FileIO.eStringPosA += 1;
					FileIO.eStringPosB += 1;
					if (FileIO.eStringPosA > 19 && FileIO.eStringPosB > 11)
					{
						FileIO.eStringNo += 1;
						FileIO.eStringNo &= 127;
						if (!FileIO.eNybbleSwap)
						{
							FileIO.eNybbleSwap = true;
							FileIO.eStringPosA = 3 + FileIO.eStringNo % 15;
							FileIO.eStringPosB = 1 + FileIO.eStringNo % 7;
						}
						else
						{
							FileIO.eNybbleSwap = false;
							FileIO.eStringPosA = 6 + FileIO.eStringNo % 12;
							FileIO.eStringPosB = 4 + FileIO.eStringNo % 5;
						}
					}
					else
					{
						if (FileIO.eStringPosA > 19)
						{
							FileIO.eStringPosA = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
						if (FileIO.eStringPosB > 11)
						{
							FileIO.eStringPosB = 1;
							if (FileIO.eNybbleSwap)
							{
								FileIO.eNybbleSwap = false;
							}
							else
							{
								FileIO.eNybbleSwap = true;
							}
						}
					}
					newFilePos -= 1U;
				}
			}
			else
			{
				FileIO.readPos = newFilePos;
			}
			FileIO.fileReader.Seek((long)((ulong)FileIO.readPos), 0);
			FileIO.FillFileBuffer();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00008F80 File Offset: 0x00007180
		public static bool ReachedEndOfFile()
		{
			if (!FileIO.useRSDKFile)
			{
				return FileIO.readPos - FileIO.readSize + FileIO.bufferPosition >= FileIO.fileSize;
			}
			return FileIO.readPos - FileIO.readSize + FileIO.bufferPosition - FileIO.virtualFileOffset >= FileIO.vFileSize;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00008FD4 File Offset: 0x000071D4
		public static byte ReadSaveRAMData()
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			try
			{
				BinaryReader binaryReader = new BinaryReader(new IsolatedStorageFileStream("SGame.bin", 3, userStoreForApplication));
				for (int i = 0; i < FileIO.saveRAM.Length; i++)
				{
					FileIO.saveRAM[i] = binaryReader.ReadInt32();
				}
				binaryReader.Close();
			}
			catch
			{
			}
			return 1;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00009034 File Offset: 0x00007234
		public static byte WriteSaveRAMData()
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			try
			{
				BinaryWriter binaryWriter = new BinaryWriter(new IsolatedStorageFileStream("SGame.bin", 4, userStoreForApplication));
				for (int i = 0; i < FileIO.saveRAM.Length; i++)
				{
					binaryWriter.Write(FileIO.saveRAM[i]);
				}
				binaryWriter.Close();
			}
			catch
			{
			}
			return 1;
		}

		// Token: 0x04000043 RID: 67
		public const int PRESENTATION_STAGE = 0;

		// Token: 0x04000044 RID: 68
		public const int ZONE_STAGE = 1;

		// Token: 0x04000045 RID: 69
		public const int BONUS_STAGE = 2;

		// Token: 0x04000046 RID: 70
		public const int SPECIAL_STAGE = 3;

		// Token: 0x04000047 RID: 71
		public static byte[] fileBuffer = new byte[8192];

		// Token: 0x04000048 RID: 72
		public static uint bufferPosition = 0U;

		// Token: 0x04000049 RID: 73
		public static uint fileSize = 0U;

		// Token: 0x0400004A RID: 74
		public static uint readSize = 0U;

		// Token: 0x0400004B RID: 75
		public static uint readPos = 0U;

		// Token: 0x0400004C RID: 76
		public static bool useRSDKFile = false;

		// Token: 0x0400004D RID: 77
		public static bool useByteCode = false;

		// Token: 0x0400004E RID: 78
		public static uint vFileSize = 0U;

		// Token: 0x0400004F RID: 79
		public static uint virtualFileOffset = 0U;

		// Token: 0x04000050 RID: 80
		public static int[] saveRAM = new int[8192];

		// Token: 0x04000051 RID: 81
		public static char[] encryptionStringA = "4RaS9D7KaEbxcp2o5r6t".ToCharArray();

		// Token: 0x04000052 RID: 82
		public static char[] encryptionStringB = "3tRaUxLmEaSn".ToCharArray();

		// Token: 0x04000053 RID: 83
		public static byte eStringPosA;

		// Token: 0x04000054 RID: 84
		public static byte eStringPosB;

		// Token: 0x04000055 RID: 85
		public static byte eStringNo;

		// Token: 0x04000056 RID: 86
		public static bool eNybbleSwap;

		// Token: 0x04000057 RID: 87
		public static char[] currentStageFolder = new char[8];

		// Token: 0x04000058 RID: 88
		public static StageList[] pStageList = new StageList[64];

		// Token: 0x04000059 RID: 89
		public static StageList[] zStageList = new StageList[128];

		// Token: 0x0400005A RID: 90
		public static StageList[] bStageList = new StageList[64];

		// Token: 0x0400005B RID: 91
		public static StageList[] sStageList = new StageList[64];

		// Token: 0x0400005C RID: 92
		public static byte activeStageList;

		// Token: 0x0400005D RID: 93
		public static byte noPresentationStages;

		// Token: 0x0400005E RID: 94
		public static byte noZoneStages;

		// Token: 0x0400005F RID: 95
		public static byte noBonusStages;

		// Token: 0x04000060 RID: 96
		public static byte noSpecialStages;

		// Token: 0x04000061 RID: 97
		public static int actNumber;

		// Token: 0x04000062 RID: 98
		private static Stream fileReader;
	}
}
