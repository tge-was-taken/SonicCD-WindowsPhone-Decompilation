using System;

namespace Retro_Engine
{
	// Token: 0x02000015 RID: 21
	public static class StageSystem
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000E1B8 File Offset: 0x0000C3B8
		static StageSystem()
		{
			for (int i = 0; i < StageSystem.stageLayouts.Length; i++)
			{
				StageSystem.stageLayouts[i] = new LayoutMap();
			}
			for (int i = 0; i < StageSystem.tileCollisions.Length; i++)
			{
				StageSystem.tileCollisions[i] = new CollisionMask16x16();
			}
			for (int i = 0; i < StageSystem.gameMenu.Length; i++)
			{
				StageSystem.gameMenu[i] = new TextMenu();
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000E334 File Offset: 0x0000C534
		public static void ProcessStage()
		{
			switch (StageSystem.stageMode)
			{
			case 0:
				AudioPlayback.StopMusic();
				GraphicsSystem.fadeMode = 0;
				GraphicsSystem.paletteMode = 0;
				GraphicsSystem.SetActivePalette(0, 0, 256);
				StageSystem.cameraEnabled = 1;
				StageSystem.cameraTarget = -1;
				StageSystem.cameraAdjustY = 0;
				StageSystem.xScrollOffset = 0;
				StageSystem.yScrollOffset = 0;
				StageSystem.yScrollA = 0;
				StageSystem.yScrollB = 240;
				StageSystem.xScrollA = 0;
				StageSystem.xScrollB = 320;
				StageSystem.xScrollMove = 0;
				StageSystem.yScrollMove = 0;
				StageSystem.screenShakeX = 0;
				StageSystem.screenShakeY = 0;
				Scene3D.numVertices = 0;
				Scene3D.numFaces = 0;
				for (int i = 0; i < 2; i++)
				{
					PlayerSystem.playerList[i].xPos = 0;
					PlayerSystem.playerList[i].yPos = 0;
					PlayerSystem.playerList[i].xVelocity = 0;
					PlayerSystem.playerList[i].yVelocity = 0;
					PlayerSystem.playerList[i].angle = 0;
					PlayerSystem.playerList[i].visible = 1;
					PlayerSystem.playerList[i].collisionPlane = 0;
					PlayerSystem.playerList[i].collisionMode = 0;
					PlayerSystem.playerList[i].gravity = 1;
					PlayerSystem.playerList[i].speed = 0;
					PlayerSystem.playerList[i].tileCollisions = 1;
					PlayerSystem.playerList[i].objectInteraction = 1;
					PlayerSystem.playerList[i].value[0] = 0;
					PlayerSystem.playerList[i].value[1] = 0;
					PlayerSystem.playerList[i].value[2] = 0;
					PlayerSystem.playerList[i].value[3] = 0;
					PlayerSystem.playerList[i].value[4] = 0;
					PlayerSystem.playerList[i].value[5] = 0;
					PlayerSystem.playerList[i].value[6] = 0;
					PlayerSystem.playerList[i].value[7] = 0;
				}
				StageSystem.pauseEnabled = 0;
				StageSystem.timeEnabled = 0;
				StageSystem.milliSeconds = 0;
				StageSystem.seconds = 0;
				StageSystem.minutes = 0;
				GlobalAppDefinitions.frameCounter = 0;
				StageSystem.ResetBackgroundSettings();
				StageSystem.LoadStageFiles();
				GraphicsSystem.texBufferMode = 0;
				for (int i = 0; i < 9; i++)
				{
					if (StageSystem.stageLayouts[i].type == 4)
					{
						GraphicsSystem.texBufferMode = 1;
					}
				}
				for (int i = 0; i < (int)StageSystem.hParallax.numEntries; i++)
				{
					if (StageSystem.hParallax.deformationEnabled[i] == 1)
					{
						GraphicsSystem.texBufferMode = 1;
					}
				}
				if (GraphicsSystem.tileGfx[204802] > 0)
				{
					GraphicsSystem.texBufferMode = 0;
				}
				if (GraphicsSystem.texBufferMode == 0)
				{
					for (int i = 0; i < 4096; i += 4)
					{
						GraphicsSystem.tileUVArray[i] = (float)((i >> 2 & 31) * 16) * 0.0009765625f;
						GraphicsSystem.tileUVArray[i + 1] = (float)((i >> 2 >> 5) * 16) * 0.0009765625f;
						GraphicsSystem.tileUVArray[i + 2] = GraphicsSystem.tileUVArray[i] + 0.015625f;
						GraphicsSystem.tileUVArray[i + 3] = GraphicsSystem.tileUVArray[i + 1] + 0.015625f;
					}
				}
				else
				{
					for (int i = 0; i < 4096; i += 4)
					{
						GraphicsSystem.tileUVArray[i] = (float)((i >> 2) % 28 * 18 + 1) * 0.0009765625f;
						GraphicsSystem.tileUVArray[i + 1] = (float)((i >> 2) / 28 * 18 + 1) * 0.0009765625f;
						GraphicsSystem.tileUVArray[i + 2] = GraphicsSystem.tileUVArray[i] + 0.015625f;
						GraphicsSystem.tileUVArray[i + 3] = GraphicsSystem.tileUVArray[i + 1] + 0.015625f;
					}
					GraphicsSystem.tileUVArray[4092] = 0.47558594f;
					GraphicsSystem.tileUVArray[4093] = 0.47558594f;
					GraphicsSystem.tileUVArray[4094] = 0.49121094f;
					GraphicsSystem.tileUVArray[4095] = 0.49121094f;
				}
				RenderDevice.UpdateHardwareTextures();
				StageSystem.stageMode = 1;
				GraphicsSystem.gfxIndexSize = 0;
				GraphicsSystem.gfxVertexSize = 0;
				GraphicsSystem.gfxIndexSizeOpaque = 0;
				GraphicsSystem.gfxVertexSizeOpaque = 0;
				StageSystem.stageMode = 1;
				return;
			case 1:
				if (GraphicsSystem.fadeMode > 0)
				{
					GraphicsSystem.fadeMode -= 1;
				}
				if (GraphicsSystem.paletteMode > 0)
				{
					GraphicsSystem.paletteMode = 0;
					GraphicsSystem.texPaletteNum = 0;
				}
				StageSystem.lastXSize = -1;
				StageSystem.lastYSize = -1;
				InputSystem.CheckKeyDown(StageSystem.gKeyDown, byte.MaxValue);
				InputSystem.CheckKeyPress(StageSystem.gKeyPress, byte.MaxValue);
				if (StageSystem.pauseEnabled == 1 && StageSystem.gKeyPress.start == 1)
				{
					StageSystem.stageMode = 2;
					AudioPlayback.PauseSound();
				}
				if (StageSystem.timeEnabled == 1)
				{
					GlobalAppDefinitions.frameCounter += 1;
					if (GlobalAppDefinitions.frameCounter == 60)
					{
						GlobalAppDefinitions.frameCounter = 0;
						StageSystem.seconds += 1;
						if (StageSystem.seconds > 59)
						{
							StageSystem.seconds = 0;
							StageSystem.minutes += 1;
							if (StageSystem.minutes > 59)
							{
								StageSystem.minutes = 0;
							}
						}
					}
					StageSystem.milliSeconds = (byte)(GlobalAppDefinitions.frameCounter * 100 / 60);
				}
				ObjectSystem.ProcessObjects();
				if (StageSystem.cameraTarget > -1)
				{
					if (StageSystem.cameraEnabled == 1)
					{
						switch (StageSystem.cameraStyle)
						{
						case 0:
							PlayerSystem.SetPlayerScreenPosition(PlayerSystem.playerList[(int)StageSystem.cameraTarget]);
							break;
						case 1:
							PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int)StageSystem.cameraTarget]);
							break;
						case 2:
							PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int)StageSystem.cameraTarget]);
							break;
						case 3:
							PlayerSystem.SetPlayerScreenPositionCDStyle(PlayerSystem.playerList[(int)StageSystem.cameraTarget]);
							break;
						case 4:
							PlayerSystem.SetPlayerHLockedScreenPosition(PlayerSystem.playerList[(int)StageSystem.cameraTarget]);
							break;
						}
					}
					else
					{
						PlayerSystem.SetPlayerLockedScreenPosition(PlayerSystem.playerList[(int)StageSystem.cameraTarget]);
					}
				}
				StageSystem.DrawStageGfx();
				if (GraphicsSystem.fadeMode > 0)
				{
					GraphicsSystem.DrawRectangle(0, 0, GlobalAppDefinitions.SCREEN_XSIZE, 240, (int)GraphicsSystem.fadeR, (int)GraphicsSystem.fadeG, (int)GraphicsSystem.fadeB, (int)GraphicsSystem.fadeA);
				}
				if (StageSystem.stageMode == 2)
				{
					GlobalAppDefinitions.gameMode = 8;
					return;
				}
				break;
			case 2:
				if (GraphicsSystem.fadeMode > 0)
				{
					GraphicsSystem.fadeMode -= 1;
				}
				if (GraphicsSystem.paletteMode > 0)
				{
					GraphicsSystem.paletteMode = 0;
					GraphicsSystem.texPaletteNum = 0;
				}
				StageSystem.lastXSize = -1;
				StageSystem.lastYSize = -1;
				InputSystem.CheckKeyDown(StageSystem.gKeyDown, byte.MaxValue);
				InputSystem.CheckKeyPress(StageSystem.gKeyPress, byte.MaxValue);
				GraphicsSystem.gfxIndexSize = 0;
				GraphicsSystem.gfxVertexSize = 0;
				GraphicsSystem.gfxIndexSizeOpaque = 0;
				GraphicsSystem.gfxVertexSizeOpaque = 0;
				ObjectSystem.ProcessPausedObjects();
				ObjectSystem.DrawObjectList(0);
				ObjectSystem.DrawObjectList(1);
				ObjectSystem.DrawObjectList(2);
				ObjectSystem.DrawObjectList(3);
				ObjectSystem.DrawObjectList(4);
				ObjectSystem.DrawObjectList(5);
				ObjectSystem.DrawObjectList(6);
				if (StageSystem.pauseEnabled == 1 && StageSystem.gKeyPress.start == 1)
				{
					StageSystem.stageMode = 1;
					AudioPlayback.ResumeSound();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000E96C File Offset: 0x0000CB6C
		public static void LoadStageFiles()
		{
			FileData fileData = new FileData();
			byte[] array = new byte[3];
			char[] array2 = new char[64];
			int num = 1;
			AudioPlayback.StopAllSFX();
			if (!FileIO.CheckCurrentStageFolder(StageSystem.stageListPosition))
			{
				AudioPlayback.ReleaseStageSFX();
				GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
				ObjectSystem.ClearScriptData();
				for (int i = 16; i > 0; i--)
				{
					GraphicsSystem.RemoveGraphicsFile("".ToCharArray(), i - 1);
				}
				if (FileIO.LoadStageFile("StageConfig.bin".ToCharArray(), StageSystem.stageListPosition, fileData))
				{
					array[0] = FileIO.ReadByte();
					FileIO.CloseFile();
				}
				if (array[0] == 1 && FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fileData))
				{
					array[0] = FileIO.ReadByte();
					for (int i = 0; i < (int)array[0]; i++)
					{
						array[1] = FileIO.ReadByte();
					}
					array[0] = FileIO.ReadByte();
					for (int i = 0; i < (int)array[0]; i++)
					{
						array[1] = FileIO.ReadByte();
					}
					array[0] = FileIO.ReadByte();
					for (int i = 0; i < (int)array[0]; i++)
					{
						array[1] = FileIO.ReadByte();
					}
					array[0] = FileIO.ReadByte();
					for (int i = 0; i < (int)array[0]; i++)
					{
						array[1] = FileIO.ReadByte();
						FileIO.ReadCharArray(ref array2, (int)array[1]);
						array2[(int)array[1]] = '\0';
						ObjectSystem.SetObjectTypeName(array2, num + i);
					}
					if (FileIO.useByteCode)
					{
						FileIO.GetFileInfo(fileData);
						FileIO.CloseFile();
						ObjectSystem.LoadByteCodeFile(4, num);
						num += (int)array[0];
						FileIO.SetFileInfo(fileData);
					}
					FileIO.CloseFile();
				}
				if (FileIO.LoadStageFile("StageConfig.bin".ToCharArray(), StageSystem.stageListPosition, fileData))
				{
					array[0] = FileIO.ReadByte();
					for (int i = 96; i < 128; i++)
					{
						FileIO.ReadByteArray(ref array, 3);
						GraphicsSystem.SetPaletteEntry((byte)i, array[0], array[1], array[2]);
					}
					array[0] = FileIO.ReadByte();
					for (int i = 0; i < (int)array[0]; i++)
					{
						array[1] = FileIO.ReadByte();
						FileIO.ReadCharArray(ref array2, (int)array[1]);
						array2[(int)array[1]] = '\0';
						ObjectSystem.SetObjectTypeName(array2, i + num);
					}
					if (FileIO.useByteCode)
					{
						for (int i = 0; i < (int)array[0]; i++)
						{
							array[1] = FileIO.ReadByte();
							FileIO.ReadCharArray(ref array2, (int)array[1]);
							array2[(int)array[1]] = '\0';
						}
						FileIO.GetFileInfo(fileData);
						FileIO.CloseFile();
						ObjectSystem.LoadByteCodeFile((int)FileIO.activeStageList, num);
						FileIO.SetFileInfo(fileData);
					}
					array[0] = FileIO.ReadByte();
					AudioPlayback.numStageSFX = (int)array[0];
					for (int i = 0; i < (int)array[0]; i++)
					{
						array[1] = FileIO.ReadByte();
						FileIO.ReadCharArray(ref array2, (int)array[1]);
						array2[(int)array[1]] = '\0';
						FileIO.GetFileInfo(fileData);
						FileIO.CloseFile();
						AudioPlayback.LoadSfx(array2, i + AudioPlayback.numGlobalSFX);
						FileIO.SetFileInfo(fileData);
					}
					FileIO.CloseFile();
				}
				GraphicsSystem.LoadStageGIFFile(StageSystem.stageListPosition);
				StageSystem.LoadStageCollisions();
				StageSystem.LoadStageBackground();
			}
			StageSystem.Load128x128Mappings();
			for (int i = 0; i < 16; i++)
			{
				AudioPlayback.SetMusicTrack("".ToCharArray(), i, 0, 0U);
			}
			for (int i = 0; i < 1184; i++)
			{
				ObjectSystem.objectEntityList[i].type = 0;
				ObjectSystem.objectEntityList[i].direction = 0;
				ObjectSystem.objectEntityList[i].animation = 0;
				ObjectSystem.objectEntityList[i].prevAnimation = 0;
				ObjectSystem.objectEntityList[i].animationSpeed = 0;
				ObjectSystem.objectEntityList[i].animationTimer = 0;
				ObjectSystem.objectEntityList[i].frame = 0;
				ObjectSystem.objectEntityList[i].priority = 0;
				ObjectSystem.objectEntityList[i].direction = 0;
				ObjectSystem.objectEntityList[i].rotation = 0;
				ObjectSystem.objectEntityList[i].state = 0;
				ObjectSystem.objectEntityList[i].propertyValue = 0;
				ObjectSystem.objectEntityList[i].xPos = 0;
				ObjectSystem.objectEntityList[i].yPos = 0;
				ObjectSystem.objectEntityList[i].drawOrder = 3;
				ObjectSystem.objectEntityList[i].scale = 512;
				ObjectSystem.objectEntityList[i].inkEffect = 0;
				ObjectSystem.objectEntityList[i].value[0] = 0;
				ObjectSystem.objectEntityList[i].value[1] = 0;
				ObjectSystem.objectEntityList[i].value[2] = 0;
				ObjectSystem.objectEntityList[i].value[3] = 0;
				ObjectSystem.objectEntityList[i].value[4] = 0;
				ObjectSystem.objectEntityList[i].value[5] = 0;
				ObjectSystem.objectEntityList[i].value[6] = 0;
				ObjectSystem.objectEntityList[i].value[7] = 0;
			}
			StageSystem.LoadActLayout();
			ObjectSystem.ProcessStartupScripts();
			StageSystem.xScrollA = (PlayerSystem.playerList[0].xPos >> 16) - 160;
			StageSystem.xScrollB = StageSystem.xScrollA + 320;
			StageSystem.yScrollA = (PlayerSystem.playerList[0].yPos >> 16) - 104;
			StageSystem.yScrollB = StageSystem.yScrollA + 240;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000EE10 File Offset: 0x0000D010
		public static void Load128x128Mappings()
		{
			FileData fData = new FileData();
			int i = 0;
			byte[] array = new byte[2];
			if (FileIO.LoadStageFile("128x128Tiles.bin".ToCharArray(), StageSystem.stageListPosition, fData))
			{
				while (i < 32768)
				{
					FileIO.ReadByteArray(ref array, 2);
					array[0] = (byte)((int)array[0] - (array[0] >> 6 << 6));
					StageSystem.tile128x128.visualPlane[i] = (byte)(array[0] >> 4);
					array[0] = (byte)((int)array[0] - (array[0] >> 4 << 4));
					StageSystem.tile128x128.direction[i] = (byte)(array[0] >> 2);
					array[0] = (byte)((int)array[0] - (array[0] >> 2 << 2));
					StageSystem.tile128x128.tile16x16[i] = (ushort)(((int)array[0] << 8) + (int)array[1]);
					StageSystem.tile128x128.gfxDataPos[i] = (int)StageSystem.tile128x128.tile16x16[i] << 2;
					array[0] = FileIO.ReadByte();
					StageSystem.tile128x128.collisionFlag[0, i] = (byte)(array[0] >> 4);
					StageSystem.tile128x128.collisionFlag[1, i] = (byte)((int)array[0] - (array[0] >> 4 << 4));
					i++;
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000EF24 File Offset: 0x0000D124
		public static void LoadStageCollisions()
		{
			FileData fData = new FileData();
			int num = 0;
			if (FileIO.LoadStageFile("CollisionMasks.bin".ToCharArray(), StageSystem.stageListPosition, fData))
			{
				for (int i = 0; i < 1024; i++)
				{
					for (int j = 0; j < 2; j++)
					{
						byte b = FileIO.ReadByte();
						int num2 = b >> 4;
						StageSystem.tileCollisions[j].flags[i] = (byte)(b & 15);
						b = FileIO.ReadByte();
						StageSystem.tileCollisions[j].angle[i] = (uint)b;
						b = FileIO.ReadByte();
						StageSystem.tileCollisions[j].angle[i] += (uint)((uint)b << 8);
						b = FileIO.ReadByte();
						StageSystem.tileCollisions[j].angle[i] += (uint)((uint)b << 16);
						b = FileIO.ReadByte();
						StageSystem.tileCollisions[j].angle[i] += (uint)((uint)b << 24);
						if (num2 == 0)
						{
							for (int k = 0; k < 16; k += 2)
							{
								b = FileIO.ReadByte();
								StageSystem.tileCollisions[j].floorMask[num + k] = (sbyte)(b >> 4);
								StageSystem.tileCollisions[j].floorMask[num + k + 1] = (sbyte)(b & 15);
							}
							b = FileIO.ReadByte();
							byte b2 = 1;
							for (int k = 0; k < 8; k++)
							{
								if ((b & b2) < 1)
								{
									StageSystem.tileCollisions[j].floorMask[num + k + 8] = 64;
									StageSystem.tileCollisions[j].roofMask[num + k + 8] = -64;
								}
								else
								{
									StageSystem.tileCollisions[j].roofMask[num + k + 8] = 15;
								}
								b2 = (byte)(b2 << 1);
							}
							b = FileIO.ReadByte();
							b2 = 1;
							for (int k = 0; k < 8; k++)
							{
								if ((b & b2) < 1)
								{
									StageSystem.tileCollisions[j].floorMask[num + k] = 64;
									StageSystem.tileCollisions[j].roofMask[num + k] = -64;
								}
								else
								{
									StageSystem.tileCollisions[j].roofMask[num + k] = 15;
								}
								b2 = (byte)(b2 << 1);
							}
							for (b = 0; b < 16; b += 1)
							{
								int k = 0;
								while (k > -1)
								{
									if (k == 16)
									{
										StageSystem.tileCollisions[j].leftWallMask[num + (int)b] = 64;
										k = -1;
									}
									else if (b >= (byte)StageSystem.tileCollisions[j].floorMask[num + k])
									{
										StageSystem.tileCollisions[j].leftWallMask[num + (int)b] = (sbyte)k;
										k = -1;
									}
									else
									{
										k++;
									}
								}
							}
							for (b = 0; b < 16; b += 1)
							{
								int k = 15;
								while (k < 16)
								{
									if (k == -1)
									{
										StageSystem.tileCollisions[j].rightWallMask[num + (int)b] = -64;
										k = 16;
									}
									else if (b >= (byte)StageSystem.tileCollisions[j].floorMask[num + k])
									{
										StageSystem.tileCollisions[j].rightWallMask[num + (int)b] = (sbyte)k;
										k = 16;
									}
									else
									{
										k--;
									}
								}
							}
						}
						else
						{
							for (int k = 0; k < 16; k += 2)
							{
								b = FileIO.ReadByte();
								StageSystem.tileCollisions[j].roofMask[num + k] = (sbyte)(b >> 4);
								StageSystem.tileCollisions[j].roofMask[num + k + 1] = (sbyte)(b & 15);
							}
							b = FileIO.ReadByte();
							byte b2 = 1;
							for (int k = 0; k < 8; k++)
							{
								if ((b & b2) < 1)
								{
									StageSystem.tileCollisions[j].floorMask[num + k + 8] = 64;
									StageSystem.tileCollisions[j].roofMask[num + k + 8] = -64;
								}
								else
								{
									StageSystem.tileCollisions[j].floorMask[num + k + 8] = 0;
								}
								b2 = (byte)(b2 << 1);
							}
							b = FileIO.ReadByte();
							b2 = 1;
							for (int k = 0; k < 8; k++)
							{
								if ((b & b2) < 1)
								{
									StageSystem.tileCollisions[j].floorMask[num + k] = 64;
									StageSystem.tileCollisions[j].roofMask[num + k] = -64;
								}
								else
								{
									StageSystem.tileCollisions[j].floorMask[num + k] = 0;
								}
								b2 = (byte)(b2 << 1);
							}
							for (b = 0; b < 16; b += 1)
							{
								int k = 0;
								while (k > -1)
								{
									if (k == 16)
									{
										StageSystem.tileCollisions[j].leftWallMask[num + (int)b] = 64;
										k = -1;
									}
									else if (b <= (byte)StageSystem.tileCollisions[j].roofMask[num + k])
									{
										StageSystem.tileCollisions[j].leftWallMask[num + (int)b] = (sbyte)k;
										k = -1;
									}
									else
									{
										k++;
									}
								}
							}
							for (b = 0; b < 16; b += 1)
							{
								int k = 15;
								while (k < 16)
								{
									if (k == -1)
									{
										StageSystem.tileCollisions[j].rightWallMask[num + (int)b] = -64;
										k = 16;
									}
									else if (b <= (byte)StageSystem.tileCollisions[j].roofMask[num + k])
									{
										StageSystem.tileCollisions[j].rightWallMask[num + (int)b] = (sbyte)k;
										k = 16;
									}
									else
									{
										k--;
									}
								}
							}
						}
					}
					num += 16;
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000F408 File Offset: 0x0000D608
		public static void LoadActLayout()
		{
			FileData fData = new FileData();
			if (FileIO.LoadActFile(".bin".ToCharArray(), StageSystem.stageListPosition, fData))
			{
				byte b = FileIO.ReadByte();
				int i = (int)b;
				StageSystem.titleCardWord2 = (char)b;
				int j;
				for (j = 0; j < i; j++)
				{
					StageSystem.titleCardText[j] = (char)FileIO.ReadByte();
					if (StageSystem.titleCardText[j] == '-')
					{
						StageSystem.titleCardWord2 = (char)(j + 1);
					}
				}
				StageSystem.titleCardText[j] = '\0';
				for (j = 0; j < 4; j++)
				{
					b = FileIO.ReadByte();
					StageSystem.activeTileLayers[j] = b;
				}
				b = FileIO.ReadByte();
				StageSystem.tLayerMidPoint = b;
				StageSystem.stageLayouts[0].xSize = FileIO.ReadByte();
				StageSystem.stageLayouts[0].ySize = FileIO.ReadByte();
				StageSystem.xBoundary1 = 0;
				StageSystem.newXBoundary1 = 0;
				StageSystem.yBoundary1 = 0;
				StageSystem.newYBoundary1 = 0;
				StageSystem.xBoundary2 = (int)StageSystem.stageLayouts[0].xSize << 7;
				StageSystem.yBoundary2 = (int)StageSystem.stageLayouts[0].ySize << 7;
				StageSystem.waterLevel = StageSystem.yBoundary2 + 128;
				StageSystem.newXBoundary2 = StageSystem.xBoundary2;
				StageSystem.newYBoundary2 = StageSystem.yBoundary2;
				for (j = 0; j < 65536; j++)
				{
					StageSystem.stageLayouts[0].tileMap[j] = 0;
				}
				for (j = 0; j < (int)StageSystem.stageLayouts[0].ySize; j++)
				{
					for (i = 0; i < (int)StageSystem.stageLayouts[0].xSize; i++)
					{
						b = FileIO.ReadByte();
						StageSystem.stageLayouts[0].tileMap[(j << 8) + i] = (ushort)(b << 8);
						b = FileIO.ReadByte();
						ushort[] tileMap = StageSystem.stageLayouts[0].tileMap;
						int num = (j << 8) + i;
						tileMap[num] += (ushort)b;
					}
				}
				b = FileIO.ReadByte();
				j = (int)b;
				int k;
				for (i = 0; i < j; i++)
				{
					b = FileIO.ReadByte();
					for (k = (int)b; k > 0; k--)
					{
						b = FileIO.ReadByte();
					}
				}
				b = FileIO.ReadByte();
				k = (int)b << 8;
				b = FileIO.ReadByte();
				k += (int)b;
				i = 32;
				for (j = 0; j < k; j++)
				{
					b = FileIO.ReadByte();
					ObjectSystem.objectEntityList[i].type = b;
					b = FileIO.ReadByte();
					ObjectSystem.objectEntityList[i].propertyValue = b;
					b = FileIO.ReadByte();
					ObjectSystem.objectEntityList[i].xPos = (int)b << 8;
					b = FileIO.ReadByte();
					ObjectSystem.objectEntityList[i].xPos += (int)b;
					ObjectSystem.objectEntityList[i].xPos <<= 16;
					b = FileIO.ReadByte();
					ObjectSystem.objectEntityList[i].yPos = (int)b << 8;
					b = FileIO.ReadByte();
					ObjectSystem.objectEntityList[i].yPos += (int)b;
					ObjectSystem.objectEntityList[i].yPos <<= 16;
					i++;
				}
				StageSystem.stageLayouts[0].type = 1;
				FileIO.CloseFile();
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
		public static void LoadStageBackground()
		{
			FileData fData = new FileData();
			byte[] array = new byte[3];
			byte[] array2 = new byte[2];
			for (int i = 0; i < 9; i++)
			{
				StageSystem.stageLayouts[i].type = 0;
				StageSystem.stageLayouts[i].deformationPos = 0;
				StageSystem.stageLayouts[i].deformationPosW = 0;
			}
			for (int i = 0; i < 256; i++)
			{
				StageSystem.hParallax.scrollPosition[i] = 0;
				StageSystem.vParallax.scrollPosition[i] = 0;
			}
			for (int i = 0; i < 32768; i++)
			{
				StageSystem.stageLayouts[0].lineScrollRef[i] = 0;
			}
			if (FileIO.LoadStageFile("Backgrounds.bin".ToCharArray(), StageSystem.stageListPosition, fData))
			{
				byte b = FileIO.ReadByte();
				byte b2 = FileIO.ReadByte();
				StageSystem.hParallax.numEntries = b2;
				for (int i = 0; i < (int)StageSystem.hParallax.numEntries; i++)
				{
					b2 = FileIO.ReadByte();
					StageSystem.hParallax.parallaxFactor[i] = (int)b2 << 8;
					b2 = FileIO.ReadByte();
					StageSystem.hParallax.parallaxFactor[i] += (int)b2;
					b2 = FileIO.ReadByte();
					StageSystem.hParallax.scrollSpeed[i] = (int)b2 << 10;
					StageSystem.hParallax.scrollPosition[i] = 0;
					b2 = FileIO.ReadByte();
					StageSystem.hParallax.deformationEnabled[i] = b2;
				}
				b2 = FileIO.ReadByte();
				StageSystem.vParallax.numEntries = b2;
				for (int i = 0; i < (int)StageSystem.vParallax.numEntries; i++)
				{
					b2 = FileIO.ReadByte();
					StageSystem.vParallax.parallaxFactor[i] = (int)b2 << 8;
					b2 = FileIO.ReadByte();
					StageSystem.vParallax.parallaxFactor[i] += (int)b2;
					b2 = FileIO.ReadByte();
					StageSystem.vParallax.scrollSpeed[i] = (int)b2 << 10;
					StageSystem.vParallax.scrollPosition[i] = 0;
					b2 = FileIO.ReadByte();
					StageSystem.vParallax.deformationEnabled[i] = b2;
				}
				for (int i = 1; i < (int)(b + 1); i++)
				{
					b2 = FileIO.ReadByte();
					StageSystem.stageLayouts[i].xSize = b2;
					b2 = FileIO.ReadByte();
					StageSystem.stageLayouts[i].ySize = b2;
					b2 = FileIO.ReadByte();
					StageSystem.stageLayouts[i].type = b2;
					b2 = FileIO.ReadByte();
					StageSystem.stageLayouts[i].parallaxFactor = (int)b2 << 8;
					b2 = FileIO.ReadByte();
					StageSystem.stageLayouts[i].parallaxFactor += (int)b2;
					b2 = FileIO.ReadByte();
					StageSystem.stageLayouts[i].scrollSpeed = (int)b2 << 10;
					StageSystem.stageLayouts[i].scrollPosition = 0;
					int j;
					for (j = 0; j < 65536; j++)
					{
						StageSystem.stageLayouts[i].tileMap[j] = 0;
					}
					for (j = 0; j < 32768; j++)
					{
						StageSystem.stageLayouts[i].lineScrollRef[j] = 0;
					}
					int num = 0;
					j = 0;
					while (j < 1)
					{
						array[0] = FileIO.ReadByte();
						if (array[0] == 255)
						{
							array[1] = FileIO.ReadByte();
							if (array[1] == 255)
							{
								j = 1;
							}
							else
							{
								array[2] = FileIO.ReadByte();
								array2[0] = array[1];
								array2[1] = (byte)(array[2] - 1);
								for (int k = 0; k < (int)array2[1]; k++)
								{
									StageSystem.stageLayouts[i].lineScrollRef[num] = array2[0];
									num++;
								}
							}
						}
						else
						{
							StageSystem.stageLayouts[i].lineScrollRef[num] = array[0];
							num++;
						}
					}
					for (int l = 0; l < (int)StageSystem.stageLayouts[i].ySize; l++)
					{
						for (j = 0; j < (int)StageSystem.stageLayouts[i].xSize; j++)
						{
							b2 = FileIO.ReadByte();
							StageSystem.stageLayouts[i].tileMap[(l << 8) + j] = (ushort)(b2 << 8);
							b2 = FileIO.ReadByte();
							ushort[] tileMap = StageSystem.stageLayouts[i].tileMap;
							int num2 = (l << 8) + j;
							tileMap[num2] += (ushort)b2;
						}
					}
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000FAE0 File Offset: 0x0000DCE0
		public static void ResetBackgroundSettings()
		{
			for (int i = 0; i < 9; i++)
			{
				StageSystem.stageLayouts[i].deformationPos = 0;
				StageSystem.stageLayouts[i].deformationPosW = 0;
				StageSystem.stageLayouts[i].scrollPosition = 0;
			}
			for (int i = 0; i < 256; i++)
			{
				StageSystem.hParallax.scrollPosition[i] = 0;
				StageSystem.vParallax.scrollPosition[i] = 0;
			}
			for (int i = 0; i < 576; i++)
			{
				StageSystem.bgDeformationData0[i] = 0;
				StageSystem.bgDeformationData1[i] = 0;
				StageSystem.bgDeformationData2[i] = 0;
				StageSystem.bgDeformationData3[i] = 0;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		public static void SetLayerDeformation(int selectedDef, int waveLength, int waveWidth, int wType, int yPos, int wSize)
		{
			int num = 0;
			switch (selectedDef)
			{
			case 0:
				switch (wType)
				{
				case 0:
					for (int i = 0; i < 131072; i += 512)
					{
						StageSystem.bgDeformationData0[num] = GlobalAppDefinitions.SinValue512[i / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				case 1:
					num += yPos;
					for (int i = 0; i < wSize; i++)
					{
						StageSystem.bgDeformationData0[num] = GlobalAppDefinitions.SinValue512[(i << 9) / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				}
				for (int i = 256; i < 576; i++)
				{
					StageSystem.bgDeformationData0[i] = StageSystem.bgDeformationData0[i - 256];
				}
				return;
			case 1:
				switch (wType)
				{
				case 0:
					for (int i = 0; i < 131072; i += 512)
					{
						StageSystem.bgDeformationData1[num] = GlobalAppDefinitions.SinValue512[i / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				case 1:
					num += yPos;
					for (int i = 0; i < wSize; i++)
					{
						StageSystem.bgDeformationData1[num] = GlobalAppDefinitions.SinValue512[(i << 9) / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				}
				for (int i = 256; i < 576; i++)
				{
					StageSystem.bgDeformationData1[i] = StageSystem.bgDeformationData1[i - 256];
				}
				return;
			case 2:
				switch (wType)
				{
				case 0:
					for (int i = 0; i < 131072; i += 512)
					{
						StageSystem.bgDeformationData2[num] = GlobalAppDefinitions.SinValue512[i / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				case 1:
					num += yPos;
					for (int i = 0; i < wSize; i++)
					{
						StageSystem.bgDeformationData2[num] = GlobalAppDefinitions.SinValue512[(i << 9) / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				}
				for (int i = 256; i < 576; i++)
				{
					StageSystem.bgDeformationData2[i] = StageSystem.bgDeformationData2[i - 256];
				}
				return;
			case 3:
				switch (wType)
				{
				case 0:
					for (int i = 0; i < 131072; i += 512)
					{
						StageSystem.bgDeformationData3[num] = GlobalAppDefinitions.SinValue512[i / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				case 1:
					num += yPos;
					for (int i = 0; i < wSize; i++)
					{
						StageSystem.bgDeformationData3[num] = GlobalAppDefinitions.SinValue512[(i << 9) / waveLength & 511] * waveWidth >> 5;
						num++;
					}
					break;
				}
				for (int i = 256; i < 576; i++)
				{
					StageSystem.bgDeformationData3[i] = StageSystem.bgDeformationData3[i - 256];
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000FE34 File Offset: 0x0000E034
		public static void DrawStageGfx()
		{
			GraphicsSystem.gfxVertexSize = 0;
			GraphicsSystem.gfxIndexSize = 0;
			GraphicsSystem.waterDrawPos = StageSystem.waterLevel - StageSystem.yScrollOffset;
			if (GraphicsSystem.waterDrawPos < -16)
			{
				GraphicsSystem.waterDrawPos = -16;
			}
			if (GraphicsSystem.waterDrawPos >= 240)
			{
				GraphicsSystem.waterDrawPos = 256;
			}
			ObjectSystem.DrawObjectList(0);
			if (StageSystem.activeTileLayers[0] < 9)
			{
				switch (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[0]].type)
				{
				case 1:
					StageSystem.DrawHLineScrollLayer8(0);
					break;
				case 3:
					StageSystem.Draw3DFloorLayer(0);
					break;
				case 4:
					StageSystem.Draw3DFloorLayer(0);
					break;
				}
			}
			GraphicsSystem.gfxIndexSizeOpaque = GraphicsSystem.gfxIndexSize;
			GraphicsSystem.gfxVertexSizeOpaque = GraphicsSystem.gfxVertexSize;
			ObjectSystem.DrawObjectList(1);
			if (StageSystem.activeTileLayers[1] < 9)
			{
				switch (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[1]].type)
				{
				case 1:
					StageSystem.DrawHLineScrollLayer8(1);
					break;
				case 3:
					StageSystem.Draw3DFloorLayer(1);
					break;
				case 4:
					StageSystem.Draw3DFloorLayer(1);
					break;
				}
			}
			ObjectSystem.DrawObjectList(2);
			if (StageSystem.activeTileLayers[2] < 9)
			{
				switch (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[2]].type)
				{
				case 1:
					StageSystem.DrawHLineScrollLayer8(2);
					break;
				case 3:
					StageSystem.Draw3DFloorLayer(2);
					break;
				case 4:
					StageSystem.Draw3DFloorLayer(2);
					break;
				}
			}
			ObjectSystem.DrawObjectList(3);
			ObjectSystem.DrawObjectList(4);
			if (StageSystem.activeTileLayers[3] < 9)
			{
				switch (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[3]].type)
				{
				case 1:
					StageSystem.DrawHLineScrollLayer8(3);
					break;
				case 3:
					StageSystem.Draw3DFloorLayer(3);
					break;
				case 4:
					StageSystem.Draw3DFloorLayer(3);
					break;
				}
			}
			ObjectSystem.DrawObjectList(5);
			ObjectSystem.DrawObjectList(6);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		public static void DrawHLineScrollLayer8(byte layerNum)
		{
			int num = 0;
			int[] gfxDataPos = StageSystem.tile128x128.gfxDataPos;
			byte[] direction = StageSystem.tile128x128.direction;
			byte[] visualPlane = StageSystem.tile128x128.visualPlane;
			int num2 = (int)StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].xSize;
			int num3 = (int)StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].ySize;
			int num4 = (GlobalAppDefinitions.SCREEN_XSIZE >> 4) + 3;
			byte b;
			if (layerNum >= StageSystem.tLayerMidPoint)
			{
				b = 1;
			}
			else
			{
				b = 0;
			}
			byte b2 = StageSystem.activeTileLayers[(int)layerNum];
			ushort[] tileMap;
			int num5;
			byte[] lineScrollRef;
			int num6;
			int num7;
			int[] array;
			int[] array2;
			if (b2 == 0)
			{
				tileMap = StageSystem.stageLayouts[0].tileMap;
				StageSystem.lastXSize = num2;
				num5 = StageSystem.yScrollOffset;
				lineScrollRef = StageSystem.stageLayouts[0].lineScrollRef;
				StageSystem.hParallax.linePos[0] = StageSystem.xScrollOffset;
				num6 = (StageSystem.stageLayouts[0].deformationPos + num5 & 255);
				num7 = (StageSystem.stageLayouts[0].deformationPosW + num5 & 255);
				array = StageSystem.bgDeformationData0;
				array2 = StageSystem.bgDeformationData1;
				num5 %= num3 << 7;
			}
			else
			{
				tileMap = StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].tileMap;
				num5 = StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].parallaxFactor * StageSystem.yScrollOffset >> 8;
				num3 <<= 7;
				StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].scrollPosition += StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].scrollSpeed;
				if (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].scrollPosition > num3 << 16)
				{
					StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].scrollPosition -= num3 << 16;
				}
				num5 += StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].scrollPosition >> 16;
				num5 %= num3;
				num3 >>= 7;
				lineScrollRef = StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].lineScrollRef;
				num6 = (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].deformationPos + num5 & 255);
				num7 = (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].deformationPosW + num5 & 255);
				array = StageSystem.bgDeformationData2;
				array2 = StageSystem.bgDeformationData3;
			}
			byte type = StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].type;
			if (type != 1)
			{
				if (type != 5)
				{
				}
			}
			else
			{
				if (StageSystem.lastXSize != num2)
				{
					num2 <<= 7;
					for (int i = 0; i < (int)StageSystem.hParallax.numEntries; i++)
					{
						StageSystem.hParallax.linePos[i] = StageSystem.hParallax.parallaxFactor[i] * StageSystem.xScrollOffset >> 8;
						StageSystem.hParallax.scrollPosition[i] += StageSystem.hParallax.scrollSpeed[i];
						if (StageSystem.hParallax.scrollPosition[i] > num2 << 16)
						{
							StageSystem.hParallax.scrollPosition[i] -= num2 << 16;
						}
						StageSystem.hParallax.linePos[i] += StageSystem.hParallax.scrollPosition[i] >> 16;
						StageSystem.hParallax.linePos[i] %= num2;
					}
					num2 >>= 7;
				}
				StageSystem.lastXSize = num2;
			}
			if (num5 < 0)
			{
				num5 += num3 << 7;
			}
			int num8 = num5 >> 4 << 4;
			num += num8;
			num6 += num8 - num5;
			num7 += num8 - num5;
			if (num6 < 0)
			{
				num6 += 256;
			}
			if (num7 < 0)
			{
				num7 += 256;
			}
			num8 = -(num5 & 15);
			int num9 = num5 >> 7;
			int num10 = (num5 & 127) >> 4;
			int num11;
			if (num8 == 0)
			{
				num11 = 256;
			}
			else
			{
				num11 = 272;
			}
			GraphicsSystem.waterDrawPos <<= 4;
			num8 <<= 4;
			for (int j = num11; j > 0; j -= 16)
			{
				int num12 = StageSystem.hParallax.linePos[(int)lineScrollRef[num]] - 16;
				num += 8;
				bool flag;
				if (num12 == StageSystem.hParallax.linePos[(int)lineScrollRef[num]] - 16)
				{
					if (StageSystem.hParallax.deformationEnabled[(int)lineScrollRef[num]] == 1)
					{
						int num13;
						if (num8 >= GraphicsSystem.waterDrawPos)
						{
							num13 = array2[num7];
						}
						else
						{
							num13 = array[num6];
						}
						num6 += 8;
						num7 += 8;
						int num14;
						if (num8 + 64 > GraphicsSystem.waterDrawPos)
						{
							num14 = array2[num7];
						}
						else
						{
							num14 = array[num6];
						}
						flag = (num13 != num14);
						num6 -= 8;
						num7 -= 8;
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = true;
				}
				num -= 8;
				if (flag)
				{
					int i = num2 << 7;
					if (num12 < 0)
					{
						num12 += i;
					}
					if (num12 >= i)
					{
						num12 -= i;
					}
					int num15 = num12 >> 7;
					int num16 = (num12 & 127) >> 4;
					int num13 = -((num12 & 15) << 4);
					num13 -= 256;
					int num14 = num13;
					if (StageSystem.hParallax.deformationEnabled[(int)lineScrollRef[num]] == 1)
					{
						if (num8 >= GraphicsSystem.waterDrawPos)
						{
							num13 -= array2[num7];
						}
						else
						{
							num13 -= array[num6];
						}
						num6 += 8;
						num7 += 8;
						if (num8 + 64 > GraphicsSystem.waterDrawPos)
						{
							num14 -= array2[num7];
						}
						else
						{
							num14 -= array[num6];
						}
					}
					else
					{
						num6 += 8;
						num7 += 8;
					}
					num += 8;
					int num17;
					if (num15 > -1 && num9 > -1)
					{
						num17 = (int)tileMap[num15 + (num9 << 8)] << 6;
					}
					else
					{
						num17 = 0;
					}
					num17 += num16 + (num10 << 3);
					for (i = num4; i > 0; i--)
					{
						if (visualPlane[num17] == b && gfxDataPos[num17] > 0)
						{
							int num18 = 0;
							switch (direction[num17])
							{
							case 0:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] - 0.0078125f;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 1:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] - 0.0078125f;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 2:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] + 0.0078125f;
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 3:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] + 0.0078125f;
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							}
						}
						num13 += 256;
						num14 += 256;
						num16++;
						if (num16 > 7)
						{
							num15++;
							if (num15 == num2)
							{
								num15 = 0;
							}
							num16 = 0;
							num17 = (int)tileMap[num15 + (num9 << 8)] << 6;
							num17 += num16 + (num10 << 3);
						}
						else
						{
							num17++;
						}
					}
					num8 += 128;
					num12 = StageSystem.hParallax.linePos[(int)lineScrollRef[num]] - 16;
					i = num2 << 7;
					if (num12 < 0)
					{
						num12 += i;
					}
					if (num12 >= i)
					{
						num12 -= i;
					}
					num15 = num12 >> 7;
					num16 = (num12 & 127) >> 4;
					num13 = -((num12 & 15) << 4);
					num13 -= 256;
					num14 = num13;
					if (StageSystem.hParallax.deformationEnabled[(int)lineScrollRef[num]] == 1)
					{
						if (num8 >= GraphicsSystem.waterDrawPos)
						{
							num13 -= array2[num7];
						}
						else
						{
							num13 -= array[num6];
						}
						num6 += 8;
						num7 += 8;
						if (num8 + 64 > GraphicsSystem.waterDrawPos)
						{
							num14 -= array2[num7];
						}
						else
						{
							num14 -= array[num6];
						}
					}
					else
					{
						num6 += 8;
						num7 += 8;
					}
					num += 8;
					if (num15 > -1 && num9 > -1)
					{
						num17 = (int)tileMap[num15 + (num9 << 8)] << 6;
					}
					else
					{
						num17 = 0;
					}
					num17 += num16 + (num10 << 3);
					for (i = num4; i > 0; i--)
					{
						if (visualPlane[num17] == b && gfxDataPos[num17] > 0)
						{
							int num18 = 0;
							switch (direction[num17])
							{
							case 0:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] + 0.0078125f;
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 1:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] + 0.0078125f;
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 2:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] - 0.0078125f;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 3:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 128);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18] - 0.0078125f;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							}
						}
						num13 += 256;
						num14 += 256;
						num16++;
						if (num16 > 7)
						{
							num15++;
							if (num15 == num2)
							{
								num15 = 0;
							}
							num16 = 0;
							num17 = (int)tileMap[num15 + (num9 << 8)] << 6;
							num17 += num16 + (num10 << 3);
						}
						else
						{
							num17++;
						}
					}
					num8 += 128;
				}
				else
				{
					int i = num2 << 7;
					if (num12 < 0)
					{
						num12 += i;
					}
					if (num12 >= i)
					{
						num12 -= i;
					}
					int num15 = num12 >> 7;
					int num16 = (num12 & 127) >> 4;
					int num13 = -((num12 & 15) << 4);
					num13 -= 256;
					int num14 = num13;
					if (StageSystem.hParallax.deformationEnabled[(int)lineScrollRef[num]] == 1)
					{
						if (num8 >= GraphicsSystem.waterDrawPos)
						{
							num13 -= array2[num7];
						}
						else
						{
							num13 -= array[num6];
						}
						num6 += 16;
						num7 += 16;
						if (num8 + 128 > GraphicsSystem.waterDrawPos)
						{
							num14 -= array2[num7];
						}
						else
						{
							num14 -= array[num6];
						}
					}
					else
					{
						num6 += 16;
						num7 += 16;
					}
					num += 16;
					int num17;
					if (num15 > -1 && num9 > -1)
					{
						num17 = (int)tileMap[num15 + (num9 << 8)] << 6;
					}
					else
					{
						num17 = 0;
					}
					num17 += num16 + (num10 << 3);
					for (i = num4; i > 0; i--)
					{
						if (visualPlane[num17] == b && gfxDataPos[num17] > 0)
						{
							int num18 = 0;
							switch (direction[num17])
							{
							case 0:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 1:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 2:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							case 3:
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num14 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num14;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)(num8 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								num18++;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)(num13 + 256);
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = (float)num8;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num17] + num18];
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.X = (float)num13;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].position.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].position.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.X = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 2)].texCoord.X;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].texCoord.Y = GraphicsSystem.gfxPolyList[(int)(GraphicsSystem.gfxVertexSize - 1)].texCoord.Y;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.R = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.G = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.B = byte.MaxValue;
								GraphicsSystem.gfxPolyList[(int)GraphicsSystem.gfxVertexSize].color.A = byte.MaxValue;
								GraphicsSystem.gfxVertexSize += 1;
								GraphicsSystem.gfxIndexSize += 2;
								break;
							}
						}
						num13 += 256;
						num14 += 256;
						num16++;
						if (num16 > 7)
						{
							num15++;
							if (num15 == num2)
							{
								num15 = 0;
							}
							num16 = 0;
							num17 = (int)tileMap[num15 + (num9 << 8)] << 6;
							num17 += num16 + (num10 << 3);
						}
						else
						{
							num17++;
						}
					}
					num8 += 256;
				}
				num10++;
				if (num10 > 7)
				{
					num9++;
					if (num9 == num3)
					{
						num9 = 0;
						num -= num3 << 7;
					}
					num10 = 0;
				}
			}
			GraphicsSystem.waterDrawPos >>= 4;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000141D8 File Offset: 0x000123D8
		public static void Draw3DFloorLayer(byte layerNum)
		{
			int[] gfxDataPos = StageSystem.tile128x128.gfxDataPos;
			byte[] direction = StageSystem.tile128x128.direction;
			int num = (int)StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].xSize << 7;
			int num2 = (int)StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].ySize << 7;
			ushort[] tileMap = StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].tileMap;
			GraphicsSystem.vertexSize3D = 0;
			GraphicsSystem.indexSize3D = 0;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = 0.5f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
			GraphicsSystem.vertexSize3D += 1;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = 4096f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = 1f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
			GraphicsSystem.vertexSize3D += 1;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = 4096f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = 0.5f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = 0.5f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
			GraphicsSystem.vertexSize3D += 1;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = 4096f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = 4096f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = 1f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = 0.5f;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
			GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
			GraphicsSystem.vertexSize3D += 1;
			GraphicsSystem.indexSize3D += 2;
			if (!GlobalAppDefinitions.HQ3DFloorEnabled)
			{
				int num3 = (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].xPos >> 16) - 160;
				num3 += GlobalAppDefinitions.SinValue512[StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].angle] / 3;
				num3 = num3 >> 4 << 4;
				int num4 = (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].zPos >> 16) - 160;
				num4 += GlobalAppDefinitions.CosValue512[StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].angle] / 3;
				num4 = num4 >> 4 << 4;
				for (int i = 20; i > 0; i--)
				{
					for (int j = 20; j > 0; j--)
					{
						if (num3 > -1 && num3 < num && num4 > -1 && num4 < num2)
						{
							int num5 = num3 >> 7;
							int num6 = num4 >> 7;
							int num7 = (num3 & 127) >> 4;
							int num8 = (num4 & 127) >> 4;
							int num9 = (int)tileMap[num5 + (num6 << 8)] << 6;
							num9 += num7 + (num8 << 3);
							if (gfxDataPos[num9] > 0)
							{
								int num10 = 0;
								switch (direction[num9])
								{
								case 0:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								case 1:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								case 2:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								case 3:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								}
							}
						}
						num3 += 16;
					}
					num3 -= 320;
					num4 += 16;
				}
			}
			else
			{
				int num3 = (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].xPos >> 16) - 256;
				num3 += GlobalAppDefinitions.SinValue512[StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].angle] >> 1;
				num3 = num3 >> 4 << 4;
				int num4 = (StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].zPos >> 16) - 256;
				num4 += GlobalAppDefinitions.CosValue512[StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].angle] >> 1;
				num4 = num4 >> 4 << 4;
				for (int i = 32; i > 0; i--)
				{
					for (int j = 32; j > 0; j--)
					{
						if (num3 > -1 && num3 < num && num4 > -1 && num4 < num2)
						{
							int num5 = num3 >> 7;
							int num6 = num4 >> 7;
							int num7 = (num3 & 127) >> 4;
							int num8 = (num4 & 127) >> 4;
							int num9 = (int)tileMap[num5 + (num6 << 8)] << 6;
							num9 += num7 + (num8 << 3);
							if (gfxDataPos[num9] > 0)
							{
								int num10 = 0;
								switch (direction[num9])
								{
								case 0:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								case 1:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								case 2:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								case 3:
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)(num3 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)(num4 + 16);
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = (float)num3;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									num10++;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = (float)num4;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.tileUVArray[gfxDataPos[num9] + num10];
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].position.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Y = 0f;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].position.Z = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].position.Z;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.X = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 2)].texCoord.X;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].texCoord.Y = GraphicsSystem.polyList3D[(int)(GraphicsSystem.vertexSize3D - 1)].texCoord.Y;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.R = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.G = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.B = byte.MaxValue;
									GraphicsSystem.polyList3D[(int)GraphicsSystem.vertexSize3D].color.A = byte.MaxValue;
									GraphicsSystem.vertexSize3D += 1;
									GraphicsSystem.indexSize3D += 2;
									break;
								}
							}
						}
						num3 += 16;
					}
					num3 -= 512;
					num4 += 16;
				}
			}
			GraphicsSystem.floor3DPos.X = (float)(StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].xPos >> 8) * -0.00390625f;
			GraphicsSystem.floor3DPos.Y = (float)(StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].yPos >> 8) * 0.00390625f;
			GraphicsSystem.floor3DPos.Z = (float)(StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].zPos >> 8) * -0.00390625f;
			GraphicsSystem.floor3DAngle = (float)StageSystem.stageLayouts[(int)StageSystem.activeTileLayers[(int)layerNum]].angle / 512f * -360f;
			GraphicsSystem.render3DEnabled = true;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000174A8 File Offset: 0x000156A8
		public static void InitFirstStage()
		{
			StageSystem.xScrollOffset = 0;
			StageSystem.yScrollOffset = 0;
			AudioPlayback.StopMusic();
			AudioPlayback.StopAllSFX();
			AudioPlayback.ReleaseStageSFX();
			GraphicsSystem.fadeMode = 0;
			PlayerSystem.playerMenuNum = 0;
			GraphicsSystem.ClearGraphicsData();
			AnimationSystem.ClearAnimationData();
			GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
			FileIO.activeStageList = 0;
			StageSystem.stageMode = 0;
			GlobalAppDefinitions.gameMode = 1;
			StageSystem.stageListPosition = 0;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00017518 File Offset: 0x00015718
		public static void InitStageSelectMenu()
		{
			StageSystem.xScrollOffset = 0;
			StageSystem.yScrollOffset = 0;
			AudioPlayback.StopMusic();
			AudioPlayback.StopAllSFX();
			AudioPlayback.ReleaseStageSFX();
			GraphicsSystem.fadeMode = 0;
			PlayerSystem.playerMenuNum = 0;
			GlobalAppDefinitions.gameMode = 0;
			GraphicsSystem.ClearGraphicsData();
			AnimationSystem.ClearAnimationData();
			GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
			TextSystem.textMenuSurfaceNo = 0;
			GraphicsSystem.LoadGIFFile("Data/Game/SystemText.gif".ToCharArray(), 0);
			StageSystem.stageMode = 0;
			TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "RETRO ENGINE DEV MENU".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "SONIC CD Version".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], GlobalAppDefinitions.gameVersion);
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "PLAY GAME".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
			TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "STAGE SELECT".ToCharArray());
			StageSystem.gameMenu[0].alignment = 2;
			StageSystem.gameMenu[0].numSelections = 2;
			StageSystem.gameMenu[0].selection1 = 0;
			StageSystem.gameMenu[0].selection2 = 7;
			StageSystem.gameMenu[1].numVisibleRows = 0;
			StageSystem.gameMenu[1].visibleRowOffset = 0;
			RenderDevice.UpdateHardwareTextures();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000176C8 File Offset: 0x000158C8
		public static void InitErrorMessage()
		{
			StageSystem.xScrollOffset = 0;
			StageSystem.yScrollOffset = 0;
			AudioPlayback.StopMusic();
			AudioPlayback.StopAllSFX();
			AudioPlayback.ReleaseStageSFX();
			GraphicsSystem.fadeMode = 0;
			PlayerSystem.playerMenuNum = 0;
			GlobalAppDefinitions.gameMode = 0;
			GraphicsSystem.ClearGraphicsData();
			AnimationSystem.ClearAnimationData();
			GraphicsSystem.LoadPalette("MasterPalette.act".ToCharArray(), 0, 0, 0, 256);
			TextSystem.textMenuSurfaceNo = 0;
			GraphicsSystem.LoadGIFFile("Data/Game/SystemText.gif".ToCharArray(), 0);
			StageSystem.gameMenu[0].alignment = 2;
			StageSystem.gameMenu[0].numSelections = 1;
			StageSystem.gameMenu[0].selection1 = 0;
			StageSystem.gameMenu[1].numVisibleRows = 0;
			StageSystem.gameMenu[1].visibleRowOffset = 0;
			RenderDevice.UpdateHardwareTextures();
			StageSystem.stageMode = 4;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00017788 File Offset: 0x00015988
		public static void ProcessStageSelectMenu()
		{
			GraphicsSystem.gfxVertexSize = 0;
			GraphicsSystem.gfxIndexSize = 0;
			GraphicsSystem.ClearScreen(240);
			InputSystem.MenuKeyDown(StageSystem.gKeyDown, 131);
			GraphicsSystem.DrawSprite(32, 66, 16, 16, 78, 240, 0);
			GraphicsSystem.DrawSprite(32, 178, 16, 16, 95, 240, 0);
			GraphicsSystem.DrawSprite(GlobalAppDefinitions.SCREEN_XSIZE - 32, 208, 16, 16, 112, 240, 0);
			StageSystem.gKeyPress.start = 0;
			StageSystem.gKeyPress.up = 0;
			StageSystem.gKeyPress.down = 0;
			if (StageSystem.gKeyDown.touches > 0)
			{
				if (StageSystem.gKeyDown.touchX[0] < 120)
				{
					if (StageSystem.gKeyDown.touchY[0] < 120)
					{
						if (StageSystem.gKeyDown.up == 0)
						{
							StageSystem.gKeyPress.up = 1;
						}
						StageSystem.gKeyDown.up = 1;
					}
					else
					{
						if (StageSystem.gKeyDown.down == 0)
						{
							StageSystem.gKeyPress.down = 1;
						}
						StageSystem.gKeyDown.down = 1;
					}
				}
				if (StageSystem.gKeyDown.touchX[0] > 200)
				{
					if (StageSystem.gKeyDown.start == 0)
					{
						StageSystem.gKeyPress.start = 1;
					}
					StageSystem.gKeyDown.start = 1;
				}
			}
			else
			{
				StageSystem.gKeyDown.start = 0;
				StageSystem.gKeyDown.up = 0;
				StageSystem.gKeyDown.down = 0;
			}
			switch (StageSystem.stageMode)
			{
			case 0:
				if (StageSystem.gKeyPress.down == 1)
				{
					StageSystem.gameMenu[0].selection2 += 2;
				}
				if (StageSystem.gKeyPress.up == 1)
				{
					StageSystem.gameMenu[0].selection2 -= 2;
				}
				if (StageSystem.gameMenu[0].selection2 > 9)
				{
					StageSystem.gameMenu[0].selection2 = 7;
				}
				if (StageSystem.gameMenu[0].selection2 < 7)
				{
					StageSystem.gameMenu[0].selection2 = 9;
				}
				TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER, 72);
				if (StageSystem.gKeyPress.start == 1)
				{
					if (StageSystem.gameMenu[0].selection2 == 7)
					{
						StageSystem.stageMode = 0;
						GlobalAppDefinitions.gameMode = 1;
						FileIO.activeStageList = 0;
						StageSystem.stageListPosition = 0;
					}
					else
					{
						TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
						TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "CHOOSE A PLAYER".ToCharArray());
						TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
						TextSystem.LoadConfigListText(StageSystem.gameMenu[1], 0);
						StageSystem.gameMenu[1].alignment = 0;
						StageSystem.gameMenu[1].numSelections = 1;
						StageSystem.gameMenu[1].selection1 = 0;
						StageSystem.stageMode = 1;
					}
				}
				break;
			case 1:
				if (StageSystem.gKeyPress.down == 1)
				{
					StageSystem.gameMenu[1].selection1++;
				}
				if (StageSystem.gKeyPress.up == 1)
				{
					StageSystem.gameMenu[1].selection1--;
				}
				if (StageSystem.gameMenu[1].selection1 == (int)StageSystem.gameMenu[1].numRows)
				{
					StageSystem.gameMenu[1].selection1 = 0;
				}
				if (StageSystem.gameMenu[1].selection1 < 0)
				{
					StageSystem.gameMenu[1].selection1 = (int)(StageSystem.gameMenu[1].numRows - 1);
				}
				TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 4, 72);
				TextSystem.DrawTextMenu(StageSystem.gameMenu[1], GlobalAppDefinitions.SCREEN_CENTER - 40, 96);
				if (StageSystem.gKeyPress.start == 1)
				{
					PlayerSystem.playerMenuNum = (byte)StageSystem.gameMenu[1].selection1;
					TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "SELECT A STAGE LIST".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   PRESENTATION".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   REGULAR".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   SPECIAL".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "   BONUS".ToCharArray());
					StageSystem.gameMenu[0].alignment = 0;
					StageSystem.gameMenu[0].selection2 = 3;
					StageSystem.stageMode = 2;
				}
				break;
			case 2:
			{
				if (StageSystem.gKeyPress.down == 1)
				{
					StageSystem.gameMenu[0].selection2 += 2;
				}
				if (StageSystem.gKeyPress.up == 1)
				{
					StageSystem.gameMenu[0].selection2 -= 2;
				}
				if (StageSystem.gameMenu[0].selection2 > 9)
				{
					StageSystem.gameMenu[0].selection2 = 3;
				}
				if (StageSystem.gameMenu[0].selection2 < 3)
				{
					StageSystem.gameMenu[0].selection2 = 9;
				}
				TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 80, 72);
				int num = 0;
				switch (StageSystem.gameMenu[0].selection2)
				{
				case 3:
					if (FileIO.noPresentationStages > 0)
					{
						num = 1;
					}
					FileIO.activeStageList = 0;
					break;
				case 5:
					if (FileIO.noZoneStages > 0)
					{
						num = 1;
					}
					FileIO.activeStageList = 1;
					break;
				case 7:
					if (FileIO.noSpecialStages > 0)
					{
						num = 1;
					}
					FileIO.activeStageList = 3;
					break;
				case 9:
					if (FileIO.noBonusStages > 0)
					{
						num = 1;
					}
					FileIO.activeStageList = 2;
					break;
				}
				if (StageSystem.gKeyPress.start == 1 && num == 1)
				{
					TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "SELECT A STAGE".ToCharArray());
					TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
					TextSystem.LoadConfigListText(StageSystem.gameMenu[1], 1 + (StageSystem.gameMenu[0].selection2 - 3 >> 1));
					StageSystem.gameMenu[1].alignment = 1;
					StageSystem.gameMenu[1].numSelections = 3;
					StageSystem.gameMenu[1].selection1 = 0;
					if (StageSystem.gameMenu[1].numRows > 18)
					{
						StageSystem.gameMenu[1].numVisibleRows = 18;
					}
					StageSystem.gameMenu[0].alignment = 2;
					StageSystem.gameMenu[0].numSelections = 1;
					StageSystem.gameMenu[1].timer = 0;
					StageSystem.stageMode = 3;
				}
				break;
			}
			case 3:
				if (StageSystem.gKeyDown.down == 1)
				{
					TextMenu textMenu = StageSystem.gameMenu[1];
					textMenu.timer += 1;
					if (StageSystem.gameMenu[1].timer > 4)
					{
						StageSystem.gameMenu[1].timer = 0;
						StageSystem.gKeyPress.down = 1;
					}
				}
				else if (StageSystem.gKeyDown.up == 1)
				{
					TextMenu textMenu2 = StageSystem.gameMenu[1];
					textMenu2.timer -= 1;
					if (StageSystem.gameMenu[1].timer < -4)
					{
						StageSystem.gameMenu[1].timer = 0;
						StageSystem.gKeyPress.up = 1;
					}
				}
				else
				{
					StageSystem.gameMenu[1].timer = 0;
				}
				if (StageSystem.gKeyPress.down == 1)
				{
					StageSystem.gameMenu[1].selection1++;
					if (StageSystem.gameMenu[1].selection1 - (int)StageSystem.gameMenu[1].visibleRowOffset >= (int)StageSystem.gameMenu[1].numVisibleRows)
					{
						TextMenu textMenu3 = StageSystem.gameMenu[1];
						textMenu3.visibleRowOffset += 1;
					}
				}
				if (StageSystem.gKeyPress.up == 1)
				{
					StageSystem.gameMenu[1].selection1--;
					if (StageSystem.gameMenu[1].selection1 - (int)StageSystem.gameMenu[1].visibleRowOffset < 0)
					{
						TextMenu textMenu4 = StageSystem.gameMenu[1];
						textMenu4.visibleRowOffset -= 1;
					}
				}
				if (StageSystem.gameMenu[1].selection1 == (int)StageSystem.gameMenu[1].numRows)
				{
					StageSystem.gameMenu[1].selection1 = 0;
					StageSystem.gameMenu[1].visibleRowOffset = 0;
				}
				if (StageSystem.gameMenu[1].selection1 < 0)
				{
					StageSystem.gameMenu[1].selection1 = (int)(StageSystem.gameMenu[1].numRows - 1);
					StageSystem.gameMenu[1].visibleRowOffset = (ushort)(StageSystem.gameMenu[1].numRows - StageSystem.gameMenu[1].numVisibleRows);
				}
				TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER - 4, 40);
				TextSystem.DrawTextMenu(StageSystem.gameMenu[1], GlobalAppDefinitions.SCREEN_CENTER + 100, 64);
				if (StageSystem.gKeyPress.start == 1)
				{
					if (StageSystem.gKeyDown.touches > 1)
					{
						StageSystem.debugMode = 1;
					}
					else
					{
						StageSystem.debugMode = 0;
					}
					StageSystem.stageMode = 0;
					GlobalAppDefinitions.gameMode = 1;
					StageSystem.stageListPosition = StageSystem.gameMenu[1].selection1;
				}
				break;
			case 4:
				TextSystem.DrawTextMenu(StageSystem.gameMenu[0], GlobalAppDefinitions.SCREEN_CENTER, 72);
				if (StageSystem.gKeyPress.start == 1)
				{
					StageSystem.stageMode = 0;
					TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "RETRO ENGINE DEV MENU".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "PLAY GAME".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], " ".ToCharArray());
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], "STAGE SELECT".ToCharArray());
					StageSystem.gameMenu[0].alignment = 2;
					StageSystem.gameMenu[0].numSelections = 2;
					StageSystem.gameMenu[0].selection1 = 0;
					StageSystem.gameMenu[0].selection2 = 7;
					StageSystem.gameMenu[1].numVisibleRows = 0;
					StageSystem.gameMenu[1].visibleRowOffset = 0;
				}
				break;
			}
			GraphicsSystem.gfxIndexSizeOpaque = GraphicsSystem.gfxIndexSize;
			GraphicsSystem.gfxVertexSizeOpaque = GraphicsSystem.gfxVertexSize;
		}

		// Token: 0x04000141 RID: 321
		public const int ACTLAYOUT = 0;

		// Token: 0x04000142 RID: 322
		public const int LOADSTAGE = 0;

		// Token: 0x04000143 RID: 323
		public const int PLAYSTAGE = 1;

		// Token: 0x04000144 RID: 324
		public const int STAGEPAUSED = 2;

		// Token: 0x04000145 RID: 325
		public static InputResult gKeyDown = new InputResult();

		// Token: 0x04000146 RID: 326
		public static InputResult gKeyPress = new InputResult();

		// Token: 0x04000147 RID: 327
		public static byte stageMode;

		// Token: 0x04000148 RID: 328
		public static byte pauseEnabled;

		// Token: 0x04000149 RID: 329
		public static int stageListPosition;

		// Token: 0x0400014A RID: 330
		public static Mappings128x128 tile128x128 = new Mappings128x128();

		// Token: 0x0400014B RID: 331
		public static LayoutMap[] stageLayouts = new LayoutMap[9];

		// Token: 0x0400014C RID: 332
		public static byte tLayerMidPoint;

		// Token: 0x0400014D RID: 333
		public static byte[] activeTileLayers = new byte[4];

		// Token: 0x0400014E RID: 334
		public static CollisionMask16x16[] tileCollisions = new CollisionMask16x16[2];

		// Token: 0x0400014F RID: 335
		public static LineScrollParallax hParallax = new LineScrollParallax();

		// Token: 0x04000150 RID: 336
		public static LineScrollParallax vParallax = new LineScrollParallax();

		// Token: 0x04000151 RID: 337
		public static int lastXSize;

		// Token: 0x04000152 RID: 338
		public static int lastYSize;

		// Token: 0x04000153 RID: 339
		public static int[] bgDeformationData0 = new int[576];

		// Token: 0x04000154 RID: 340
		public static int[] bgDeformationData1 = new int[576];

		// Token: 0x04000155 RID: 341
		public static int[] bgDeformationData2 = new int[576];

		// Token: 0x04000156 RID: 342
		public static int[] bgDeformationData3 = new int[576];

		// Token: 0x04000157 RID: 343
		public static int xBoundary1 = 0;

		// Token: 0x04000158 RID: 344
		public static int xBoundary2;

		// Token: 0x04000159 RID: 345
		public static int yBoundary1 = 0;

		// Token: 0x0400015A RID: 346
		public static int yBoundary2;

		// Token: 0x0400015B RID: 347
		public static int newXBoundary1 = 0;

		// Token: 0x0400015C RID: 348
		public static int newXBoundary2;

		// Token: 0x0400015D RID: 349
		public static int newYBoundary1 = 0;

		// Token: 0x0400015E RID: 350
		public static int newYBoundary2;

		// Token: 0x0400015F RID: 351
		public static byte cameraEnabled;

		// Token: 0x04000160 RID: 352
		public static sbyte cameraTarget;

		// Token: 0x04000161 RID: 353
		public static byte cameraShift = 0;

		// Token: 0x04000162 RID: 354
		public static byte cameraStyle = 0;

		// Token: 0x04000163 RID: 355
		public static int cameraAdjustY;

		// Token: 0x04000164 RID: 356
		public static int xScrollOffset = 0;

		// Token: 0x04000165 RID: 357
		public static int yScrollOffset = 0;

		// Token: 0x04000166 RID: 358
		public static int yScrollA = 0;

		// Token: 0x04000167 RID: 359
		public static int yScrollB = 240;

		// Token: 0x04000168 RID: 360
		public static int xScrollA = 0;

		// Token: 0x04000169 RID: 361
		public static int xScrollB = 320;

		// Token: 0x0400016A RID: 362
		public static int xScrollMove = 0;

		// Token: 0x0400016B RID: 363
		public static int yScrollMove = 0;

		// Token: 0x0400016C RID: 364
		public static int screenShakeX = 0;

		// Token: 0x0400016D RID: 365
		public static int screenShakeY = 0;

		// Token: 0x0400016E RID: 366
		public static int waterLevel;

		// Token: 0x0400016F RID: 367
		public static char[] titleCardText = new char[24];

		// Token: 0x04000170 RID: 368
		public static char titleCardWord2;

		// Token: 0x04000171 RID: 369
		public static TextMenu[] gameMenu = new TextMenu[2];

		// Token: 0x04000172 RID: 370
		public static byte timeEnabled;

		// Token: 0x04000173 RID: 371
		public static byte milliSeconds;

		// Token: 0x04000174 RID: 372
		public static byte seconds;

		// Token: 0x04000175 RID: 373
		public static byte minutes;

		// Token: 0x04000176 RID: 374
		public static byte debugMode = 0;
	}
}
