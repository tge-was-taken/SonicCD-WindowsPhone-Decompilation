using System;

namespace Retro_Engine
{
	// Token: 0x0200001A RID: 26
	public static class AnimationSystem
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00018350 File Offset: 0x00016550
		static AnimationSystem()
		{
			for (int i = 0; i < AnimationSystem.animationFrames.Length; i++)
			{
				AnimationSystem.animationFrames[i] = new SpriteFrame();
			}
			for (int i = 0; i < AnimationSystem.animationList.Length; i++)
			{
				AnimationSystem.animationList[i] = new SpriteAnimation();
			}
			for (int i = 0; i < AnimationSystem.animationFile.Length; i++)
			{
				AnimationSystem.animationFile[i] = new AnimationFileList();
			}
			for (int i = 0; i < AnimationSystem.collisionBoxList.Length; i++)
			{
				AnimationSystem.collisionBoxList[i] = new CollisionBox();
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00018428 File Offset: 0x00016628
		public static void LoadAnimationFile(char[] filePath)
		{
			char[] array = new char[32];
			byte[] array2 = new byte[24];
			FileData fileData = new FileData();
			if (FileIO.LoadFile(filePath, fileData))
			{
				byte b = FileIO.ReadByte();
				for (int i = 0; i < 24; i++)
				{
					array2[i] = 0;
				}
				byte b2;
				for (int i = 0; i < (int)b; i++)
				{
					b2 = FileIO.ReadByte();
					int j = 0;
					if (b2 > 0)
					{
						while ((int)b2 > j)
						{
							array[j] = (char)FileIO.ReadByte();
							j++;
						}
						array[j] = '\0';
						FileIO.GetFileInfo(fileData);
						FileIO.CloseFile();
						array2[i] = GraphicsSystem.AddGraphicsFile(array);
						FileIO.SetFileInfo(fileData);
					}
				}
				b2 = FileIO.ReadByte();
				AnimationSystem.animationFile[AnimationSystem.animationFileNo].numAnimations = (int)b2;
				AnimationSystem.animationFile[AnimationSystem.animationFileNo].aniListOffset = AnimationSystem.animationListNo;
				for (int i = 0; i < AnimationSystem.animationFile[AnimationSystem.animationFileNo].numAnimations; i++)
				{
					b = FileIO.ReadByte();
					int j;
					for (j = 0; j < (int)b; j++)
					{
						AnimationSystem.animationList[AnimationSystem.animationListNo].name[j] = (char)FileIO.ReadByte();
					}
					AnimationSystem.animationList[AnimationSystem.animationListNo].name[j] = '\0';
					b2 = FileIO.ReadByte();
					AnimationSystem.animationList[AnimationSystem.animationListNo].numFrames = b2;
					b2 = FileIO.ReadByte();
					AnimationSystem.animationList[AnimationSystem.animationListNo].animationSpeed = b2;
					b2 = FileIO.ReadByte();
					AnimationSystem.animationList[AnimationSystem.animationListNo].loopPosition = b2;
					b2 = FileIO.ReadByte();
					AnimationSystem.animationList[AnimationSystem.animationListNo].rotationFlag = b2;
					AnimationSystem.animationList[AnimationSystem.animationListNo].frameListOffset = AnimationSystem.animationFramesNo;
					for (j = 0; j < (int)AnimationSystem.animationList[AnimationSystem.animationListNo].numFrames; j++)
					{
						b2 = FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].surfaceNum = array2[(int)b2];
						b2 = FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].collisionBox = b2;
						b2 = FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].left = (int)b2;
						b2 = FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].top = (int)b2;
						b2 = FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].xSize = (int)b2;
						b2 = FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].ySize = (int)b2;
						sbyte b3 = (sbyte)FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].xPivot = (int)b3;
						b3 = (sbyte)FileIO.ReadByte();
						AnimationSystem.animationFrames[AnimationSystem.animationFramesNo].yPivot = (int)b3;
						AnimationSystem.animationFramesNo++;
					}
					if (AnimationSystem.animationList[AnimationSystem.animationListNo].rotationFlag == 3)
					{
						SpriteAnimation spriteAnimation = AnimationSystem.animationList[AnimationSystem.animationListNo];
						spriteAnimation.numFrames = (byte)(spriteAnimation.numFrames >> 1);
					}
					AnimationSystem.animationListNo++;
				}
				b = FileIO.ReadByte();
				AnimationSystem.animationFile[AnimationSystem.animationFileNo].cbListOffset = AnimationSystem.collisionBoxNo;
				for (int i = 0; i < (int)b; i++)
				{
					for (int j = 0; j < 8; j++)
					{
						AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].left[j] = (sbyte)FileIO.ReadByte();
						AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].top[j] = (sbyte)FileIO.ReadByte();
						AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].right[j] = (sbyte)FileIO.ReadByte();
						AnimationSystem.collisionBoxList[AnimationSystem.collisionBoxNo].bottom[j] = (sbyte)FileIO.ReadByte();
					}
					AnimationSystem.collisionBoxNo++;
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000187C4 File Offset: 0x000169C4
		public static AnimationFileList AddAnimationFile(char[] fileName)
		{
			int i = 0;
			char[] array = "Data/Animations/".ToCharArray();
			char[] filePath = new char[64];
			FileIO.StrCopy(ref filePath, ref array);
			FileIO.StrAdd(ref filePath, ref fileName);
			while (i < 256)
			{
				if (FileIO.StringLength(ref AnimationSystem.animationFile[i].fileName) <= 0)
				{
					FileIO.StrCopy(ref AnimationSystem.animationFile[i].fileName, ref fileName);
					AnimationSystem.LoadAnimationFile(filePath);
					AnimationSystem.animationFileNo++;
					return AnimationSystem.animationFile[i];
				}
				if (FileIO.StringComp(ref AnimationSystem.animationFile[i].fileName, ref fileName))
				{
					return AnimationSystem.animationFile[i];
				}
				i++;
			}
			return null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00018865 File Offset: 0x00016A65
		public static AnimationFileList GetDefaultAnimationRef()
		{
			return AnimationSystem.animationFile[0];
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00018870 File Offset: 0x00016A70
		public static void ClearAnimationData()
		{
			char[] array = "".ToCharArray();
			for (int i = 0; i < 4096; i++)
			{
				AnimationSystem.animationFrames[i].left = 0;
				AnimationSystem.animationFrames[i].top = 0;
				AnimationSystem.animationFrames[i].xSize = 0;
				AnimationSystem.animationFrames[i].ySize = 0;
				AnimationSystem.animationFrames[i].xPivot = 0;
				AnimationSystem.animationFrames[i].yPivot = 0;
				AnimationSystem.animationFrames[i].surfaceNum = 0;
				AnimationSystem.animationFrames[i].collisionBox = 0;
			}
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					AnimationSystem.collisionBoxList[i].left[j] = 0;
					AnimationSystem.collisionBoxList[i].top[j] = 0;
					AnimationSystem.collisionBoxList[i].right[j] = 0;
					AnimationSystem.collisionBoxList[i].bottom[j] = 0;
				}
			}
			for (int i = 0; i < 256; i++)
			{
				FileIO.StrCopy(ref AnimationSystem.animationFile[i].fileName, ref array);
			}
			AnimationSystem.animationFramesNo = 0;
			AnimationSystem.animationListNo = 0;
			AnimationSystem.animationFileNo = 0;
			AnimationSystem.collisionBoxNo = 0;
			AnimationSystem.animationFile[0].numAnimations = 0;
			AnimationSystem.animationList[0].frameListOffset = AnimationSystem.animationFramesNo;
			AnimationSystem.animationFile[0].aniListOffset = AnimationSystem.animationListNo;
			AnimationSystem.animationFile[0].cbListOffset = AnimationSystem.collisionBoxNo;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000189D0 File Offset: 0x00016BD0
		public static void ProcessObjectAnimation(SpriteAnimation animationRef, ObjectEntity currentObject)
		{
			if (currentObject.animationSpeed > 0)
			{
				if (currentObject.animationSpeed > 240)
				{
					currentObject.animationSpeed = 240;
				}
				currentObject.animationTimer += currentObject.animationSpeed;
			}
			else
			{
				currentObject.animationTimer += (int)animationRef.animationSpeed;
			}
			if (currentObject.animation != currentObject.prevAnimation)
			{
				currentObject.prevAnimation = currentObject.animation;
				currentObject.frame = 0;
				currentObject.animationTimer = 0;
				currentObject.animationSpeed = 0;
			}
			if (currentObject.animationTimer > 239)
			{
				currentObject.animationTimer -= 240;
				currentObject.frame += 1;
			}
			if (currentObject.frame >= animationRef.numFrames)
			{
				currentObject.frame = animationRef.loopPosition;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00018AA0 File Offset: 0x00016CA0
		public static void DrawObjectAnimation(SpriteAnimation animationRef, ObjectEntity currentObject, int xPos, int yPos)
		{
			switch (animationRef.rotationFlag)
			{
			case 0:
			{
				SpriteFrame spriteFrame = AnimationSystem.animationFrames[animationRef.frameListOffset + (int)currentObject.frame];
				switch (currentObject.direction)
				{
				case 0:
					GraphicsSystem.DrawSpriteFlipped(xPos + spriteFrame.xPivot, yPos + spriteFrame.yPivot, spriteFrame.xSize, spriteFrame.ySize, spriteFrame.left, spriteFrame.top, (int)currentObject.direction, (int)spriteFrame.surfaceNum);
					return;
				case 1:
					GraphicsSystem.DrawSpriteFlipped(xPos - spriteFrame.xSize - spriteFrame.xPivot, yPos + spriteFrame.yPivot, spriteFrame.xSize, spriteFrame.ySize, spriteFrame.left, spriteFrame.top, (int)currentObject.direction, (int)spriteFrame.surfaceNum);
					return;
				case 2:
					GraphicsSystem.DrawSpriteFlipped(xPos + spriteFrame.xPivot, yPos - spriteFrame.ySize - spriteFrame.yPivot, spriteFrame.xSize, spriteFrame.ySize, spriteFrame.left, spriteFrame.top, (int)currentObject.direction, (int)spriteFrame.surfaceNum);
					return;
				case 3:
					GraphicsSystem.DrawSpriteFlipped(xPos - spriteFrame.xSize - spriteFrame.xPivot, yPos - spriteFrame.ySize - spriteFrame.yPivot, spriteFrame.xSize, spriteFrame.ySize, spriteFrame.left, spriteFrame.top, (int)currentObject.direction, (int)spriteFrame.surfaceNum);
					return;
				default:
					return;
				}
				break;
			}
			case 1:
			{
				SpriteFrame spriteFrame = AnimationSystem.animationFrames[animationRef.frameListOffset + (int)currentObject.frame];
				GraphicsSystem.DrawRotatedSprite(currentObject.direction, xPos, yPos, -spriteFrame.xPivot, -spriteFrame.yPivot, spriteFrame.left, spriteFrame.top, spriteFrame.xSize, spriteFrame.ySize, currentObject.rotation, (int)spriteFrame.surfaceNum);
				return;
			}
			case 2:
			{
				SpriteFrame spriteFrame = AnimationSystem.animationFrames[animationRef.frameListOffset + (int)currentObject.frame];
				int rotAngle;
				if (currentObject.rotation < 256)
				{
					rotAngle = currentObject.rotation + 20 >> 6 << 6;
				}
				else
				{
					rotAngle = 512 - (532 - currentObject.rotation >> 6 << 6);
				}
				GraphicsSystem.DrawRotatedSprite(currentObject.direction, xPos, yPos, -spriteFrame.xPivot, -spriteFrame.yPivot, spriteFrame.left, spriteFrame.top, spriteFrame.xSize, spriteFrame.ySize, rotAngle, (int)spriteFrame.surfaceNum);
				return;
			}
			case 3:
			{
				int rotAngle;
				if (currentObject.rotation < 256)
				{
					rotAngle = currentObject.rotation + 20 >> 6;
				}
				else
				{
					rotAngle = 8 - (532 - currentObject.rotation >> 6);
				}
				int num = (int)currentObject.frame;
				switch (rotAngle)
				{
				case 0:
				case 8:
					rotAngle = 0;
					break;
				case 1:
					num += (int)animationRef.numFrames;
					if (currentObject.direction == 0)
					{
						rotAngle = 128;
					}
					else
					{
						rotAngle = 0;
					}
					break;
				case 2:
					rotAngle = 128;
					break;
				case 3:
					num += (int)animationRef.numFrames;
					if (currentObject.direction == 0)
					{
						rotAngle = 256;
					}
					else
					{
						rotAngle = 128;
					}
					break;
				case 4:
					rotAngle = 256;
					break;
				case 5:
					num += (int)animationRef.numFrames;
					if (currentObject.direction == 0)
					{
						rotAngle = 384;
					}
					else
					{
						rotAngle = 256;
					}
					break;
				case 6:
					rotAngle = 384;
					break;
				case 7:
					num += (int)animationRef.numFrames;
					if (currentObject.direction == 0)
					{
						rotAngle = 0;
					}
					else
					{
						rotAngle = 384;
					}
					break;
				}
				SpriteFrame spriteFrame = AnimationSystem.animationFrames[animationRef.frameListOffset + num];
				GraphicsSystem.DrawRotatedSprite(currentObject.direction, xPos, yPos, -spriteFrame.xPivot, -spriteFrame.yPivot, spriteFrame.left, spriteFrame.top, spriteFrame.xSize, spriteFrame.ySize, rotAngle, (int)spriteFrame.surfaceNum);
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x04000196 RID: 406
		public static SpriteFrame[] animationFrames = new SpriteFrame[4096];

		// Token: 0x04000197 RID: 407
		public static int animationFramesNo = 0;

		// Token: 0x04000198 RID: 408
		public static SpriteAnimation[] animationList = new SpriteAnimation[1024];

		// Token: 0x04000199 RID: 409
		public static int animationListNo = 0;

		// Token: 0x0400019A RID: 410
		public static AnimationFileList[] animationFile = new AnimationFileList[256];

		// Token: 0x0400019B RID: 411
		public static int animationFileNo = 0;

		// Token: 0x0400019C RID: 412
		public static CollisionBox[] collisionBoxList = new CollisionBox[32];

		// Token: 0x0400019D RID: 413
		public static int collisionBoxNo = 0;
	}
}
