using System;

namespace Retro_Engine
{
	// Token: 0x02000013 RID: 19
	public static class GifLoader
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000DA98 File Offset: 0x0000BC98
		public static void InitGifDecoder()
		{
			byte b = FileIO.ReadByte();
			int num = (int)b;
			int num2 = num;
			GifLoader.gifDecoder.fileState = 0;
			GifLoader.gifDecoder.position = 0;
			GifLoader.gifDecoder.bufferSize = 0;
			GifLoader.gifDecoder.buffer[0] = 0;
			GifLoader.gifDecoder.depth = num2;
			GifLoader.gifDecoder.clearCode = 1 << num2;
			GifLoader.gifDecoder.eofCode = GifLoader.gifDecoder.clearCode + 1;
			GifLoader.gifDecoder.runningCode = GifLoader.gifDecoder.eofCode + 1;
			GifLoader.gifDecoder.runningBits = num2 + 1;
			GifLoader.gifDecoder.maxCodePlusOne = 1 << GifLoader.gifDecoder.runningBits;
			GifLoader.gifDecoder.stackPtr = 0;
			GifLoader.gifDecoder.prevCode = 4098;
			GifLoader.gifDecoder.shiftState = 0;
			GifLoader.gifDecoder.shiftData = 0U;
			for (int i = 0; i <= 4095; i++)
			{
				GifLoader.gifDecoder.prefix[i] = 4098U;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000DBC0 File Offset: 0x0000BDC0
		public static void ReadGifPictureData(int width, int height, bool interlaced, ref byte[] gfxData, int offset)
		{
			int[] array = new int[]
			{
				0,
				4,
				2,
				1
			};
			int[] array2 = new int[]
			{
				8,
				8,
				4,
				2
			};
			GifLoader.InitGifDecoder();
			if (interlaced)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = array[i]; j < height; j += array2[i])
					{
						GifLoader.ReadGifLine(ref gfxData, width, j * width + offset);
					}
				}
				return;
			}
			for (int j = 0; j < height; j++)
			{
				GifLoader.ReadGifLine(ref gfxData, width, j * width + offset);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		public static void ReadGifLine(ref byte[] line, int length, int offset)
		{
			int i = 0;
			int num = GifLoader.gifDecoder.stackPtr;
			int eofCode = GifLoader.gifDecoder.eofCode;
			int clearCode = GifLoader.gifDecoder.clearCode;
			int num2 = GifLoader.gifDecoder.prevCode;
			if (num != 0)
			{
				while (num != 0)
				{
					if (i >= length)
					{
						break;
					}
					line[offset++] = GifLoader.gifDecoder.stack[--num];
					i++;
				}
			}
			while (i < length)
			{
				int num3 = GifLoader.ReadGifCode();
				if (num3 == eofCode)
				{
					if (i != length - 1 | GifLoader.gifDecoder.pixelCount != 0U)
					{
						return;
					}
					i++;
				}
				else if (num3 == clearCode)
				{
					for (int j = 0; j <= 4095; j++)
					{
						GifLoader.gifDecoder.prefix[j] = 4098U;
					}
					GifLoader.gifDecoder.runningCode = GifLoader.gifDecoder.eofCode + 1;
					GifLoader.gifDecoder.runningBits = GifLoader.gifDecoder.depth + 1;
					GifLoader.gifDecoder.maxCodePlusOne = 1 << GifLoader.gifDecoder.runningBits;
					num2 = (GifLoader.gifDecoder.prevCode = 4098);
				}
				else
				{
					if (num3 < clearCode)
					{
						line[offset] = (byte)num3;
						offset++;
						i++;
					}
					else
					{
						if (num3 < 0 | num3 > 4095)
						{
							return;
						}
						int num4;
						if (GifLoader.gifDecoder.prefix[num3] == 4098U)
						{
							if (num3 != GifLoader.gifDecoder.runningCode - 2)
							{
								return;
							}
							num4 = num2;
							GifLoader.gifDecoder.suffix[GifLoader.gifDecoder.runningCode - 2] = (GifLoader.gifDecoder.stack[num++] = GifLoader.TracePrefix(ref GifLoader.gifDecoder.prefix, num2, clearCode));
						}
						else
						{
							num4 = num3;
						}
						int j = 0;
						while (j++ <= 4095 && num4 > clearCode && num4 <= 4095)
						{
							GifLoader.gifDecoder.stack[num++] = GifLoader.gifDecoder.suffix[num4];
							num4 = (int)GifLoader.gifDecoder.prefix[num4];
						}
						if (j >= 4095 | num4 > 4095)
						{
							return;
						}
						GifLoader.gifDecoder.stack[num++] = (byte)num4;
						while (num != 0 && i < length)
						{
							line[offset++] = GifLoader.gifDecoder.stack[--num];
							i++;
						}
					}
					if (num2 != 4098)
					{
						if (GifLoader.gifDecoder.runningCode < 2 | GifLoader.gifDecoder.runningCode > 4097)
						{
							return;
						}
						GifLoader.gifDecoder.prefix[GifLoader.gifDecoder.runningCode - 2] = (uint)num2;
						if (num3 == GifLoader.gifDecoder.runningCode - 2)
						{
							GifLoader.gifDecoder.suffix[GifLoader.gifDecoder.runningCode - 2] = GifLoader.TracePrefix(ref GifLoader.gifDecoder.prefix, num2, clearCode);
						}
						else
						{
							GifLoader.gifDecoder.suffix[GifLoader.gifDecoder.runningCode - 2] = GifLoader.TracePrefix(ref GifLoader.gifDecoder.prefix, num3, clearCode);
						}
					}
					num2 = num3;
				}
			}
			GifLoader.gifDecoder.prevCode = num2;
			GifLoader.gifDecoder.stackPtr = num;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000DF74 File Offset: 0x0000C174
		private static int ReadGifCode()
		{
			while (GifLoader.gifDecoder.shiftState < GifLoader.gifDecoder.runningBits)
			{
				byte b = GifLoader.ReadGifByte();
				GifLoader.gifDecoder.shiftData |= (uint)((uint)b << GifLoader.gifDecoder.shiftState);
				GifLoader.gifDecoder.shiftState += 8;
			}
			int result = (int)((ulong)GifLoader.gifDecoder.shiftData & (ulong)((long)GifLoader.codeMasks[GifLoader.gifDecoder.runningBits]));
			GifLoader.gifDecoder.shiftData >>= GifLoader.gifDecoder.runningBits;
			GifLoader.gifDecoder.shiftState -= GifLoader.gifDecoder.runningBits;
			if (++GifLoader.gifDecoder.runningCode > GifLoader.gifDecoder.maxCodePlusOne && GifLoader.gifDecoder.runningBits < 12)
			{
				GifLoader.gifDecoder.maxCodePlusOne <<= 1;
				GifLoader.gifDecoder.runningBits++;
			}
			return result;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000E080 File Offset: 0x0000C280
		private static byte ReadGifByte()
		{
			char c = '\0';
			if (GifLoader.gifDecoder.fileState == 1)
			{
				return (byte)c;
			}
			byte b;
			if (GifLoader.gifDecoder.position == GifLoader.gifDecoder.bufferSize)
			{
				b = FileIO.ReadByte();
				GifLoader.gifDecoder.bufferSize = (int)b;
				if (GifLoader.gifDecoder.bufferSize == 0)
				{
					GifLoader.gifDecoder.fileState = 1;
					return (byte)c;
				}
				FileIO.ReadByteArray(ref GifLoader.gifDecoder.buffer, GifLoader.gifDecoder.bufferSize);
				b = GifLoader.gifDecoder.buffer[0];
				GifLoader.gifDecoder.position = 1;
			}
			else
			{
				b = GifLoader.gifDecoder.buffer[GifLoader.gifDecoder.position++];
			}
			return b;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000E138 File Offset: 0x0000C338
		private static byte TracePrefix(ref uint[] prefix, int code, int clearCode)
		{
			int num = 0;
			while (code > clearCode && num++ <= 4095)
			{
				code = (int)prefix[code];
			}
			return (byte)code;
		}

		// Token: 0x04000123 RID: 291
		public const int LOADING_IMAGE = 0;

		// Token: 0x04000124 RID: 292
		public const int LOAD_COMPLETE = 1;

		// Token: 0x04000125 RID: 293
		public const int LZ_MAX_CODE = 4095;

		// Token: 0x04000126 RID: 294
		public const int LZ_BITS = 12;

		// Token: 0x04000127 RID: 295
		public const int FLUSH_OUTPUT = 4096;

		// Token: 0x04000128 RID: 296
		public const int FIRST_CODE = 4097;

		// Token: 0x04000129 RID: 297
		public const int NO_SUCH_CODE = 4098;

		// Token: 0x0400012A RID: 298
		public const int HT_SIZE = 8192;

		// Token: 0x0400012B RID: 299
		public const int HT_KEY_MASK = 8191;

		// Token: 0x0400012C RID: 300
		private static GifDecoder gifDecoder = new GifDecoder();

		// Token: 0x0400012D RID: 301
		private static int[] codeMasks = new int[]
		{
			0,
			1,
			3,
			7,
			15,
			31,
			63,
			127,
			255,
			511,
			1023,
			2047,
			4095
		};
	}
}
