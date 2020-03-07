using System;

namespace Retro_Engine
{
	// Token: 0x0200002A RID: 42
	public static class ObjectSystem
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x000223A8 File Offset: 0x000205A8
		static ObjectSystem()
		{
			for (int i = 0; i < ObjectSystem.scriptFrames.Length; i++)
			{
				ObjectSystem.scriptFrames[i] = new SpriteFrame();
			}
			for (int i = 0; i < ObjectSystem.objectScriptList.Length; i++)
			{
				ObjectSystem.objectScriptList[i] = new ObjectScript();
			}
			for (int i = 0; i < ObjectSystem.functionScriptList.Length; i++)
			{
				ObjectSystem.functionScriptList[i] = new FunctionScript();
			}
			for (int i = 0; i < ObjectSystem.objectEntityList.Length; i++)
			{
				ObjectSystem.objectEntityList[i] = new ObjectEntity();
			}
			for (int i = 0; i < ObjectSystem.objectDrawOrderList.Length; i++)
			{
				ObjectSystem.objectDrawOrderList[i] = new ObjectDrawList();
			}
			for (int i = 0; i < ObjectSystem.cSensor.Length; i++)
			{
				ObjectSystem.cSensor[i] = new CollisionSensor();
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00022594 File Offset: 0x00020794
		public static void ClearScriptData()
		{
			char[] typeName = "BlankObject".ToCharArray();
			for (int i = 0; i < 262144; i++)
			{
				ObjectSystem.scriptData[i] = 0;
			}
			for (int i = 0; i < 16384; i++)
			{
				ObjectSystem.jumpTableData[i] = 0;
			}
			ObjectSystem.scriptDataPos = 0;
			ObjectSystem.jumpTableDataPos = 0;
			ObjectSystem.scriptFramesNo = 0;
			ObjectSystem.NUM_FUNCTIONS = 0;
			AnimationSystem.ClearAnimationData();
			for (int i = 0; i < 2; i++)
			{
				PlayerSystem.playerList[i].animationFile = AnimationSystem.GetDefaultAnimationRef();
				PlayerSystem.playerList[i].objectPtr = ObjectSystem.objectEntityList[0];
			}
			for (int i = 0; i < 256; i++)
			{
				ObjectSystem.objectScriptList[i].mainScript = 262143;
				ObjectSystem.objectScriptList[i].mainJumpTable = 16383;
				ObjectSystem.objectScriptList[i].playerScript = 262143;
				ObjectSystem.objectScriptList[i].playerJumpTable = 16383;
				ObjectSystem.objectScriptList[i].drawScript = 262143;
				ObjectSystem.objectScriptList[i].drawJumpTable = 16383;
				ObjectSystem.objectScriptList[i].startupScript = 262143;
				ObjectSystem.objectScriptList[i].startupJumpTable = 16383;
				ObjectSystem.objectScriptList[i].frameListOffset = 0;
				ObjectSystem.objectScriptList[i].numFrames = 0;
				ObjectSystem.objectScriptList[i].surfaceNum = 0;
				ObjectSystem.objectScriptList[i].animationFile = AnimationSystem.GetDefaultAnimationRef();
				ObjectSystem.functionScriptList[i].mainScript = 262143;
				ObjectSystem.functionScriptList[i].mainJumpTable = 16383;
				ObjectSystem.typeNames[i, 0] = '\0';
			}
			ObjectSystem.SetObjectTypeName(typeName, 0);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00022738 File Offset: 0x00020938
		public static void SetObjectTypeName(char[] typeName, int scriptNum)
		{
			int i = 0;
			int num = 0;
			while (i < typeName.Length)
			{
				if (typeName[i] != '\0')
				{
					if (typeName[i] != ' ')
					{
						ObjectSystem.typeNames[scriptNum, num] = typeName[i];
						num++;
					}
					i++;
				}
				else
				{
					i = typeName.Length;
				}
			}
			if (num < ObjectSystem.typeNames.GetLength(1))
			{
				ObjectSystem.typeNames[scriptNum, num] = '\0';
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00022798 File Offset: 0x00020998
		public static void LoadByteCodeFile(int fileType, int scriptNum)
		{
			FileData fData = new FileData();
			char[] array = "Data/Scripts/ByteCode/".ToCharArray();
			char[] array2 = ".bin".ToCharArray();
			char[] array3 = "GlobalCode.bin".ToCharArray();
			FileIO.StrCopy(ref ObjectSystem.scriptText, ref array);
			switch (fileType)
			{
			case 0:
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.pStageList[StageSystem.stageListPosition].stageFolderName);
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref array2);
				break;
			case 1:
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.zStageList[StageSystem.stageListPosition].stageFolderName);
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref array2);
				break;
			case 2:
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.bStageList[StageSystem.stageListPosition].stageFolderName);
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref array2);
				break;
			case 3:
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref FileIO.sStageList[StageSystem.stageListPosition].stageFolderName);
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref array2);
				break;
			case 4:
				FileIO.StrAdd(ref ObjectSystem.scriptText, ref array3);
				break;
			}
			if (FileIO.LoadFile(ObjectSystem.scriptText, fData))
			{
				int num = ObjectSystem.scriptDataPos;
				byte b = FileIO.ReadByte();
				int i = (int)b;
				b = FileIO.ReadByte();
				i += (int)b << 8;
				b = FileIO.ReadByte();
				i += (int)b << 16;
				b = FileIO.ReadByte();
				i += (int)b << 24;
				int num2;
				while (i > 0)
				{
					b = FileIO.ReadByte();
					byte b2 = (byte)(b & 127);
					if (b < 128)
					{
						while (b2 > 0)
						{
							b = FileIO.ReadByte();
							ObjectSystem.scriptData[num] = (int)b;
							num++;
							ObjectSystem.scriptDataPos++;
							i--;
							b2 -= 1;
						}
					}
					else
					{
						while (b2 > 0)
						{
							b = FileIO.ReadByte();
							num2 = (int)b;
							b = FileIO.ReadByte();
							num2 += (int)b << 8;
							b = FileIO.ReadByte();
							num2 += (int)b << 16;
							b = FileIO.ReadByte();
							num2 += (int)b << 24;
							ObjectSystem.scriptData[num] = num2;
							num++;
							ObjectSystem.scriptDataPos++;
							i--;
							b2 -= 1;
						}
					}
				}
				num = ObjectSystem.jumpTableDataPos;
				b = FileIO.ReadByte();
				i = (int)b;
				b = FileIO.ReadByte();
				i += (int)b << 8;
				b = FileIO.ReadByte();
				i += (int)b << 16;
				b = FileIO.ReadByte();
				i += (int)b << 24;
				while (i > 0)
				{
					b = FileIO.ReadByte();
					byte b2 = (byte)(b & 127);
					if (b < 128)
					{
						while (b2 > 0)
						{
							b = FileIO.ReadByte();
							ObjectSystem.jumpTableData[num] = (int)b;
							num++;
							ObjectSystem.jumpTableDataPos++;
							i--;
							b2 -= 1;
						}
					}
					else
					{
						while (b2 > 0)
						{
							b = FileIO.ReadByte();
							num2 = (int)b;
							b = FileIO.ReadByte();
							num2 += (int)b << 8;
							b = FileIO.ReadByte();
							num2 += (int)b << 16;
							b = FileIO.ReadByte();
							num2 += (int)b << 24;
							ObjectSystem.jumpTableData[num] = num2;
							num++;
							ObjectSystem.jumpTableDataPos++;
							i--;
							b2 -= 1;
						}
					}
				}
				b = FileIO.ReadByte();
				i = (int)b;
				b = FileIO.ReadByte();
				i += (int)b << 8;
				num2 = scriptNum;
				for (int j = i; j > 0; j--)
				{
					b = FileIO.ReadByte();
					int num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].mainScript = num3;
					b = FileIO.ReadByte();
					num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].playerScript = num3;
					b = FileIO.ReadByte();
					num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].drawScript = num3;
					b = FileIO.ReadByte();
					num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].startupScript = num3;
					num2++;
				}
				num2 = scriptNum;
				for (int j = i; j > 0; j--)
				{
					b = FileIO.ReadByte();
					int num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].mainJumpTable = num3;
					b = FileIO.ReadByte();
					num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].playerJumpTable = num3;
					b = FileIO.ReadByte();
					num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].drawJumpTable = num3;
					b = FileIO.ReadByte();
					num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.objectScriptList[num2].startupJumpTable = num3;
					num2++;
				}
				b = FileIO.ReadByte();
				i = (int)b;
				b = FileIO.ReadByte();
				i += (int)b << 8;
				num2 = 0;
				for (int j = i; j > 0; j--)
				{
					b = FileIO.ReadByte();
					int num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.functionScriptList[num2].mainScript = num3;
					num2++;
				}
				num2 = 0;
				for (int j = i; j > 0; j--)
				{
					b = FileIO.ReadByte();
					int num3 = (int)b;
					b = FileIO.ReadByte();
					num3 += (int)b << 8;
					b = FileIO.ReadByte();
					num3 += (int)b << 16;
					b = FileIO.ReadByte();
					num3 += (int)b << 24;
					ObjectSystem.functionScriptList[num2].mainJumpTable = num3;
					num2++;
				}
				FileIO.CloseFile();
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00022DB0 File Offset: 0x00020FB0
		public static void ProcessScript(int scriptCodePtr, int jumpTablePtr, int scriptSub)
		{
			bool flag = false;
			int num = 0;
			int num2 = scriptCodePtr;
			ObjectSystem.jumpTableStackPos = 0;
			ObjectSystem.functionStackPos = 0;
			while (!flag)
			{
				int num3 = ObjectSystem.scriptData[scriptCodePtr];
				scriptCodePtr++;
				int num4 = 0;
				sbyte b = ObjectSystem.scriptOpcodeSizes[num3];
				for (int i = 0; i < (int)b; i++)
				{
					switch (ObjectSystem.scriptData[scriptCodePtr])
					{
					case 1:
						scriptCodePtr++;
						num4++;
						switch (ObjectSystem.scriptData[scriptCodePtr])
						{
						case 0:
							num = ObjectSystem.objectLoop;
							break;
						case 1:
							scriptCodePtr++;
							if (ObjectSystem.scriptData[scriptCodePtr] == 1)
							{
								scriptCodePtr++;
								num = ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
							}
							else
							{
								scriptCodePtr++;
								num = ObjectSystem.scriptData[scriptCodePtr];
							}
							num4 += 2;
							break;
						case 2:
							scriptCodePtr++;
							if (ObjectSystem.scriptData[scriptCodePtr] == 1)
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop + ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
							}
							else
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop + ObjectSystem.scriptData[scriptCodePtr];
							}
							num4 += 2;
							break;
						case 3:
							scriptCodePtr++;
							if (ObjectSystem.scriptData[scriptCodePtr] == 1)
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop - ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
							}
							else
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop - ObjectSystem.scriptData[scriptCodePtr];
							}
							num4 += 2;
							break;
						}
						scriptCodePtr++;
						num4++;
						switch (ObjectSystem.scriptData[scriptCodePtr])
						{
						case 0:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[0];
							break;
						case 1:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[1];
							break;
						case 2:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[2];
							break;
						case 3:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[3];
							break;
						case 4:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[4];
							break;
						case 5:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[5];
							break;
						case 6:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[6];
							break;
						case 7:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.tempValue[7];
							break;
						case 8:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.checkResult;
							break;
						case 9:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.arrayPosition[0];
							break;
						case 10:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptEng.arrayPosition[1];
							break;
						case 11:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.globalVariables[num];
							break;
						case 12:
							ObjectSystem.scriptEng.operands[i] = num;
							break;
						case 13:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].type;
							break;
						case 14:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].propertyValue;
							break;
						case 15:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].xPos;
							break;
						case 16:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].yPos;
							break;
						case 17:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].xPos >> 16;
							break;
						case 18:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].yPos >> 16;
							break;
						case 19:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].state;
							break;
						case 20:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].rotation;
							break;
						case 21:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].scale;
							break;
						case 22:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].priority;
							break;
						case 23:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].drawOrder;
							break;
						case 24:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].direction;
							break;
						case 25:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].inkEffect;
							break;
						case 26:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].alpha;
							break;
						case 27:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].frame;
							break;
						case 28:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].animation;
							break;
						case 29:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectEntityList[num].prevAnimation;
							break;
						case 30:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].animationSpeed;
							break;
						case 31:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].animationTimer;
							break;
						case 32:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[0];
							break;
						case 33:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[1];
							break;
						case 34:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[2];
							break;
						case 35:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[3];
							break;
						case 36:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[4];
							break;
						case 37:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[5];
							break;
						case 38:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[6];
							break;
						case 39:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectEntityList[num].value[7];
							break;
						case 40:
							ObjectSystem.scriptEng.sRegister = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16;
							if (ObjectSystem.scriptEng.sRegister > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && ObjectSystem.scriptEng.sRegister < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2)
							{
								ObjectSystem.scriptEng.sRegister = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16;
								if (ObjectSystem.scriptEng.sRegister > StageSystem.yScrollOffset - 256 && ObjectSystem.scriptEng.sRegister < StageSystem.yScrollOffset + 496)
								{
									ObjectSystem.scriptEng.operands[i] = 0;
								}
								else
								{
									ObjectSystem.scriptEng.operands[i] = 1;
								}
							}
							else
							{
								ObjectSystem.scriptEng.operands[i] = 1;
							}
							break;
						case 41:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.state;
							break;
						case 42:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].controlMode;
							break;
						case 43:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].controlLock;
							break;
						case 44:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].collisionMode;
							break;
						case 45:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].collisionPlane;
							break;
						case 46:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].xPos;
							break;
						case 47:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].yPos;
							break;
						case 48:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].xPos >> 16;
							break;
						case 49:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].yPos >> 16;
							break;
						case 50:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].screenXPos;
							break;
						case 51:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].screenYPos;
							break;
						case 52:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].speed;
							break;
						case 53:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].xVelocity;
							break;
						case 54:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].yVelocity;
							break;
						case 55:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].gravity;
							break;
						case 56:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].angle;
							break;
						case 57:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].skidding;
							break;
						case 58:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].pushing;
							break;
						case 59:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].trackScroll;
							break;
						case 60:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].up;
							break;
						case 61:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].down;
							break;
						case 62:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].left;
							break;
						case 63:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].right;
							break;
						case 64:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].jumpPress;
							break;
						case 65:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].jumpHold;
							break;
						case 66:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].followPlayer1;
							break;
						case 67:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].lookPos;
							break;
						case 68:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].water;
							break;
						case 69:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.topSpeed;
							break;
						case 70:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.acceleration;
							break;
						case 71:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.deceleration;
							break;
						case 72:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airAcceleration;
							break;
						case 73:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airDeceleration;
							break;
						case 74:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.gravity;
							break;
						case 75:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpStrength;
							break;
						case 76:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpCap;
							break;
						case 77:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingAcceleration;
							break;
						case 78:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingDeceleration;
							break;
						case 79:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectNum;
							break;
						case 80:
							ObjectSystem.scriptEng.operands[i] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].left[0];
							break;
						case 81:
							ObjectSystem.scriptEng.operands[i] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].top[0];
							break;
						case 82:
							ObjectSystem.scriptEng.operands[i] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].right[0];
							break;
						case 83:
							ObjectSystem.scriptEng.operands[i] = (int)AnimationSystem.collisionBoxList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation].frameListOffset + (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame].collisionBox].bottom[0];
							break;
						case 84:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].flailing[num];
							break;
						case 85:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].timer;
							break;
						case 86:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].tileCollisions;
							break;
						case 87:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction;
							break;
						case 88:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].visible;
							break;
						case 89:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.rotation;
							break;
						case 90:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.scale;
							break;
						case 91:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.priority;
							break;
						case 92:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.drawOrder;
							break;
						case 93:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.direction;
							break;
						case 94:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.inkEffect;
							break;
						case 95:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.alpha;
							break;
						case 96:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame;
							break;
						case 97:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation;
							break;
						case 98:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.prevAnimation;
							break;
						case 99:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationSpeed;
							break;
						case 100:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationTimer;
							break;
						case 101:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[0];
							break;
						case 102:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[1];
							break;
						case 103:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[2];
							break;
						case 104:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[3];
							break;
						case 105:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[4];
							break;
						case 106:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[5];
							break;
						case 107:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[6];
							break;
						case 108:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[7];
							break;
						case 109:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[0];
							break;
						case 110:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[1];
							break;
						case 111:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[2];
							break;
						case 112:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[3];
							break;
						case 113:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[4];
							break;
						case 114:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[5];
							break;
						case 115:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[6];
							break;
						case 116:
							ObjectSystem.scriptEng.operands[i] = PlayerSystem.playerList[ObjectSystem.playerNum].value[7];
							break;
						case 117:
							ObjectSystem.scriptEng.sRegister = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.xPos >> 16;
							if (ObjectSystem.scriptEng.sRegister > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && ObjectSystem.scriptEng.sRegister < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2)
							{
								ObjectSystem.scriptEng.sRegister = PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.yPos >> 16;
								if (ObjectSystem.scriptEng.sRegister > StageSystem.yScrollOffset - 256 && ObjectSystem.scriptEng.sRegister < StageSystem.yScrollOffset + 496)
								{
									ObjectSystem.scriptEng.operands[i] = 0;
								}
								else
								{
									ObjectSystem.scriptEng.operands[i] = 1;
								}
							}
							else
							{
								ObjectSystem.scriptEng.operands[i] = 1;
							}
							break;
						case 118:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.stageMode;
							break;
						case 119:
							ObjectSystem.scriptEng.operands[i] = (int)FileIO.activeStageList;
							break;
						case 120:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageListPosition;
							break;
						case 121:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.timeEnabled;
							break;
						case 122:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.milliSeconds;
							break;
						case 123:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.seconds;
							break;
						case 124:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.minutes;
							break;
						case 125:
							ObjectSystem.scriptEng.operands[i] = FileIO.actNumber;
							break;
						case 126:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.pauseEnabled;
							break;
						case 127:
							switch (FileIO.activeStageList)
							{
							case 0:
								ObjectSystem.scriptEng.operands[i] = (int)FileIO.noPresentationStages;
								break;
							case 1:
								ObjectSystem.scriptEng.operands[i] = (int)FileIO.noZoneStages;
								break;
							case 2:
								ObjectSystem.scriptEng.operands[i] = (int)FileIO.noBonusStages;
								break;
							case 3:
								ObjectSystem.scriptEng.operands[i] = (int)FileIO.noSpecialStages;
								break;
							}
							break;
						case 128:
							ObjectSystem.scriptEng.operands[i] = StageSystem.newXBoundary1;
							break;
						case 129:
							ObjectSystem.scriptEng.operands[i] = StageSystem.newXBoundary2;
							break;
						case 130:
							ObjectSystem.scriptEng.operands[i] = StageSystem.newYBoundary1;
							break;
						case 131:
							ObjectSystem.scriptEng.operands[i] = StageSystem.newYBoundary2;
							break;
						case 132:
							ObjectSystem.scriptEng.operands[i] = StageSystem.xBoundary1;
							break;
						case 133:
							ObjectSystem.scriptEng.operands[i] = StageSystem.xBoundary2;
							break;
						case 134:
							ObjectSystem.scriptEng.operands[i] = StageSystem.yBoundary1;
							break;
						case 135:
							ObjectSystem.scriptEng.operands[i] = StageSystem.yBoundary2;
							break;
						case 136:
							ObjectSystem.scriptEng.operands[i] = StageSystem.bgDeformationData0[num];
							break;
						case 137:
							ObjectSystem.scriptEng.operands[i] = StageSystem.bgDeformationData1[num];
							break;
						case 138:
							ObjectSystem.scriptEng.operands[i] = StageSystem.bgDeformationData2[num];
							break;
						case 139:
							ObjectSystem.scriptEng.operands[i] = StageSystem.bgDeformationData3[num];
							break;
						case 140:
							ObjectSystem.scriptEng.operands[i] = StageSystem.waterLevel;
							break;
						case 141:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.activeTileLayers[num];
							break;
						case 142:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.tLayerMidPoint;
							break;
						case 143:
							ObjectSystem.scriptEng.operands[i] = (int)PlayerSystem.playerMenuNum;
							break;
						case 144:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.playerNum;
							break;
						case 145:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.cameraEnabled;
							break;
						case 146:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.cameraTarget;
							break;
						case 147:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.cameraStyle;
							break;
						case 148:
							ObjectSystem.scriptEng.operands[i] = ObjectSystem.objectDrawOrderList[num].listSize;
							break;
						case 149:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.SCREEN_CENTER;
							break;
						case 150:
							ObjectSystem.scriptEng.operands[i] = 120;
							break;
						case 151:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.SCREEN_XSIZE;
							break;
						case 152:
							ObjectSystem.scriptEng.operands[i] = 240;
							break;
						case 153:
							ObjectSystem.scriptEng.operands[i] = StageSystem.xScrollOffset;
							break;
						case 154:
							ObjectSystem.scriptEng.operands[i] = StageSystem.yScrollOffset;
							break;
						case 155:
							ObjectSystem.scriptEng.operands[i] = StageSystem.screenShakeX;
							break;
						case 156:
							ObjectSystem.scriptEng.operands[i] = StageSystem.screenShakeY;
							break;
						case 157:
							ObjectSystem.scriptEng.operands[i] = StageSystem.cameraAdjustY;
							break;
						case 158:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.touchDown[num];
							break;
						case 159:
							ObjectSystem.scriptEng.operands[i] = StageSystem.gKeyDown.touchX[num];
							break;
						case 160:
							ObjectSystem.scriptEng.operands[i] = StageSystem.gKeyDown.touchY[num];
							break;
						case 161:
							ObjectSystem.scriptEng.operands[i] = AudioPlayback.musicVolume;
							break;
						case 162:
							ObjectSystem.scriptEng.operands[i] = AudioPlayback.currentMusicTrack;
							break;
						case 163:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.up;
							break;
						case 164:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.down;
							break;
						case 165:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.left;
							break;
						case 166:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.right;
							break;
						case 167:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.buttonA;
							break;
						case 168:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.buttonB;
							break;
						case 169:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.buttonC;
							break;
						case 170:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyDown.start;
							break;
						case 171:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.up;
							break;
						case 172:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.down;
							break;
						case 173:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.left;
							break;
						case 174:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.right;
							break;
						case 175:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.buttonA;
							break;
						case 176:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.buttonB;
							break;
						case 177:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.buttonC;
							break;
						case 178:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.start;
							break;
						case 179:
							ObjectSystem.scriptEng.operands[i] = StageSystem.gameMenu[0].selection1;
							break;
						case 180:
							ObjectSystem.scriptEng.operands[i] = StageSystem.gameMenu[1].selection1;
							break;
						case 181:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.stageLayouts[num].xSize;
							break;
						case 182:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.stageLayouts[num].ySize;
							break;
						case 183:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.stageLayouts[num].type;
							break;
						case 184:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].angle;
							break;
						case 185:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].xPos;
							break;
						case 186:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].yPos;
							break;
						case 187:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].zPos;
							break;
						case 188:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].parallaxFactor;
							break;
						case 189:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].scrollSpeed;
							break;
						case 190:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].scrollPosition;
							break;
						case 191:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].deformationPos;
							break;
						case 192:
							ObjectSystem.scriptEng.operands[i] = StageSystem.stageLayouts[num].deformationPosW;
							break;
						case 193:
							ObjectSystem.scriptEng.operands[i] = StageSystem.hParallax.parallaxFactor[num];
							break;
						case 194:
							ObjectSystem.scriptEng.operands[i] = StageSystem.hParallax.scrollSpeed[num];
							break;
						case 195:
							ObjectSystem.scriptEng.operands[i] = StageSystem.hParallax.scrollPosition[num];
							break;
						case 196:
							ObjectSystem.scriptEng.operands[i] = StageSystem.vParallax.parallaxFactor[num];
							break;
						case 197:
							ObjectSystem.scriptEng.operands[i] = StageSystem.vParallax.scrollSpeed[num];
							break;
						case 198:
							ObjectSystem.scriptEng.operands[i] = StageSystem.vParallax.scrollPosition[num];
							break;
						case 199:
							ObjectSystem.scriptEng.operands[i] = Scene3D.numVertices;
							break;
						case 200:
							ObjectSystem.scriptEng.operands[i] = Scene3D.numFaces;
							break;
						case 201:
							ObjectSystem.scriptEng.operands[i] = Scene3D.vertexBuffer[num].x;
							break;
						case 202:
							ObjectSystem.scriptEng.operands[i] = Scene3D.vertexBuffer[num].y;
							break;
						case 203:
							ObjectSystem.scriptEng.operands[i] = Scene3D.vertexBuffer[num].z;
							break;
						case 204:
							ObjectSystem.scriptEng.operands[i] = Scene3D.vertexBuffer[num].u;
							break;
						case 205:
							ObjectSystem.scriptEng.operands[i] = Scene3D.vertexBuffer[num].v;
							break;
						case 206:
							ObjectSystem.scriptEng.operands[i] = Scene3D.indexBuffer[num].a;
							break;
						case 207:
							ObjectSystem.scriptEng.operands[i] = Scene3D.indexBuffer[num].b;
							break;
						case 208:
							ObjectSystem.scriptEng.operands[i] = Scene3D.indexBuffer[num].c;
							break;
						case 209:
							ObjectSystem.scriptEng.operands[i] = Scene3D.indexBuffer[num].d;
							break;
						case 210:
							ObjectSystem.scriptEng.operands[i] = (int)Scene3D.indexBuffer[num].flag;
							break;
						case 211:
							ObjectSystem.scriptEng.operands[i] = Scene3D.indexBuffer[num].color;
							break;
						case 212:
							ObjectSystem.scriptEng.operands[i] = Scene3D.projectionX;
							break;
						case 213:
							ObjectSystem.scriptEng.operands[i] = Scene3D.projectionY;
							break;
						case 214:
							ObjectSystem.scriptEng.operands[i] = (int)GlobalAppDefinitions.gameMode;
							break;
						case 215:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.debugMode;
							break;
						case 216:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.gameMessage;
							break;
						case 217:
							ObjectSystem.scriptEng.operands[i] = FileIO.saveRAM[num];
							break;
						case 218:
							ObjectSystem.scriptEng.operands[i] = (int)GlobalAppDefinitions.gameLanguage;
							break;
						case 219:
							ObjectSystem.scriptEng.operands[i] = (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum;
							break;
						case 220:
							ObjectSystem.scriptEng.operands[i] = (int)GlobalAppDefinitions.gameOnlineActive;
							break;
						case 221:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.frameSkipTimer;
							break;
						case 222:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.frameSkipSetting;
							break;
						case 223:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.gameSFXVolume;
							break;
						case 224:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.gameBGMVolume;
							break;
						case 225:
							ObjectSystem.scriptEng.operands[i] = GlobalAppDefinitions.gamePlatformID;
							break;
						case 226:
							ObjectSystem.scriptEng.operands[i] = (int)GlobalAppDefinitions.gameTrialMode;
							break;
						case 227:
							ObjectSystem.scriptEng.operands[i] = (int)StageSystem.gKeyPress.start;
							break;
						case 228:
							ObjectSystem.scriptEng.operands[i] = (int)GlobalAppDefinitions.gameHapticsEnabled;
							break;
						}
						scriptCodePtr++;
						num4++;
						break;
					case 2:
						scriptCodePtr++;
						ObjectSystem.scriptEng.operands[i] = ObjectSystem.scriptData[scriptCodePtr];
						scriptCodePtr++;
						num4 += 2;
						break;
					case 3:
					{
						scriptCodePtr++;
						num4++;
						int j = 0;
						num = 0;
						ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptData[scriptCodePtr];
						ObjectSystem.scriptText[ObjectSystem.scriptEng.sRegister] = '\0';
						while (j < ObjectSystem.scriptEng.sRegister)
						{
							switch (num)
							{
							case 0:
								scriptCodePtr++;
								num4++;
								ObjectSystem.scriptText[j] = (char)(ObjectSystem.scriptData[scriptCodePtr] >> 24);
								num++;
								break;
							case 1:
								ObjectSystem.scriptText[j] = (char)((ObjectSystem.scriptData[scriptCodePtr] & 16777215) >> 16);
								num++;
								break;
							case 2:
								ObjectSystem.scriptText[j] = (char)((ObjectSystem.scriptData[scriptCodePtr] & 65535) >> 8);
								num++;
								break;
							case 3:
								ObjectSystem.scriptText[j] = (char)(ObjectSystem.scriptData[scriptCodePtr] & 255);
								num = 0;
								break;
							}
							j++;
						}
						if (num == 0)
						{
							scriptCodePtr += 2;
							num4 += 2;
						}
						else
						{
							scriptCodePtr++;
							num4++;
						}
						break;
					}
					}
				}
				switch (num3)
				{
				case 0:
					flag = true;
					break;
				case 1:
					ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.operands[1];
					break;
				case 2:
					ObjectSystem.scriptEng.operands[0] += ObjectSystem.scriptEng.operands[1];
					break;
				case 3:
					ObjectSystem.scriptEng.operands[0] -= ObjectSystem.scriptEng.operands[1];
					break;
				case 4:
					ObjectSystem.scriptEng.operands[0]++;
					break;
				case 5:
					ObjectSystem.scriptEng.operands[0]--;
					break;
				case 6:
					ObjectSystem.scriptEng.operands[0] *= ObjectSystem.scriptEng.operands[1];
					break;
				case 7:
					ObjectSystem.scriptEng.operands[0] /= ObjectSystem.scriptEng.operands[1];
					break;
				case 8:
					ObjectSystem.scriptEng.operands[0] >>= ObjectSystem.scriptEng.operands[1];
					break;
				case 9:
					ObjectSystem.scriptEng.operands[0] <<= ObjectSystem.scriptEng.operands[1];
					break;
				case 10:
					ObjectSystem.scriptEng.operands[0] &= ObjectSystem.scriptEng.operands[1];
					break;
				case 11:
					ObjectSystem.scriptEng.operands[0] |= ObjectSystem.scriptEng.operands[1];
					break;
				case 12:
					ObjectSystem.scriptEng.operands[0] ^= ObjectSystem.scriptEng.operands[1];
					break;
				case 13:
					ObjectSystem.scriptEng.operands[0] %= ObjectSystem.scriptEng.operands[1];
					break;
				case 14:
					ObjectSystem.scriptEng.operands[0] = -ObjectSystem.scriptEng.operands[0];
					break;
				case 15:
					if (ObjectSystem.scriptEng.operands[0] == ObjectSystem.scriptEng.operands[1])
					{
						ObjectSystem.scriptEng.checkResult = 1;
					}
					else
					{
						ObjectSystem.scriptEng.checkResult = 0;
					}
					b = 0;
					break;
				case 16:
					if (ObjectSystem.scriptEng.operands[0] > ObjectSystem.scriptEng.operands[1])
					{
						ObjectSystem.scriptEng.checkResult = 1;
					}
					else
					{
						ObjectSystem.scriptEng.checkResult = 0;
					}
					b = 0;
					break;
				case 17:
					if (ObjectSystem.scriptEng.operands[0] < ObjectSystem.scriptEng.operands[1])
					{
						ObjectSystem.scriptEng.checkResult = 1;
					}
					else
					{
						ObjectSystem.scriptEng.checkResult = 0;
					}
					b = 0;
					break;
				case 18:
					if (ObjectSystem.scriptEng.operands[0] != ObjectSystem.scriptEng.operands[1])
					{
						ObjectSystem.scriptEng.checkResult = 1;
					}
					else
					{
						ObjectSystem.scriptEng.checkResult = 0;
					}
					b = 0;
					break;
				case 19:
					if (ObjectSystem.scriptEng.operands[1] == ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					b = 0;
					break;
				case 20:
					if (ObjectSystem.scriptEng.operands[1] > ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					b = 0;
					break;
				case 21:
					if (ObjectSystem.scriptEng.operands[1] >= ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					b = 0;
					break;
				case 22:
					if (ObjectSystem.scriptEng.operands[1] < ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					b = 0;
					break;
				case 23:
					if (ObjectSystem.scriptEng.operands[1] <= ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					b = 0;
					break;
				case 24:
					if (ObjectSystem.scriptEng.operands[1] != ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]];
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					b = 0;
					break;
				case 25:
					b = 0;
					scriptCodePtr = num2;
					scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] + 1];
					ObjectSystem.jumpTableStackPos--;
					break;
				case 26:
					b = 0;
					ObjectSystem.jumpTableStackPos--;
					break;
				case 27:
					if (ObjectSystem.scriptEng.operands[1] == ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
					}
					b = 0;
					break;
				case 28:
					if (ObjectSystem.scriptEng.operands[1] > ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
					}
					b = 0;
					break;
				case 29:
					if (ObjectSystem.scriptEng.operands[1] >= ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
					}
					b = 0;
					break;
				case 30:
					if (ObjectSystem.scriptEng.operands[1] < ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
					}
					b = 0;
					break;
				case 31:
					if (ObjectSystem.scriptEng.operands[1] <= ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
					}
					b = 0;
					break;
				case 32:
					if (ObjectSystem.scriptEng.operands[1] != ObjectSystem.scriptEng.operands[2])
					{
						ObjectSystem.jumpTableStackPos++;
						ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1];
					}
					b = 0;
					break;
				case 33:
					b = 0;
					scriptCodePtr = num2;
					scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos]];
					ObjectSystem.jumpTableStackPos--;
					break;
				case 34:
					ObjectSystem.jumpTableStackPos++;
					ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] = ObjectSystem.scriptEng.operands[0];
					if (ObjectSystem.scriptEng.operands[1] >= ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]] && ObjectSystem.scriptEng.operands[1] <= ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 1])
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 4 + (ObjectSystem.scriptEng.operands[1] - ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0]])];
					}
					else
					{
						scriptCodePtr = num2;
						scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.scriptEng.operands[0] + 2];
					}
					b = 0;
					break;
				case 35:
					b = 0;
					scriptCodePtr = num2;
					scriptCodePtr += ObjectSystem.jumpTableData[jumpTablePtr + ObjectSystem.jumpTableStack[ObjectSystem.jumpTableStackPos] + 3];
					ObjectSystem.jumpTableStackPos--;
					break;
				case 36:
					b = 0;
					ObjectSystem.jumpTableStackPos--;
					break;
				case 37:
					ObjectSystem.scriptEng.operands[0] = ObjectSystem.rand.Next(0, ObjectSystem.scriptEng.operands[1]);
					break;
				case 38:
					ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
					if (ObjectSystem.scriptEng.sRegister < 0)
					{
						ObjectSystem.scriptEng.sRegister = 512 - ObjectSystem.scriptEng.sRegister;
					}
					ObjectSystem.scriptEng.sRegister &= 511;
					ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.SinValue512[ObjectSystem.scriptEng.sRegister];
					break;
				case 39:
					ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
					if (ObjectSystem.scriptEng.sRegister < 0)
					{
						ObjectSystem.scriptEng.sRegister = 512 - ObjectSystem.scriptEng.sRegister;
					}
					ObjectSystem.scriptEng.sRegister &= 511;
					ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.CosValue512[ObjectSystem.scriptEng.sRegister];
					break;
				case 40:
					ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
					if (ObjectSystem.scriptEng.sRegister < 0)
					{
						ObjectSystem.scriptEng.sRegister = 256 - ObjectSystem.scriptEng.sRegister;
					}
					ObjectSystem.scriptEng.sRegister &= 255;
					ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.SinValue256[ObjectSystem.scriptEng.sRegister];
					break;
				case 41:
					ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
					if (ObjectSystem.scriptEng.sRegister < 0)
					{
						ObjectSystem.scriptEng.sRegister = 256 - ObjectSystem.scriptEng.sRegister;
					}
					ObjectSystem.scriptEng.sRegister &= 255;
					ObjectSystem.scriptEng.operands[0] = GlobalAppDefinitions.CosValue256[ObjectSystem.scriptEng.sRegister];
					break;
				case 42:
					ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
					if (ObjectSystem.scriptEng.sRegister < 0)
					{
						ObjectSystem.scriptEng.sRegister = 512 - ObjectSystem.scriptEng.sRegister;
					}
					ObjectSystem.scriptEng.sRegister &= 511;
					ObjectSystem.scriptEng.operands[0] = (GlobalAppDefinitions.SinValue512[ObjectSystem.scriptEng.sRegister] >> ObjectSystem.scriptEng.operands[2]) + ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[4];
					break;
				case 43:
					ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptEng.operands[1];
					if (ObjectSystem.scriptEng.sRegister < 0)
					{
						ObjectSystem.scriptEng.sRegister = 512 - ObjectSystem.scriptEng.sRegister;
					}
					ObjectSystem.scriptEng.sRegister &= 511;
					ObjectSystem.scriptEng.operands[0] = (GlobalAppDefinitions.CosValue512[ObjectSystem.scriptEng.sRegister] >> ObjectSystem.scriptEng.operands[2]) + ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[4];
					break;
				case 44:
					ObjectSystem.scriptEng.operands[0] = (int)GlobalAppDefinitions.ArcTanLookup(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
					break;
				case 45:
					ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.operands[1] * ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptEng.operands[2] * (256 - ObjectSystem.scriptEng.operands[3]) >> 8;
					break;
				case 46:
					ObjectSystem.scriptEng.operands[0] = (ObjectSystem.scriptEng.operands[2] * ObjectSystem.scriptEng.operands[6] >> 8) + (ObjectSystem.scriptEng.operands[3] * (256 - ObjectSystem.scriptEng.operands[6]) >> 8);
					ObjectSystem.scriptEng.operands[1] = (ObjectSystem.scriptEng.operands[4] * ObjectSystem.scriptEng.operands[6] >> 8) + (ObjectSystem.scriptEng.operands[5] * (256 - ObjectSystem.scriptEng.operands[6]) >> 8);
					break;
				case 47:
					b = 0;
					ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum = GraphicsSystem.AddGraphicsFile(ObjectSystem.scriptText);
					break;
				case 48:
					b = 0;
					GraphicsSystem.RemoveGraphicsFile(ObjectSystem.scriptText, -1);
					break;
				case 49:
					b = 0;
					GraphicsSystem.DrawSprite((ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
					break;
				case 50:
					b = 0;
					GraphicsSystem.DrawSprite((ObjectSystem.scriptEng.operands[1] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
					break;
				case 51:
					b = 0;
					GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
					break;
				case 52:
					b = 0;
					GraphicsSystem.DrawTintRectangle(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
					break;
				case 53:
					b = 0;
					ObjectSystem.scriptEng.operands[7] = 10;
					if (ObjectSystem.scriptEng.operands[6] == 0)
					{
						if (ObjectSystem.scriptEng.operands[3] == 0)
						{
							ObjectSystem.scriptEng.operands[8] = 10;
						}
						else
						{
							ObjectSystem.scriptEng.operands[8] = ObjectSystem.scriptEng.operands[3] * 10;
						}
						while (ObjectSystem.scriptEng.operands[4] > 0)
						{
							if (ObjectSystem.scriptEng.operands[8] >= ObjectSystem.scriptEng.operands[7])
							{
								ObjectSystem.scriptEng.sRegister = (ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[3] / ObjectSystem.scriptEng.operands[7] * ObjectSystem.scriptEng.operands[7]) / (ObjectSystem.scriptEng.operands[7] / 10);
								ObjectSystem.scriptEng.sRegister += ObjectSystem.scriptEng.operands[0];
								GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							}
							ObjectSystem.scriptEng.operands[1] -= ObjectSystem.scriptEng.operands[5];
							ObjectSystem.scriptEng.operands[7] *= 10;
							ObjectSystem.scriptEng.operands[4]--;
						}
					}
					else
					{
						while (ObjectSystem.scriptEng.operands[4] > 0)
						{
							ObjectSystem.scriptEng.sRegister = (ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptEng.operands[3] / ObjectSystem.scriptEng.operands[7] * ObjectSystem.scriptEng.operands[7]) / (ObjectSystem.scriptEng.operands[7] / 10);
							ObjectSystem.scriptEng.sRegister += ObjectSystem.scriptEng.operands[0];
							GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.sRegister].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							ObjectSystem.scriptEng.operands[1] -= ObjectSystem.scriptEng.operands[5];
							ObjectSystem.scriptEng.operands[7] *= 10;
							ObjectSystem.scriptEng.operands[4]--;
						}
					}
					break;
				case 54:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[3])
					{
					case 1:
						ObjectSystem.scriptEng.sRegister = 0;
						if (ObjectSystem.scriptEng.operands[4] == 1 && StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != '\0')
						{
							ObjectSystem.scriptEng.operands[7] = (int)StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
							if (ObjectSystem.scriptEng.operands[7] == 32)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] == 45)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
							{
								ObjectSystem.scriptEng.operands[7] -= 22;
							}
							if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
							{
								ObjectSystem.scriptEng.operands[7] -= 65;
							}
							if (ObjectSystem.scriptEng.operands[7] > -1)
							{
								ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
								GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
							}
							else
							{
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
							}
							ObjectSystem.scriptEng.operands[0] += 26;
							ObjectSystem.scriptEng.sRegister++;
						}
						while (StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != '\0')
						{
							if (StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] == '-')
							{
								break;
							}
							ObjectSystem.scriptEng.operands[7] = (int)StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
							if (ObjectSystem.scriptEng.operands[7] == 32)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] == 45)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
							{
								ObjectSystem.scriptEng.operands[7] -= 22;
							}
							if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
							{
								ObjectSystem.scriptEng.operands[7] -= 65;
							}
							if (ObjectSystem.scriptEng.operands[7] > -1)
							{
								ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
								GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
							}
							else
							{
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
							}
							ObjectSystem.scriptEng.sRegister++;
						}
						break;
					case 2:
						ObjectSystem.scriptEng.sRegister = (int)StageSystem.titleCardWord2;
						if (ObjectSystem.scriptEng.operands[4] == 1 && StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != '\0')
						{
							ObjectSystem.scriptEng.operands[7] = (int)StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
							if (ObjectSystem.scriptEng.operands[7] == 32)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] == 45)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
							{
								ObjectSystem.scriptEng.operands[7] -= 22;
							}
							if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
							{
								ObjectSystem.scriptEng.operands[7] -= 65;
							}
							if (ObjectSystem.scriptEng.operands[7] > -1)
							{
								ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
								GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
							}
							else
							{
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
							}
							ObjectSystem.scriptEng.operands[0] += 26;
							ObjectSystem.scriptEng.sRegister++;
						}
						while (StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister] != '\0')
						{
							ObjectSystem.scriptEng.operands[7] = (int)StageSystem.titleCardText[ObjectSystem.scriptEng.sRegister];
							if (ObjectSystem.scriptEng.operands[7] == 32)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] == 45)
							{
								ObjectSystem.scriptEng.operands[7] = 0;
							}
							if (ObjectSystem.scriptEng.operands[7] > 47 && ObjectSystem.scriptEng.operands[7] < 58)
							{
								ObjectSystem.scriptEng.operands[7] -= 22;
							}
							if (ObjectSystem.scriptEng.operands[7] > 57 && ObjectSystem.scriptEng.operands[7] < 102)
							{
								ObjectSystem.scriptEng.operands[7] -= 65;
							}
							if (ObjectSystem.scriptEng.operands[7] > -1)
							{
								ObjectSystem.scriptEng.operands[7] += ObjectSystem.scriptEng.operands[0];
								GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xPivot, ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[7]].xSize + ObjectSystem.scriptEng.operands[6];
							}
							else
							{
								ObjectSystem.scriptEng.operands[1] += ObjectSystem.scriptEng.operands[5] + ObjectSystem.scriptEng.operands[6];
							}
							ObjectSystem.scriptEng.sRegister++;
						}
						break;
					}
					break;
				case 55:
					b = 0;
					TextSystem.textMenuSurfaceNo = (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum;
					TextSystem.DrawTextMenu(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
					break;
				case 56:
					b = 0;
					if (scriptSub == 3 && ObjectSystem.scriptFramesNo < 4096)
					{
						ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].xPivot = ObjectSystem.scriptEng.operands[0];
						ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].yPivot = ObjectSystem.scriptEng.operands[1];
						ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].xSize = ObjectSystem.scriptEng.operands[2];
						ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].ySize = ObjectSystem.scriptEng.operands[3];
						ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].left = ObjectSystem.scriptEng.operands[4];
						ObjectSystem.scriptFrames[ObjectSystem.scriptFramesNo].top = ObjectSystem.scriptEng.operands[5];
						ObjectSystem.scriptFramesNo++;
					}
					break;
				case 57:
					b = 0;
					break;
				case 58:
					b = 0;
					GraphicsSystem.LoadPalette(ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4]);
					break;
				case 59:
					b = 0;
					GraphicsSystem.RotatePalette((byte)ObjectSystem.scriptEng.operands[0], (byte)ObjectSystem.scriptEng.operands[1], (byte)ObjectSystem.scriptEng.operands[2]);
					break;
				case 60:
					b = 0;
					GraphicsSystem.SetFade((byte)ObjectSystem.scriptEng.operands[0], (byte)ObjectSystem.scriptEng.operands[1], (byte)ObjectSystem.scriptEng.operands[2], (ushort)ObjectSystem.scriptEng.operands[3]);
					break;
				case 61:
					b = 0;
					GraphicsSystem.SetActivePalette((byte)ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
					break;
				case 62:
					GraphicsSystem.SetLimitedFade((byte)ObjectSystem.scriptEng.operands[0], (byte)ObjectSystem.scriptEng.operands[1], (byte)ObjectSystem.scriptEng.operands[2], (byte)ObjectSystem.scriptEng.operands[3], (ushort)ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[6]);
					break;
				case 63:
					b = 0;
					GraphicsSystem.CopyPalette((byte)ObjectSystem.scriptEng.operands[0], (byte)ObjectSystem.scriptEng.operands[1]);
					break;
				case 64:
					b = 0;
					GraphicsSystem.ClearScreen((byte)ObjectSystem.scriptEng.operands[0]);
					break;
				case 65:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[1])
					{
					case 0:
						GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						break;
					case 1:
						GraphicsSystem.DrawRotatedSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						break;
					case 2:
						GraphicsSystem.DrawRotoZoomSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						break;
					case 3:
						switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect)
						{
						case 0:
							GraphicsSystem.DrawSprite((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 1:
							GraphicsSystem.DrawBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 2:
							GraphicsSystem.DrawAlphaBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 3:
							GraphicsSystem.DrawAdditiveBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 4:
							GraphicsSystem.DrawSubtractiveBlendedSprite((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						}
						break;
					case 4:
					{
						byte inkEffect = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect;
						if (inkEffect != 2)
						{
							GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						}
						else
						{
							GraphicsSystem.DrawScaledTintMask(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						}
						break;
					}
					case 5:
						switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction)
						{
						case 0:
							GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 1:
							GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 2:
							GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 3:
							GraphicsSystem.DrawSpriteFlipped((ObjectSystem.scriptEng.operands[2] >> 16) - StageSystem.xScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, (ObjectSystem.scriptEng.operands[3] >> 16) - StageSystem.yScrollOffset - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						}
						break;
					}
					break;
				case 66:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[1])
					{
					case 0:
						GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						break;
					case 1:
						GraphicsSystem.DrawRotatedSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						break;
					case 2:
						GraphicsSystem.DrawRotoZoomSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].rotation, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						break;
					case 3:
						switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect)
						{
						case 0:
							GraphicsSystem.DrawSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 1:
							GraphicsSystem.DrawBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 2:
							GraphicsSystem.DrawAlphaBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 3:
							GraphicsSystem.DrawAdditiveBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 4:
							GraphicsSystem.DrawSubtractiveBlendedSprite(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].alpha, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						}
						break;
					case 4:
					{
						byte inkEffect2 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].inkEffect;
						if (inkEffect2 != 2)
						{
							GraphicsSystem.DrawScaledSprite(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						}
						else
						{
							GraphicsSystem.DrawScaledTintMask(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, -ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.objectEntityList[ObjectSystem.objectLoop].scale, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
						}
						break;
					}
					case 5:
						switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction)
						{
						case 0:
							GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 1:
							GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 2:
							GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						case 3:
							GraphicsSystem.DrawSpriteFlipped(ObjectSystem.scriptEng.operands[2] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xPivot, ObjectSystem.scriptEng.operands[3] - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize - ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].yPivot, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].xSize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].ySize, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].left, ObjectSystem.scriptFrames[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].frameListOffset + ObjectSystem.scriptEng.operands[0]].top, (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].direction, (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
							break;
						}
						break;
					}
					break;
				case 67:
					b = 0;
					ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile = AnimationSystem.AddAnimationFile(ObjectSystem.scriptText);
					break;
				case 68:
					b = 0;
					TextSystem.SetupTextMenu(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptEng.operands[1]);
					StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].numSelections = (byte)ObjectSystem.scriptEng.operands[2];
					StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].alignment = (byte)ObjectSystem.scriptEng.operands[3];
					break;
				case 69:
					b = 0;
					StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].entryHighlight[(int)StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].numRows] = (byte)ObjectSystem.scriptEng.operands[2];
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText);
					break;
				case 70:
					b = 0;
					TextSystem.EditTextMenuEntry(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[2]);
					StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].entryHighlight[ObjectSystem.scriptEng.operands[2]] = (byte)ObjectSystem.scriptEng.operands[3];
					break;
				case 71:
					b = 0;
					StageSystem.stageMode = 0;
					break;
				case 72:
					b = 0;
					GraphicsSystem.DrawRectangle(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[6], ObjectSystem.scriptEng.operands[7]);
					break;
				case 73:
					b = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].type = (byte)ObjectSystem.scriptEng.operands[1];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].propertyValue = (byte)ObjectSystem.scriptEng.operands[2];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].xPos = ObjectSystem.scriptEng.operands[3];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].yPos = ObjectSystem.scriptEng.operands[4];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].direction = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].frame = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].priority = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].rotation = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].state = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].drawOrder = 3;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].scale = 512;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].inkEffect = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[0] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[1] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[2] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[3] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[4] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[5] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[6] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[0]].value[7] = 0;
					break;
				case 74:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						ObjectSystem.scriptEng.operands[5] = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16;
						ObjectSystem.scriptEng.operands[6] = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16;
						ObjectSystem.BasicCollision(ObjectSystem.scriptEng.operands[1] + ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[2] + ObjectSystem.scriptEng.operands[6], ObjectSystem.scriptEng.operands[3] + ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[4] + ObjectSystem.scriptEng.operands[6]);
						break;
					case 1:
					case 2:
						ObjectSystem.BoxCollision((ObjectSystem.scriptEng.operands[1] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[2] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos, (ObjectSystem.scriptEng.operands[3] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[4] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos);
						break;
					case 3:
						ObjectSystem.PlatformCollision((ObjectSystem.scriptEng.operands[1] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[2] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos, (ObjectSystem.scriptEng.operands[3] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos, (ObjectSystem.scriptEng.operands[4] << 16) + ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos);
						break;
					}
					break;
				case 75:
					b = 0;
					if (ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].type > 0)
					{
						ObjectSystem.scriptEng.arrayPosition[2]++;
						if (ObjectSystem.scriptEng.arrayPosition[2] == 1184)
						{
							ObjectSystem.scriptEng.arrayPosition[2] = 1056;
						}
					}
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].type = (byte)ObjectSystem.scriptEng.operands[0];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].propertyValue = (byte)ObjectSystem.scriptEng.operands[1];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].xPos = ObjectSystem.scriptEng.operands[2];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].yPos = ObjectSystem.scriptEng.operands[3];
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].direction = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].frame = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].priority = 1;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].rotation = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].state = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].drawOrder = 3;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].scale = 512;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].inkEffect = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].alpha = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].animation = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].prevAnimation = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].animationSpeed = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].animationTimer = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[0] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[1] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[2] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[3] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[4] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[5] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[6] = 0;
					ObjectSystem.objectEntityList[ObjectSystem.scriptEng.arrayPosition[2]].value[7] = 0;
					break;
				case 76:
					b = 0;
					PlayerSystem.playerList[ObjectSystem.scriptEng.operands[0]].animationFile = ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[1]].type].animationFile;
					PlayerSystem.playerList[ObjectSystem.scriptEng.operands[0]].objectPtr = ObjectSystem.objectEntityList[ObjectSystem.scriptEng.operands[1]];
					PlayerSystem.playerList[ObjectSystem.scriptEng.operands[0]].objectNum = ObjectSystem.scriptEng.operands[1];
					break;
				case 77:
					b = 0;
					if (PlayerSystem.playerList[ObjectSystem.playerNum].tileCollisions == 1)
					{
						PlayerSystem.ProcessPlayerTileCollisions(PlayerSystem.playerList[ObjectSystem.playerNum]);
					}
					else
					{
						PlayerSystem.playerList[ObjectSystem.playerNum].xPos += PlayerSystem.playerList[ObjectSystem.playerNum].xVelocity;
						PlayerSystem.playerList[ObjectSystem.playerNum].yPos += PlayerSystem.playerList[ObjectSystem.playerNum].yVelocity;
					}
					break;
				case 78:
					b = 0;
					PlayerSystem.ProcessPlayerControl(PlayerSystem.playerList[ObjectSystem.playerNum]);
					break;
				case 79:
					AnimationSystem.ProcessObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop]);
					b = 0;
					break;
				case 80:
					b = 0;
					AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop], (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) - StageSystem.xScrollOffset, (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) - StageSystem.yScrollOffset);
					break;
				case 81:
					b = 0;
					if (PlayerSystem.playerList[ObjectSystem.playerNum].visible == 1)
					{
						if ((int)StageSystem.cameraEnabled == ObjectSystem.playerNum)
						{
							AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop], PlayerSystem.playerList[ObjectSystem.playerNum].screenXPos, PlayerSystem.playerList[ObjectSystem.playerNum].screenYPos);
						}
						else
						{
							AnimationSystem.DrawObjectAnimation(AnimationSystem.animationList[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].animationFile.aniListOffset + (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].animation], ObjectSystem.objectEntityList[ObjectSystem.objectLoop], (PlayerSystem.playerList[ObjectSystem.playerNum].xPos >> 16) - StageSystem.xScrollOffset, (PlayerSystem.playerList[ObjectSystem.playerNum].yPos >> 16) - StageSystem.yScrollOffset);
						}
					}
					break;
				case 82:
					b = 0;
					if (ObjectSystem.scriptEng.operands[2] > 1)
					{
						AudioPlayback.SetMusicTrack(ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[1], 1, (uint)ObjectSystem.scriptEng.operands[2]);
					}
					else
					{
						AudioPlayback.SetMusicTrack(ObjectSystem.scriptText, ObjectSystem.scriptEng.operands[1], (byte)ObjectSystem.scriptEng.operands[2], 0U);
					}
					break;
				case 83:
					b = 0;
					AudioPlayback.PlayMusic(ObjectSystem.scriptEng.operands[0]);
					break;
				case 84:
					b = 0;
					AudioPlayback.StopMusic();
					break;
				case 85:
					b = 0;
					AudioPlayback.PlaySfx(ObjectSystem.scriptEng.operands[0], (byte)ObjectSystem.scriptEng.operands[1]);
					break;
				case 86:
					b = 0;
					AudioPlayback.StopSfx(ObjectSystem.scriptEng.operands[0]);
					break;
				case 87:
					b = 0;
					AudioPlayback.SetSfxAttributes(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
					break;
				case 88:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						ObjectSystem.ObjectFloorCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 1:
						ObjectSystem.ObjectLWallCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 2:
						ObjectSystem.ObjectRWallCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 3:
						ObjectSystem.ObjectRoofCollision(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					}
					break;
				case 89:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						ObjectSystem.ObjectFloorGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 1:
						ObjectSystem.ObjectLWallGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 2:
						ObjectSystem.ObjectRWallGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 3:
						ObjectSystem.ObjectRoofGrip(ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					}
					break;
				case 90:
					b = 0;
					AudioPlayback.PauseSound();
					EngineCallbacks.PlayVideoFile(ObjectSystem.scriptText);
					AudioPlayback.ResumeSound();
					break;
				case 91:
					b = 0;
					break;
				case 92:
					b = 0;
					AudioPlayback.PlaySfx(ObjectSystem.scriptEng.operands[0] + AudioPlayback.numGlobalSFX, (byte)ObjectSystem.scriptEng.operands[1]);
					break;
				case 93:
					b = 0;
					AudioPlayback.StopSfx(ObjectSystem.scriptEng.operands[0] + AudioPlayback.numGlobalSFX);
					break;
				case 94:
					ObjectSystem.scriptEng.operands[0] = ~ObjectSystem.scriptEng.operands[0];
					break;
				case 95:
					b = 0;
					Scene3D.TransformVertexBuffer();
					Scene3D.Sort3DDrawList();
					Scene3D.Draw3DScene((int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum);
					break;
				case 96:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.SetIdentityMatrix(ref Scene3D.matWorld);
						break;
					case 1:
						Scene3D.SetIdentityMatrix(ref Scene3D.matView);
						break;
					case 2:
						Scene3D.SetIdentityMatrix(ref Scene3D.matTemp);
						break;
					}
					break;
				case 97:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						switch (ObjectSystem.scriptEng.operands[1])
						{
						case 0:
							Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matWorld);
							break;
						case 1:
							Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matView);
							break;
						case 2:
							Scene3D.MatrixMultiply(ref Scene3D.matWorld, ref Scene3D.matTemp);
							break;
						}
						break;
					case 1:
						switch (ObjectSystem.scriptEng.operands[1])
						{
						case 0:
							Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matWorld);
							break;
						case 1:
							Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matView);
							break;
						case 2:
							Scene3D.MatrixMultiply(ref Scene3D.matView, ref Scene3D.matTemp);
							break;
						}
						break;
					case 2:
						switch (ObjectSystem.scriptEng.operands[1])
						{
						case 0:
							Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matWorld);
							break;
						case 1:
							Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matView);
							break;
						case 2:
							Scene3D.MatrixMultiply(ref Scene3D.matTemp, ref Scene3D.matTemp);
							break;
						}
						break;
					}
					break;
				case 98:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.MatrixTranslateXYZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 1:
						Scene3D.MatrixTranslateXYZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 2:
						Scene3D.MatrixTranslateXYZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					}
					break;
				case 99:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.MatrixScaleXYZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 1:
						Scene3D.MatrixScaleXYZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 2:
						Scene3D.MatrixScaleXYZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					}
					break;
				case 100:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.MatrixRotateX(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1]);
						break;
					case 1:
						Scene3D.MatrixRotateX(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1]);
						break;
					case 2:
						Scene3D.MatrixRotateX(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1]);
						break;
					}
					break;
				case 101:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.MatrixRotateY(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1]);
						break;
					case 1:
						Scene3D.MatrixRotateY(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1]);
						break;
					case 2:
						Scene3D.MatrixRotateY(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1]);
						break;
					}
					break;
				case 102:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.MatrixRotateZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1]);
						break;
					case 1:
						Scene3D.MatrixRotateZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1]);
						break;
					case 2:
						Scene3D.MatrixRotateZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1]);
						break;
					}
					break;
				case 103:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.MatrixRotateXYZ(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 1:
						Scene3D.MatrixRotateXYZ(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					case 2:
						Scene3D.MatrixRotateXYZ(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3]);
						break;
					}
					break;
				case 104:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						Scene3D.TransformVertices(ref Scene3D.matWorld, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
						break;
					case 1:
						Scene3D.TransformVertices(ref Scene3D.matView, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
						break;
					case 2:
						Scene3D.TransformVertices(ref Scene3D.matTemp, ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2]);
						break;
					}
					break;
				case 105:
					b = 0;
					ObjectSystem.functionStack[ObjectSystem.functionStackPos] = scriptCodePtr;
					ObjectSystem.functionStackPos++;
					ObjectSystem.functionStack[ObjectSystem.functionStackPos] = jumpTablePtr;
					ObjectSystem.functionStackPos++;
					ObjectSystem.functionStack[ObjectSystem.functionStackPos] = num2;
					ObjectSystem.functionStackPos++;
					scriptCodePtr = ObjectSystem.functionScriptList[ObjectSystem.scriptEng.operands[0]].mainScript;
					num2 = scriptCodePtr;
					jumpTablePtr = ObjectSystem.functionScriptList[ObjectSystem.scriptEng.operands[0]].mainJumpTable;
					break;
				case 106:
					b = 0;
					ObjectSystem.functionStackPos--;
					num2 = ObjectSystem.functionStack[ObjectSystem.functionStackPos];
					ObjectSystem.functionStackPos--;
					jumpTablePtr = ObjectSystem.functionStack[ObjectSystem.functionStackPos];
					ObjectSystem.functionStackPos--;
					scriptCodePtr = ObjectSystem.functionStack[ObjectSystem.functionStackPos];
					break;
				case 107:
					b = 0;
					StageSystem.SetLayerDeformation(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5]);
					break;
				case 108:
					b = 0;
					ObjectSystem.scriptEng.checkResult = -1;
					while ((int)b < StageSystem.gKeyDown.touches)
					{
						if (StageSystem.gKeyDown.touchDown[(int)b] == 1 && StageSystem.gKeyDown.touchX[(int)b] > ObjectSystem.scriptEng.operands[0] && StageSystem.gKeyDown.touchX[(int)b] < ObjectSystem.scriptEng.operands[2] && StageSystem.gKeyDown.touchY[(int)b] > ObjectSystem.scriptEng.operands[1] && StageSystem.gKeyDown.touchY[(int)b] < ObjectSystem.scriptEng.operands[3])
						{
							ObjectSystem.scriptEng.checkResult = (int)b;
						}
						b += 1;
					}
					b = 0;
					break;
				case 109:
					if (ObjectSystem.scriptEng.operands[2] > -1 && ObjectSystem.scriptEng.operands[3] > -1)
					{
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.stageLayouts[ObjectSystem.scriptEng.operands[1]].tileMap[ObjectSystem.scriptEng.operands[2] + (ObjectSystem.scriptEng.operands[3] << 8)];
					}
					else
					{
						ObjectSystem.scriptEng.operands[0] = 0;
					}
					break;
				case 110:
					if (ObjectSystem.scriptEng.operands[2] > -1 && ObjectSystem.scriptEng.operands[3] > -1)
					{
						StageSystem.stageLayouts[ObjectSystem.scriptEng.operands[1]].tileMap[ObjectSystem.scriptEng.operands[2] + (ObjectSystem.scriptEng.operands[3] << 8)] = (ushort)ObjectSystem.scriptEng.operands[0];
					}
					break;
				case 111:
					ObjectSystem.scriptEng.operands[0] = (ObjectSystem.scriptEng.operands[1] & 1 << ObjectSystem.scriptEng.operands[2]) >> ObjectSystem.scriptEng.operands[2];
					break;
				case 112:
					if (ObjectSystem.scriptEng.operands[2] > 0)
					{
						ObjectSystem.scriptEng.operands[0] |= 1 << ObjectSystem.scriptEng.operands[1];
					}
					else
					{
						ObjectSystem.scriptEng.operands[0] &= ~(1 << ObjectSystem.scriptEng.operands[1]);
					}
					break;
				case 113:
					b = 0;
					AudioPlayback.PauseSound();
					break;
				case 114:
					b = 0;
					AudioPlayback.ResumeSound();
					break;
				case 115:
					b = 0;
					ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].listSize = 0;
					break;
				case 116:
					b = 0;
					ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].entityRef[ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].listSize] = ObjectSystem.scriptEng.operands[1];
					ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[0]].listSize++;
					break;
				case 117:
					ObjectSystem.scriptEng.operands[0] = ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[1]].entityRef[ObjectSystem.scriptEng.operands[2]];
					break;
				case 118:
					b = 0;
					ObjectSystem.objectDrawOrderList[ObjectSystem.scriptEng.operands[1]].entityRef[ObjectSystem.scriptEng.operands[2]] = ObjectSystem.scriptEng.operands[0];
					break;
				case 119:
					ObjectSystem.scriptEng.operands[4] = ObjectSystem.scriptEng.operands[1] >> 7;
					ObjectSystem.scriptEng.operands[5] = ObjectSystem.scriptEng.operands[2] >> 7;
					if (ObjectSystem.scriptEng.operands[4] > -1 && ObjectSystem.scriptEng.operands[5] > -1)
					{
						ObjectSystem.scriptEng.operands[6] = (int)StageSystem.stageLayouts[0].tileMap[ObjectSystem.scriptEng.operands[4] + (ObjectSystem.scriptEng.operands[5] << 8)] << 6;
					}
					else
					{
						ObjectSystem.scriptEng.operands[6] = 0;
					}
					ObjectSystem.scriptEng.operands[6] += ((ObjectSystem.scriptEng.operands[1] & 127) >> 4) + ((ObjectSystem.scriptEng.operands[2] & 127) >> 4 << 3);
					switch (ObjectSystem.scriptEng.operands[3])
					{
					case 0:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]];
						break;
					case 1:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tile128x128.direction[ObjectSystem.scriptEng.operands[6]];
						break;
					case 2:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tile128x128.visualPlane[ObjectSystem.scriptEng.operands[6]];
						break;
					case 3:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tile128x128.collisionFlag[0, ObjectSystem.scriptEng.operands[6]];
						break;
					case 4:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tile128x128.collisionFlag[1, ObjectSystem.scriptEng.operands[6]];
						break;
					case 5:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tileCollisions[0].flags[(int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
						break;
					case 6:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tileCollisions[0].angle[(int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
						break;
					case 7:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tileCollisions[1].flags[(int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
						break;
					case 8:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.tileCollisions[1].angle[(int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]];
						break;
					}
					break;
				case 120:
					b = 0;
					GraphicsSystem.Copy16x16Tile(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1]);
					break;
				case 121:
					ObjectSystem.scriptEng.operands[4] = ObjectSystem.scriptEng.operands[1] >> 7;
					ObjectSystem.scriptEng.operands[5] = ObjectSystem.scriptEng.operands[2] >> 7;
					if (ObjectSystem.scriptEng.operands[4] > -1 && ObjectSystem.scriptEng.operands[5] > -1)
					{
						ObjectSystem.scriptEng.operands[6] = (int)StageSystem.stageLayouts[0].tileMap[ObjectSystem.scriptEng.operands[4] + (ObjectSystem.scriptEng.operands[5] << 8)] << 6;
					}
					else
					{
						ObjectSystem.scriptEng.operands[6] = 0;
					}
					ObjectSystem.scriptEng.operands[6] += ((ObjectSystem.scriptEng.operands[1] & 127) >> 4) + ((ObjectSystem.scriptEng.operands[2] & 127) >> 4 << 3);
					switch (ObjectSystem.scriptEng.operands[3])
					{
					case 0:
						StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]] = (ushort)ObjectSystem.scriptEng.operands[0];
						StageSystem.tile128x128.gfxDataPos[ObjectSystem.scriptEng.operands[6]] = (int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]] << 2;
						break;
					case 1:
						StageSystem.tile128x128.direction[ObjectSystem.scriptEng.operands[6]] = (byte)ObjectSystem.scriptEng.operands[0];
						break;
					case 2:
						StageSystem.tile128x128.visualPlane[ObjectSystem.scriptEng.operands[6]] = (byte)ObjectSystem.scriptEng.operands[0];
						break;
					case 3:
						StageSystem.tile128x128.collisionFlag[0, ObjectSystem.scriptEng.operands[6]] = (byte)ObjectSystem.scriptEng.operands[0];
						break;
					case 4:
						StageSystem.tile128x128.collisionFlag[1, ObjectSystem.scriptEng.operands[6]] = (byte)ObjectSystem.scriptEng.operands[0];
						break;
					case 5:
						StageSystem.tileCollisions[0].flags[(int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]] = (byte)ObjectSystem.scriptEng.operands[0];
						break;
					case 6:
						StageSystem.tileCollisions[0].angle[(int)StageSystem.tile128x128.tile16x16[ObjectSystem.scriptEng.operands[6]]] = (uint)((byte)ObjectSystem.scriptEng.operands[0]);
						break;
					}
					break;
				case 122:
					ObjectSystem.scriptEng.operands[0] = -1;
					ObjectSystem.scriptEng.sRegister = 0;
					while (ObjectSystem.scriptEng.operands[0] == -1)
					{
						if (FileIO.StringComp(ref ObjectSystem.scriptText, ref AnimationSystem.animationList[PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.aniListOffset + ObjectSystem.scriptEng.sRegister].name))
						{
							ObjectSystem.scriptEng.operands[0] = ObjectSystem.scriptEng.sRegister;
						}
						else
						{
							ObjectSystem.scriptEng.sRegister++;
							if (ObjectSystem.scriptEng.sRegister == PlayerSystem.playerList[ObjectSystem.playerNum].animationFile.numAnimations)
							{
								ObjectSystem.scriptEng.operands[0] = 0;
							}
						}
					}
					break;
				case 123:
					b = 0;
					ObjectSystem.scriptEng.checkResult = (int)FileIO.ReadSaveRAMData();
					break;
				case 124:
					b = 0;
					ObjectSystem.scriptEng.checkResult = (int)FileIO.WriteSaveRAMData();
					break;
				case 125:
					b = 0;
					TextSystem.LoadFontFile(ObjectSystem.scriptText);
					break;
				case 126:
					b = 0;
					if (ObjectSystem.scriptEng.operands[2] == 0)
					{
						TextSystem.LoadTextFile(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText, 0);
					}
					else
					{
						TextSystem.LoadTextFile(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptText, 1);
					}
					break;
				case 127:
					b = 0;
					TextSystem.textMenuSurfaceNo = (int)ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum;
					TextSystem.DrawBitmapText(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], ObjectSystem.scriptEng.operands[1], ObjectSystem.scriptEng.operands[2], ObjectSystem.scriptEng.operands[3], ObjectSystem.scriptEng.operands[4], ObjectSystem.scriptEng.operands[5], ObjectSystem.scriptEng.operands[6]);
					break;
				case 128:
					switch (ObjectSystem.scriptEng.operands[2])
					{
					case 0:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].textData[StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].entryStart[ObjectSystem.scriptEng.operands[3]] + ObjectSystem.scriptEng.operands[4]];
						break;
					case 1:
						ObjectSystem.scriptEng.operands[0] = StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].entrySize[ObjectSystem.scriptEng.operands[3]];
						break;
					case 2:
						ObjectSystem.scriptEng.operands[0] = (int)StageSystem.gameMenu[ObjectSystem.scriptEng.operands[1]].numRows;
						break;
					}
					break;
				case 129:
					b = 0;
					StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].entryHighlight[(int)StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]].numRows] = (byte)ObjectSystem.scriptEng.operands[1];
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[ObjectSystem.scriptEng.operands[0]], GlobalAppDefinitions.gameVersion);
					break;
				case 130:
					b = 0;
					EngineCallbacks.OnlineSetAchievement(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1]);
					break;
				case 131:
					b = 0;
					EngineCallbacks.OnlineSetLeaderboard(ObjectSystem.scriptEng.operands[0], ObjectSystem.scriptEng.operands[1]);
					break;
				case 132:
					b = 0;
					switch (ObjectSystem.scriptEng.operands[0])
					{
					case 0:
						EngineCallbacks.OnlineLoadAchievementsMenu();
						break;
					case 1:
						EngineCallbacks.OnlineLoadLeaderboardsMenu();
						break;
					}
					break;
				case 133:
					b = 0;
					EngineCallbacks.RetroEngineCallback(ObjectSystem.scriptEng.operands[0]);
					break;
				case 134:
					b = 0;
					break;
				}
				if (b > 0)
				{
					scriptCodePtr -= num4;
				}
				for (int i = 0; i < (int)b; i++)
				{
					switch (ObjectSystem.scriptData[scriptCodePtr])
					{
					case 1:
						scriptCodePtr++;
						switch (ObjectSystem.scriptData[scriptCodePtr])
						{
						case 0:
							num = ObjectSystem.objectLoop;
							break;
						case 1:
							scriptCodePtr++;
							if (ObjectSystem.scriptData[scriptCodePtr] == 1)
							{
								scriptCodePtr++;
								num = ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
							}
							else
							{
								scriptCodePtr++;
								num = ObjectSystem.scriptData[scriptCodePtr];
							}
							break;
						case 2:
							scriptCodePtr++;
							if (ObjectSystem.scriptData[scriptCodePtr] == 1)
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop + ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
							}
							else
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop + ObjectSystem.scriptData[scriptCodePtr];
							}
							break;
						case 3:
							scriptCodePtr++;
							if (ObjectSystem.scriptData[scriptCodePtr] == 1)
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop - ObjectSystem.scriptEng.arrayPosition[ObjectSystem.scriptData[scriptCodePtr]];
							}
							else
							{
								scriptCodePtr++;
								num = ObjectSystem.objectLoop - ObjectSystem.scriptData[scriptCodePtr];
							}
							break;
						}
						scriptCodePtr++;
						switch (ObjectSystem.scriptData[scriptCodePtr])
						{
						case 0:
							ObjectSystem.scriptEng.tempValue[0] = ObjectSystem.scriptEng.operands[i];
							break;
						case 1:
							ObjectSystem.scriptEng.tempValue[1] = ObjectSystem.scriptEng.operands[i];
							break;
						case 2:
							ObjectSystem.scriptEng.tempValue[2] = ObjectSystem.scriptEng.operands[i];
							break;
						case 3:
							ObjectSystem.scriptEng.tempValue[3] = ObjectSystem.scriptEng.operands[i];
							break;
						case 4:
							ObjectSystem.scriptEng.tempValue[4] = ObjectSystem.scriptEng.operands[i];
							break;
						case 5:
							ObjectSystem.scriptEng.tempValue[5] = ObjectSystem.scriptEng.operands[i];
							break;
						case 6:
							ObjectSystem.scriptEng.tempValue[6] = ObjectSystem.scriptEng.operands[i];
							break;
						case 7:
							ObjectSystem.scriptEng.tempValue[7] = ObjectSystem.scriptEng.operands[i];
							break;
						case 8:
							ObjectSystem.scriptEng.checkResult = ObjectSystem.scriptEng.operands[i];
							break;
						case 9:
							ObjectSystem.scriptEng.arrayPosition[0] = ObjectSystem.scriptEng.operands[i];
							break;
						case 10:
							ObjectSystem.scriptEng.arrayPosition[1] = ObjectSystem.scriptEng.operands[i];
							break;
						case 11:
							ObjectSystem.globalVariables[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 13:
							ObjectSystem.objectEntityList[num].type = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 14:
							ObjectSystem.objectEntityList[num].propertyValue = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 15:
							ObjectSystem.objectEntityList[num].xPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 16:
							ObjectSystem.objectEntityList[num].yPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 17:
							ObjectSystem.objectEntityList[num].xPos = ObjectSystem.scriptEng.operands[i] << 16;
							break;
						case 18:
							ObjectSystem.objectEntityList[num].yPos = ObjectSystem.scriptEng.operands[i] << 16;
							break;
						case 19:
							ObjectSystem.objectEntityList[num].state = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 20:
							ObjectSystem.objectEntityList[num].rotation = ObjectSystem.scriptEng.operands[i];
							break;
						case 21:
							ObjectSystem.objectEntityList[num].scale = ObjectSystem.scriptEng.operands[i];
							break;
						case 22:
							ObjectSystem.objectEntityList[num].priority = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 23:
							ObjectSystem.objectEntityList[num].drawOrder = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 24:
							ObjectSystem.objectEntityList[num].direction = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 25:
							ObjectSystem.objectEntityList[num].inkEffect = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 26:
							ObjectSystem.objectEntityList[num].alpha = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 27:
							ObjectSystem.objectEntityList[num].frame = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 28:
							ObjectSystem.objectEntityList[num].animation = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 29:
							ObjectSystem.objectEntityList[num].prevAnimation = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 30:
							ObjectSystem.objectEntityList[num].animationSpeed = ObjectSystem.scriptEng.operands[i];
							break;
						case 31:
							ObjectSystem.objectEntityList[num].animationTimer = ObjectSystem.scriptEng.operands[i];
							break;
						case 32:
							ObjectSystem.objectEntityList[num].value[0] = ObjectSystem.scriptEng.operands[i];
							break;
						case 33:
							ObjectSystem.objectEntityList[num].value[1] = ObjectSystem.scriptEng.operands[i];
							break;
						case 34:
							ObjectSystem.objectEntityList[num].value[2] = ObjectSystem.scriptEng.operands[i];
							break;
						case 35:
							ObjectSystem.objectEntityList[num].value[3] = ObjectSystem.scriptEng.operands[i];
							break;
						case 36:
							ObjectSystem.objectEntityList[num].value[4] = ObjectSystem.scriptEng.operands[i];
							break;
						case 37:
							ObjectSystem.objectEntityList[num].value[5] = ObjectSystem.scriptEng.operands[i];
							break;
						case 38:
							ObjectSystem.objectEntityList[num].value[6] = ObjectSystem.scriptEng.operands[i];
							break;
						case 39:
							ObjectSystem.objectEntityList[num].value[7] = ObjectSystem.scriptEng.operands[i];
							break;
						case 41:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.state = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 42:
							PlayerSystem.playerList[ObjectSystem.playerNum].controlMode = (sbyte)ObjectSystem.scriptEng.operands[i];
							break;
						case 43:
							PlayerSystem.playerList[ObjectSystem.playerNum].controlLock = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 44:
							PlayerSystem.playerList[ObjectSystem.playerNum].collisionMode = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 45:
							PlayerSystem.playerList[ObjectSystem.playerNum].collisionPlane = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 46:
							PlayerSystem.playerList[ObjectSystem.playerNum].xPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 47:
							PlayerSystem.playerList[ObjectSystem.playerNum].yPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 48:
							PlayerSystem.playerList[ObjectSystem.playerNum].xPos = ObjectSystem.scriptEng.operands[i] << 16;
							break;
						case 49:
							PlayerSystem.playerList[ObjectSystem.playerNum].yPos = ObjectSystem.scriptEng.operands[i] << 16;
							break;
						case 50:
							PlayerSystem.playerList[ObjectSystem.playerNum].screenXPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 51:
							PlayerSystem.playerList[ObjectSystem.playerNum].screenYPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 52:
							PlayerSystem.playerList[ObjectSystem.playerNum].speed = ObjectSystem.scriptEng.operands[i];
							break;
						case 53:
							PlayerSystem.playerList[ObjectSystem.playerNum].xVelocity = ObjectSystem.scriptEng.operands[i];
							break;
						case 54:
							PlayerSystem.playerList[ObjectSystem.playerNum].yVelocity = ObjectSystem.scriptEng.operands[i];
							break;
						case 55:
							PlayerSystem.playerList[ObjectSystem.playerNum].gravity = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 56:
							PlayerSystem.playerList[ObjectSystem.playerNum].angle = ObjectSystem.scriptEng.operands[i];
							break;
						case 57:
							PlayerSystem.playerList[ObjectSystem.playerNum].skidding = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 58:
							PlayerSystem.playerList[ObjectSystem.playerNum].pushing = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 59:
							PlayerSystem.playerList[ObjectSystem.playerNum].trackScroll = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 60:
							PlayerSystem.playerList[ObjectSystem.playerNum].up = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 61:
							PlayerSystem.playerList[ObjectSystem.playerNum].down = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 62:
							PlayerSystem.playerList[ObjectSystem.playerNum].left = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 63:
							PlayerSystem.playerList[ObjectSystem.playerNum].right = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 64:
							PlayerSystem.playerList[ObjectSystem.playerNum].jumpPress = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 65:
							PlayerSystem.playerList[ObjectSystem.playerNum].jumpHold = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 66:
							PlayerSystem.playerList[ObjectSystem.playerNum].followPlayer1 = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 67:
							PlayerSystem.playerList[ObjectSystem.playerNum].lookPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 68:
							PlayerSystem.playerList[ObjectSystem.playerNum].water = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 69:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.topSpeed = ObjectSystem.scriptEng.operands[i];
							break;
						case 70:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.acceleration = ObjectSystem.scriptEng.operands[i];
							break;
						case 71:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.deceleration = ObjectSystem.scriptEng.operands[i];
							break;
						case 72:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airAcceleration = ObjectSystem.scriptEng.operands[i];
							break;
						case 73:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.airDeceleration = ObjectSystem.scriptEng.operands[i];
							break;
						case 74:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.gravity = ObjectSystem.scriptEng.operands[i];
							break;
						case 75:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpStrength = ObjectSystem.scriptEng.operands[i];
							break;
						case 76:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.jumpCap = ObjectSystem.scriptEng.operands[i];
							break;
						case 77:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingAcceleration = ObjectSystem.scriptEng.operands[i];
							break;
						case 78:
							PlayerSystem.playerList[ObjectSystem.playerNum].movementStats.rollingDeceleration = ObjectSystem.scriptEng.operands[i];
							break;
						case 84:
							PlayerSystem.playerList[ObjectSystem.playerNum].flailing[num] = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 85:
							PlayerSystem.playerList[ObjectSystem.playerNum].timer = ObjectSystem.scriptEng.operands[i];
							break;
						case 86:
							PlayerSystem.playerList[ObjectSystem.playerNum].tileCollisions = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 87:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 88:
							PlayerSystem.playerList[ObjectSystem.playerNum].visible = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 89:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.rotation = ObjectSystem.scriptEng.operands[i];
							break;
						case 90:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.scale = ObjectSystem.scriptEng.operands[i];
							break;
						case 91:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.priority = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 92:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.drawOrder = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 93:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.direction = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 94:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.inkEffect = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 95:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.alpha = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 96:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.frame = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 97:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animation = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 98:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.prevAnimation = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 99:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationSpeed = ObjectSystem.scriptEng.operands[i];
							break;
						case 100:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.animationTimer = ObjectSystem.scriptEng.operands[i];
							break;
						case 101:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[0] = ObjectSystem.scriptEng.operands[i];
							break;
						case 102:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[1] = ObjectSystem.scriptEng.operands[i];
							break;
						case 103:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[2] = ObjectSystem.scriptEng.operands[i];
							break;
						case 104:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[3] = ObjectSystem.scriptEng.operands[i];
							break;
						case 105:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[4] = ObjectSystem.scriptEng.operands[i];
							break;
						case 106:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[5] = ObjectSystem.scriptEng.operands[i];
							break;
						case 107:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[6] = ObjectSystem.scriptEng.operands[i];
							break;
						case 108:
							PlayerSystem.playerList[ObjectSystem.playerNum].objectPtr.value[7] = ObjectSystem.scriptEng.operands[i];
							break;
						case 109:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[0] = ObjectSystem.scriptEng.operands[i];
							break;
						case 110:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[1] = ObjectSystem.scriptEng.operands[i];
							break;
						case 111:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[2] = ObjectSystem.scriptEng.operands[i];
							break;
						case 112:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[3] = ObjectSystem.scriptEng.operands[i];
							break;
						case 113:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[4] = ObjectSystem.scriptEng.operands[i];
							break;
						case 114:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[5] = ObjectSystem.scriptEng.operands[i];
							break;
						case 115:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[6] = ObjectSystem.scriptEng.operands[i];
							break;
						case 116:
							PlayerSystem.playerList[ObjectSystem.playerNum].value[7] = ObjectSystem.scriptEng.operands[i];
							break;
						case 118:
							StageSystem.stageMode = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 119:
							FileIO.activeStageList = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 120:
							StageSystem.stageListPosition = ObjectSystem.scriptEng.operands[i];
							break;
						case 121:
							StageSystem.timeEnabled = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 122:
							StageSystem.milliSeconds = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 123:
							StageSystem.seconds = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 124:
							StageSystem.minutes = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 125:
							FileIO.actNumber = ObjectSystem.scriptEng.operands[i];
							break;
						case 126:
							StageSystem.pauseEnabled = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 128:
							StageSystem.newXBoundary1 = ObjectSystem.scriptEng.operands[i];
							break;
						case 129:
							StageSystem.newXBoundary2 = ObjectSystem.scriptEng.operands[i];
							break;
						case 130:
							StageSystem.newYBoundary1 = ObjectSystem.scriptEng.operands[i];
							break;
						case 131:
							StageSystem.newYBoundary2 = ObjectSystem.scriptEng.operands[i];
							break;
						case 132:
							if (StageSystem.xBoundary1 != ObjectSystem.scriptEng.operands[i])
							{
								StageSystem.xBoundary1 = ObjectSystem.scriptEng.operands[i];
								StageSystem.newXBoundary1 = ObjectSystem.scriptEng.operands[i];
							}
							break;
						case 133:
							if (StageSystem.xBoundary2 != ObjectSystem.scriptEng.operands[i])
							{
								StageSystem.xBoundary2 = ObjectSystem.scriptEng.operands[i];
								StageSystem.newXBoundary2 = ObjectSystem.scriptEng.operands[i];
							}
							break;
						case 134:
							if (StageSystem.yBoundary1 != ObjectSystem.scriptEng.operands[i])
							{
								StageSystem.yBoundary1 = ObjectSystem.scriptEng.operands[i];
								StageSystem.newYBoundary1 = ObjectSystem.scriptEng.operands[i];
							}
							break;
						case 135:
							if (StageSystem.yBoundary2 != ObjectSystem.scriptEng.operands[i])
							{
								StageSystem.yBoundary2 = ObjectSystem.scriptEng.operands[i];
								StageSystem.newYBoundary2 = ObjectSystem.scriptEng.operands[i];
							}
							break;
						case 136:
							StageSystem.bgDeformationData0[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 137:
							StageSystem.bgDeformationData1[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 138:
							StageSystem.bgDeformationData2[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 139:
							StageSystem.bgDeformationData3[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 140:
							StageSystem.waterLevel = ObjectSystem.scriptEng.operands[i];
							break;
						case 141:
							StageSystem.activeTileLayers[num] = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 142:
							StageSystem.tLayerMidPoint = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 143:
							PlayerSystem.playerMenuNum = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 144:
							ObjectSystem.playerNum = ObjectSystem.scriptEng.operands[i];
							break;
						case 145:
							StageSystem.cameraEnabled = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 146:
							StageSystem.cameraTarget = (sbyte)ObjectSystem.scriptEng.operands[i];
							break;
						case 147:
							StageSystem.cameraStyle = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 148:
							ObjectSystem.objectDrawOrderList[num].listSize = ObjectSystem.scriptEng.operands[i];
							break;
						case 153:
							StageSystem.xScrollOffset = ObjectSystem.scriptEng.operands[i];
							StageSystem.xScrollA = StageSystem.xScrollOffset;
							StageSystem.xScrollB = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE;
							break;
						case 154:
							StageSystem.yScrollOffset = ObjectSystem.scriptEng.operands[i];
							StageSystem.yScrollA = StageSystem.yScrollOffset;
							StageSystem.yScrollB = StageSystem.yScrollOffset + 240;
							break;
						case 155:
							StageSystem.screenShakeX = ObjectSystem.scriptEng.operands[i];
							break;
						case 156:
							StageSystem.screenShakeY = ObjectSystem.scriptEng.operands[i];
							break;
						case 157:
							StageSystem.cameraAdjustY = ObjectSystem.scriptEng.operands[i];
							break;
						case 161:
							AudioPlayback.SetMusicVolume(ObjectSystem.scriptEng.operands[i]);
							break;
						case 163:
							StageSystem.gKeyDown.up = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 164:
							StageSystem.gKeyDown.down = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 165:
							StageSystem.gKeyDown.left = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 166:
							StageSystem.gKeyDown.right = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 167:
							StageSystem.gKeyDown.buttonA = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 168:
							StageSystem.gKeyDown.buttonB = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 169:
							StageSystem.gKeyDown.buttonC = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 170:
							StageSystem.gKeyDown.start = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 171:
							StageSystem.gKeyPress.up = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 172:
							StageSystem.gKeyPress.down = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 173:
							StageSystem.gKeyPress.left = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 174:
							StageSystem.gKeyPress.right = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 175:
							StageSystem.gKeyPress.buttonA = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 176:
							StageSystem.gKeyPress.buttonB = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 177:
							StageSystem.gKeyPress.buttonC = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 178:
							StageSystem.gKeyPress.start = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 179:
							StageSystem.gameMenu[0].selection1 = ObjectSystem.scriptEng.operands[i];
							break;
						case 180:
							StageSystem.gameMenu[1].selection1 = ObjectSystem.scriptEng.operands[i];
							break;
						case 181:
							StageSystem.stageLayouts[num].xSize = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 182:
							StageSystem.stageLayouts[num].ySize = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 183:
							StageSystem.stageLayouts[num].type = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 184:
							StageSystem.stageLayouts[num].angle = ObjectSystem.scriptEng.operands[i];
							if (StageSystem.stageLayouts[num].angle < 0)
							{
								StageSystem.stageLayouts[num].angle += 512;
							}
							StageSystem.stageLayouts[num].angle &= 511;
							break;
						case 185:
							StageSystem.stageLayouts[num].xPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 186:
							StageSystem.stageLayouts[num].yPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 187:
							StageSystem.stageLayouts[num].zPos = ObjectSystem.scriptEng.operands[i];
							break;
						case 188:
							StageSystem.stageLayouts[num].parallaxFactor = ObjectSystem.scriptEng.operands[i];
							break;
						case 189:
							StageSystem.stageLayouts[num].scrollSpeed = ObjectSystem.scriptEng.operands[i];
							break;
						case 190:
							StageSystem.stageLayouts[num].scrollPosition = ObjectSystem.scriptEng.operands[i];
							break;
						case 191:
							StageSystem.stageLayouts[num].deformationPos = ObjectSystem.scriptEng.operands[i];
							StageSystem.stageLayouts[num].deformationPos &= 255;
							break;
						case 192:
							StageSystem.stageLayouts[num].deformationPosW = ObjectSystem.scriptEng.operands[i];
							StageSystem.stageLayouts[num].deformationPosW &= 255;
							break;
						case 193:
							StageSystem.hParallax.parallaxFactor[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 194:
							StageSystem.hParallax.scrollSpeed[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 195:
							StageSystem.hParallax.scrollPosition[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 196:
							StageSystem.vParallax.parallaxFactor[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 197:
							StageSystem.vParallax.scrollSpeed[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 198:
							StageSystem.vParallax.scrollPosition[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 199:
							Scene3D.numVertices = ObjectSystem.scriptEng.operands[i];
							break;
						case 200:
							Scene3D.numFaces = ObjectSystem.scriptEng.operands[i];
							break;
						case 201:
							Scene3D.vertexBuffer[num].x = ObjectSystem.scriptEng.operands[i];
							break;
						case 202:
							Scene3D.vertexBuffer[num].y = ObjectSystem.scriptEng.operands[i];
							break;
						case 203:
							Scene3D.vertexBuffer[num].z = ObjectSystem.scriptEng.operands[i];
							break;
						case 204:
							Scene3D.vertexBuffer[num].u = ObjectSystem.scriptEng.operands[i];
							break;
						case 205:
							Scene3D.vertexBuffer[num].v = ObjectSystem.scriptEng.operands[i];
							break;
						case 206:
							Scene3D.indexBuffer[num].a = ObjectSystem.scriptEng.operands[i];
							break;
						case 207:
							Scene3D.indexBuffer[num].b = ObjectSystem.scriptEng.operands[i];
							break;
						case 208:
							Scene3D.indexBuffer[num].c = ObjectSystem.scriptEng.operands[i];
							break;
						case 209:
							Scene3D.indexBuffer[num].d = ObjectSystem.scriptEng.operands[i];
							break;
						case 210:
							Scene3D.indexBuffer[num].flag = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 211:
							Scene3D.indexBuffer[num].color = ObjectSystem.scriptEng.operands[i];
							break;
						case 212:
							Scene3D.projectionX = ObjectSystem.scriptEng.operands[i];
							break;
						case 213:
							Scene3D.projectionY = ObjectSystem.scriptEng.operands[i];
							break;
						case 214:
							GlobalAppDefinitions.gameMode = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 215:
							StageSystem.debugMode = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 217:
							FileIO.saveRAM[num] = ObjectSystem.scriptEng.operands[i];
							break;
						case 218:
							GlobalAppDefinitions.gameLanguage = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 219:
							ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].surfaceNum = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 221:
							GlobalAppDefinitions.frameSkipTimer = ObjectSystem.scriptEng.operands[i];
							break;
						case 222:
							GlobalAppDefinitions.frameSkipSetting = ObjectSystem.scriptEng.operands[i];
							break;
						case 223:
							GlobalAppDefinitions.gameSFXVolume = ObjectSystem.scriptEng.operands[i];
							AudioPlayback.SetGameVolumes(GlobalAppDefinitions.gameBGMVolume, GlobalAppDefinitions.gameSFXVolume);
							break;
						case 224:
							GlobalAppDefinitions.gameBGMVolume = ObjectSystem.scriptEng.operands[i];
							AudioPlayback.SetGameVolumes(GlobalAppDefinitions.gameBGMVolume, GlobalAppDefinitions.gameSFXVolume);
							break;
						case 227:
							StageSystem.gKeyPress.start = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						case 228:
							GlobalAppDefinitions.gameHapticsEnabled = (byte)ObjectSystem.scriptEng.operands[i];
							break;
						}
						scriptCodePtr++;
						break;
					case 2:
						scriptCodePtr += 2;
						break;
					case 3:
					{
						scriptCodePtr++;
						int j = 0;
						num = 0;
						ObjectSystem.scriptEng.sRegister = ObjectSystem.scriptData[scriptCodePtr];
						while (j < ObjectSystem.scriptEng.sRegister)
						{
							switch (num)
							{
							case 0:
								scriptCodePtr++;
								num++;
								break;
							case 1:
								num++;
								break;
							case 2:
								num++;
								break;
							case 3:
								num = 0;
								break;
							}
							j++;
						}
						if (num == 0)
						{
							scriptCodePtr += 2;
						}
						else
						{
							scriptCodePtr++;
						}
						break;
					}
					}
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0002EEF4 File Offset: 0x0002D0F4
		public static void ProcessStartupScripts()
		{
			ObjectSystem.objectEntityList[1057].type = ObjectSystem.objectEntityList[0].type;
			ObjectSystem.scriptFramesNo = 0;
			ObjectSystem.playerNum = 0;
			ObjectSystem.scriptEng.arrayPosition[2] = 1056;
			for (int i = 0; i < 256; i++)
			{
				ObjectSystem.objectLoop = 1056;
				ObjectSystem.objectEntityList[1056].type = (byte)i;
				ObjectSystem.objectScriptList[i].numFrames = 0;
				ObjectSystem.objectScriptList[i].surfaceNum = 0;
				ObjectSystem.objectScriptList[i].frameListOffset = ObjectSystem.scriptFramesNo;
				ObjectSystem.objectScriptList[i].numFrames = ObjectSystem.scriptFramesNo;
				if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[i].startupScript] > 0)
				{
					ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[i].startupScript, ObjectSystem.objectScriptList[i].startupJumpTable, 3);
				}
				ObjectSystem.objectScriptList[i].numFrames = ObjectSystem.scriptFramesNo - ObjectSystem.objectScriptList[i].numFrames;
			}
			ObjectSystem.objectEntityList[1056].type = ObjectSystem.objectEntityList[1057].type;
			ObjectSystem.objectEntityList[1056].type = 0;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0002F02C File Offset: 0x0002D22C
		public static void ProcessObjects()
		{
			bool flag = false;
			ObjectSystem.objectDrawOrderList[0].listSize = 0;
			ObjectSystem.objectDrawOrderList[1].listSize = 0;
			ObjectSystem.objectDrawOrderList[2].listSize = 0;
			ObjectSystem.objectDrawOrderList[3].listSize = 0;
			ObjectSystem.objectDrawOrderList[4].listSize = 0;
			ObjectSystem.objectDrawOrderList[5].listSize = 0;
			ObjectSystem.objectDrawOrderList[6].listSize = 0;
			ObjectSystem.objectLoop = 0;
			while (ObjectSystem.objectLoop < 1184)
			{
				switch (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].priority)
				{
				case 0:
				{
					int num = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16;
					int num2 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16;
					flag = (num > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2 && num2 > StageSystem.yScrollOffset - 256 && num2 < StageSystem.yScrollOffset + 496);
					break;
				}
				case 1:
					flag = true;
					break;
				case 2:
					flag = true;
					break;
				case 3:
				{
					int num = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16;
					flag = (num > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2);
					break;
				}
				case 4:
				{
					int num = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16;
					int num2 = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16;
					if (num > StageSystem.xScrollOffset - GlobalAppDefinitions.OBJECT_BORDER_X1 && num < StageSystem.xScrollOffset + GlobalAppDefinitions.OBJECT_BORDER_X2 && num2 > StageSystem.yScrollOffset - 256 && num2 < StageSystem.yScrollOffset + 496)
					{
						flag = true;
					}
					else
					{
						flag = false;
						ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type = 0;
					}
					break;
				}
				case 5:
					flag = false;
					break;
				}
				if (flag && ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type > 0)
				{
					int num3 = (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type;
					ObjectSystem.playerNum = 0;
					if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[num3].mainScript] > 0)
					{
						ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[num3].mainScript, ObjectSystem.objectScriptList[num3].mainJumpTable, 0);
					}
					if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[num3].playerScript] > 0)
					{
						while (ObjectSystem.playerNum < (int)PlayerSystem.numActivePlayers)
						{
							if (PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction == 1)
							{
								ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[num3].playerScript, ObjectSystem.objectScriptList[num3].playerJumpTable, 1);
							}
							ObjectSystem.playerNum++;
						}
					}
					num3 = (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].drawOrder;
					if (num3 < 7)
					{
						ObjectSystem.objectDrawOrderList[num3].entityRef[ObjectSystem.objectDrawOrderList[num3].listSize] = ObjectSystem.objectLoop;
						ObjectSystem.objectDrawOrderList[num3].listSize++;
					}
				}
				ObjectSystem.objectLoop++;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0002F330 File Offset: 0x0002D530
		public static void ProcessPausedObjects()
		{
			ObjectSystem.objectDrawOrderList[0].listSize = 0;
			ObjectSystem.objectDrawOrderList[1].listSize = 0;
			ObjectSystem.objectDrawOrderList[2].listSize = 0;
			ObjectSystem.objectDrawOrderList[3].listSize = 0;
			ObjectSystem.objectDrawOrderList[4].listSize = 0;
			ObjectSystem.objectDrawOrderList[5].listSize = 0;
			ObjectSystem.objectDrawOrderList[6].listSize = 0;
			ObjectSystem.objectLoop = 0;
			while (ObjectSystem.objectLoop < 1184)
			{
				if (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].priority == 2 && ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type > 0)
				{
					int num = (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type;
					ObjectSystem.playerNum = 0;
					if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[num].mainScript] > 0)
					{
						ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[num].mainScript, ObjectSystem.objectScriptList[num].mainJumpTable, 0);
					}
					if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[num].playerScript] > 0)
					{
						while (ObjectSystem.playerNum < (int)PlayerSystem.numActivePlayers)
						{
							if (PlayerSystem.playerList[ObjectSystem.playerNum].objectInteraction == 1)
							{
								ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[num].playerScript, ObjectSystem.objectScriptList[num].playerJumpTable, 1);
							}
							ObjectSystem.playerNum++;
						}
					}
					num = (int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].drawOrder;
					if (num < 7)
					{
						ObjectSystem.objectDrawOrderList[num].entityRef[ObjectSystem.objectDrawOrderList[num].listSize] = ObjectSystem.objectLoop;
						ObjectSystem.objectDrawOrderList[num].listSize++;
					}
				}
				ObjectSystem.objectLoop++;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0002F4DC File Offset: 0x0002D6DC
		public static void DrawObjectList(int DrawListNo)
		{
			int listSize = ObjectSystem.objectDrawOrderList[DrawListNo].listSize;
			for (int i = 0; i < listSize; i++)
			{
				ObjectSystem.objectLoop = ObjectSystem.objectDrawOrderList[DrawListNo].entityRef[i];
				if (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type > 0)
				{
					ObjectSystem.playerNum = 0;
					if (ObjectSystem.scriptData[ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].drawScript] > 0)
					{
						ObjectSystem.ProcessScript(ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].drawScript, ObjectSystem.objectScriptList[(int)ObjectSystem.objectEntityList[ObjectSystem.objectLoop].type].drawJumpTable, 2);
					}
				}
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0002F594 File Offset: 0x0002D794
		public static void BasicCollision(int cLeft, int cTop, int cRight, int cBottom)
		{
			PlayerObject playerObject = PlayerSystem.playerList[ObjectSystem.playerNum];
			CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerObject.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerObject.animationFile.aniListOffset + (int)playerObject.objectPtr.animation].frameListOffset + (int)playerObject.objectPtr.frame].collisionBox];
			PlayerSystem.collisionLeft = playerObject.xPos >> 16;
			PlayerSystem.collisionTop = playerObject.yPos >> 16;
			PlayerSystem.collisionRight = PlayerSystem.collisionLeft;
			PlayerSystem.collisionBottom = PlayerSystem.collisionTop;
			PlayerSystem.collisionLeft += (int)collisionBox.left[0];
			PlayerSystem.collisionTop += (int)collisionBox.top[0];
			PlayerSystem.collisionRight += (int)collisionBox.right[0];
			PlayerSystem.collisionBottom += (int)collisionBox.bottom[0];
			if (PlayerSystem.collisionRight > cLeft && PlayerSystem.collisionLeft < cRight && PlayerSystem.collisionBottom > cTop && PlayerSystem.collisionTop < cBottom)
			{
				ObjectSystem.scriptEng.checkResult = 1;
				return;
			}
			ObjectSystem.scriptEng.checkResult = 0;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0002F6AC File Offset: 0x0002D8AC
		public static void BoxCollision(int cLeft, int cTop, int cRight, int cBottom)
		{
			int i = 0;
			PlayerObject playerObject = PlayerSystem.playerList[ObjectSystem.playerNum];
			CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerObject.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerObject.animationFile.aniListOffset + (int)playerObject.objectPtr.animation].frameListOffset + (int)playerObject.objectPtr.frame].collisionBox];
			PlayerSystem.collisionLeft = (int)collisionBox.left[0];
			PlayerSystem.collisionTop = (int)collisionBox.top[0];
			PlayerSystem.collisionRight = (int)collisionBox.right[0];
			PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
			ObjectSystem.scriptEng.checkResult = 0;
			switch (playerObject.collisionMode)
			{
			case 0:
			case 2:
				if (playerObject.xVelocity != 0)
				{
					i = Math.Abs(playerObject.xVelocity);
				}
				else
				{
					i = Math.Abs(playerObject.speed);
				}
				break;
			case 1:
			case 3:
				i = Math.Abs(playerObject.xVelocity);
				break;
			}
			if (i > Math.Abs(playerObject.yVelocity))
			{
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionRight << 16);
				ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
				ObjectSystem.cSensor[0].yPos = playerObject.yPos - 131072;
				ObjectSystem.cSensor[1].yPos = playerObject.yPos + 524288;
				for (i = 0; i < 2; i++)
				{
					if (ObjectSystem.cSensor[i].xPos >= cLeft && playerObject.xPos - playerObject.xVelocity < cLeft && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
					{
						ObjectSystem.cSensor[i].collided = 1;
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1)
				{
					playerObject.xPos = cLeft - (PlayerSystem.collisionRight << 16);
					if (playerObject.xVelocity > 0)
					{
						if (playerObject.objectPtr.direction == 0)
						{
							playerObject.pushing = 2;
						}
						playerObject.xVelocity = 0;
						playerObject.speed = 0;
					}
					ObjectSystem.scriptEng.checkResult = 2;
					return;
				}
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft << 16);
				ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
				ObjectSystem.cSensor[0].yPos = playerObject.yPos - 131072;
				ObjectSystem.cSensor[1].yPos = playerObject.yPos + 524288;
				for (i = 0; i < 2; i++)
				{
					if (ObjectSystem.cSensor[i].xPos <= cRight && playerObject.xPos - playerObject.xVelocity > cRight && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
					{
						ObjectSystem.cSensor[i].collided = 1;
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1)
				{
					playerObject.xPos = cRight - (PlayerSystem.collisionLeft << 16);
					if (playerObject.xVelocity < 0)
					{
						if (playerObject.objectPtr.direction == 1)
						{
							playerObject.pushing = 2;
						}
						playerObject.xVelocity = 0;
						playerObject.speed = 0;
					}
					ObjectSystem.scriptEng.checkResult = 3;
					return;
				}
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[2].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft + 2 << 16);
				ObjectSystem.cSensor[1].xPos = playerObject.xPos;
				ObjectSystem.cSensor[2].xPos = playerObject.xPos + (PlayerSystem.collisionRight - 2 << 16);
				ObjectSystem.cSensor[0].yPos = playerObject.yPos + (PlayerSystem.collisionBottom << 16);
				ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
				ObjectSystem.cSensor[2].yPos = ObjectSystem.cSensor[0].yPos;
				if (playerObject.yVelocity > -1)
				{
					for (i = 0; i < 3; i++)
					{
						if (ObjectSystem.cSensor[i].xPos > cLeft && ObjectSystem.cSensor[i].xPos < cRight && ObjectSystem.cSensor[i].yPos >= cTop && playerObject.yPos - playerObject.yVelocity < cTop)
						{
							ObjectSystem.cSensor[i].collided = 1;
							playerObject.flailing[i] = 1;
						}
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1 || ObjectSystem.cSensor[2].collided == 1)
				{
					if (playerObject.gravity == 0 && (playerObject.collisionMode == 1 || playerObject.collisionMode == 3))
					{
						playerObject.xVelocity = 0;
						playerObject.speed = 0;
					}
					playerObject.yPos = cTop - (PlayerSystem.collisionBottom << 16);
					playerObject.gravity = 0;
					playerObject.yVelocity = 0;
					playerObject.angle = 0;
					playerObject.objectPtr.rotation = 0;
					playerObject.controlLock = 0;
					ObjectSystem.scriptEng.checkResult = 1;
					return;
				}
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft + 2 << 16);
				ObjectSystem.cSensor[1].xPos = playerObject.xPos + (PlayerSystem.collisionRight - 2 << 16);
				ObjectSystem.cSensor[0].yPos = playerObject.yPos + (PlayerSystem.collisionTop << 16);
				ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
				for (i = 0; i < 2; i++)
				{
					if (ObjectSystem.cSensor[i].xPos > cLeft && ObjectSystem.cSensor[i].xPos < cRight && ObjectSystem.cSensor[i].yPos <= cBottom && playerObject.yPos - playerObject.yVelocity > cBottom)
					{
						ObjectSystem.cSensor[i].collided = 1;
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1)
				{
					if (playerObject.gravity == 1)
					{
						playerObject.yPos = cBottom - (PlayerSystem.collisionTop << 16);
					}
					if (playerObject.yVelocity < 1)
					{
						playerObject.yVelocity = 0;
					}
					ObjectSystem.scriptEng.checkResult = 4;
					return;
				}
			}
			else
			{
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[2].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft + 2 << 16);
				ObjectSystem.cSensor[1].xPos = playerObject.xPos;
				ObjectSystem.cSensor[2].xPos = playerObject.xPos + (PlayerSystem.collisionRight - 2 << 16);
				ObjectSystem.cSensor[0].yPos = playerObject.yPos + (PlayerSystem.collisionBottom << 16);
				ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
				ObjectSystem.cSensor[2].yPos = ObjectSystem.cSensor[0].yPos;
				if (playerObject.yVelocity > -1)
				{
					for (i = 0; i < 3; i++)
					{
						if (ObjectSystem.cSensor[i].xPos > cLeft && ObjectSystem.cSensor[i].xPos < cRight && ObjectSystem.cSensor[i].yPos >= cTop && playerObject.yPos - playerObject.yVelocity < cTop)
						{
							ObjectSystem.cSensor[i].collided = 1;
							playerObject.flailing[i] = 1;
						}
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1 || ObjectSystem.cSensor[2].collided == 1)
				{
					if (playerObject.gravity == 0 && (playerObject.collisionMode == 1 || playerObject.collisionMode == 3))
					{
						playerObject.xVelocity = 0;
						playerObject.speed = 0;
					}
					playerObject.yPos = cTop - (PlayerSystem.collisionBottom << 16);
					playerObject.gravity = 0;
					playerObject.yVelocity = 0;
					playerObject.angle = 0;
					playerObject.objectPtr.rotation = 0;
					playerObject.controlLock = 0;
					ObjectSystem.scriptEng.checkResult = 1;
					return;
				}
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft + 2 << 16);
				ObjectSystem.cSensor[1].xPos = playerObject.xPos + (PlayerSystem.collisionRight - 2 << 16);
				ObjectSystem.cSensor[0].yPos = playerObject.yPos + (PlayerSystem.collisionTop << 16);
				ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
				for (i = 0; i < 2; i++)
				{
					if (ObjectSystem.cSensor[i].xPos > cLeft && ObjectSystem.cSensor[i].xPos < cRight && ObjectSystem.cSensor[i].yPos <= cBottom && playerObject.yPos - playerObject.yVelocity > cBottom)
					{
						ObjectSystem.cSensor[i].collided = 1;
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1)
				{
					if (playerObject.gravity == 1)
					{
						playerObject.yPos = cBottom - (PlayerSystem.collisionTop << 16);
					}
					if (playerObject.yVelocity < 1)
					{
						playerObject.yVelocity = 0;
					}
					ObjectSystem.scriptEng.checkResult = 4;
					return;
				}
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionRight << 16);
				ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
				ObjectSystem.cSensor[0].yPos = playerObject.yPos - 131072;
				ObjectSystem.cSensor[1].yPos = playerObject.yPos + 524288;
				for (i = 0; i < 2; i++)
				{
					if (ObjectSystem.cSensor[i].xPos >= cLeft && playerObject.xPos - playerObject.xVelocity < cLeft && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
					{
						ObjectSystem.cSensor[i].collided = 1;
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1)
				{
					playerObject.xPos = cLeft - (PlayerSystem.collisionRight << 16);
					if (playerObject.xVelocity > 0)
					{
						if (playerObject.objectPtr.direction == 0)
						{
							playerObject.pushing = 2;
						}
						playerObject.xVelocity = 0;
						playerObject.speed = 0;
					}
					ObjectSystem.scriptEng.checkResult = 2;
					return;
				}
				ObjectSystem.cSensor[0].collided = 0;
				ObjectSystem.cSensor[1].collided = 0;
				ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft << 16);
				ObjectSystem.cSensor[1].xPos = ObjectSystem.cSensor[0].xPos;
				ObjectSystem.cSensor[0].yPos = playerObject.yPos - 131072;
				ObjectSystem.cSensor[1].yPos = playerObject.yPos + 524288;
				for (i = 0; i < 2; i++)
				{
					if (ObjectSystem.cSensor[i].xPos <= cRight && playerObject.xPos - playerObject.xVelocity > cRight && ObjectSystem.cSensor[1].yPos > cTop && ObjectSystem.cSensor[0].yPos < cBottom)
					{
						ObjectSystem.cSensor[i].collided = 1;
					}
				}
				if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1)
				{
					playerObject.xPos = cRight - (PlayerSystem.collisionLeft << 16);
					if (playerObject.xVelocity < 0)
					{
						if (playerObject.objectPtr.direction == 1)
						{
							playerObject.pushing = 2;
						}
						playerObject.xVelocity = 0;
						playerObject.speed = 0;
					}
					ObjectSystem.scriptEng.checkResult = 3;
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00030274 File Offset: 0x0002E474
		public static void PlatformCollision(int cLeft, int cTop, int cRight, int cBottom)
		{
			PlayerObject playerObject = PlayerSystem.playerList[ObjectSystem.playerNum];
			CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerObject.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerObject.animationFile.aniListOffset + (int)playerObject.objectPtr.animation].frameListOffset + (int)playerObject.objectPtr.frame].collisionBox];
			PlayerSystem.collisionLeft = (int)collisionBox.left[0];
			PlayerSystem.collisionTop = (int)collisionBox.top[0];
			PlayerSystem.collisionRight = (int)collisionBox.right[0];
			PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
			ObjectSystem.cSensor[0].collided = 0;
			ObjectSystem.cSensor[1].collided = 0;
			ObjectSystem.cSensor[2].collided = 0;
			ObjectSystem.cSensor[0].xPos = playerObject.xPos + (PlayerSystem.collisionLeft + 1 << 16);
			ObjectSystem.cSensor[1].xPos = playerObject.xPos;
			ObjectSystem.cSensor[2].xPos = playerObject.xPos + (PlayerSystem.collisionRight << 16);
			ObjectSystem.cSensor[0].yPos = playerObject.yPos + (PlayerSystem.collisionBottom << 16);
			ObjectSystem.cSensor[1].yPos = ObjectSystem.cSensor[0].yPos;
			ObjectSystem.cSensor[2].yPos = ObjectSystem.cSensor[0].yPos;
			ObjectSystem.scriptEng.checkResult = 0;
			for (int i = 0; i < 3; i++)
			{
				if (ObjectSystem.cSensor[i].xPos > cLeft && ObjectSystem.cSensor[i].xPos < cRight && ObjectSystem.cSensor[i].yPos > cTop - 2 && ObjectSystem.cSensor[i].yPos < cBottom && playerObject.yVelocity >= 0)
				{
					ObjectSystem.cSensor[i].collided = 1;
					playerObject.flailing[i] = 1;
				}
			}
			if (ObjectSystem.cSensor[0].collided == 1 || ObjectSystem.cSensor[1].collided == 1 || ObjectSystem.cSensor[2].collided == 1)
			{
				if (playerObject.gravity == 0 && (playerObject.collisionMode == 1 || playerObject.collisionMode == 3))
				{
					playerObject.xVelocity = 0;
					playerObject.speed = 0;
				}
				playerObject.yPos = cTop - (PlayerSystem.collisionBottom << 16);
				playerObject.gravity = 0;
				playerObject.yVelocity = 0;
				playerObject.angle = 0;
				playerObject.objectPtr.rotation = 0;
				playerObject.controlLock = 0;
				ObjectSystem.scriptEng.checkResult = 1;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000304D8 File Offset: 0x0002E6D8
		public static void ObjectFloorCollision(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7)
			{
				int num3 = num >> 7;
				int num4 = (num & 127) >> 4;
				int num5 = num2 >> 7;
				int num6 = (num2 & 127) >> 4;
				int num7 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6;
				num7 += num4 + (num6 << 3);
				int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
				if (StageSystem.tile128x128.collisionFlag[cPlane, num7] != 2 && StageSystem.tile128x128.collisionFlag[cPlane, num7] != 3)
				{
					switch (StageSystem.tile128x128.direction[num7])
					{
					case 0:
					{
						int num9 = (num & 15) + (num8 << 4);
						if ((num2 & 15) > (int)StageSystem.tileCollisions[cPlane].floorMask[num9])
						{
							num2 = (int)StageSystem.tileCollisions[cPlane].floorMask[num9] + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 1:
					{
						int num9 = 15 - (num & 15) + (num8 << 4);
						if ((num2 & 15) > (int)StageSystem.tileCollisions[cPlane].floorMask[num9])
						{
							num2 = (int)StageSystem.tileCollisions[cPlane].floorMask[num9] + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 2:
					{
						int num9 = (num & 15) + (num8 << 4);
						if ((num2 & 15) > (int)(15 - StageSystem.tileCollisions[cPlane].roofMask[num9]))
						{
							num2 = (int)(15 - StageSystem.tileCollisions[cPlane].roofMask[num9]) + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 3:
					{
						int num9 = 15 - (num & 15) + (num8 << 4);
						if ((num2 & 15) > (int)(15 - StageSystem.tileCollisions[cPlane].roofMask[num9]))
						{
							num2 = (int)(15 - StageSystem.tileCollisions[cPlane].roofMask[num9]) + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					}
				}
				if (ObjectSystem.scriptEng.checkResult == 1)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num2 - yOffset << 16;
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0003075C File Offset: 0x0002E95C
		public static void ObjectLWallCollision(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7)
			{
				int num3 = num >> 7;
				int num4 = (num & 127) >> 4;
				int num5 = num2 >> 7;
				int num6 = (num2 & 127) >> 4;
				int num7 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6;
				num7 += num4 + (num6 << 3);
				int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
				if (StageSystem.tile128x128.collisionFlag[cPlane, num7] != 1 && StageSystem.tile128x128.collisionFlag[cPlane, num7] < 3)
				{
					switch (StageSystem.tile128x128.direction[num7])
					{
					case 0:
					{
						int num9 = (num2 & 15) + (num8 << 4);
						if ((num & 15) > (int)StageSystem.tileCollisions[cPlane].leftWallMask[num9])
						{
							num = (int)StageSystem.tileCollisions[cPlane].leftWallMask[num9] + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 1:
					{
						int num9 = (num2 & 15) + (num8 << 4);
						if ((num & 15) > (int)(15 - StageSystem.tileCollisions[cPlane].rightWallMask[num9]))
						{
							num = (int)(15 - StageSystem.tileCollisions[cPlane].rightWallMask[num9]) + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 2:
					{
						int num9 = 15 - (num2 & 15) + (num8 << 4);
						if ((num & 15) > (int)StageSystem.tileCollisions[cPlane].leftWallMask[num9])
						{
							num = (int)StageSystem.tileCollisions[cPlane].leftWallMask[num9] + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 3:
					{
						int num9 = 15 - (num2 & 15) + (num8 << 4);
						if ((num & 15) > (int)(15 - StageSystem.tileCollisions[cPlane].rightWallMask[num9]))
						{
							num = (int)(15 - StageSystem.tileCollisions[cPlane].rightWallMask[num9]) + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					}
				}
				if (ObjectSystem.scriptEng.checkResult == 1)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num - xOffset << 16;
				}
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000309E0 File Offset: 0x0002EBE0
		public static void ObjectRWallCollision(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7)
			{
				int num3 = num >> 7;
				int num4 = (num & 127) >> 4;
				int num5 = num2 >> 7;
				int num6 = (num2 & 127) >> 4;
				int num7 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6;
				num7 += num4 + (num6 << 3);
				int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
				if (StageSystem.tile128x128.collisionFlag[cPlane, num7] != 1 && StageSystem.tile128x128.collisionFlag[cPlane, num7] < 3)
				{
					switch (StageSystem.tile128x128.direction[num7])
					{
					case 0:
					{
						int num9 = (num2 & 15) + (num8 << 4);
						if ((num & 15) < (int)StageSystem.tileCollisions[cPlane].rightWallMask[num9])
						{
							num = (int)StageSystem.tileCollisions[cPlane].rightWallMask[num9] + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 1:
					{
						int num9 = (num2 & 15) + (num8 << 4);
						if ((num & 15) < (int)(15 - StageSystem.tileCollisions[cPlane].leftWallMask[num9]))
						{
							num = (int)(15 - StageSystem.tileCollisions[cPlane].leftWallMask[num9]) + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 2:
					{
						int num9 = 15 - (num2 & 15) + (num8 << 4);
						if ((num & 15) < (int)StageSystem.tileCollisions[cPlane].rightWallMask[num9])
						{
							num = (int)StageSystem.tileCollisions[cPlane].rightWallMask[num9] + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 3:
					{
						int num9 = 15 - (num2 & 15) + (num8 << 4);
						if ((num & 15) < (int)(15 - StageSystem.tileCollisions[cPlane].leftWallMask[num9]))
						{
							num = (int)(15 - StageSystem.tileCollisions[cPlane].leftWallMask[num9]) + (num3 << 7) + (num4 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					}
				}
				if (ObjectSystem.scriptEng.checkResult == 1)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num - xOffset << 16;
				}
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00030C64 File Offset: 0x0002EE64
		public static void ObjectRoofCollision(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7)
			{
				int num3 = num >> 7;
				int num4 = (num & 127) >> 4;
				int num5 = num2 >> 7;
				int num6 = (num2 & 127) >> 4;
				int num7 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num5 << 8)] << 6;
				num7 += num4 + (num6 << 3);
				int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
				if (StageSystem.tile128x128.collisionFlag[cPlane, num7] != 1 && StageSystem.tile128x128.collisionFlag[cPlane, num7] < 3)
				{
					switch (StageSystem.tile128x128.direction[num7])
					{
					case 0:
					{
						int num9 = (num & 15) + (num8 << 4);
						if ((num2 & 15) < (int)StageSystem.tileCollisions[cPlane].roofMask[num9])
						{
							num2 = (int)StageSystem.tileCollisions[cPlane].roofMask[num9] + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 1:
					{
						int num9 = 15 - (num & 15) + (num8 << 4);
						if ((num2 & 15) < (int)StageSystem.tileCollisions[cPlane].roofMask[num9])
						{
							num2 = (int)StageSystem.tileCollisions[cPlane].roofMask[num9] + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 2:
					{
						int num9 = (num & 15) + (num8 << 4);
						if ((num2 & 15) < (int)(15 - StageSystem.tileCollisions[cPlane].floorMask[num9]))
						{
							num2 = (int)(15 - StageSystem.tileCollisions[cPlane].floorMask[num9]) + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					case 3:
					{
						int num9 = 15 - (num & 15) + (num8 << 4);
						if ((num2 & 15) < (int)(15 - StageSystem.tileCollisions[cPlane].floorMask[num9]))
						{
							num2 = (int)(15 - StageSystem.tileCollisions[cPlane].floorMask[num9]) + (num5 << 7) + (num6 << 4);
							ObjectSystem.scriptEng.checkResult = 1;
						}
						break;
					}
					}
				}
				if (ObjectSystem.scriptEng.checkResult == 1)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num2 - yOffset << 16;
				}
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00030EE8 File Offset: 0x0002F0E8
		public static void ObjectFloorGrip(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			int num3 = num2;
			num2 -= 16;
			for (int i = 3; i > 0; i--)
			{
				if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
				{
					int num4 = num >> 7;
					int num5 = (num & 127) >> 4;
					int num6 = num2 >> 7;
					int num7 = (num2 & 127) >> 4;
					int num8 = (int)StageSystem.stageLayouts[0].tileMap[num4 + (num6 << 8)] << 6;
					num8 += num5 + (num7 << 3);
					int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
					if (StageSystem.tile128x128.collisionFlag[cPlane, num8] != 2 && StageSystem.tile128x128.collisionFlag[cPlane, num8] != 3)
					{
						switch (StageSystem.tile128x128.direction[num8])
						{
						case 0:
						{
							int num10 = (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].floorMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].floorMask[num10] + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 1:
						{
							int num10 = 15 - (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].floorMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].floorMask[num10] + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 2:
						{
							int num10 = (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].roofMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)(15 - StageSystem.tileCollisions[cPlane].roofMask[num10]) + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 3:
						{
							int num10 = 15 - (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].roofMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)(15 - StageSystem.tileCollisions[cPlane].roofMask[num10]) + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						}
					}
				}
				num2 += 16;
			}
			if (ObjectSystem.scriptEng.checkResult == 1)
			{
				if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - num3) < 16)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - yOffset << 16;
					return;
				}
				ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num3 - yOffset << 16;
				ObjectSystem.scriptEng.checkResult = 0;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0003121C File Offset: 0x0002F41C
		public static void ObjectLWallGrip(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			int num3 = num;
			num -= 16;
			for (int i = 3; i > 0; i--)
			{
				if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
				{
					int num4 = num >> 7;
					int num5 = (num & 127) >> 4;
					int num6 = num2 >> 7;
					int num7 = (num2 & 127) >> 4;
					int num8 = (int)StageSystem.stageLayouts[0].tileMap[num4 + (num6 << 8)] << 6;
					num8 += num5 + (num7 << 3);
					int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
					if (StageSystem.tile128x128.collisionFlag[cPlane, num8] < 3)
					{
						switch (StageSystem.tile128x128.direction[num8])
						{
						case 0:
						{
							int num10 = (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].leftWallMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].leftWallMask[num10] + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 1:
						{
							int num10 = (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].rightWallMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)(15 - StageSystem.tileCollisions[cPlane].rightWallMask[num10]) + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 2:
						{
							int num10 = 15 - (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].leftWallMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].leftWallMask[num10] + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 3:
						{
							int num10 = 15 - (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].rightWallMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)(15 - StageSystem.tileCollisions[cPlane].rightWallMask[num10]) + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						}
					}
				}
				num += 16;
			}
			if (ObjectSystem.scriptEng.checkResult == 1)
			{
				if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - num3) < 16)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - xOffset << 16;
					return;
				}
				ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num3 - xOffset << 16;
				ObjectSystem.scriptEng.checkResult = 0;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00031538 File Offset: 0x0002F738
		public static void ObjectRWallGrip(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			int num3 = num;
			num += 16;
			for (int i = 3; i > 0; i--)
			{
				if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
				{
					int num4 = num >> 7;
					int num5 = (num & 127) >> 4;
					int num6 = num2 >> 7;
					int num7 = (num2 & 127) >> 4;
					int num8 = (int)StageSystem.stageLayouts[0].tileMap[num4 + (num6 << 8)] << 6;
					num8 += num5 + (num7 << 3);
					int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
					if (StageSystem.tile128x128.collisionFlag[cPlane, num8] < 3)
					{
						switch (StageSystem.tile128x128.direction[num8])
						{
						case 0:
						{
							int num10 = (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].rightWallMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].rightWallMask[num10] + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 1:
						{
							int num10 = (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].leftWallMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)(15 - StageSystem.tileCollisions[cPlane].leftWallMask[num10]) + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 2:
						{
							int num10 = 15 - (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].rightWallMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)StageSystem.tileCollisions[cPlane].rightWallMask[num10] + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 3:
						{
							int num10 = 15 - (num2 & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].leftWallMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = (int)(15 - StageSystem.tileCollisions[cPlane].leftWallMask[num10]) + (num4 << 7) + (num5 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						}
					}
				}
				num -= 16;
			}
			if (ObjectSystem.scriptEng.checkResult == 1)
			{
				if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - num3) < 16)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos - xOffset << 16;
					return;
				}
				ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos = num3 - xOffset << 16;
				ObjectSystem.scriptEng.checkResult = 0;
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00031854 File Offset: 0x0002FA54
		public static void ObjectRoofGrip(int xOffset, int yOffset, int cPlane)
		{
			ObjectSystem.scriptEng.checkResult = 0;
			int num = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].xPos >> 16) + xOffset;
			int num2 = (ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos >> 16) + yOffset;
			int num3 = num2;
			num2 += 16;
			for (int i = 3; i > 0; i--)
			{
				if (num > 0 && num < (int)StageSystem.stageLayouts[0].xSize << 7 && num2 > 0 && num2 < (int)StageSystem.stageLayouts[0].ySize << 7 && ObjectSystem.scriptEng.checkResult == 0)
				{
					int num4 = num >> 7;
					int num5 = (num & 127) >> 4;
					int num6 = num2 >> 7;
					int num7 = (num2 & 127) >> 4;
					int num8 = (int)StageSystem.stageLayouts[0].tileMap[num4 + (num6 << 8)] << 6;
					num8 += num5 + (num7 << 3);
					int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
					if (StageSystem.tile128x128.collisionFlag[cPlane, num8] < 3)
					{
						switch (StageSystem.tile128x128.direction[num8])
						{
						case 0:
						{
							int num10 = (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].roofMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].roofMask[num10] + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 1:
						{
							int num10 = 15 - (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].roofMask[num10] > -64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)StageSystem.tileCollisions[cPlane].roofMask[num10] + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 2:
						{
							int num10 = (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].floorMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)(15 - StageSystem.tileCollisions[cPlane].floorMask[num10]) + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						case 3:
						{
							int num10 = 15 - (num & 15) + (num9 << 4);
							if (StageSystem.tileCollisions[cPlane].floorMask[num10] < 64)
							{
								ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = (int)(15 - StageSystem.tileCollisions[cPlane].floorMask[num10]) + (num6 << 7) + (num7 << 4);
								ObjectSystem.scriptEng.checkResult = 1;
							}
							break;
						}
						}
					}
				}
				num2 -= 16;
			}
			if (ObjectSystem.scriptEng.checkResult == 1)
			{
				if (Math.Abs(ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - num3) < 16)
				{
					ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos - yOffset << 16;
					return;
				}
				ObjectSystem.objectEntityList[ObjectSystem.objectLoop].yPos = num3 - yOffset << 16;
				ObjectSystem.scriptEng.checkResult = 0;
			}
		}

		// Token: 0x0400020A RID: 522
		public const int SUB_MAIN = 0;

		// Token: 0x0400020B RID: 523
		public const int SUB_PLAYER = 1;

		// Token: 0x0400020C RID: 524
		public const int SUB_DRAW = 2;

		// Token: 0x0400020D RID: 525
		public const int SUB_STARTUP = 3;

		// Token: 0x0400020E RID: 526
		public const int NUM_ARITHMETIC_TOKENS = 13;

		// Token: 0x0400020F RID: 527
		public const int NUM_EVALUATION_TOKENS = 6;

		// Token: 0x04000210 RID: 528
		public const int NUM_OPCODES = 135;

		// Token: 0x04000211 RID: 529
		public const int NUM_VARIABLE_NAMES = 229;

		// Token: 0x04000212 RID: 530
		public const int NUM_CONSTANTS = 31;

		// Token: 0x04000213 RID: 531
		public const int SCRIPT_DATA_SIZE = 262144;

		// Token: 0x04000214 RID: 532
		public const int JUMP_TABLE_SIZE = 16384;

		// Token: 0x04000215 RID: 533
		private static char[,] functionNames = new char[512, 32];

		// Token: 0x04000216 RID: 534
		private static char[,] typeNames = new char[256, 32];

		// Token: 0x04000217 RID: 535
		public static int[] scriptData = new int[262144];

		// Token: 0x04000218 RID: 536
		public static int scriptDataPos = 0;

		// Token: 0x04000219 RID: 537
		public static int scriptDataOffset;

		// Token: 0x0400021A RID: 538
		public static int scriptLineNumber;

		// Token: 0x0400021B RID: 539
		public static int[] jumpTableData = new int[16384];

		// Token: 0x0400021C RID: 540
		public static int jumpTableDataPos = 0;

		// Token: 0x0400021D RID: 541
		public static int jumpTableOffset;

		// Token: 0x0400021E RID: 542
		public static int[] jumpTableStack = new int[1024];

		// Token: 0x0400021F RID: 543
		public static int jumpTableStackPos = 0;

		// Token: 0x04000220 RID: 544
		public static int NUM_FUNCTIONS;

		// Token: 0x04000221 RID: 545
		public static int[] functionStack = new int[1024];

		// Token: 0x04000222 RID: 546
		public static int functionStackPos = 0;

		// Token: 0x04000223 RID: 547
		public static SpriteFrame[] scriptFrames = new SpriteFrame[4096];

		// Token: 0x04000224 RID: 548
		public static int scriptFramesNo = 0;

		// Token: 0x04000225 RID: 549
		public static byte NO_GLOBALVARIABLES;

		// Token: 0x04000226 RID: 550
		public static char[,] globalVariableNames = new char[256, 32];

		// Token: 0x04000227 RID: 551
		public static int[] globalVariables = new int[256];

		// Token: 0x04000228 RID: 552
		public static int objectLoop;

		// Token: 0x04000229 RID: 553
		public static ScriptEngine scriptEng = new ScriptEngine();

		// Token: 0x0400022A RID: 554
		public static char[] scriptText = new char[256];

		// Token: 0x0400022B RID: 555
		public static ObjectScript[] objectScriptList = new ObjectScript[256];

		// Token: 0x0400022C RID: 556
		public static FunctionScript[] functionScriptList = new FunctionScript[512];

		// Token: 0x0400022D RID: 557
		public static ObjectEntity[] objectEntityList = new ObjectEntity[1184];

		// Token: 0x0400022E RID: 558
		public static ObjectDrawList[] objectDrawOrderList = new ObjectDrawList[7];

		// Token: 0x0400022F RID: 559
		public static int playerNum;

		// Token: 0x04000230 RID: 560
		public static CollisionSensor[] cSensor = new CollisionSensor[6];

		// Token: 0x04000231 RID: 561
		private static Random rand = new Random();

		// Token: 0x04000232 RID: 562
		private static sbyte[] scriptOpcodeSizes = new sbyte[]
		{
			0,
			2,
			2,
			2,
			1,
			1,
			2,
			2,
			2,
			2,
			2,
			2,
			2,
			2,
			1,
			2,
			2,
			2,
			2,
			3,
			3,
			3,
			3,
			3,
			3,
			0,
			0,
			3,
			3,
			3,
			3,
			3,
			3,
			0,
			2,
			0,
			0,
			2,
			2,
			2,
			2,
			2,
			5,
			5,
			3,
			4,
			7,
			1,
			1,
			1,
			3,
			3,
			4,
			7,
			7,
			3,
			6,
			6,
			5,
			3,
			4,
			3,
			7,
			2,
			1,
			4,
			4,
			1,
			4,
			3,
			4,
			0,
			8,
			5,
			5,
			4,
			2,
			0,
			0,
			0,
			0,
			0,
			3,
			1,
			0,
			2,
			1,
			3,
			4,
			4,
			1,
			0,
			2,
			1,
			1,
			0,
			1,
			2,
			4,
			4,
			2,
			2,
			2,
			4,
			3,
			1,
			0,
			6,
			4,
			4,
			4,
			3,
			3,
			0,
			0,
			1,
			2,
			3,
			3,
			4,
			2,
			4,
			2,
			0,
			0,
			1,
			3,
			7,
			5,
			2,
			2,
			2,
			1,
			1,
			4
		};
	}
}
